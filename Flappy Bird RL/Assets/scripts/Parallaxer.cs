using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxer : MonoBehaviour
{

    class PoolObject
    {
        public Transform transform;
        public bool inUse;
        public PoolObject(Transform t)
        {
            transform = t;
        }
        public void Use()
        {
            inUse = true;
        }
        public void Dispose()
        {
            inUse = false;
        }
    }

    [System.Serializable]
    public struct YSpawnRange
    {
        public float min;
        public float max;
    }

    public GameObject Prefab;
    public int poolSize;
    public float shiftSpeed;
    public float spawnRate;

    public YSpawnRange YspawnRange;
    public Vector3 defaultSpawnPos;
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    GameManager game;

    private void Awake()
    {
        Configure();
    }

    private void Start()
    {
        game = GameManager.Instance;
    }
    

    
    void OnGameOverConfirmed()
    {
        for (int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        game.GameOver = false;
        Configure();
    }
    

    private void Update()
    {
        if (game.GameOver) OnGameOverConfirmed();

        Shift();
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnRate)
        {
            Spawn();
            spawnTimer = 0;
        }
    }

    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for(int i = 0; i < poolObjects.Length; i++)
        {
            GameObject go = Instantiate(Prefab) as GameObject;
            go.SetActive(true);
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }

    }

    void Spawn()
    {
        Transform t = GetPoolObject();
        if (t == null) return;
        Vector3 pos = Vector3.zero;
        pos.x = 5;
        pos.y = UnityEngine.Random.Range(YspawnRange.min, YspawnRange.max);
        t.position = pos;
    }

    void Shift()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            poolObjects[i].transform.position -= Vector3.right * shiftSpeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }
    }

    void CheckDisposeObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x < -6.0)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    }

    Transform GetPoolObject()
    {
        for(int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }
        }
        return null;
    }
}
