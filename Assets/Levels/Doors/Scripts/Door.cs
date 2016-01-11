using Ascent.Managers.Audio;
using Ascent.WaypointsSystem;
using DG.Tweening;
using System;
using UnityEngine;

namespace Ascent
{
    public class Door : MonoBehaviour
    {
        [Header("Locking")]
        public bool locked;

        [Header("Components")]
        public DoorPart leftDoor;
        public DoorPart rightDoor;
        public DoorPresenceTrigger presenceTrigger;
        public GameObject[] lights;

        [Header("Animation Settings")]
        public float openTranslation = 5f;
        public float translationDuration = .5f;
        public float openDuration = 3f;

        [Header("Waypoints Integration")]
        public WaypointNetworkV2 waypoints;
        public string waypointsConnectionToToggle;

        private enum States { Closed, Opening, Open, Closing }
        private States state = States.Closed;
        private float leftClosedPos;
        private float leftOpenedPos;
        private float rightClosedPos;
        private float rightOpenedPos;
        private float openTime;

        private void Start()
        {
            if (leftDoor == null)
                throw new Exception("leftDoor is null.");

            if (rightDoor == null)
                throw new Exception("rightDoor is null.");

            if (presenceTrigger == null)
                throw new Exception("presenceTrigger is null.");

            leftClosedPos = leftDoor.transform.localPosition.x;
            leftOpenedPos = leftClosedPos - openTranslation;

            rightClosedPos = rightDoor.transform.localPosition.x;
            rightOpenedPos = rightClosedPos + openTranslation;

            UpdateLights();
        }

        private void Update()
        {
            if (state == States.Open && Time.time - openTime > openDuration)
            {
                if (presenceTrigger.ContainsObjects())
                {
                    openTime = Time.time;
                }
                else
                {
                    state = States.Closing;

                    SetWaypointsConnectionEnabled(false);

                    leftDoor.transform.DOLocalMoveX(leftClosedPos, translationDuration);
                    rightDoor.transform.DOLocalMoveX(rightClosedPos, translationDuration).OnComplete(() =>
                    {
                        state = States.Closed;
                    });
                }
            }
        }

        public void SetLocked(bool value)
        {
            locked = value;
            UpdateLights();
        }

        private void UpdateLights()
        {
            var color = locked ? Color.red : Color.green;

            foreach (var lightObj in lights)
            {
                var light = lightObj.GetComponentInChildren<Light>();
                if (light != null)
                {
                    light.color = color;
                }
            }
          
            var meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.materials[1].SetColor("_Color", color);
            }
        }

        public void Activate()
        {
            if (state == States.Closed)
            {
                if (locked)
                {
                    SoundFxsManager.instance.PlayOneShot(SoundFx.DoorLocked, this.transform.position);
                }
                else
                {
                    //SoundFxsManager.instance.PlayOneShot(SoundFx.Door, this.transform.position);

                    state = States.Opening;

                    leftDoor.transform.DOLocalMoveX(leftOpenedPos, translationDuration);
                    rightDoor.transform.DOLocalMoveX(rightOpenedPos, translationDuration).OnComplete(() =>
                    {
                        state = States.Open;
                        openTime = Time.time;
                        SetWaypointsConnectionEnabled(true);
                    });
                }
            }
        }

        private void SetWaypointsConnectionEnabled(bool value)
        {
            if (waypoints != null && !string.IsNullOrEmpty(waypointsConnectionToToggle))
            {
                if (!waypoints.isReady)
                {
                    Debug.LogWarning("Waypoint not ready yet. Connection did not open.");
                }

                waypoints.SetNamedConnectionEnabled(waypointsConnectionToToggle, value);
            }
        }
    }
}