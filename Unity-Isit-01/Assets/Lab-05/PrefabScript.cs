using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabScript : MonoBehaviour
{
    public float Speed = 1f;
    public float TimeToLive = 5f;
    void Start()
    {
        Destroy(gameObject, TimeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        if (this)
        {
            transform.position += Vector3.forward * Speed;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveFromCounter();
    }
}
