using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    Vector2 startLoc;
    float maxDist = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, startLoc)>maxDist){ Destroy(gameObject); }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Platform")
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if (other.transform.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    public void spawnProj(float md=10.0f)
    {
        transform.gameObject.SetActive(true);
        startLoc = transform.position;
        maxDist = md;
        GetComponent<Rigidbody2D>().velocity = transform.up * 10;
    }
}
