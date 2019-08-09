using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {

    Image GameOver_img;
    private bool IsGameOver = false;

	// Use this for initialization
	void Start () {
        GameOver_img = GetComponent<Image>();
	}
	
    public void gOver()
    {
        IsGameOver = true;
    }

	// Update is called once per frame
	void Update () {
        if (IsGameOver) GameOver_img.fillAmount += Time.deltaTime / 1.2f;
        if (IsGameOver && GameOver_img.fillAmount >= 1) { SceneManager.LoadScene("GameOver"); IsGameOver = false; }
    }
}
