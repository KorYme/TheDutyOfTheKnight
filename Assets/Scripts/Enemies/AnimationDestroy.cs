using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDestroy : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
