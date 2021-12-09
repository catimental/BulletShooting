using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private GameObject player;

    public static GameManager Instance;
    public bool IsPlayerAlive = true;

    private int killedCount = 0;

    public void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("StartGame", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsClearGame())
        {
            var spawnManager = SpawnManager.Instance;
            spawnManager.isSpawn = false;
            spawnManager.ClearEnemies();
            TextContrl.Instance.ShowGameClear();
        }
    }

    void StartGame()
    {
        player.GetComponent<Player>().canShoot = true;
        SpawnManager.Instance.isSpawn = true;
    }

    public void KillPlayer()
    {
        IsPlayerAlive = false;
        SpawnManager.Instance.isSpawn = false;
        TextContrl.Instance.ShowGameOver();
    }

    public void RestartGame()
    {
        ObjectManager.Instance.ClearBullets();
        SpawnManager.Instance.Reset();
        TextContrl.Instance.Restart();
        SoundManager.Instance.StopBGM();
        Invoke("RetryGame", 3f);
    }

    void RetryGame()
    {
        StartGame();
        player.SetActive(true);
    }

    public bool IsReadyBoss()
    {
        return killedCount >= 0;
    }

    private bool IsClearGame()
    {
        return SpawnManager.Instance.IsBossDead();
    }

    public void IncrementKilledCount()
    {
        killedCount++;
    }

}
