#region photon
using Photon.Pun;
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


namespace Com.TriggerAppsProduction.NeverRage
{
    #region PlayerAnimatorManager 
    public class vThirdPersonInput : MonoBehaviourPun
    {
        /// <summary>
        /// This Scripts Handles Buttons Input From Player-It Reference the carController 
        /// and 
        /// Set it self as a target for the camera
        /// This is the playerAnimatorManager
        /// </summary>

        #region PlayerInput: Variables

        [Header("Default Inputs")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;

        [Header("Camera Settings")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        protected vThirdPersonCamera tpCamera;                // acess camera info        
        [HideInInspector]
        public string customCameraState;                    // generic string to change the CameraState        
        [HideInInspector]
        public string customlookAtPoint;                    // generic string to change the CameraPoint of the Fixed Point Mode        
        [HideInInspector]
        public bool changeCameraState;                      // generic bool to change the CameraState        
        [HideInInspector]
        public bool smoothCameraState;                      // generic bool to know if the state will change with or without lerp  
        [HideInInspector]
        public bool keepDirection;                          // keep the current direction in case you change the cameraState

        protected vThirdPersonController cc;                // access the ThirdPersonController component                

        #endregion

        #region  MonoBehaviour Callbacks
        #region Start ()
        protected virtual void Start()
        {
            CharacterInit();
        
        }
        #endregion

        #region Update()
        protected virtual void Update()
        {
            #region Photon Network Script: Check CharacterOwnership 02 During Local Test
            //Detect which character is ours
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            #endregion
            cc.UpdateMotor();                   // call ThirdPersonMotor methods               
            cc.UpdateAnimator();                // call ThirdPersonAnimator methods	

        }
        #endregion
        #endregion


        #region //Check For CharacterController  ALSO Make tpCamera Target the object with this script
        public virtual void CharacterInit()
        {
            #region Access to the Character Controller and Animator
            cc = GetComponent<vThirdPersonController>();
            if (cc != null)
                cc.Init();
            #endregion

            tpCamera = FindObjectOfType<vThirdPersonCamera>();
            if (tpCamera) tpCamera.SetMainTarget(this.transform);

            //Locks Cursor when clicks
            /*  Cursor.visible = false;
              Cursor.lockState = CursorLockMode.Locked;
              */
        }

        public void use_UpdateMotor()
        {
            LateUpdate();
            FixedUpdate();
            InputHandle();
            CharacterInit();
        }

        public virtual void LateUpdate()
        {
            if (cc == null) return;             // returns if didn't find the controller		    
            InputHandle();                      // update input methods
            UpdateCameraStates();               // update camera states
        }

        public virtual void FixedUpdate()
        {
            cc.AirControl();
            CameraInput();
        }
        #endregion

        public virtual void InputHandle()
        {
            
            CameraInput();
            Cursor_Controller();

            #region Check If Player Can Move (Then Movement Funtions)
            if (!cc.lockMovement)
            {
                MoveCharacter();
                SprintInput();
                StrafeInput();
                JumpInput();
            }
            #endregion 
        }

        #region Basic Player (++ Reference to Sprint & Jump In Cc) Locomotion Inputs      

        protected virtual void MoveCharacter()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.y = Input.GetAxis(verticallInput);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput))
                cc.Jump();
        }

        //Cursor (TOGGLE ON AND OFF)
        protected virtual void Cursor_Controller()
        {
            // just a example to quit the application 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!Cursor.visible)
                    Cursor.visible = true;
                else
                if(Cursor.visible == true)
                {
                    Cursor.visible = false;
                }
            }
        }

        #endregion

        #region Camera Methods

        protected virtual void CameraInput()
        {
            if (tpCamera == null)
                return;
            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);

            // tranform Character direction from camera if not KeepDirection
            if (!keepDirection)
                cc.UpdateTargetDirection(tpCamera != null ? tpCamera.transform : null);
            // rotate the character with the camera while strafing        
            RotateWithCamera(tpCamera != null ? tpCamera.transform : null);
        }

        protected virtual void UpdateCameraStates()
        {
            // CAMERA STATE - you can change the CameraState here, the bool means if you want lerp of not, make sure to use the same CameraState String that you named on TPCameraListData
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void RotateWithCamera(Transform cameraTransform)
        {
            if (cc.isStrafing && !cc.lockMovement && !cc.lockMovement)
            {
                cc.RotateWithAnotherTransform(cameraTransform);
            }
        }

        #endregion     
    }
    #endregion
}