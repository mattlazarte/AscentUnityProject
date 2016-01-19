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

namespace Ascent.Managers.Game
{
    public class GameManager : MonoBehaviour
    {
        public enum States
        {
            Splashing,
            MainMenu,
            Loading,
            Game,
            Paused,
            Dead,
        }

        public static GameManager instance;

        [Header("Splash Graphics")]
        public Image vfsLogo;
        public Image gameLogoBig;
        public Image gameLogoSmall;
        public Text copyrightText;

        [Header("UI Objects")]
        public Camera uiCamera;
        public Canvas uiCanvas;

        [HideInInspector]
        public GameRoot gameRoot;

        private SurvivalMatchManager matchManager;

        [HideInInspector]
        public States state;

        private void Awake()
        {
            instance = this;

            if (vfsLogo == null)
                throw new Exception("vfsLogo is null.");

            if (gameLogoBig == null)
                throw new Exception("gameLogoBig is null.");

            if (gameLogoSmall == null)
                throw new Exception("gameLogoSmall is null.");

            if (copyrightText == null)
                throw new Exception("copyrightText is null.");

            if (uiCamera == null)
                throw new Exception("uiCamera is null.");

            if (uiCanvas == null)
                throw new Exception("uiCanvas is null.");
        }

        private void Start()
        {
            state = States.Splashing;
            //MusicManager.instance.PlayLooped(Music.MenuMusic);

#if UNITY_EDITOR

            uiCanvas.pixelPerfect = true;
            FadeInMainMenu();

#else 
            ActivateAndMakeTransparent(vfsLogo);
            ActivateAndMakeTransparent(gameLogoBig);

            var sequence = DOTween.Sequence();

            sequence.Insert(0, vfsLogo.DOFade(1f, 1f));
            sequence.Insert(0, vfsLogo.rectTransform.DOScale(1.1f, 5f));
            sequence.Insert(4, vfsLogo.DOFade(0f, 1f));

            sequence.Insert(6, gameLogoBig.DOFade(1f, 1f));
            sequence.Insert(6, gameLogoBig.rectTransform.DOScale(1.1f, 5f));
            sequence.Insert(9, gameLogoBig.DOFade(0f, 1f));
            
            sequence.OnComplete(() => {
                uiCanvas.pixelPerfect = true;
                FadeInMainMenu();
            });

            sequence.Play();
#endif
        }
        private void ActivateAndMakeTransparent(Graphic graphic)
        {
            graphic.gameObject.SetActive(true);
            graphic.color = gameLogoSmall.color.Copy(0);
        }
        private void FadeInMainMenu()
        {
            ActivateAndMakeTransparent(gameLogoSmall);
            ActivateAndMakeTransparent(copyrightText);

            UIMainMenuWindow.instance.FadeIn(duration: 1f);
            gameLogoSmall.DOFade(1f, 1f);
            copyrightText.DOFade(1f, 1f).OnComplete(() =>
            {
                state = States.MainMenu;
                UICursorManager.instance.ShowCursor();
            });
        }

        private void Update()
        {
            if (state == States.Game)
            {
                var input = InputManager.instance.GetPlayerInput();
                if (input.Pause)
                {
                    Pause();
                }
            }
        }

        public void StartGame()
        {
            state = States.Loading;
            StartCoroutine(StartGameCoroutine());
        }
        private IEnumerator StartGameCoroutine()
        {
            //MusicManager.instance.FadeOut(1f);
            UICursorManager.instance.HideCursor();
            UIMainMenuWindow.instance.FadeOut();

            var sequence = DOTween.Sequence();
            sequence.Insert(0, gameLogoSmall.DOFade(0f, 1f));
            sequence.Insert(0, copyrightText.DOFade(0f, 1f));
            sequence.OnComplete(() =>
            {
                gameLogoSmall.gameObject.SetActive(false);
                copyrightText.gameObject.SetActive(false);
            });

            var loadTask = Application.LoadLevelAdditiveAsync("AlphaGameScene");

            while (!loadTask.isDone)
                yield return null;

            uiCamera.gameObject.SetActive(false);

            // Unity requires one frame to initialize loaded scene objects.
            yield return null;

            gameRoot = FindObjectOfType<GameRoot>();
            if (gameRoot == null)
                throw new Exception("Game root object not found after loading.");

            gameRoot.pilotCamera.gameObject.SetActive(true);

            matchManager = FindObjectOfType<SurvivalMatchManager>();
            if (matchManager == null)
                throw new Exception("Match manager not found after loading.");

            // Do an extra interval, just to be dramatic.
            yield return new WaitForSeconds(1);

            //MusicManager.instance.PlayIntroThenLooped(Music.InGameMusicIntro, Music.InGameMusic);
            gameRoot.pilotCamera.FadeIn(1f, () =>
            {
                gameRoot.playerShipController.lockControls = false;
                state = States.Game;
            });
        }

        public void KillPlayer()
        {
            state = States.Dead;

            UICursorManager.instance.ShowCursor();
            //MusicManager.instance.PlayOnce(Music.DeathMusic);

            gameRoot.playerShipController.ForceStopVibration();
            gameRoot.playerShipWeaponryController.DisableAlarms();

            foreach (var enemy in gameRoot.GetComponentsInChildren<EnemyShipControllerV2>())
                enemy.PlayerHasDied();

            gameRoot.pilotCamera.EnableBlur();
            gameRoot.pilotCamera.transform.parent = gameRoot.transform;

            var explosionPosition =
                gameRoot.pilotCamera.transform.position +
                gameRoot.pilotCamera.transform.localRotation *
                gameRoot.pilotCamera.transform.forward;

            PoolManager.instance.Spawn<EnemyExplosion01>(null, explosionPosition, Quaternion.identity);

            gameRoot.playerShipController.gameObject.SetActive(false);

            // To-do:
            // Destroy Ship Object
            // Spawn an explosion
            // Create a camera cam
            // Big vibration

            UIDeathWindow.instance.statsLabel.text =
                string.Format(
                    "You survived {0} waves and killed {1} enemies.",
                    matchManager.waveNumber,
                    matchManager.totalKills
                );

            UIDeathWindow.instance.FadeIn();
        }
        public void Pause()
        {
            UICursorManager.instance.ShowCursor();
            //gameRoot.playerShipController.ForceStopVibration();
            gameRoot.pilotCamera.EnableBlur();
            PauseManager.instance.Pause();
            UIPauseWindow.instance.FadeIn(() =>
            {
                state = States.Paused;
            });
        }
        public void Unpause()
        {
            UICursorManager.instance.HideCursor();
            UIPauseWindow.instance.FadeOut(() =>
            {
                gameRoot.pilotCamera.DisableBlur();
                PauseManager.instance.Unpause();
                state = States.Game;
            });
        }

        public void KillGameAndBringBackMainMenu()
        {
            gameRoot.pilotCamera.FadeOut(1f, () =>
            {
                gameRoot.pilotCamera.gameObject.SetActive(false);
                uiCamera.gameObject.SetActive(true);
                //MusicManager.instance.FadeToMusic(Music.MenuMusic, 1f);
                Destroy(gameRoot.gameObject);
                gameRoot = null;
                FadeInMainMenu();
                PauseManager.instance.GameKilled();
            });
        }
    }
}