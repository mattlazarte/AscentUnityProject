using Ascent.Managers.Audio;
using Ascent.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Ascent.PlayerShip;
using Ascent.Managers.Input;
using Ascent.Managers.Pools;
using Ascent.Enemies;
using Ascent.WaypointsSystem;

namespace Ascent.Managers.Game
{
    public class SurvivalMatchManager : MonoBehaviour
    {
        public enum States
        {
            InitialDelay,
            IntroObjectivesSetup,
            IntroObjectives,
            WaitWinOrDeath,
            IntroWave,
            IntroWaveSetup,
            IntroWaveBlinkSetup,
            IntroWaveBlink,
            IntroStatsSetup,
            IntroStats,
            WaveSpawnSetup,
            WaveSpawn,
        }

        public float initialDelay = 2f;
        public float writingInterval = 0.3f;
        public float blinkInterval = 0.1f;
        public float waveSpawnInterval = 1f;
        public Text objectivesLabel;
        public Text wavesLabel;
        public Text statsLabel;
        public GameObject enemyPrefab;
        public WaypointNetworkV2 waypoints;
        public Transform[] spawnPoints;
        public Collider playerShipCollider;

        [HideInInspector]
        public int waveNumber;

        [HideInInspector]
        public int totalKills;

        private States state;
        private float matchStartTime;
        private int waveKills;
        private int waveTotal;

        private int currentWaveSpawnCount;
        private float lastWaveSpawnTime;
        private bool waveSpawnDone;

        private float lastTimeBlinkToggle;
        private int blinkTimes;
        private int blinkCount;
        private GameObject gameObjectToBlink;
        private bool blinkDone;

        private float lastWriteTime;
        private Text labelToWrite;
        private string textToWrite;
        private int writingCursor;
        private bool writingDone;

        private void Awake()
        {
            state = States.InitialDelay;
            matchStartTime = Time.time;
            waveNumber = 1;
            waveKills = 0;
            waveTotal = 3;
            totalKills = 0;
            EnemyShipControllerV2.OnEnemyDestroy += OnEnemyDestroy;
        }
        protected virtual void OnDestroy()
        {
            EnemyShipControllerV2.OnEnemyDestroy -= OnEnemyDestroy;
        }
        private void VerifyBindings()
        {
            if (objectivesLabel == null)
                throw new Exception("objectivesLabel is null.");

            if (wavesLabel == null)
                throw new Exception("wavesLabel is null.");

            if (statsLabel == null)
                throw new Exception("statsLabel is null.");

            if (enemyPrefab == null)
                throw new Exception("enemyPrefab is null.");

            if (waypoints == null)
                throw new Exception("waypoints is null.");

            if (spawnPoints == null || spawnPoints.Length == 0)
                throw new Exception("spawnPoints is null or empty.");

            foreach (var sp in spawnPoints)
                if (sp == null)
                    throw new Exception("An element of spawnPoints is null.");

            if (playerShipCollider == null)
                throw new Exception("playerShipCollider is null.");
        }

        private void Update()
        {
            if (!PauseManager.instance.paused)
            {
                switch (state)
                {
                    case States.InitialDelay:
                        if (Time.time - matchStartTime > initialDelay)
                            state = States.IntroObjectivesSetup;
                        break;

                    case States.IntroObjectivesSetup:
                        SetupTextToWriteOnHud(objectivesLabel, "Objective: Kill all enemies!");
                        state = States.IntroObjectives;
                        break;

                    case States.IntroObjectives:
                        WriteTextUpdate();
                        if (writingDone)
                            state = States.IntroStatsSetup;
                        break;

                    case States.IntroStatsSetup:
                        SetupTextToWriteOnHud(statsLabel, "Wave Kills: 0/0\nTotal Kills: 0");
                        state = States.IntroStats;
                        break;

                    case States.IntroStats:
                        WriteTextUpdate();
                        if (writingDone)
                            state = States.IntroWaveSetup;
                        break;

                    case States.IntroWaveSetup:
                        SetupTextToWriteOnHud(wavesLabel, "Prepare to wave " + waveNumber.ToString() + "!");
                        state = States.IntroWave;
                        break;

                    case States.IntroWave:
                        WriteTextUpdate();
                        if (writingDone)
                            state = States.IntroWaveBlinkSetup;
                        break;

                    case States.IntroWaveBlinkSetup:
                        SetupBlink(wavesLabel.gameObject, 10);
                        state = States.IntroWaveBlink;
                        break;

                    case States.IntroWaveBlink:
                        BlinkUpdate();
                        if (blinkDone)
                        {
                            wavesLabel.text = "Wave " + waveNumber.ToString();
                            UpdateStats();
                            state = States.WaveSpawnSetup;
                        }
                        break;

                    case States.WaveSpawnSetup:
                        currentWaveSpawnCount = 0;
                        lastWaveSpawnTime = 0;
                        waveSpawnDone = false;
                        state = States.WaveSpawn;
                        break;

                    case States.WaveSpawn:
                        WaveSpawnUpdate();
                        if (waveSpawnDone)
                            state = States.WaitWinOrDeath;
                        break;

                    case States.WaitWinOrDeath:
                        break;

                    default:
                        throw new Exception("Unexpected state.");
                }
            }
        }
        private void SetupTextToWriteOnHud(Text label, string text)
        {
            writingCursor = 0;
            writingDone = false;
            labelToWrite = label;
            textToWrite = text;
            lastWriteTime = 0;
            label.text = string.Empty;
        }
        private void WriteTextUpdate()
        {
            if (Time.time - lastWriteTime >= writingInterval)
            {
                lastWriteTime = Time.time;

                if (writingCursor > textToWrite.Length)
                {
                    writingDone = true;
                }
                else
                {
                    labelToWrite.text = textToWrite.Substring(0, writingCursor);
                }

                writingCursor++;
            }
        }
        private void SetupBlink(GameObject gameObject, int times)
        {
            lastTimeBlinkToggle = 0;
            blinkTimes = times * 2;
            blinkCount = 0;
            gameObjectToBlink = gameObject;
            blinkDone = false;
        }
        private void BlinkUpdate()
        {
            if (Time.time - lastTimeBlinkToggle >= blinkInterval)
            {
                lastTimeBlinkToggle = Time.time;

                if (blinkCount >= blinkTimes)
                {
                    blinkDone = true;
                }
                else
                {
                    gameObjectToBlink.SetActive(!gameObjectToBlink.activeSelf);
                }

                blinkCount++;
            }
        }
        private void UpdateStats()
        {
            statsLabel.text = "Wave Kills: " + waveKills + "/" + waveTotal + "\nTotal Kills: " + totalKills;
        }
        private void WaveSpawnUpdate()
        {
            if (Time.time - lastWaveSpawnTime >= waveSpawnInterval)
            {
                lastWaveSpawnTime = Time.time;

                if (currentWaveSpawnCount >= waveTotal)
                {
                    waveSpawnDone = true;
                }
                else
                {
                    var spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length - 1)];

                    var enemy = (GameObject)Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                    enemy.transform.SetParent(GameManager.instance.gameRoot.transform);
                    
                    var controller = enemy.GetComponentInChildren<EnemyShipControllerV2>();
                    controller.waypoints = waypoints;
                    controller.SetTarget(playerShipCollider);
                }

                currentWaveSpawnCount++;
            }
        }
        private void OnEnemyDestroy()
        {
            waveKills++;
            totalKills++;

            UpdateStats();

            if (waveKills == waveTotal)
            {
                Debug.LogWarning("Wave done!");

                waveNumber++;
                waveKills = 0;
                waveTotal *= 2;
                state = States.IntroWaveSetup;
            }
        }

        //private void OnGUI()
        //{
        //    GUILayout.TextField("Match state: " + state.ToString());
        //}
    }
}
