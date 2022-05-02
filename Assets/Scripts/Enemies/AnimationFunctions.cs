using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    public Skeleton skeleton;

    public void HasHitted()
    {
        skeleton.HasHitted();
    }

    public void Hit()
    {
        skeleton.Hit();
    }

    public void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
