/*
Program contains all of the .
*/
using System;
using System.Threading;

namespace BDVR_Dedicated_Server
{
    class Program
    {
        private static bool isRunning = false;

        // main server method
        static void Main(string[] args)
        {
            Console.Title = $"BDVR Dedicated Server ({Constants.VERSION})";
            isRunning = true;

            Thread mainThread = new Thread(new ThreadStart(MainThread));
            mainThread.Start();

            Server.Start(Constants.MAX_PLAYERS, Constants.PORT);
        }

        // logic for the main server thread (has Update() loop)
        private static void MainThread()
        {
            Console.WriteLine($"Started main thread. Running at {Constants.TICKS_PER_SEC} TPS");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    GameLogic.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK);

                    if (_nextLoop > DateTime.Now)
                        Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
    }
}
