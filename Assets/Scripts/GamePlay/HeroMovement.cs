using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    public static HeroMovement instance;
    //Instance Generation
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one HeroMovement instance in the game !");
        }
        instance = this;
    }

    //Necessary variables definition
    [field:HideInInspector]
    public Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private Animator animator;
    private bool canPlayerMove;

    //Initialization
    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
        canPlayerMove = true;
    }

    void FixedUpdate()
    {
        if (canPlayerMove)
        {
            //Déplacement horizontal et vertical à travers deux variables "input"
            float horizontalInput = Input.GetAxis("Horizontal") * HeroStats.instance.speed * Time.fixedDeltaTime;
            float verticalInput = Input.GetAxis("Vertical") * HeroStats.instance.speed * Time.fixedDeltaTime;
            Vector3 targetVelocity = new Vector2(horizontalInput, verticalInput);
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

            //Look at the the speed of the player to play idle or run animation
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        }
    }

    public void AllowMovement(bool that)
    {
        canPlayerMove = that;
    }
}
