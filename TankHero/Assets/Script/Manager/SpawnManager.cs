using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance = null;
    
    public List<Transform> spawns = new List<Transform>();

    public float spawnTime;

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
        }

        StartCoroutine(Spawn());
    }

    public void SpawnStart()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);

        int random = Random.Range(0, spawns.Count);
        var clone = ObjectPoolManager.Instance.GetEnemy();
        clone.transform.position = spawns[random].transform.position;

        StartCoroutine(Spawn());
    }
    
    
}
