using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[SerializeField]
public enum CursorMode
{
    Select = 0,
    Create = 1,
    Push = 2,
    Remove = 3
}

public class GameManager : MonoBehaviour
{
    private GameObject _cubePrefab;
    private GameObject _pushSpherePrefab;
    private GameObject _tempPushSphere;

    UnityEvent _unselectEvent;    

    private Transform _objectForPush;
    private Vector3 _pointOfPush;
    private float _currentPushForce = 0f;

    float MaxPushForce = 100f;
    float PushIncreasingSpeed = 0.1f;

    private GameManager() { }

    private static GameManager _singleInstance;

    public CursorMode CurrentCursorMode;
    public static GameManager SingleInstance
    {
        get
        {
            if (_singleInstance == null)
            {
                _singleInstance = new GameManager();
                GameObject emptyObject = new GameObject();
                emptyObject.name = "GameManager";
                _singleInstance = emptyObject.AddComponent<GameManager>();
            }
            return _singleInstance;
        }
    }
    public void ChangeCursorMode(int cursorMode)
    {
        CurrentCursorMode = (CursorMode)cursorMode;
    }

    void Start()
    {
        _unselectEvent = new UnityEvent();
        _cubePrefab = Resources.Load<GameObject>("Cube");
        _pushSpherePrefab = Resources.Load<GameObject>("PushSphere");        
    }

    void UnselectCube()
    {
        _unselectEvent.Invoke();
        _unselectEvent.RemoveAllListeners();
    }


    void Update()
    {
        if (_currentPushForce > 0)
        {
            if (_tempPushSphere == null)
            {
                _tempPushSphere = Instantiate(_pushSpherePrefab, _pointOfPush, Quaternion.identity);
            }
            _tempPushSphere.transform.localScale = Vector3.one * (_currentPushForce / 100f);
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Transform hittedObject = hit.transform;
                if(hittedObject.tag == "Plane")
                {
                    switch (CurrentCursorMode)
                    {
                        case CursorMode.Select:
                            UnselectCube();
                            break;
                        case CursorMode.Create:
                            Debug.Log($"Cube created on");
                            Vector3 newPosition = new Vector3(hit.point.x, 1f, hit.point.z);

                            Instantiate(_cubePrefab, newPosition, Quaternion.identity);
                            break;
                    }                    
                }
                
                if (hittedObject.tag == "Cube")
                {
                    switch (CurrentCursorMode)
                    {
                        case CursorMode.Select:                            
                            CubeScript cubeScript = hittedObject.GetComponent<CubeScript>();
                            bool cubeWasSelected = cubeScript.Selected;

                            UnselectCube();

                            // Если куб уже был выбран, то по нажатию мы отменяем выделение, чтобы он не выделился еще раз
                            if (cubeWasSelected)
                            {                                
                                return;
                            }
                            cubeScript.Select();
                            _unselectEvent.AddListener(cubeScript.Unselect);
                            break;

                        case CursorMode.Push:
                            _objectForPush = hittedObject;
                            _pointOfPush = hit.point;
                            StartCoroutine(IncreasePushForse());
                            break;

                        case CursorMode.Remove:
                            Destroy(hittedObject.gameObject);
                            break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && CurrentCursorMode == CursorMode.Push)
        {
            Rigidbody rbOfPushedObject = _objectForPush.GetComponent<Rigidbody>();
            rbOfPushedObject.AddForceAtPosition(Vector3.forward * _currentPushForce, _pointOfPush);

            Destroy(_tempPushSphere);
            _tempPushSphere = null;
            StopAllCoroutines();
            _currentPushForce = 0f;
        }
    }

    IEnumerator IncreasePushForse()
    {
        while (_currentPushForce < MaxPushForce)
        {
            _currentPushForce += 1;
            Debug.Log(_currentPushForce);
            yield return new WaitForSeconds(PushIncreasingSpeed);
        }
    }
}
