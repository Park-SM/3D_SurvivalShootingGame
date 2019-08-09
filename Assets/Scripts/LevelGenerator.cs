using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour {

    KillScript KillScript;
    StageClearScript StageClearScript;

    private static int CurrentLevel = 1;
	// Use this for initialization
	void Start () {
        KillScript = GameObject.Find("KillText").GetComponent<KillScript>();
        StageClearScript = GameObject.Find("StageClear").GetComponent<StageClearScript>();
    }
	
    public int getLevel()
    {
        return CurrentLevel;
    }

    public void UpLevel()
    {
        CurrentLevel++;
    }

	// Update is called once per frame
	void Update () {
        if (CurrentLevel == 1 && KillScript.KillCount >= 10) { StageClearScript.sClear(); }
        else if (CurrentLevel == 2 && KillScript.KillCount >= 35) { StageClearScript.sClear(); }
    }
}
