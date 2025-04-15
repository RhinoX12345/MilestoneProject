using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wpn_Attack : MonoBehaviour
{   
    public Collider2D collider;
    public float atkDur = 2.5f;
    // Start is called before the first frame update
    void Start()
    {
        
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
        collider.enabled = true;
        for (int i=0; i<Mathf.Floor(atkDur/0.25f); i++){
            yield return new WaitForSeconds(0.25f);
        }
        collider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other){
        if (other.tag != "TerrainCollider"){
            //Debug.Log(other.gameObject.layer);
            //Debug.Log(other.tag);
            //if (other.transform.tag == "Enemies"){
            //    Debug.Log("AAAAAAAAAAAAAAA");
            //}
        }
    }

}
