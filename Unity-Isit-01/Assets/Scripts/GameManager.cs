using System.Collections;
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

    public CubeScript SelectedCube;
    private float _lastRotationYOfCube = 0f;

    public UnityEvent SelectEvent;
    public UnityEvent UnselectEvent;

    private Transform _objectForPush;
    private Vector3 _pointOfPush;
    private float _currentPushForce = 0f;

    float MaxPushForce = 100f;
    float PushIncreasingSpeed = 0.05f;

    private GameManager() { }

    private static GameManager _singleInstance;

    public CursorMode CurrentCursorMode;
    public static GameManager SingleInstance
    {
        get
        {
            if (_singleInstance == null)
            {
                GameObject emptyObject = GameObject.FindGameObjectWithTag("GameController");
                _singleInstance = emptyObject.GetComponent<GameManager>();
            }
            return _singleInstance;
        }
    }
    public void ChangeCursorMode(int cursorMode)
    {
        CurrentCursorMode = (CursorMode)cursorMode;
        Debug.Log("Changed cursor to another mode");
        UnselectCube();
    }

    void Start()
    {
        _cubePrefab = Resources.Load<GameObject>("Cube");
        _pushSpherePrefab = Resources.Load<GameObject>("PushSphere");        
    }

    void UnselectCube()
    {
        if (SelectedCube)
        {
            Debug.Log("Unselected Cube");
            _lastRotationYOfCube = SelectedCube.transform.rotation.eulerAngles.y;
            SelectedCube.Unselect();
            SelectedCube = null;
            UnselectEvent.Invoke();
        }
    }

    void SelectCube(CubeScript Cube)
    {
        SelectedCube = Cube;
        Cube.Select();        
        SelectEvent.Invoke();
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

                            Instantiate(_cubePrefab, newPosition, Quaternion.Euler(0, _lastRotationYOfCube, 0));
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

                            if (cubeWasSelected)
                            {                                
                                return;
                            }

                            UnselectCube();
                            SelectCube(cubeScript);
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
            rbOfPushedObject.AddForceAtPosition(rbOfPushedObject.transform.forward * _currentPushForce, _pointOfPush);

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
