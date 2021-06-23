/*
Constants contains a variety of immutable, constant values used by
the server. Can be used to configure parts of the server.
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace BDVR_Dedicated_Server
{
    class Constants
    {
        // server settings that maybe should be exposed
        public const int MAX_PLAYERS = 16;
        public const int PORT = 6900;
        public const string WELCOME_MSG = "Welcome to the server lol";

        // server settings that should probably be hidden
        public const string VERSION = "v0.7indev";
        public const int TICKS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SEC;
    }

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

    class Debug
    {
        public static void Log(string _msg)
        {
            Console.WriteLine($"[{DateTimeOffset.Now.ToString("T")}] [INFO] {_msg}");
        }
        public static void LogWarning(string _msg)
        {
            Console.WriteLine($"[{DateTimeOffset.Now.ToString("T")}] [WARNING] {_msg}");
        }
    }
}
