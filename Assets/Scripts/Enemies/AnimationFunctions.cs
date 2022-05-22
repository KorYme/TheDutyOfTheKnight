using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFunctions : MonoBehaviour
{
    [SerializeField] public Skeleton skeleton;
    [SerializeField] public Host host;

    public void HasHitted()
    {
        skeleton.HasHitted();
    }

    public void Hit()
    {
        skeleton.Hit();
    }

    public void HostFireBallCast()
    {
        host.FireBallCast();
    }

    public void HostInvicibilityTrue()
    {
        host.invulnerable = true;
    }

    public void HostInvicibilityFalse()
    {
        host.invulnerable = false;
    }

    public void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
