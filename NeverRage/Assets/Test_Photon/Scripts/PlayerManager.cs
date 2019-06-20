#region photon
using Photon.Pun;
#endregion
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

// [static] keyword, meaning that this variable is available
//so you can simply do GameManager.Instance.xxx()  (referencing it)
namespace Com.TriggerAppsProduction.NeverRage
{
    #region PlayerManager
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Private Fields For Loading Scenes
#if UNITY_5_4_OR_NEWER
void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
{
    this.CalledOnLevelWasLoaded(scene.buildIndex);
}
#endif
        #region Shooting Script: Beams 01 : Boolean

        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        //True, when the user is firing
        bool IsFiring;

        #endregion

        #endregion

        #region Public Fields

        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion

        #region MonoBehaviour Callbacks

        void Awake()
        {
            // #Important
            #region Photon: Hold Player Object For Instantiation
            // used in GameManager: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
            #endregion

            #region ShootScript: Beams 02 : Shooting Color and Activing beam
            if (beams == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }
            #endregion
        }


        void Start()
        {
            #region Photon: Register A Completion of A Loaded Scene
            // Unity 5.4 has a new scene management. 
            //register a method to call CalledOnLevelWasLoaded.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            #endregion

            #region Photon CameraWork Reference
            vThirdPersonCamera _cameraWork = this.gameObject.GetComponent<vThirdPersonCamera>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
            #endregion
        }
        void Update()
        {

            #region ShootScript: Show and Turn Off Beam
            if (photonView.IsMine)
            {
                ProcessInputs();
            }
            // trigger Beams active state
            if (beams != null && IsFiring != beams.activeSelf)
            {
                beams.SetActive(IsFiring);
            }
            #endregion

            #region ShootScript: Health Tracker
            if (Health <= 0f)
            {
                GameManager.Instance.LeaveRoom();
            }
            #endregion
        }

        #region IPunObservable implementation
        //see beam from others (sync)


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            #region Show other's Beams
            if (stream.IsWriting)
            {
                // I own this player: send the others our hit data
                stream.SendNext(IsFiring);
                //I own this player: only take health from the other player.
                stream.SendNext(Health);
            }
            else
            {
                // I own this damage: only hurt us
                this.IsFiring = (bool)stream.ReceiveNext();
                // I own this health: only subtract from mine Health
                this.Health = (float)stream.ReceiveNext();
            }
            #endregion

            #region Syn Health, Seperate my health from others
            if(stream.IsWriting)
            {
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }
            #endregion
        }


        #endregion



     #region Custom ShootScript: Shooting INPUT

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>

        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }

        #endregion

     #region Custom ShootScript: HIT Detection
        //SHOOTING DETECTION  
        //Detect Hits From Ray 
        //(Note: time frame is different; base on network speed, so time.deltime needs some research
        void OnTriggerEnter(Collider other)
        {
            #region Other Player Detect Ray Penetration
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            Health -= 0.1f;
            #endregion

        }
        //Dont Give Me The Damage I Did To Other Player
        void OnTriggerStay(Collider other)
        {
            #region Seperate My Player So I dont Take the Damage I Inflicted 
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine)
            {
                return;
            }
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
            Health -= 0.1f * Time.deltaTime;
            #endregion
        }

        #endregion



        #endregion


        #region Photon: Process Loading The Scenes and Player
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, 
            //spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }

        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        #endregion


    }
    #endregion
}