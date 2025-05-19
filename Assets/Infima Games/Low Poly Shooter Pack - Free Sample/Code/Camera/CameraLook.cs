// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;
using UnityEngine.InputSystem;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Camera Look. Handles the rotation of the camera.
    /// </summary>
    public class CameraLook : MonoBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Header("Settings")]
        [SerializeField] private Vector2 sensitivity = new Vector2(1, 1);
        [SerializeField] private Vector2 yClamp = new Vector2(-60, 60);
        [SerializeField] private bool smooth;
        [SerializeField] private float interpolationSpeed = 25.0f;

        #endregion

        #region FIELDS

        private GameObject player;
        private Rigidbody rB;
        private Quaternion rotationCharacter;
        private Quaternion rotationCamera;
        public InputActionReference Input;
        #endregion
        
        #region UNITY

        private void Awake()
        {
            FindPlayer();
        }

        private void FindPlayer()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            rB = player.GetComponent<Rigidbody>();

            if (player != null && rB != null)
                return;
            else 
                FindPlayer();
        }

        private void Start()
        {
            //Cache the character's initial rotation.
            rotationCharacter = player.transform.localRotation;
            //Cache the camera's initial rotation.
            rotationCamera = transform.localRotation;
        }
        private void LateUpdate()
        {
            //Frame Input. The Input to add this frame!
            Vector2 frameInput = Input.action.ReadValue<Vector2>();
            //Sensitivity.
            frameInput *= sensitivity;

            //Yaw.
            Quaternion rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
            //Pitch.
            Quaternion rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);
            
            //Save rotation. We use this for smooth rotation.
            rotationCamera *= rotationPitch;
            rotationCharacter *= rotationYaw;
            
            //Local Rotation.
            Quaternion localRotation = transform.localRotation;

            //Smooth.
            if (smooth)
            {
                //Interpolate local rotation.
                localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
                //Interpolate character rotation.
                rB.MoveRotation(Quaternion.Slerp(rB.rotation, rotationCharacter, Time.deltaTime * interpolationSpeed));
            }
            else
            {
                //Rotate local.
                localRotation *= rotationPitch;
                //Clamp.
                localRotation = Clamp(localRotation);

                //Rotate character.
                rB.MoveRotation(rB.rotation * rotationYaw);
            }
            
            //Set.
            transform.localRotation = localRotation;
        }

        #endregion

        #region FUNCTIONS

        /// <summary>
        /// Clamps the pitch of a quaternion according to our clamps.
        /// </summary>
        private Quaternion Clamp(Quaternion rotation)
        {
            rotation.x /= rotation.w;
            rotation.y /= rotation.w;
            rotation.z /= rotation.w;
            rotation.w = 1.0f;

            //Pitch.
            float pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

            //Clamp.
            pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
            rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

            //Return.
            return rotation;
        }

        #endregion
    }
}