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
        spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        foreach (GameObject spawner in spawners) {
            Instantiate(enemy, spawner.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("s")){
            spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
            foreach (GameObject spawner in spawners) {
                Instantiate(enemy, spawner.transform);
            }
        }
    }
}
