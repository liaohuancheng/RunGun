using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour {

    public enum MonsterState { idle,trace,attack,die}
    public MonsterState monsterState = MonsterState.idle;

    

    private bool isDie = false;

    private int hp = 100;
    public float attackDist;
    public NavMeshAgent nvAgent;
    public Transform playerTr;
    public Transform monsterTr;
    public Animator animator;
    public GameUI gameUI;
    public float traceDist;
    public GameObject bloodEffect;
    public GameObject bloodDecal;

    void Awake () {
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        monsterTr = this.gameObject.GetComponent<Transform>();
        animator = this.gameObject.GetComponent<Animator>();

        gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();

        nvAgent.destination = playerTr.position;

        StartCoroutine(CheckMonsterState());

        StartCoroutine(MonsterAction());
	}

    void OnEnable()
    {
        PlayerCtrl.OnplayerDie += this.OnPlayerDie;
        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }

    private void OnDisable()
    {
        PlayerCtrl.OnplayerDie -= this.OnPlayerDie;
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            var dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if (dist <= attackDist)
            {
                monsterState = MonsterState.attack;
                
            }
            else if (dist <= traceDist)
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle;
            }
        }
        
    }

    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsTrace", false);
                    break;
                case MonsterState.attack:
                    nvAgent.isStopped = true;
                    animator.SetBool("IsAttack", true);

                    break;
                case MonsterState.trace:
                    animator.SetBool("IsTrace", true);
                    animator.SetBool("IsAttack", false);
                    nvAgent.destination = playerTr.position;
                    nvAgent.isStopped = false;
                    break;
            }
            yield return null;
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log(other.gameObject.tag);
    //}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BULLET")
        {
            CreatBloodEffect(collision.transform.position);
            hp -= collision.gameObject.GetComponent<BulletCtrl>().damage;

            if (hp < 0)
            {
                MonsterDie();
            }
            Destroy(collision.gameObject);
            animator.SetTrigger("IsHit");

        }
    }
    void OnDamage(object[] _params)
    {
        CreatBloodEffect((Vector3)_params[0]);
        hp -=(int)_params[1];
        if (hp < 0)
        {
            MonsterDie();
        }
        animator.SetTrigger("IsHit");
        //Debug.Log(String.Format("Hit ray {0} : {1}", _params[0], _params[1]));
    }
    void MonsterDie()
    {
        gameObject.tag = "Untagged";
        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.die;
        nvAgent.isStopped = true;
        animator.SetTrigger("IsDie");

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = false;

        foreach(Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = false;
        }
        gameUI.DispScore(50);
        StartCoroutine(this.PushObjectPool());
    }

    IEnumerator PushObjectPool()
    {
        yield return new WaitForSeconds(3.0f);

        isDie = false;
        hp = 100;
        gameObject.tag = "MONSTER";
        monsterState = MonsterState.idle;

        gameObject.GetComponentInChildren<CapsuleCollider>().enabled = true;
        foreach(Collider coll in gameObject.GetComponentsInChildren<SphereCollider>())
        {
            coll.enabled = true;
        }

        gameObject.SetActive(false);
    }

    //创建出血特效
    void CreatBloodEffect(Vector3 pos)
    {
        GameObject blood1 = Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(blood1, 0.2f);
        Quaternion deaclRot = Quaternion.Euler(90,0,UnityEngine.Random.Range(0,360));

        Vector3 decalPos = monsterTr.position + Vector3.up * 0.05f;
        
        GameObject blood2 = Instantiate(bloodDecal, decalPos, deaclRot);
        float scale = UnityEngine.Random.Range(1.5f, 3.5f);
        blood2.transform.localScale = Vector3.one * scale;

        Destroy(blood2, 5.0f);
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();
        nvAgent.isStopped = true;
        animator.SetTrigger("IsPlayerDie");
    }
}
