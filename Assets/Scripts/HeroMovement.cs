using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    //Necessary variables definition
    public HeroStats HeroStats;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private Animator animator;

    public static HeroMovement instance;

    //Instance
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one HeroMovement instance in the game !");
        }
        instance = this;
    }

    //Initialization
    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Déplacement horizontal et vertical à travers deux variables "input"
        float horizontalInput = Input.GetAxis("Horizontal") * HeroStats.speed * Time.fixedDeltaTime;
        float verticalInput = Input.GetAxis("Vertical") * HeroStats.speed * Time.fixedDeltaTime;
        Vector3 targetVelocity = new Vector2(horizontalInput, verticalInput);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        //Définit si le héros possède une vitesse
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
    }


}
