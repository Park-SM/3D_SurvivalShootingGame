using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {

    public GameObject EnergyGage_prefab;
    public int AttackSpeed = 75;

    private int AttackSpeed_frame = 0;
    private Transform player_t;
    private NavMeshAgent nav;
    private Transform EnergyGagePos;
    private int LifeCount = 10;
    private GameObject EnergyGage;
    private float EG_backRage = 0;
    private GameObject kill_text;
    private Animator anim;

    // Use this for initialization
    void Start () {
        player_t = GameObject.Find("Player").transform;
        EnergyGagePos = transform.GetChild(0);
        kill_text = GameObject.Find("KillText");
        nav = GetComponent<NavMeshAgent>();
        EnergyGage = GameObject.Instantiate(EnergyGage_prefab, EnergyGagePos);
        EnergyGage.transform.parent = transform;
        anim = GetComponent<Animator>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            GetComponent<ParticleSystem>().Play();
            EnergyGage.transform.Translate(-EG_backRage, 0, 0);
            transform.Translate(0, 0, -0.3f);
            EnergyGage.transform.localScale += new Vector3(-0.1f, 0, 0);
            EG_backRage = (1f - EnergyGage.transform.localScale.x) / 2f;
            EnergyGage.transform.Translate(EG_backRage, 0, 0);
            if (--LifeCount == 0)
            {
                Destroy(gameObject);
                kill_text.GetComponent<KillScript>().AddScore();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("IsAttack", true);
            if (--AttackSpeed_frame <= 0)
            {
                player_t.GetComponent<CharacterMove>().HaveDamage(10);
                AttackSpeed_frame = AttackSpeed;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            anim.SetBool("IsAttack", false);
        }
    }

    // Update is called once per frame
    void Update () {
        if (nav.destination != player_t.position) nav.SetDestination(player_t.position);
        else nav.SetDestination(transform.position);

    }
}
