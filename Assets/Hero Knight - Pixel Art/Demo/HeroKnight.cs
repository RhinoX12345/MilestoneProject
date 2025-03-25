using UnityEngine;
using System.Collections;
public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] float      m_rollForce = 6.0f;
    [SerializeField] bool       m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private bool                m_rolling = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;
    private float               m_delayToIdle = 0.0f;
    private float               m_rollDuration = 8.0f / 14.0f;
    private float               m_rollCurrentTime;
    
    private Wpn_Attack slash1;
    private Wpn_Attack slash2;
    private Wpn_Attack slash3;

    public HudManager hud;
    public int health = 10;
    private bool died = false;
    private bool invincible = false;
    public float invincibleTime = 1.0f;
    public bool controlEnabled = true;
    public Vector2 lastCheckpointPos = new Vector2(0,0);

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        slash1 = transform.Find("Slash_1").GetComponent<Wpn_Attack>();
        slash2 = transform.Find("Slash_2").GetComponent<Wpn_Attack>();
        slash3 = transform.Find("Slash_3").GetComponent<Wpn_Attack>();
    }

    // Update is called once per frame
    void Update ()
    {
        hud.updateHealth();
        if (health <= 0){
            controlEnabled = false;
            died = true;
        }
        

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        // Increase timer that checks roll duration
        if(m_rolling)
            m_rollCurrentTime += Time.deltaTime;
        // Disable rolling if timer extends duration
        if(m_rollCurrentTime > m_rollDuration)
            m_rolling = false;
        if (!died){
            //Check if character just landed on the ground
            if (!m_grounded && m_groundSensor.State())
            {
                m_grounded = true;
                m_animator.SetBool("Grounded", m_grounded);
            }
            //Check if character just started falling
            if (m_grounded && !m_groundSensor.State())
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }

            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);
            //Wall Slide
            m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
            m_animator.SetBool("WallSlide", m_isWallSliding);
            

            //Hurt
            if (Input.GetKeyDown("q") && !m_rolling){
                damage(1);
            }

            // -- Handle input and movement --
            
            if (controlEnabled){
                // Swap direction of sprite depending on walk direction
                float inputX = Input.GetAxis("Horizontal");
                if (inputX > 0){
                    GetComponent<SpriteRenderer>().flipX = false;
                    m_facingDirection = 1;
                }else if (inputX < 0){
                    GetComponent<SpriteRenderer>().flipX = true;
                    m_facingDirection = -1;
                }
                // Move
                if (!m_rolling){
                    m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
                    // Roll
                    if (Input.GetKeyDown("left shift") && !m_isWallSliding)
                    {
                        m_rolling = true;
                        m_animator.SetTrigger("Roll");
                        m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
                    }
                    //Jump
                    else if (Input.GetKeyDown("space") && m_grounded){
                        m_animator.SetTrigger("Jump");
                        m_grounded = false;
                        m_animator.SetBool("Grounded", m_grounded);
                        m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                        m_groundSensor.Disable(0.2f);
                    }
                    //Attack
                    else if(Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
                    {
                        m_currentAttack++;

                        // Loop back to one after third attack
                        if (m_currentAttack > 3)
                            m_currentAttack = 1;

                        // Reset Attack combo if time since last attack is too large
                        if (m_timeSinceAttack > 1.0f)
                            m_currentAttack = 1;
                        switch (m_currentAttack){
                            case 1:slash1.attack(m_facingDirection);break;
                            case 2:slash2.attack(m_facingDirection);break;
                            case 3:slash3.attack(m_facingDirection);break;
                        }
                        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                        m_animator.SetTrigger("Attack" + m_currentAttack);

                        // Reset timer
                        m_timeSinceAttack = 0.0f;
                    }
                    // Block
                    else if (Input.GetMouseButtonDown(1)){
                        m_animator.SetTrigger("Block");
                        m_animator.SetBool("IdleBlock", true);
                    }else if (Input.GetMouseButtonUp(1)){
                        m_animator.SetBool("IdleBlock", false);
                    }
                    //Run
                    if (Mathf.Abs(inputX) > Mathf.Epsilon){
                        // Reset timer
                        m_delayToIdle = 0.05f;
                        m_animator.SetInteger("AnimState", 1);
                    }
                    //Idle
                    else{
                        // Prevents flickering transitions to idle
                        m_delayToIdle -= Time.deltaTime;
                            if(m_delayToIdle < 0)
                                m_animator.SetInteger("AnimState", 0);
                    }
                }
                //Idle
                else{
                    // Prevents flickering transitions to idle
                    m_delayToIdle -= Time.deltaTime;
                        if(m_delayToIdle < 0)
                            m_animator.SetInteger("AnimState", 0);
                }
            }
        }
        else {
            if (Input.GetKeyDown("space")){
                respawn();
            }
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    void damage(int dmgAmnt){
        if (!invincible){
            health-=dmgAmnt;
            if (health>0){
                StartCoroutine("iframes");
            } else if (!died){
                m_animator.SetBool("noBlood", m_noBlood);
                m_animator.SetTrigger("Death");
            }
        }
    }

    IEnumerator iframes(){
        invincible = true;
        hud.changeInvincibleState(invincible);
        int loopDuration = (int)Mathf.Round(invincibleTime/0.3f);
        for (int i=0; i<loopDuration;i++){
            m_animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.3f);
        }
        invincible = false;
        hud.changeInvincibleState(invincible);
    }

    void respawn(){
        m_animator.ResetTrigger("Death");
        m_animator.SetTrigger("Respawn");
        transform.position = lastCheckpointPos;
        health = 10;
        died = false;
        controlEnabled = true;
    }

    void OnTriggerEnter2D(Collider2D trigger){
        if (trigger.tag == "Checkpoint"){
            lastCheckpointPos = trigger.transform.position;
        }else if (trigger.tag == "Enemies"){
            if (!died && !m_rolling){
                damage(1);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if (other.transform.tag == "Enemies"){
            //Debug.Log("AAAAAAAAAAAAAAA");
        }
    }


}
