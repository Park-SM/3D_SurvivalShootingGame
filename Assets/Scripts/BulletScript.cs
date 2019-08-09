using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    GameObject bp, bpt;
    public float Speed = 50f;
	// Use this for initialization
	void Start () {
        bp = GameObject.Find("BulletPosition");
        bpt = GameObject.Find("BulletPosition_t");
        transform.position = new Vector3(bp.transform.position.x, bp.transform.position.y, bp.transform.position.z);
        transform.LookAt(bpt.transform);
        Destroy(gameObject, 3);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(0, 0, Speed * Time.deltaTime);
	}
}
