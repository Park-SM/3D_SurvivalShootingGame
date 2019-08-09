using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGeneratorScript : MonoBehaviour {

    public GameObject enemy_Prefab;
    public GameObject bird_Prefab;
    public GameObject Boss_Prefab;
    public float AppearanceTime = 3f;

    private static int EnemyKind = 1;
    private float BossTime = 0;
    private LevelGenerator LevelGenerator;
    private float AppearanceTime_frame = 0;
    private bool IsNotBoss = true;
	// Use this for initialization
	void Start () {
        LevelGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        if (LevelGenerator.getLevel() == 2) AppearanceTime -= 1f;
	}

    void CreateEnemy()
    {
        GameObject NewEnemy;
        if (Random.Range(0, EnemyKind) == 0) NewEnemy = Instantiate(enemy_Prefab, transform) as GameObject;
        else NewEnemy = Instantiate(bird_Prefab, transform) as GameObject;
        NewEnemy.transform.parent = null;
    }

    public int GetEnemyKind()
    {
        return EnemyKind;
    }

    public void UpEnemyKind()
    {
        EnemyKind++;
    }

    void Update()
    {
        if (IsNotBoss && LevelGenerator.getLevel() == 3 && (BossTime += Time.deltaTime) >= 30f)
        {
            GameObject NewEnemy = Instantiate(Boss_Prefab, transform) as GameObject;
            NewEnemy.transform.parent = null;
            IsNotBoss = false;
        }

        if ( (AppearanceTime_frame += Time.deltaTime) >= AppearanceTime)
        {
            if (LevelGenerator.getLevel() == 1) transform.position = new Vector3(Random.Range(-10f, 11f), 0f, Random.Range(-10f, 11f));
            else transform.position = new Vector3(Random.Range(-38f, 39f), 5.5f, Random.Range(-38f, 39f));
            CreateEnemy();
            AppearanceTime_frame = 0f;
        }
    }
}
