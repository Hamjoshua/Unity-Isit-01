using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectGhosts : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        string info = $"GameObjcet \"{other.gameObject.name}\" has fallen";
        Debug.Log(info);
    }
}
