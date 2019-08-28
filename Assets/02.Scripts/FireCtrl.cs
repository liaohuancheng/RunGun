using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FireCtrl : MonoBehaviour
{

    public GameObject bullet;
    public Transform firePos;
    public AudioClip fireSfx;
    private AudioSource source = null;
    public MeshRenderer MuzzleFlash;

    void Start()
    {
        source = GetComponent<AudioSource>();
        MuzzleFlash.enabled = false;
    }
    void Update()
    {
        Debug.DrawRay(firePos.position, firePos.forward * 10.0f, Color.green);
        if (Input.GetMouseButtonDown(0))
        {
            Fire();

            RaycastHit hit;

            if (Physics.Raycast(firePos.position, firePos.forward, out hit, 10.0f))
            {
                if (hit.collider.tag == "MONSTER")
                {
                    object[] _parms = new object[2];
                    _parms[0] = hit.point;
                    _parms[1] = 20;

                    hit.collider.gameObject.SendMessage("OnDamage"
                                                        , _parms
                                                        , SendMessageOptions.DontRequireReceiver);
                }
                if (hit.collider.tag == "Barrel")
                {
                    object[] _parms = new object[2];
                    _parms[0] = firePos.position;
                    _parms[1] = hit.point;

                    hit.collider.gameObject.SendMessage("OnDamage"
                                                        , _parms
                                                        , SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

    void Fire()
    {
        CreateBullet();
        GameMgr.instance.PlaySfx(firePos.position, fireSfx);
        StartCoroutine(this.ShowMuzzleFlash());
    }
    //展示开火特效，随机大小角度
    IEnumerator ShowMuzzleFlash()
    {
        float scale = UnityEngine.Random.Range(1.0f, 2.0f);
        MuzzleFlash.transform.localScale = Vector3.one * scale;

        Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
        MuzzleFlash.transform.localRotation = rot;


        MuzzleFlash.enabled = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.3f));
        MuzzleFlash.enabled = false;
    }

    void CreateBullet()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }

}
