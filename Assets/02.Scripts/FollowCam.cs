using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    public Transform targetTrans;
    public float dist = 10.0f;
    public float height = 3.0f;
    public float dampTrace = 20.0f;

    private Transform tr;

	void Start () {
        tr = GetComponent<Transform>();
	}
	

	void Update () {

        tr.position 
            = Vector3.Lerp(tr.position,
            targetTrans.position - (targetTrans.forward * dist) + (Vector3.up * height)
            , Time.deltaTime * dampTrace);

        tr.LookAt(targetTrans.position);
	}
}
