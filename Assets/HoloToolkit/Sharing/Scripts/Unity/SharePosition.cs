using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Sharing;
using HoloToolkit.Unity;

namespace HoloToolkit.Sharing.Tests
{
    public class SharePosition : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            CustomMessages.Instance.MessageHandlers[CustomMessages.TestMessageID.StageTransform] = this.OnStageTransfrom;
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// When a remote system has a transform for us, we'll get it here.
        /// </summary>
        /// <param name="msg"></param>
        void OnStageTransfrom(NetworkInMessage msg)
        {
            Debug.Log("tamere");
            // We read the user ID but we don't use it here.
            msg.ReadInt64();

            transform.localPosition = CustomMessages.Instance.ReadVector3(msg);
            transform.localRotation = CustomMessages.Instance.ReadQuaternion(msg);

            //add any scripts you want.

        }

    }
}
