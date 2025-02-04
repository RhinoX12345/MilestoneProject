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
        OriginalOffset = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float camX = this.transform.position.x;
        float camY = this.transform.position.y;
        float playerX = player.transform.position.x;
        float playerY = player.transform.position.y;
        float dist = Vector2.Distance(new Vector2(camX,camY), new Vector2(playerX,playerY));
        float step = defaultSpd * Time.deltaTime * Mathf.Log10(dist);
        Vector3 target = new Vector3(playerX,playerY+OriginalOffset.y,this.transform.position.z);
        this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);
    }
}
