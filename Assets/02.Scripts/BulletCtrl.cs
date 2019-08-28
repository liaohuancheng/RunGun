using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour {
    public int damage = 20;
    public float speed = 1000.0f;
    Rigidbody rigi;


	void Start () {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
	}

	void Update () {
        
	}
}
