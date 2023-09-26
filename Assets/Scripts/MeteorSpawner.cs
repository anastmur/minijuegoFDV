using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float spawnRatePerMinute = 30f;
    public float spawnRateIncrement = 1f;

    private float spawnNext = 0;

    public float xLimit = 9;

    List<GameObject> pooledMeteors;
    public int meteorsToPool = 20;

    void Start()
    {
        pooledMeteors = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < meteorsToPool ;i++)
        {
            tmp = Instantiate(meteorPrefab);
            tmp.SetActive(false);
            pooledMeteors.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > spawnNext)
        {
            spawnNext = Time.time + 60 / spawnRatePerMinute; // Determina si hace falta crear meteorito ya
            spawnRatePerMinute += spawnRateIncrement; // incrementa el Spawn Rate en cada iteracion

            float rand = Random.Range(-xLimit, xLimit);
            Vector3 spawnPosition = new Vector2(rand,8f);

            GameObject meteor = GetPooledObject(); 
            if (meteor != null) 
            {
                meteor.transform.position = spawnPosition;
                meteor.transform.rotation = Quaternion.identity;
                meteor.SetActive(true);
            }
        }
    }

    private GameObject GetPooledObject()
    {
        for(int i = 0; i < meteorsToPool; i++)
        {
            if(!pooledMeteors[i].activeInHierarchy)
            {
                return pooledMeteors[i];
            }
        }
        return null;
    }
}
