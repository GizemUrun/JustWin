using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    private Animator anim;
    private SpriteRenderer sp;
    private Vector2 inputVector, moveVector;
    private Vector3 groundCheckA, groundCheckB, ceilingCheckA, ceilingCheckB;
    private float yVel;
    public float gravity = 9.81f;
    public float jumpVel = 9.81f;
    public float climbVel = 9.81f;
    public float speed = 5f;
    public float groundCheckRadius= 0.1f;
    public LayerMask groundLayers, enemyLayer, ceilingLayers;
    bool grounded, jumpPressed, jumping, squishEnemy, extraJump, ceilinged, climbing;
    public bool laddered, wasLaddered;

    public GameObject audioObject;

    public AudioManager am;
    float sinceLastFootstep;
    float timeBetweenFootsteps = 2f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        am = FindObjectOfType<AudioManager>();
        CalculateScales();
        Manager.lastCheckPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        CalculateMovement();
        ControlAnimation();
        // print("The input vector is " + inputVector);
        // print("Laddered = " + laddered);
    }

    void GetInputs()
    {
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        jumpPressed = Input.GetButtonDown("Jump");
    }


    void ControlAnimation()
    {
        if (!Manager.gamePaused)
        {
            if (inputVector.x != 0f)
            {
                anim.SetBool("run", true);
                if (inputVector.x > 0f)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;
                }
            }
            else
            {
                anim.SetBool("run", false);
            }

            anim.SetBool("jump", jumping);
            anim.SetBool("climbing", climbing);
        } 
    }
    

    void CalculateMovement()
    {
        if (!Manager.gamePaused)
        {
            grounded = CheckCollision(groundCheckA, groundCheckB, groundLayers); //Am I touching the ground
            ceilinged = CheckCollision(ceilingCheckA, ceilingCheckB, ceilingLayers); //Have I hit the ceiling

            //print("grounded = " + grounded);

            if (jumpPressed) //if player has pressed jump button
            {
                jumpPressed = false; //set jumpPressed to false so we have jumped, but cannot jump again mid-air 
                if (grounded) // then if player is touching the ground
                {
                    jumping = true; //jumping is true
                    yVel = jumpVel; //set the yVel to the positive jump velocity
                    am.AudioTrigger(AudioManager.SoundFXCat.Jump, transform.position, 1f); //jumping audio
                }
                if (extraJump)
                {
                    extraJump = false;
                    jumping = true;
                    yVel = jumpVel;
                }
            }

            if (!grounded && yVel < 0f)
            {
                squishEnemy = CheckCollision(groundCheckA, groundCheckB, enemyLayer); //grounded, but have collided with enemy
                if (squishEnemy)
                {
                    yVel = jumpVel * 0.5f;
                    jumping = true;
                    extraJump = true;
                }
            }

            if (grounded && yVel <= 0f || ceilinged && yVel > 0f) //jumped up, either landed on the ground or hit the ceiling
            {
                yVel = 0f;
                jumping = false;
            }
            else
            {
                yVel -= gravity * Time.deltaTime;
            }


            if (laddered && !wasLaddered)
            {
                if (inputVector.y != 0f)
                {
                    climbing = true;
                    wasLaddered = true;
                }
            }

            if (wasLaddered && !laddered)
            {
                climbing = false;
                wasLaddered = false;
            }

            if (climbing)
            {
                yVel = climbVel * inputVector.y;
            }

            moveVector.y = yVel;
            moveVector.x = inputVector.x * speed;

            sinceLastFootstep += Time.deltaTime; //audio code starts here for footsteps & ladder
            if(moveVector.x != 0f && grounded)
            {
                if(sinceLastFootstep > timeBetweenFootsteps)
                {
                    sinceLastFootstep = 0f;
                    am.AudioTrigger(AudioManager.SoundFXCat.FootStepConcrete, transform.position, 0.02f);
                }
            }
            if (moveVector.y != 0f && laddered)
            {
                if (sinceLastFootstep > timeBetweenFootsteps)
                {
                    sinceLastFootstep = 0f;
                    am.AudioTrigger(AudioManager.SoundFXCat.Ladder, transform.position, 0.05f);
                }
            }
        }
    }

    bool CheckCollision(Vector3 a, Vector3 b, LayerMask l)
    {
        Collider2D colA = Physics2D.OverlapCircle(transform.position - a, groundCheckRadius, l);
        Collider2D colB = Physics2D.OverlapCircle(transform.position - b, groundCheckRadius, l);
        if (colA)
        {
            if(l == enemyLayer && yVel < 0f)
            {
                colA.gameObject.GetComponent<EnemyHealthSystem>().ReceiveHit(1);
            }
            return true;
        } else if (colB)
        {
            if (l == enemyLayer && yVel < 0f)
            {
                colB.gameObject.GetComponent<EnemyHealthSystem>().ReceiveHit(1);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    void CalculateScales ()
    {
        groundCheckA = -col.offset - new Vector2(col.size.x/2f - (groundCheckRadius * 1.2f), -col.size.y/2f); //calculates ground points of the player
        groundCheckB = -col.offset - new Vector2(- col.size.x / 2f + (groundCheckRadius * 1.2f), -col.size.y / 2f);

        ceilingCheckA = -col.offset - new Vector2(col.size.x / 2f - (groundCheckRadius * 1.2f), col.size.y / 2f); //calculates ceiling/head points of the player
        ceilingCheckB = -col.offset - new Vector2(-col.size.x / 2f + (groundCheckRadius * 1.2f), col.size.y / 2f);

    }

    private void FixedUpdate()
    {
        rb.velocity = moveVector;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - groundCheckA, groundCheckRadius); //draws spheres
        Gizmos.DrawWireSphere(transform.position - groundCheckB, groundCheckRadius);

        Gizmos.color = Color.cyan; //change colour of spheres for ceiling
        Gizmos.DrawWireSphere(transform.position - ceilingCheckA, groundCheckRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - ceilingCheckB, groundCheckRadius);
    }
}
