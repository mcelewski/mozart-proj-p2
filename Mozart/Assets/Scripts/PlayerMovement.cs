using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D Controller;
	public Animator Animator;
    private GameMaster GameMaster;
    public Rigidbody2D rb;    

	public float runSpeed = 7.5f;

    private float inputHorizontal;
    private float inputVertical;
    public float distance;
    public LayerMask whatisLadder;
    private bool isClimbing;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;


    void Start()
    {
        GameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		Animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			Animator.SetBool("IsJumping", true);
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
            runSpeed = 3.75f;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
            runSpeed = 7.5f;
		}

	}

	public void OnLanding ()
	{
		Animator.SetBool("IsJumping", false);
	}

	public void OnCrouching (bool isCrouching)
	{
		Animator.SetBool("IsCrouching", isCrouching);
	}

	void FixedUpdate ()
	{
		// Move our character
		Controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * runSpeed, rb.velocity.y);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatisLadder);

        if (hitInfo.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                isClimbing = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                isClimbing = false;
            }
        }

        if (isClimbing == true && hitInfo.collider != null)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * runSpeed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 3;
        }
        
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            Destroy(collision.gameObject);
            GameMaster.points += 1;
        }
    }
}
