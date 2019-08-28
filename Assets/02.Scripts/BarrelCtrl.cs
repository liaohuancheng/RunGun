using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour {

    public GameObject expEffect;
    private Transform tr;

    public Texture[] textures;

    private int hitcount = 0;

	void Start () {
        tr = GetComponent<Transform>();
        //获取随机贴图
        int idx = UnityEngine.Random.Range(0, textures.Length);
        GetComponentInChildren<MeshRenderer>().material.mainTexture = textures[idx];
	}

    void OnCollisionEnter(Collision collision)
    {
        //如果碰撞物为子弹则爆炸
        if (collision.collider.tag == "BULLET")
        {
            Destroy(collision.gameObject);

            if (++hitcount >= 3)
            {
                ExpBarrel();
            }
        }
    }
    void OnDamage(object[] _params)
    {
        Vector3 firePos = (Vector3)_params[0];

        Vector3 hitPoint = (Vector3)_params[1];

        Vector3 incomeVector = hitPoint - firePos;

        incomeVector = incomeVector.normalized;

        GetComponent<Rigidbody>().AddForceAtPosition(incomeVector * 1000f, hitPoint);

        if (++hitcount >= 3)
        {
            ExpBarrel();
        }
    }

    void ExpBarrel()
    {
        //生成爆炸粒子特效
        Instantiate(expEffect, tr.position, Quaternion.identity);
        //获得10单位内的物体
        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);
        //对10单位内的为体施加爆炸力
        foreach (var coll in colls)
        {
            Rigidbody rigidbody = coll.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.mass = 1.0f;
                rigidbody.AddExplosionForce(1000.0f, tr.position, 10.0f, 300.0f);

            }

            Destroy(gameObject, 0.1f);
        }
    }

}
