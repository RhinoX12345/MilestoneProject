using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wpn_Attack : MonoBehaviour
{   
    private Collider2D atkCollider;
    public float atkDur = 2.5f;
    private Rigidbody2D player;

    // Start is called before the first frame update
    void Start()
    {
        atkCollider = GetComponent<PolygonCollider2D>();
        player = transform.parent.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void attack(int direction){
        if (direction==1){
            transform.localScale = new Vector3(1,1,1);
        } else if (direction==-1){
            transform.localScale = new Vector3(-1,1,1);
        }
        StartCoroutine("colliderOn");
    }

    IEnumerator colliderOn(){
        yield return new WaitForSeconds(0.1f);
        atkCollider.enabled = true;
        for (int i=0; i<Mathf.Floor(atkDur/0.25f); i++){
            yield return new WaitForSeconds(0.25f);
        }
        atkCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.tag != "TerrainCollider"){
            //Debug.Log(other.gameObject.layer);
            //Debug.Log(other.tag);
            if (other.transform.tag == "Enemies"){
                Debug.Log("push");
                player.AddForce(new Vector2(Mathf.Sign(player.position.x - other.transform.position.x),0.1f)*3,ForceMode2D.Impulse);
            }
        }
    }

}
