using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        StartCoroutine(FlipSprite());
    }

    IEnumerator FlipSprite()
    {
        yield return new WaitForSeconds(4f);
        sprite.flipX = !sprite.flipX;
        StartCoroutine(FlipSprite());
    }

}
