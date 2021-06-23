using System;
using System.Collections.Generic;
using System.Text;

namespace BDVR_Dedicated_Server
{
    static class Settings
    {
        public static int sceneToLoad = 1; // load classic by default
        public static List<int> serverAdmins = new List<int>();

        /* 
        // probably not gonna use this - just store an int in Player instead and let client handle it
 
        public static Dictionary<string, List<Vector3>> spawnPoints = new Dictionary<string, List<Vector3>>()
        {
            { "CLASSIC", new List<Vector3>() {}},
            { "ADVENTURE", new List<Vector3>() {}}
        };
        */
    }
}
