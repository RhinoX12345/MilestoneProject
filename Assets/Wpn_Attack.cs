using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wpn_Attack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float atkDur = 3.0f;

    public void attack(int direction){
        IEnumerator coroutine = sleep(atkDur);
        if (direction==1){
            transform.localScale = new Vector3(1,1,1);
        } else if (direction==-1){
            transform.localScale = new Vector3(-1,1,1);
        }
        
        this.transform.gameObject.SetActive(true);
        StartCoroutine(coroutine);

    }

    IEnumerator sleep(float dur){
        for (int i=0; i<2; i++){
            yield return new WaitForSeconds(0.25f);
        }
        this.transform.gameObject.SetActive(false);
    }
}
