using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour {

	public void onClickStartBtn()
    {
        Debug.Log("Click Button");
        Application.LoadLevel("scLevel01");
        Application.LoadLevelAdditive("scPlay");
        
    }
}
