/*
NetworkedObject ...
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace BDVR_Dedicated_Server
{
    class NetworkedObject
    {
        public string UID;
        public string name;

        public Vector3 position;
        public Quaternion rotation;

        /// <summary>
        /// Constructs a new Item with given properties
        /// </summary>
        /// <param name="_UID">Item UID</param>
        /// <param name="_name">Item name</param>
        /// <param name="_pos">Item position</param>
        /// <param name="_rot">Item rotation</param>
        public NetworkedObject(string _UID, string _name, Vector3 _pos, Quaternion _rot)
        {
            UID = _UID;
            name = _name;
            position = _pos;
            rotation = _rot;
        }


    }
}
