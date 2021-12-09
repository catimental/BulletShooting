using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private List<GameObject> enemies = new List<GameObject>();

    private Vector3[] positions = new Vector3[5];

    public GameObject enemy;
    
    public GameObject Boss;
    private GameObject bossInstance;

    public bool isSpawn = false;
    private bool isBossSpawned = false;
    public float spawnDelay = 1.5f;

    private float spawnTimer = 0f;

    private void Awake()
    {
        if (SpawnManager.Instance == null)
        {
            SpawnManager.Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CreatePositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsReadyBoss())
        {
            if (!isBossSpawned)
            {
                SoundManager.Instance.PlayBGM();
                isBossSpawned = true;
                SpawnBoss();
            }
        }
        else
        {
            SpawnEnemy();
        }
    }

    public void Reset()
    {
        ClearEnemies();
        isBossSpawned = false;
    }

    void CreatePositions()
    {
        float viewPosY = 1.2f;
        float gapX = 1f / 6f;
        float viewPosX = 0f;

        for (int i = 0; i < positions.Length; i++)
        {
            viewPosX = gapX + gapX * i;

            Vector3 viewPos = new Vector3(viewPosX, viewPosY, 0);

            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);

            worldPos.z = 0f;

            positions[i] = worldPos;
        }
    }

    void SpawnBoss()
    {
        if (bossInstance == null)
        {
            bossInstance = Instantiate(Boss, PositionHelper.Instance.GetPositionFromNormalizedVector(0.5f, 0.8f), Quaternion.identity);
        }
    }

    void SpawnEnemy()
    {
        if (isSpawn)
        {
            if (spawnTimer > spawnDelay)
            {
                int rand = Random.Range(0, positions.Length);

                var enemyObj = Instantiate(enemy, positions[rand], Quaternion.identity);
                enemies.Add(enemyObj);

                spawnTimer = 0f;
            }

            spawnTimer += Time.deltaTime;
        }
    }

    public bool IsBossDead()
    {
        return bossInstance == null && isBossSpawned;
    }

    public void ClearEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }

        enemies.Clear();
        if (bossInstance != null)
        {
            Destroy(bossInstance);
            ObjectManager.Instance.BossHpSlider.gameObject.SetActive(false);
        }
    }
}
