using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendedGround_HeroKnight : MonoBehaviour
{
    private int m_ColCount = 0;

    private float m_DisableTimer;
    public bool grounded;
    public Sensor_HeroKnight groundSensor;
    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        List<Collider2D> array = new List<Collider2D>();
        try{
            gameObject.GetComponent<CircleCollider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), array);
        } catch {
            gameObject.GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), array);
        }
        int a = 0;
        foreach (Collider2D collider in array){
            if (collider.tag == "Platform"){
                a+=1;
            }
        }
        if (m_DisableTimer > 0)
            return false;
        return a > 0 && grounded;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Platform"){
            m_ColCount++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Platform"){
            m_ColCount--;
        }
    }

    void Start()
    {
        grounded = State();
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
        if (groundSensor.State() != grounded)
        {
            if (grounded) {
                StartCoroutine("disableJump");
            } else {
                grounded = groundSensor.State();
            }
        }
    }

    IEnumerator disableJump() {
        float totalTime = 0;
        while (totalTime < 0.01f)
        {
            totalTime += Time.deltaTime;
            Debug.Log(totalTime<0.01f);
            if (State() == groundSensor.State())
                Debug.Log(totalTime);
                grounded = groundSensor.State();
                yield break;
        }
        Debug.Log(totalTime);
        grounded = false;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
