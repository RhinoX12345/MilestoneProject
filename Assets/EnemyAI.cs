using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D self;
    private Collider2D hitbox;
    private GenCollider colliderBLeft;
    private GenCollider colliderBRight;
    private GenCollider colliderTLeft;
    private GenCollider colliderTRight;
    private int facing;
    private bool Turn = false;
    private bool Turned = false;
    public float spd = 4.0f;
    public int health = 3;
    public bool invincible = false;
    public float knockback = 5.0f;
    public bool move = true;

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

        hitbox = gameObject.GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Turned){
            Turned = colliderBRight.returnColliding()!=colliderBLeft.returnColliding();
        } else {
            if (colliderBRight.returnColliding()!=colliderBLeft.returnColliding()){
                //Debug.Log("Bottom");
                Turn = true;
            } else if (colliderTRight.returnColliding()!=colliderTLeft.returnColliding()) {
                //Debug.Log("Top");
                Turn = true;
            }
        }
        if (Turn){
            facing*=-1;
            Turn = false;
            Turned = true;
        }
        if (move){
            self.velocity = new Vector2(facing * spd, self.velocity.y);
        }

        List<Collider2D> array = new List<Collider2D>();
        hitbox.OverlapCollider(new ContactFilter2D().NoFilter(), array);
        foreach (Collider2D collider in array){
            //if (collider.name == "HeroKnight" || collider.name == "Slash_1" || collider.name == "Slash_2" || collider.name == "Slash_3"){
            //    Debug.Log(collider.name);
            //}
            if ((collider.name == "Slash_1" || collider.name == "Slash_2" || collider.name == "Slash_3")&&!invincible){
                StartCoroutine("stun");
                self.velocity = new Vector2(Mathf.Sign(self.position.x - collider.transform.parent.position.x)*knockback,self.velocity.y+1);
                Damage(1);
            }

        }
    }

    public void Damage(int n){
        if (!invincible){
            health-=n;
            if (health>0){
                StartCoroutine("iframes");
            } else {
                transform.gameObject.SetActive(false);
                //transform.gameObject.Destroy();
            }
        }
    }

    IEnumerator iframes(){
        invincible = true;
        yield return new WaitForSeconds(0.3f);
        invincible = false;
    }

    IEnumerator stun(){
        move = false;
        yield return new WaitForSeconds(0.2f);
        move = true;
    }
}
