using System.Collections.Generic;
using UnityEngine;
using System.Collections;
public class HeroKnight : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private GroundSensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    public bool m_isWallTouchR = false;
    public bool m_isWallTouchL = false;
    private bool m_grounded = false;
    private bool m_coyote = false;
    public bool m_rolling = false;
    public int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;

    private float wallJumpTime = 0.0f;

    public FollowPlayer Cam;
    private Wpn_Attack slash1;
    private Wpn_Attack slash2;
    private Wpn_Attack slash3;

    public HudManager hud;
    public int health = 10;
    private bool died = false;
    public bool invincible = false;
    public float invincibleTime = 1.0f;
    public float knockback = 5.0f;
    public bool controlEnabled = true;
    public Vector2 lastCheckpointPos = new Vector2(0, 0);
    public float ySpd;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<GroundSensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        slash1 = transform.Find("Slash_1").GetComponent<Wpn_Attack>();
        slash2 = transform.Find("Slash_2").GetComponent<Wpn_Attack>();
        slash3 = transform.Find("Slash_3").GetComponent<Wpn_Attack>();
    }

    // Update is called once per frame
    void Update()
    {
        //Background Texture, Enemy Sprite & multiple enemies



        hud.updateHealth();
        if (health <= 0)
        {
            controlEnabled = false;
            died = true;
        }

        if (wallJumpTime > 0){wallJumpTime -= Time.deltaTime;}else{wallJumpTime = 0;}
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;
        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;
        // Disable rolling if timer extends duration
        if (m_rolling && m_rollCurrentTime > m_rollDuration)
        {
            if (m_grounded)
            {
                m_body2d.velocity = new Vector2(0, 0);
            }
            m_rolling = false;
            invincible = false;
        }
        if (!died)
        {
            //Check if character just landed on the ground
            if (!m_grounded && m_groundSensor.actualState())
            {
                m_grounded = true;
                m_coyote = true;
                m_animator.SetBool("Grounded", m_grounded);
            }
            //Check if character just started falling
            if (m_grounded && !m_groundSensor.actualState())
            {
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
            }
            ySpd = m_body2d.velocity.y;
            //Set AirSpeed in animator
            m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);



            //Debug Keys
            if (Input.GetKeyDown("q") && !m_rolling)
                damage(1);
            if (Input.GetKeyDown("e") && !m_rolling)
                damage(-1);
            if (Input.GetKeyDown("r"))
                respawn();



            // -- Handle input and movement --
            if (controlEnabled)
            {
                float inputX = Input.GetAxis("Horizontal");

                //Wall Slide
                m_isWallTouchR = m_wallSensorR1.State() || m_wallSensorR2.State();
                m_isWallTouchL = m_wallSensorL1.State() || m_wallSensorL2.State();

                //Debug.LogFormat("WallslideAnim On/Off: {0}", (m_isWallTouchR || m_isWallTouchL) && !m_grounded && ySpd<0 && !m_rolling);
                m_animator.SetBool("WallSlide", (m_isWallTouchR || m_isWallTouchL) && !m_grounded && ySpd<0 && !m_rolling);

                if (m_isWallTouchL && !m_isWallTouchR && !m_grounded && inputX > 0)
                {
                    m_animator.SetBool("WallSlide", false);
                    m_facingDirection = 1;
                    GetComponent<SpriteRenderer>().flipX = false;
                    m_wallSensorL1.Disable(0.1f);
                    m_wallSensorL2.Disable(0.1f);
                }
                else if (m_isWallTouchR && !m_isWallTouchL && !m_grounded && inputX < 0)
                {
                    m_animator.SetBool("WallSlide", false);
                    m_facingDirection = -1;
                    GetComponent<SpriteRenderer>().flipX = true;
                    m_wallSensorR1.Disable(0.1f);
                    m_wallSensorR2.Disable(0.1f);
                }


                if (m_animator.GetBool("Attacking") == false && wallJumpTime <= 0)
                {
                    if (inputX > 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                        m_facingDirection = 1;
                    }
                    else if (inputX < 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                        m_facingDirection = -1;
                    }
                }


                if (m_rolling && !(m_isWallTouchR || m_isWallTouchL))
                {
                    m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
                }


                //Jump
                if (!m_grounded && m_coyote)
                {
                    m_coyote = m_groundSensor.State();
                }
                if (Input.GetKeyDown("space") && m_coyote)
                {
                    m_animator.SetTrigger("Jump");
                    m_grounded = false;
                    m_coyote = false;
                    m_animator.SetBool("Grounded", m_grounded);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_body2d.velocity.y / 3 + m_jumpForce);
                    m_groundSensor.Disable(0.2f);
                    if (m_rolling)
                    {
                        m_rollCurrentTime = 0;
                        m_rolling = false;
                        invincible = false;
                    }
                }
                //Walljump
                else if (Input.GetKeyDown("space") && m_isWallTouchR && !m_grounded && !m_rolling)
                {
                    m_animator.SetTrigger("Jump");
                    // m_body2d.velocity = new Vector2(m_body2d.velocity.x - m_jumpForce / 2, m_body2d.velocity.y / 3 + m_jumpForce / 2);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x - m_jumpForce*2/3, m_jumpForce*2/3);
                    wallJumpTime = 0.2f;
                }
                else if (Input.GetKeyDown("space") && m_isWallTouchL && !m_grounded && !m_rolling)
                {
                    m_animator.SetTrigger("Jump");
                    // m_body2d.velocity = new Vector2(m_body2d.velocity.x + m_jumpForce / 2, m_body2d.velocity.y / 2 + m_jumpForce / 2);
                    m_body2d.velocity = new Vector2(m_body2d.velocity.x + m_jumpForce*2/3, m_jumpForce*2/3);
                    wallJumpTime = 0.2f;
                }
                // Move
                if (!m_rolling)
                {
                    if (!m_isWallTouchL && inputX < 0 && wallJumpTime <= 0.0f)
                    {
                        if (m_body2d.velocity.x >= -m_speed)
                        {
                            m_body2d.velocity = new Vector2(
                                Mathf.Max(m_body2d.velocity.x + inputX * m_speed * 0.5f, inputX * m_speed),
                                m_body2d.velocity.y
                                );
                        }
                        else
                        {
                            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_body2d.velocity.y);
                        }
                    }
                    if (!m_isWallTouchR && inputX > 0 && wallJumpTime <= 0.0f)
                    {
                        if (m_body2d.velocity.x <= m_speed)
                        {
                            m_body2d.velocity = new Vector2(
                                Mathf.Min(m_body2d.velocity.x + inputX * m_speed * 0.5f, inputX * m_speed),
                                m_body2d.velocity.y
                                );
                        }
                        else
                        {
                            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_body2d.velocity.y);
                        }
                    }
                    // Roll
                    if (Input.GetKeyDown("left shift"))
                    {
                        m_rolling = true;
                        invincible = true;
                        m_rollCurrentTime = 0.0f;
                        m_animator.SetTrigger("Roll");
                        m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, Mathf.Min(0.1f, m_body2d.velocity.y));
                    }
                    //Attack
                    else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f)
                    {
                        m_animator.SetBool("Attacking", true);
                        m_currentAttack++;

                        // Loop back to one after third attack
                        if (m_currentAttack > 3)
                            m_currentAttack = 1;

                        // Reset Attack combo if time since last attack is too large
                        if (m_timeSinceAttack > 1.0f)
                            m_currentAttack = 1;
                        float atkDur = 0;
                        switch (m_currentAttack)
                        {
                            case 1:
                                slash1.attack(m_facingDirection);
                                atkDur = slash1.atkDur;
                                break;
                            case 2:
                                slash2.attack(m_facingDirection);
                                atkDur = slash2.atkDur;
                                break;
                            case 3:
                                slash3.attack(m_facingDirection);
                                atkDur = slash3.atkDur;
                                break;
                        }
                        IEnumerator atkAnimation = attacking(atkDur);
                        StartCoroutine(atkAnimation);
                        // Call one of three attack animations "Attack1", "Attack2", "Attack3"
                        m_animator.SetTrigger("Attack" + m_currentAttack);

                        // Reset timer
                        m_timeSinceAttack = 0.0f;
                    }
                    // Block
                    else if (Input.GetMouseButtonDown(1))
                    {
                        m_animator.SetTrigger("Block");
                        m_animator.SetBool("IdleBlock", true);
                    }
                    else if (Input.GetMouseButtonUp(1))
                    {
                        m_animator.SetBool("IdleBlock", false);
                    }
                    //Run
                    if (Mathf.Abs(inputX) > Mathf.Epsilon)
                    {
                        // Reset timer
                        m_delayToIdle = 0.05f;
                        m_animator.SetInteger("AnimState", 1);
                    }
                    //Idle
                    else
                    {
                        // Prevents flickering transitions to idle
                        m_delayToIdle -= Time.deltaTime;
                        if (m_delayToIdle < 0)
                            m_animator.SetInteger("AnimState", 0);
                    }
                }
                //Idle
                else
                {
                    // Prevents flickering transitions to idle
                    m_delayToIdle -= Time.deltaTime;
                    if (m_delayToIdle < 0)
                        m_animator.SetInteger("AnimState", 0);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown("space"))
            {
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
    IEnumerator attacking(float dur)
    {
        yield return new WaitForSeconds(dur);
        m_animator.SetBool("Attacking", false);
    }

    public void damage(int dmgAmnt)
    {
        if (!invincible)
        {
            health -= dmgAmnt;
            if (health > 0)
            {
                StartCoroutine("iframes");
            }
            else if (!died)
            {
                m_animator.SetBool("noBlood", m_noBlood);
                m_animator.SetTrigger("Death");
            }
        }
    }


    IEnumerator iframes()
    {
        invincible = true;
        StartCoroutine("stun");
        hud.changeInvincibleState(invincible);
        int loopDuration = (int)Mathf.Round(invincibleTime / 0.3f);
        for (int i = 0; i < loopDuration; i++)
        {
            m_animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(0.3f);
        }
        invincible = false;
        hud.changeInvincibleState(invincible);
    }

    IEnumerator stun()
    {
        controlEnabled = false;
        yield return new WaitForSeconds(0.2f);
        controlEnabled = true;
    }

    void respawn()
    {
        m_animator.ResetTrigger("Death");
        m_animator.SetTrigger("Respawn");
        transform.position = lastCheckpointPos;
        health = 10;
        died = false;
        controlEnabled = true;
        Cam.reset();
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        Vector2 save = trigger.transform.position;
        //Debug.Log(trigger.gameObject.layer);
        if (trigger.tag == "Checkpoint")
        {
            lastCheckpointPos = save;

        }
        else if (trigger.tag == "Finish")
        {
            Debug.Log("Finish");
        }
        else if (trigger.gameObject.layer == 8)
        {
            if (trigger.tag == "Enemies")
            {
                if (!died && !m_rolling && !invincible)
                {
                    // m_body2d.velocity = new Vector2(Mathf.Sign(m_body2d.position.x - trigger.transform.parent.position.x) * knockback, m_body2d.velocity.y + 1);
                    m_body2d.velocity = new Vector2(Mathf.Sign(m_body2d.position.x - save.x) * knockback, m_body2d.velocity.y + 1);
                    damage(1);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log(other.transform.gameObject.layer);
        if (other.transform.gameObject.layer == 8)
        {
            if (other.transform.tag == "Enemies")
            {
                damage(1);
                Debug.Log("AAAAAAAAAAAAAAA");
            }
        }
    }

}
