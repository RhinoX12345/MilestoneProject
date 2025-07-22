using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D p_body2d;
    private Vector2 OriginalOffset;
    public float defaultSpd = 2.0f;

    private float xVel = 0.0f;
    private float yVel = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        OriginalOffset = new Vector2(0,1.5f);
        p_body2d = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        move3();
    }

    private void move1()
    {
        float camX = this.transform.position.x;
        float camY = this.transform.position.y;
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;


        //float targetX = Mathf.Round((playerX - camX)/2*1000)/1000;
        //float targetX = Mathf.Lerp(camX, playerX, Mathf.Clamp(Mathf.Round(Mathf.Abs(playerX-camX)*10)/100,0.95f,1));
        float targetX = Mathf.Lerp(camX, playerX+p_body2d.velocity.x+OriginalOffset.x, 1);
        if (Mathf.Abs(playerX-targetX)<0.05){targetX=playerX;}
        float targetY = playerY+(p_body2d.velocity.y*0.6f)+OriginalOffset.y;

        float dist = Vector2.Distance(new Vector2(camX,camY), new Vector2(targetX,targetY));
        float step = defaultSpd * Time.deltaTime * dist/1.5f;

        Vector3 target = new Vector3(targetX,targetY,this.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);
    }

    private void move2()
    {
        float camX = this.transform.position.x;
        float camY = this.transform.position.y;
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        float targetX = Mathf.SmoothDamp(camX, playerX + p_body2d.velocity.x + OriginalOffset.x, ref xVel, 1.0f);
        float targetY = Mathf.SmoothDamp(camY, playerY + p_body2d.velocity.y + OriginalOffset.y, ref yVel, 1.0f);

        Vector3 target = new Vector3(targetX, targetY, this.transform.position.z);
        this.transform.position = target;
    }

    private void move3()
    {
        Vector2 camPos = new Vector2(transform.position.x, transform.position.y);
        Vector2 playerPos = new Vector2(player.transform.position.x + OriginalOffset.x, player.transform.position.y + OriginalOffset.y);
        Vector2 newPos = Vector2.Lerp(camPos, playerPos, 0.2f);
        // Debug.Log(newPos);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    public void reset()
    {
        gameObject.transform.position = new Vector3(player.transform.position.x + OriginalOffset.x, player.transform.position.y + OriginalOffset.y, gameObject.transform.position.z);
    }
}
