using UnityEngine;
#region  Libararies 
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
#endregion
#region
using System.Collections;
#endregion
/*Player Name inputField script that collects and stores user name*/
namespace Com.TriggerAppsProduction.NeverRage
{
    #region List Additional Scripts to Add
    [RequireComponent(typeof(InputField))]
    #endregion

    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants
        /*Name to be saved */

        const string playerNamePrefKey = "PlayerName";

        #endregion
        #region MonoBehaviour CallBacks
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during Initialization phase.
        /// </summary>

        void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();

            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }
            PhotonNetwork.NickName = defaultName;
        }
        #endregion

        #region Public Methods

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is Not Available");
                return;

            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
        #endregion
    }
        // Update is called once per frame
       

}