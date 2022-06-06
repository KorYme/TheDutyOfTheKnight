using UnityEngine;

/// <summary>
/// Script used to call functions with animations
/// </summary>
public class AnimationFunctions : MonoBehaviour
{
    [Header ("Enemies GameObjects")]
    [SerializeField] private Skeleton skeleton;
    [SerializeField] private Host host;

    /// <summary>
    /// Call the HasHitted function -
    /// Called at the end of the hit animation
    /// </summary>
    public void HasHitted()
    {
        skeleton.HasHitted();
    }

    /// <summary>
    /// Call the Hit function -
    /// Called at the frame with the sword movement
    /// </summary>
    public void Hit()
    {
        skeleton.Hit();
    }

    /// <summary>
    /// Call the HostFireBallCast function -
    /// Called at the frame of the cast
    /// </summary>
    public void HostFireBallCast()
    {
        host.FireBallCast();
    }

    /// <summary>
    /// Make the host invincible - 
    /// Called at the moment the host gets in its shell
    /// </summary>
    public void HostInvicibilityTrue()
    {
        host.invulnerable = true;
    }

    /// <summary>
    /// Make the host vulnerable - 
    /// Called at the moment the host gets out of its shell
    /// </summary>
    public void HostInvicibilityFalse()
    {
        host.invulnerable = false;
    }

    /// <summary>
    /// Destroy the GameObject - 
    /// Called at the last key of the death animation
    /// </summary>
    public void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
