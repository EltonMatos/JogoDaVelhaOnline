using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorteArea : MonoBehaviour {

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("pass"))
        {
            Destroy(col.gameObject);
        }
    }
}
