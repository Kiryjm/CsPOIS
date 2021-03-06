﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Warship
{
    public class EnemyServer
    {
        private const int Port = 50000;
        private char[,] Map;
        private TcpListener server;
        private Thread serverThread;
        private Form1 mainForm;


        public EnemyServer(char[,] HomeMap, Form1 mainForm)
        {
            Map = HomeMap;
            this.mainForm = mainForm;
        }

        public void serverStart()
        {
            server = new TcpListener(Port);
            serverThread = new Thread(ServerWorker);
            serverThread.Start();
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        private void ServerWorker()
        {
            server.Start();

            byte[] data;
            while (true)
            {
                Console.WriteLine("Ожидание подключений... ");

                // getting incoming connection
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Подключен клиент. Выполнение запроса...");

                // getting network stream for reading and writing
                NetworkStream stream = client.GetStream();

                IFormatter formatter = new BinaryFormatter();

                Message receivedMessage = (Message)formatter.Deserialize(stream);
                Message response = new Message();


                switch (receivedMessage.MessageType)
                {
                    case MessageType.startPlayerMessage:
                        response.ProcessId = Process.GetCurrentProcess().Id;
                        break;

                    case MessageType.pointMessage:
                        response.PointValue = Map[receivedMessage.Point.X, receivedMessage.Point.Y];
                        break;

                    case MessageType.turnMessage:
                        mainForm.unlockEnemyShips();
                        break;

                }


                // message for client

                // convert message to byte array
                data = ObjectToByteArray(response);

                // sending message
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Отправлено сообщение: {0}", response);
                stream.Close();
                client.Close();
            }
        }

    }
}
