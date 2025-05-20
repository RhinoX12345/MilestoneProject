using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor_HeroKnight : MonoBehaviour {

    private int m_ColCount = 0;

    private float m_DisableTimer;
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
        return a > 0;
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

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
