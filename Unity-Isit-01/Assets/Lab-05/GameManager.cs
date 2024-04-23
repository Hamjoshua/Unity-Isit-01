using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    [SerializeField]
    private float _counter;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            return _instance;
        }
    }
    public void AddToCounter()
    {
        _counter += 1;
    }
    public void RemoveFromCounter()
    {
        _counter -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
