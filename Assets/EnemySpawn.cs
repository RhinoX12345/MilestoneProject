using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject[] spawners;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(123123);
        spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        foreach (GameObject spawner in spawners) {
            GameObject obj = spawner.GetComponent<SpawnObject>().obj;
            Instantiate(obj, spawner.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m")){
            spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
            foreach (GameObject spawner in spawners) {
                GameObject obj = spawner.GetComponent<SpawnObject>().obj;
                Instantiate(obj, spawner.transform);
            }
        }
    }
}
