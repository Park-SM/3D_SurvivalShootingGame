using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillScript : MonoBehaviour {

    public int KillCount;
	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "Kill: 0";
	}
	
    public void AddScore()
    {
        GetComponent<Text>().text = "Kill: " + ++KillCount;
    }
}
