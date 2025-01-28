using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector2 OriginalOffset;
    public float spd = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        OriginalOffset = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(player.transform.position.x,player.transform.position.y+OriginalOffset.y,this.transform.position.z);
        float step = spd * Time.deltaTime;

        this.transform.position = Vector3.MoveTowards(this.transform.position, target, step);
    }
}
