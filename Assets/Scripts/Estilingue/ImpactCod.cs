using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactCod : MonoBehaviour
{
    private int limite;
    private SpriteRenderer spriteR;
    [SerializeField]
    private Sprite[] sprites;


    void Start()
    {
        limite = 0;
        spriteR = GetComponent<SpriteRenderer>();
        spriteR.sprite = sprites[0];
    }
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.relativeVelocity.magnitude > 4 && collision.relativeVelocity.magnitude < 10)
        {
            if(limite < sprites.Length - 1)
            {
                limite++;
                spriteR.sprite = sprites[limite];
            }
            else if(limite == sprites.Length - 1)
            {
                Destroy(gameObject);
            }
        }
        else if(collision.relativeVelocity.magnitude > 12 && collision.gameObject.CompareTag("birdPlayer"))
        {
            Destroy(gameObject);
        }
    }
}
