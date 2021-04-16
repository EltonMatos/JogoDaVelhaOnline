using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriaPassaroUi : MonoBehaviour {

    public GameObject[] passaros;

	void Start () {
        InvokeRepeating("TiroPassaro", 2f, 2f);		
	}

    void TiroPassaro()
    {
        Instantiate(passaros[Random.Range(0,3)], transform.position, Quaternion.identity); 
    }
	
	
}
