using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSensor_HeroKnight : MonoBehaviour
{
    private int m_ColCount = 0;
    public float coyoteSetting = 0.2f;
    public float coyote = 0;

    private float m_DisableTimer;
    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool actualState()
    {
        List<Collider2D> array = new List<Collider2D>();
        gameObject.GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D().NoFilter(), array);
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
    
    public bool State()
    {
        return actualState() || coyote>0;
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
        if (actualState() == true)
        {
            coyote = coyoteSetting;
        }
        else
        {
            coyote -= Time.deltaTime;
        }  
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
