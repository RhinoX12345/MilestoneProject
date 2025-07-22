using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPlayer : MonoBehaviour
{
    private GameObject projectile;
    public GameObject target;
    public float shootDelay = 1.0f;
    private float cooldown = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        projectile = transform.Find("Projectile").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0.0f)
        {
            cooldown -= Time.deltaTime;
        }
        else
        {
            Vector2 tPos = target.transform.position;
            Vector2 sPos = transform.position;
            float deltaX = tPos.x - sPos.x;
            float deltaY = tPos.y - sPos.y;
            float newAngle = Mathf.Rad2Deg * Mathf.Atan(deltaY / deltaX)+90;
            if (deltaX<0){ newAngle = 180 + newAngle; } else if (deltaY<0){ newAngle = 360 + newAngle; }
            transform.Rotate(0.0f,0.0f,newAngle-transform.eulerAngles.z);
            Debug.Log(transform.eulerAngles.z);
        }
    }
}
