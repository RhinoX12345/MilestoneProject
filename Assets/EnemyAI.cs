using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D self;
    private GenCollider colliderBLeft;
    private GenCollider colliderBRight;
    private GenCollider colliderTLeft;
    private GenCollider colliderTRight;
    private int facing;
    private bool Turn = false;
    public float spd = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<Rigidbody2D>();
        colliderBLeft = transform.Find("ColliderBL").GetComponent<GenCollider>();
        colliderBRight = transform.Find("ColliderBR").GetComponent<GenCollider>();
        colliderTLeft = transform.Find("ColliderTL").GetComponent<GenCollider>();
        colliderTRight = transform.Find("ColliderTR").GetComponent<GenCollider>();
        if (Random.value < 0.5f){
            facing = 1;
        } else {
            facing = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (colliderBRight.returnColliding()!=colliderBLeft.returnColliding()){
            Debug.Log("Bottom");
            Turn = true;
        } else if (colliderTRight.returnColliding()!=colliderTLeft.returnColliding()) {
            Debug.Log("Top");
            Turn = true;
        } else {
            Turn = false;
        }
        if (Turn){
            facing*=-1;
        }
        self.velocity = new Vector2(facing * spd, self.velocity.y);
    }
}
