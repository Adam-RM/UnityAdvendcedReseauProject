using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.InputSystem;

namespace Mirror
{
    public class PlayerCamera : NetworkBehaviour
    {
        Camera mainCam;
        public GameObject Target;
        public PlayerController playerController;
        public CanvasManager canvasManager;
        public float rotationSpeed = 1;
        private float mouseX;
        private float mouseY;
        private bool isAlive = true;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        private const float _threshold = 0.01f;

        private float _targetRotation = 0.0f;
        public float RotationSmoothTime = 0.12f;
        private float _rotationVelocity = 0.5f;

        private bool observer =  false;

        void Awake()
        {
            mainCam = Camera.main;
            canvasManager = GameObject.Find("Canvas").GetComponent<CanvasManager>();
        }
        void LateUpdate()
        {
            if (isLocalPlayer)
            {
                if (observer == false && playerController.isDead == true)
                    observer = true;
                if (playerController.isDead == false && observer == true)
                {
                    GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = Target.transform;
                    CinemachineCameraTarget = Target;
                    _cinemachineTargetYaw = transform.rotation.eulerAngles.y;
                    observer = false;
                }
                if (playerController.isDead == false && canvasManager.isPaused == false)
                    CameraRotation();
                
                //mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
                //mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
                //mouseY = Mathf.Clamp(mouseY, -35, 60);

                //mainCam.transform.LookAt(Target);

                //Target.rotation = Quaternion.Euler(mouseY, mouseX, 0);
                //transform.rotation = Quaternion.Euler(mouseY, mouseX, 0);
            }
        }
        private void CameraRotation()
        {
            Vector2 look = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            if (look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {

                _cinemachineTargetYaw += look.x * rotationSpeed;
                _cinemachineTargetPitch += look.y * rotationSpeed;
            }

            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
            _targetRotation = mainCam.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }
        public override void OnStartLocalPlayer()
        {
            if (mainCam != null)
            {

                GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = Target.transform;
                CinemachineCameraTarget = Target;
                _cinemachineTargetYaw = transform.rotation.eulerAngles.y;
            }
            else
                Debug.LogWarning("PlayerCamera: Could not find a camera in scene with 'MainCamera' tag.");
        }

        public override void OnStopLocalPlayer()
        {
            if (mainCam != null)
            {/*
                mainCam.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(mainCam.gameObject, SceneManager.GetActiveScene());
                mainCam.orthographic = true;
                mainCam.orthographicSize = 15f;
                mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
                mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);*/
            }
        }


    }
}