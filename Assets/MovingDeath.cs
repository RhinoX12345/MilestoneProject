using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDeath : MonoBehaviour
{
    public GameObject player;
    private HeroKnight playerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<HeroKnight>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(player.transform.position.x, gameObject.transform.position.y);
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.transform.tag == "Player"){
            playerScript.damage(99999);
        }
        if (other.transform.tag == "Enemies"){
            other.gameObject.GetComponent<EnemyAI>().Damage(99999);
        }
    }
}
