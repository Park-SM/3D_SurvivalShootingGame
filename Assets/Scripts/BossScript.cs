using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossScript : MonoBehaviour {

    public int AttackSpeed = 60;

    private int AttackSpeed_frame = 0;
    private Transform player_t;
    private NavMeshAgent nav;
    private int LifeCount = 500;


    // Use this for initialization
    void Start()
    {
        player_t = GameObject.Find("Player").transform;
        nav = GetComponent<NavMeshAgent>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            GetComponent<ParticleSystem>().Play();
            transform.Translate(0, 0, -0.3f);
            if (--LifeCount == 0)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("GameClear");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (--AttackSpeed_frame <= 0)
            {
                player_t.GetComponent<CharacterMove>().HaveDamage(30);
                AttackSpeed_frame = AttackSpeed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nav.destination != player_t.position) nav.SetDestination(player_t.position);
        else nav.SetDestination(transform.position);

    }
}
