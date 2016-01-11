using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Ascent.Enemies;
using Ascent.WaypointsSystem;
using Ascent.Items;
using Ascent;
using Ascent.Managers.Game;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 5;
    public GameObject playerShip;
    public GameObject enemyPrefab;
    public WaypointNetworkV2 waypoints;

    private EnemySpawnPoint[] spawnPoints;
    private float lastSpawn;

    private void Start()
    {
        if (playerShip == null)
            throw new Exception("playerShip is null.");

        if (enemyPrefab == null)
            throw new Exception("enemyPrefab is null.");

        if (waypoints == null)
            throw new Exception("waypoints is null.");

        spawnPoints = GameObject.FindObjectsOfType<EnemySpawnPoint>();
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("spawnPoints.Length == 0"); 
        }

        lastSpawn = Time.time;
        PauseManager.OnUnpause += Unpause;
    }

    private void Unpause(float pauseDeltaTime)
    {
        lastSpawn += pauseDeltaTime;
    }

    private void Update()
    {
        if (!PauseManager.instance.paused && Time.time - lastSpawn > spawnInterval)
        {
            lastSpawn = Time.time;

            var excluded = new List<EnemySpawnPoint>();
            bool enemySpawned = false;
            for (var i = 0; i < 50; i++) // Try at maximum 50 times.
            {
                var sp = GetSpawnPoint(excluded);
                if (sp == null) break;

                excluded.Add(sp);

                if (CanSpawnAtPoint(sp))
                {
                    SpawnEnemy(sp);
                    enemySpawned = true;
                    break;
                }
            }

            if (!enemySpawned)
            {
                Debug.LogWarning("EnemySpawner not able to spawn an enemy."); 
            }
        }
    }

    private bool CanSpawnAtPoint(EnemySpawnPoint sp)
    {
        return true;
    }

    private void SpawnEnemy(EnemySpawnPoint sp)
    {
        var enemy = (GameObject)Instantiate(enemyPrefab, sp.transform.position, Quaternion.identity);
        var controller = enemy.GetComponentInChildren<EnemyShipControllerV2>();
        controller.waypoints = waypoints;
    }

    private EnemySpawnPoint GetSpawnPoint(List<EnemySpawnPoint> excluded)
    {
        //EnemySpawnPoint sp = null;
        //float graterSrqMag = 0;

        //foreach (var currSp in spawnPoints)
        //{
        //    if (excluded.Contains(currSp)) continue;

        //    var currSqrMag = (playerShip.transform.position - currSp.transform.position).sqrMagnitude;
        //    if (currSqrMag > graterSrqMag)
        //    {
        //        graterSrqMag = currSqrMag;
        //        sp = currSp;
        //    }
        //}

        if (spawnPoints == null || spawnPoints.Length == 0)
            return null;

        return spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];
    }
}