using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenCollider : MonoBehaviour
{
    private int collisionCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool returnColliding(){
        return collisionCount>0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag!="Player"){
            collisionCount++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag!="Player"){
            collisionCount--;
        }
    }
}
