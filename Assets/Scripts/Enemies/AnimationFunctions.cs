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
        AudioManager.instance.PlayClip("EnemySwing" + Random.Range(1,6));
    }

    public void HostFireBallCast()
    {
        host.FireBallCast();
        AudioManager.instance.PlayClip("EnemyFireBall");
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
