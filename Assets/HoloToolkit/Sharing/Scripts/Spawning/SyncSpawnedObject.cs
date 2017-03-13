//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//

using UnityEngine;
using HoloToolkit.Sharing.SyncModel;

namespace HoloToolkit.Sharing.Spawning
{
    /// <summary>
    /// A SpawnedObject contains all the information needed for another device to spawn an object in the same location
    /// as where it was originally created on this device.
    /// </summary>
    [SyncDataClass]
    public class SyncSpawnedObject : SyncObject
    {
        /// <summary>
        /// Transform (position, orientation and scale) for the object.
        /// </summary>
        //[SyncData] public SyncTransform Transform;
        [SyncData]
        public SyncVector3 Position;

        [SyncData]
        public SyncQuaternion Rotation;

        [SyncData]
        public SyncVector3 Scale;

        /// <summary>
        /// Name of the object.
        /// </summary>
        [SyncData] public SyncString Name;

        /// <summary>
        /// Path to the parent object in the game object.
        /// </summary>
        [SyncData] public SyncString ParentPath;

        /// <summary>
        /// Path to the object
        /// </summary>
        [SyncData] public SyncString ObjectPath;


        public GameObject GameObject { get; set; }

        public virtual void Initialize(string name, string parentPath)
        {
            Name.Value = name;
            ParentPath.Value = parentPath;
            ObjectPath.Value = string.Empty;
            if (!string.IsNullOrEmpty(ParentPath.Value))
            {
                ObjectPath.Value = ParentPath.Value + "/";
            }

            ObjectPath.Value += Name.Value;
        }
    }
}
