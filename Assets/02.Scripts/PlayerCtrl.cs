using System;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip runForward;
    public AnimationClip runBackward;
    public AnimationClip runRight;
    public AnimationClip runLeft;
}

public class PlayerCtrl : MonoBehaviour {
    //人物移动速度
    public float moveSpeed=10.0f;
    //获取键盘输入移动或转向指令
    private float h = 0.0f;
    private float v = 0.0f;
    private float m = 0.0f;
    //人物转声速度
    public float rotateSpeed=100.0f;

    public Anim anim;

    public Animation _animation;

    Transform trans;

    public int hp = 100;
    private int initHp;
    public Image imgHpbar;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnplayerDie;
	
	void Start () {
        trans = GetComponent<Transform>();

        initHp = hp;
        //查找位于自身下级的Animation组件并分配到变量。
        _animation = GetComponentInChildren<Animation>();

        _animation.clip = anim.idle;
        _animation.Play();
        
	}
	
	
	void Update () {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        m = Input.GetAxis("Mouse X");
        //移动方向
        Vector3 moveDir = Vector3.forward * v + Vector3.right * h;
        
        trans.Translate(moveSpeed * moveDir.normalized * Time.deltaTime, Space.Self);

        trans.Rotate(Vector3.up * Time.deltaTime * rotateSpeed * m);


        //Debug.Log("v:" + v.ToString());
        //Debug.Log("h:" + h.ToString());
        if (v >= 0.1f)
        {
            _animation.CrossFade(anim.runForward.name, 0.3f);
        }
        else if (v <= -0.1f)
        {
            _animation.CrossFade(anim.runBackward.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            _animation.CrossFade(anim.runLeft.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            _animation.CrossFade(anim.runRight.name, 0.3f);
        }
        else
        {
            _animation.CrossFade(anim.idle.name
                , 0.3f);
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "PUNCH")
        {
            hp -= 10;

            imgHpbar.fillAmount = (float)hp / (float)initHp;

            Debug.Log("player hp=" + hp.ToString());
            if (hp <= 0)
            {
                PlayerDie();
            }
        }
    }

    void PlayerDie()
    {
        Debug.Log("player die!" );

        GameMgr.instance.isGameOver = true;

        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        //foreach(var monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        OnplayerDie();
    }
}
