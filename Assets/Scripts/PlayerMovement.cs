using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float defaultMoveSpeed;
    public float moveSpeed;
    public float jumpForce;
    Rigidbody rb;
    float sprintSpeed;

    public bool canMove;
    public bool dead;

    public bool changeCameraDirection;
    
    public Transform cam;

    bool godMode;

    bool onGround;
    bool dashRecharged;
    bool spinRecharged;
    bool longJump;
    float normalMoveSpeed;
    Vector3 playerBottom;

    Vector3 targetRotPos;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalMoveSpeed = defaultMoveSpeed;
        canMove = true;
    }

    void Update()
    {
        //
        // DEATH
        //

        if (dead)
        {
            canMove = false;
            return;
        }
        //
        // JUMP
        //

        playerBottom = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z);
        onGround = Physics.CheckSphere(playerBottom, 0.33f, 3);

        if (Input.GetButtonDown("Jump") && onGround && !godMode)
        {
            if (!longJump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            else
            {
                print("long jump activated");
                GetComponent<ParticleSystem>().Play();
                rb.AddForce(Vector3.up * (jumpForce * 1.5f) + transform.forward * 6, ForceMode.VelocityChange);
            }
            
        }

        ConstantForce cf = GetComponent<ConstantForce>();

        if (rb.velocity.y < 0)
        {
            cf.force = new Vector3(0, rb.velocity.y, 0);
            cf.force = new Vector3(0, Mathf.Clamp(cf.force.y, -9, 0), 0);
        }
        else
        {
            cf.force = new Vector3(0, -2, 0);
        }

        //
        // DASH
        //
        Animator anim = GetComponent<Animator>();

        if (Input.GetButtonDown("Jump") && !onGround && dashRecharged && !godMode)
        {
            dashRecharged = false;
            rb.AddRelativeForce(Vector3.forward * 12, ForceMode.VelocityChange);
            anim.enabled = true;
            anim.SetTrigger("Dash");
            defaultMoveSpeed = 2.5f;
            StartCoroutine(SpinTimer());
        }

        if (onGround)
        {
            if (dashRecharged == false)
            {
                dashRecharged = true;
                print("Velocity at landing: " + rb.velocity.magnitude);

                if (rb.velocity.magnitude > 13)
                {
                    StartCoroutine(LongJumpTimer());
                }
            }
            spinRecharged = true;
            anim.enabled = false;
            defaultMoveSpeed = normalMoveSpeed;
        }

        //
        // DASH SPIN
        //
        if (dashRecharged == false && spinRecharged && !onGround && Input.GetButtonDown("Jump"))
        {
            spinRecharged = false;
            rb.velocity /= 4;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            anim.SetTrigger("Spin");
            rb.AddForce(Vector3.up * 7, ForceMode.Impulse);
            defaultMoveSpeed = normalMoveSpeed;
        }

        //
        // DEBUG
        //

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (SceneManager.GetActiveScene().name.Equals("Level1"))
                SceneManager.LoadScene("Level2");
            else
                SceneManager.LoadScene("Level1");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            godMode = !godMode;
        }

        rb.useGravity = !godMode;

        if (godMode)
        {
            rb.velocity = Vector3.zero;
            if (Input.GetButton("Jump"))
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            }
            if (Input.GetButton("Sprint"))
            {
                transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            }

            moveSpeed = defaultMoveSpeed * 2;
            if (Input.GetKey(KeyCode.LeftControl))
            {
                moveSpeed = defaultMoveSpeed * 8;
            }
        }
    }

    IEnumerator SpinTimer()
    {
        spinRecharged = false;

        yield return new WaitForSeconds(0.1f);

        spinRecharged = true;
    }

    IEnumerator LongJumpTimer()
    {
        longJump = true;

        print("long jump active");

        yield return new WaitForSeconds(0.1f);

        print("long jump disactive");

        longJump = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(playerBottom, 0.33f);
    }

    void FixedUpdate()
    {
        //
        // DEATH
        //

        if (dead)
        {
            return;
        }

        //
        // MOVEMENT
        //

        float vertSpeed = Input.GetAxis("Vertical");
        float horiSpeed = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(vertSpeed, 0, -horiSpeed);

        if (movement.magnitude >= 0.1f && canMove)
        {
            transform.Translate(movement * moveSpeed * Time.fixedDeltaTime, Space.World);
        }

        //
        // ROTATION
        //

        targetRotPos = Vector3.MoveTowards(targetRotPos, transform.position + movement * 10, 1);

        Debug.DrawLine(transform.position, targetRotPos, Color.red);

        transform.LookAt(targetRotPos);

        //
        // WALK
        //


        if (movement != Vector3.zero && Input.GetButton("Sprint"))
        {
            sprintSpeed += 0.1f;
            sprintSpeed = Mathf.Clamp(sprintSpeed, 0, 3);

            moveSpeed = defaultMoveSpeed - sprintSpeed;
        }
        else
        {
            sprintSpeed = 0;
            moveSpeed = defaultMoveSpeed;
        }
    }
}
