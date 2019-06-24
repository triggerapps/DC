using UnityEngine;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

namespace Invector.CharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region variables

        [Header("Default Inputs")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;

        [Header("Camera Settings")]
        public string rotateCameraXInput ="Mouse X";
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

        #region Start()
        protected virtual void Start()
        {
         
            //without it the scene will freeze midair, this calls the Init() for cc, which is in the VthirdPersonMotor
            moveCharacter();
            //this controls the Camera and connect it to follow player gameObject
            OnStartFollowing();

        }
        //
        #region Movements Methods
        #region CharacterMovement Method
        protected virtual void moveCharacter()
        {
            cc = GetComponent<vThirdPersonController>();
            if (cc != null)
                cc.Init();



            //this is the only connecter for the camera to follow the player
            //without it the camera will be in the scene... but not behind the player

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        #region CameraMovement Method
        protected virtual void OnStartFollowing()
        {
            tpCamera = FindObjectOfType<vThirdPersonCamera>();
            if (tpCamera) tpCamera.SetMainTarget(this.transform);
        }
        #endregion
        #endregion
        //

        #endregion
        //
        #endregion

        #region Update()

        protected virtual void Update()
        {


            //call in Update or Late Update only, doesn't work in start, controls the camera
              InputHandle();                      

            //DIRECT: call ThirdPersonMotor methods; this give direct access to vthirdpersonMotor, and it does the Update      
             cc.UpdateMotor();

            //DIRECT: call ThirdPersonAnimator methods; this give direct access to vthirdpersonMotor, and it does the Update
             cc.UpdateAnimator();  
       
            //
        }

        #endregion

        //METHOD to call

        #region  InputHandle Method     

        //Basic' 'Locomotion Inputs' Region Controller which is below
        protected virtual void InputHandle()
        {
            ExitGameInput();
            //this can only be called here
            CameraInput();

            if (!cc.lockMovement)
            {
                MoveCharacter();
                SprintInput();
                StrafeInput();
                JumpInput();
            }
        }
        #region Locomotion Inputs (needed for camera to work)
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
            else if(Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput))
                cc.Jump();
        }
        #endregion
        //


        #region Camera Input Methods

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


        protected virtual void RotateWithCamera(Transform cameraTransform)
        {
            if (cc.isStrafing && !cc.lockMovement && !cc.lockMovement)
            {
                cc.RotateWithAnotherTransform(cameraTransform);
            }
        }

        #endregion
        //
        
        #endregion

        #region Escape & Cursor Input Method            (quit the game as well)
        //Exc button and Cursor Input Method to be called above
        protected virtual void ExitGameInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!Cursor.visible)
                    Cursor.visible = true;
                else
                    Application.Quit();
            }
        }
        //
        #endregion
  
    }
}