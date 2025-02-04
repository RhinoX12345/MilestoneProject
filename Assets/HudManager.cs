using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    public TMP_Text invicibility;
    public TMP_Text healthText;
    public HeroKnight player;
    // Start is called before the first frame update
    void Start()
    {
        healthText.text = "Health: "+ player.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeInvincibleState(bool state){
        invicibility.text = "Invicible: "+ state.ToString();
    }

    public void updateHealth(){
        healthText.text = "Health: "+ player.health;
    }
}
