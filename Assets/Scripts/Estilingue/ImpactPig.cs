using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactPig : MonoBehaviour
{
       
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > 4 && collision.relativeVelocity.magnitude < 10 && collision.gameObject.CompareTag("birdPlayer"))
        {
            Destroy(gameObject);            
        }
        else if (collision.relativeVelocity.magnitude > 12 && collision.gameObject.CompareTag("birdPlayer"))
        {            
            Destroy(gameObject);
        }
    }
}
