using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector2 OriginalOffset;
    public float defaultSpd = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        OriginalOffset = new Vector2(0,1.5f);

    }

    // Update is called once per frame
    void Update()
    {
        float camX = this.transform.position.x;
        float camY = this.transform.position.y;
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;

        float dist = Vector2.Distance(new Vector2(camX,camY), new Vector2(playerX,playerY)+OriginalOffset);
        float step = defaultSpd * Time.deltaTime * dist/1.5f;

        //float targetX = Mathf.Round((playerX - camX)/2*1000)/1000;
        //float targetX = Mathf.Lerp(camX, playerX, Mathf.Clamp(Mathf.Round(Mathf.Abs(playerX-camX)*10)/100,0.95f,1));
        float targetX = Mathf.Lerp(camX, playerX, 1);
        if (Mathf.Abs(playerX-targetX)<0.05){targetX=playerX;}

        Vector3 target = new Vector3(targetX,playerY+OriginalOffset.y,this.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);
    }

    public void reset(){
        gameObject.transform.position = new Vector3(player.transform.position.x+OriginalOffset.x,player.transform.position.y+OriginalOffset.y, gameObject.transform.position.z);
    }
}
