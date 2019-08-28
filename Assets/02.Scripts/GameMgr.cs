﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

    public List<GameObject> monsterPool = new List<GameObject>();

    public Transform[] points;
    public int maxMonster=10;
    public float createTime = 2.0f;
    public GameObject monsterPrefab;
    public bool isGameOver = false;
    public static GameMgr instance = null;
    public float sfxVolum = 1.0f;
    public bool isSfxMute = false;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {

        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();
        
        for(int i=0; i < maxMonster; i++)
        {
            GameObject monster = (GameObject)Instantiate(monsterPrefab);

            monster.name = "Monster_" + i.ToString();
            monster.SetActive(false);
            monsterPool.Add(monster);
        }

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
        
	}
	
    IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            yield return new WaitForSeconds(createTime);

            int monsterCount = (int)GameObject.FindGameObjectsWithTag("MONSTER").Length;

            if (isGameOver) yield break;

            foreach(GameObject monster in monsterPool)
            {
                if (!monster.activeSelf)
                {
                    int idx = Random.Range(1, points.Length);
                    monster.transform.position = points[idx].transform.position;
                    monster.SetActive(true);
                    break;
                }
            }
        }
    }

    public void PlaySfx(Vector3 pos, AudioClip sfx)
    {
        if (isSfxMute) return;

        GameObject soundObj = new GameObject("Sfx");
        soundObj.transform.position = pos;

        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.clip = sfx;
        audioSource.minDistance = 10.0f;
        audioSource.maxDistance = 30.0f;
        audioSource.volume = sfxVolum;
        audioSource.Play();
        Destroy(soundObj, sfx.length);
    }

}
