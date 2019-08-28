using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCtrl : MonoBehaviour {


    public GameObject sparkEffect;

	void Start () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "BULLET")
        {
            //生成子弹碰撞墙体造成火花粒子
            GameObject spark=(GameObject)Instantiate(sparkEffect
                                                     ,collision.transform.position
                                                     ,Quaternion.identity);
            Destroy(spark, spark.GetComponent<ParticleSystem>().main.duration + 0.2f);
            Destroy(collision.gameObject);
        }
    }

    void Update () {
		
	}
}
