using System;
using System.Collections;
using UnityEngine;

public class FactoryScript : MonoBehaviour
{
    public GameObject JeepPrefab;
    public GameObject JeepVariantPrefab;
    public GameObject SpawnPoint;
    public float SpawnDelay = 3f;
    public Action OnSpawn;

    // Start is called before the first frame update
    void Start()
    {
        OnSpawn += GameManager.Instance.AddToCounter;

        StartCoroutine("Spawn");        
    }    

    // Update is called once per frame
    void Update()
    {        
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            float randomNumber = UnityEngine.Random.Range(0f, 1f);
            GameObject prefab = randomNumber > 0.5f ? JeepPrefab : JeepVariantPrefab;
            Instantiate(prefab, SpawnPoint.transform.position, Quaternion.identity);

            OnSpawn?.Invoke();

            yield return new WaitForSeconds(SpawnDelay);
        }        
    }
}
