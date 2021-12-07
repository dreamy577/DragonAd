using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager EM_inst = null;

    static public EnemyManager EM
    {
        get
        {
            if (EM_inst == null)
                return null;

            return EM_inst;
        }
    }

    public GameObject teruteru_Pref;
    public List<GameObject> teruterus = new List<GameObject>();
    int maxTeruTeru = 2;

    public GameObject tonton_Pref;
    public List<GameObject> tontons = new List<GameObject>();
    int maxTonTon = 4;

    public GameObject magicTree_Pref;
    public List<GameObject> magicTrees = new List<GameObject>();
    int maxMagicTree = 3;

    public GameObject momo_Pref;
    public List<GameObject> momos = new List<GameObject>();
    int maxMomo = 4;

    public GameObject mushroom_Pref;
    public List<GameObject> mushrooms = new List<GameObject>();
    int maxMushroom = 4;


    public List<Transform> emptySpawnPoints = new List<Transform>();
    public List<Transform> spawnPoints = new List<Transform>();

    private void Awake()
    {
        GameObject[] tmp = GameObject.FindGameObjectsWithTag("SpawnPoint");
        for (int a = 0; a < tmp.Length; a++)
        {
            emptySpawnPoints.Add(tmp[a].GetComponent<Transform>());
        }
    }

    private void Start()
    {
        SpawnEnemy_();
        RespawnEnemy();
    }

    void SpawnEnemy_()
    {
        StartCoroutine(SpawnEnemy("TeruTerus", maxTeruTeru, "Teruteru", teruterus, teruteru_Pref));
        StartCoroutine(SpawnEnemy("TonTons", maxTonTon, "Tonton", tontons, tonton_Pref));
        StartCoroutine(SpawnEnemy("MagicTrees", maxMagicTree, "MagicTree", magicTrees, magicTree_Pref));
        StartCoroutine(SpawnEnemy("MoMos", maxMomo, "Momo", momos, momo_Pref));
        StartCoroutine(SpawnEnemy("Mushrooms", maxMushroom, "Mushroom", mushrooms, mushroom_Pref));
    }

    void RespawnEnemy()
    {
        StartCoroutine(EnemyObjPool(teruterus));
        StartCoroutine(EnemyObjPool(tontons));
        StartCoroutine(EnemyObjPool(magicTrees));
        StartCoroutine(EnemyObjPool(momos));
        StartCoroutine(EnemyObjPool(mushrooms));
    }

    /// <summary>
    /// (몬스터의 부모 오브젝트명, 몬스터 최대 스폰 수, 몬스터명, 몬스터를 저장할 리스트, 몬스터 프리팹, 몬스터 타입(종류))
    /// </summary>
    /// <param name="enemyGroup"></param>
    /// <param name="enemyMaxCount"></param>
    /// <param name="enemyName"></param>
    /// <param name="enemyList"></param>
    /// <param name="enemyPref"></param>
    /// <returns></returns>
    IEnumerator SpawnEnemy(string enemyGroup, int enemyMaxCount, string enemyName, List<GameObject> enemyList, GameObject enemyPref)
    {
        while (enemyList.Count < enemyMaxCount)
        {
            yield return null;
            int idx = Random.Range(0, emptySpawnPoints.Count);

            GameObject eGroup = GameObject.Find(enemyGroup);

            GameObject enemy = Instantiate(enemyPref, emptySpawnPoints[idx].position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            enemy.name = (enemyName);
            enemy.transform.SetParent(eGroup.transform);
            enemy.GetComponentInChildren<Monster>().spawnPoint = emptySpawnPoints[idx];
            //switch (type)
            //{
            //    case Type.Teru:
            //        enemy.GetComponentInChildren<TeruTeru>().spawnPoint
            //         = emptySpawnPoints[idx];
            //        break;

            //    case Type.Ton:
            //        enemy.GetComponentInChildren<TonTon>().spawnPoint
            //             = emptySpawnPoints[idx];
            //        break;

            //    case Type.MagicTree:
            //        enemy.GetComponentInChildren<MagicTree>().spawnPoint
            //             = emptySpawnPoints[idx];
            //        break;

            //    case Type.Momo:
            //        enemy.GetComponentInChildren<Momo>().spawnPoint
            //            = emptySpawnPoints[idx];
            //        break;

            //    case Type.Mushroom:
            //        enemy.GetComponentInChildren<Mushroom>().spawnPoint
            //             = emptySpawnPoints[idx];
            //        break;

            //}
            enemyList.Add(enemy);
            spawnPoints.Add(emptySpawnPoints[idx]);
            emptySpawnPoints.Remove(emptySpawnPoints[idx]);
        }

        yield return null;
    }

    /// <summary>
    /// (몬스터가 저장된 리스트, 몬스터 타입(종류))
    /// </summary>
    /// <param name="enemyList"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    IEnumerator EnemyObjPool(List<GameObject> enemyList)
    {
        while (true)
        {
            yield return null;
            for (int a = 0; a < enemyList.Count; a++)
            {
                if (enemyList[a].activeSelf == false)
                {
                    yield return new WaitForSeconds(Random.Range(10, 20));
                    int idx = Random.Range(0, emptySpawnPoints.Count);
                    enemyList[a].transform.position = emptySpawnPoints[idx].transform.position;
                    enemyList[a].GetComponentInChildren<Monster>().spawnPoint = emptySpawnPoints[idx];
                    //switch (type)
                    //{
                    //    case Type.Teru:
                    //        enemyList[a].GetComponentInChildren<TeruTeru>().spawnPoint
                    //            = emptySpawnPoints[idx];
                    //        break;

                    //    case Type.Ton:
                    //        enemyList[a].GetComponentInChildren<TonTon>().spawnPoint
                    //            = emptySpawnPoints[idx];
                    //        break;

                    //    case Type.MagicTree:
                    //        enemyList[a].GetComponentInChildren<MagicTree>().spawnPoint
                    //             = emptySpawnPoints[idx];
                    //        break;

                    //    case Type.Momo:
                    //        enemyList[a].GetComponentInChildren<Momo>().spawnPoint
                    //             = emptySpawnPoints[idx];
                    //        break;

                    //    case Type.Mushroom:
                    //        enemyList[a].GetComponentInChildren<Mushroom>().spawnPoint
                    //             = emptySpawnPoints[idx];
                    //        break;

                    //}
                    spawnPoints.Add(emptySpawnPoints[idx]);
                    emptySpawnPoints.Remove(emptySpawnPoints[idx]);
                    enemyList[a].SetActive(true);
                }
            }
        }
    }

}

