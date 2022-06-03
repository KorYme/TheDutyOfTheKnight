using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;
    private bool canBeOpen;
    private bool isInRange;
    public InputData inputData;
    private GameObject sprinkleGameobject;

    void Start()
    {
        animator = GetComponent<Animator>();
        canBeOpen = false;
        sprinkleGameobject = transform.Find("ParticleSystem").gameObject;
        sprinkleGameobject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(inputData.interact) && isInRange && canBeOpen)
        {
            OpenTheChest();
        }
    }

    public void CanBeOpen(bool that)
    {
        canBeOpen = that;
        sprinkleGameobject.SetActive(that);
        tag = that ? "Chest" : "Untagged";
    }

    public void OpenTheChest()
    {
        animator.SetTrigger("Opening");
        AudioManager.instance.PlayClip("Chest");
        Interaction_Player.instance.ForceExit();
        CanBeOpen(false);
    }

    public void InstantiateItem()
    {
        Instantiate(LootManager.instance.GetALoot(), transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Coordinates")
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Coordinates")
        {
            isInRange = false;
        }
    }
}
