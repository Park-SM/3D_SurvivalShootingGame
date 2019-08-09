using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageClearScript : MonoBehaviour {

    Image StageClear_img;
    LevelGenerator LevelGenerator;
    EnemyGeneratorScript EGScript;
    private bool IsStageClear = false;
    // Use this for initialization
    void Start () {
        StageClear_img = GameObject.Find("StageClear").GetComponent<Image>();
        LevelGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        EGScript = GameObject.Find("EnemyGenerator").GetComponent<EnemyGeneratorScript>();
	}

    public void sClear()
    {
        IsStageClear = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStageClear) StageClear_img.fillAmount += Time.deltaTime / 1.2f;
        if (IsStageClear && StageClear_img.fillAmount >= 1)
        {
            if (LevelGenerator.getLevel() == 1)
            {
                SceneManager.LoadScene("Level2_stage");
                IsStageClear = false;
                LevelGenerator.UpLevel();
                EGScript.UpEnemyKind();
            }
            else if (LevelGenerator.getLevel() == 2)
            {
                SceneManager.LoadScene("Level3_stage");
                IsStageClear = false;
                LevelGenerator.UpLevel();
            }
        }
    }

}
