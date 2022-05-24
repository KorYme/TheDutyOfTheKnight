using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenTheChest()
    {
        animator.SetTrigger("Opening");
    }

    public void InstantiateItem()
    {
        Debug.Log("SALUT");
    }
}
