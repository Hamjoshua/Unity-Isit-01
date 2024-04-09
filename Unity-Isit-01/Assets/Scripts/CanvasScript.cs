using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    [SerializeField]
    public void ChangeCursorMode(Dropdown change)
    {
        GameManager.SingleInstance.ChangeCursorMode(change.value);
    }
}
