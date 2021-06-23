/*
GameLogic provides some analogues to Unity's MonoBehaviour methods,
since C# doesn't natively support that stuff
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace BDVR_Dedicated_Server
{
    class GameLogic
    {
        // equivalent to Unity's Update() method
        public static void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}
