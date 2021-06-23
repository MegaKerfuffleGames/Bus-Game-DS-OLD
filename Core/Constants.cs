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
        // These should be configurable outside of VS
        public const int MAX_PLAYERS = 16;
        public const int PORT = 6900;
        public const string WELCOME_MSG = "Welcome to the server lol";

        // These should probably remain hidden
        public const string VERSION = "v0.7indev";
        public const int TICKS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICKS_PER_SEC;
        
        // World time on server start (24hr time)
        public const float WORLD_START_TIME = 12.0f;
    }
}
