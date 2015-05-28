using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using System.Collections;
using System.Collections.Generic;

public class ClientConnection
{
    byte[] bytes = new Byte[1024];
    Socket handler;
    List<Socket> list_of_clients;
    string data;
    Queue dataQ = new Queue();
    AutoResetEvent lock_thread = new AutoResetEvent(false);
    int players_connected;

    public ClientConnection(Socket s, int ps_connected, List<Socket> a)
    {
        handler = s;
        players_connected = ps_connected;
        list_of_clients = a;
    }

    public void ReceivingThread()
    {
        // Data buffer for incoming data.
        while (true)
        {
            int bytesRec = handler.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

            dataQ.Enqueue(data);
            lock_thread.Set();

        }
    }

    //send from server to client
    public void DataHandler()
    {

        while (true)
        {
            try
            {
                // Show the data on the console.
                lock_thread.WaitOne();
                Console.WriteLine("Text received: {0}", dataQ.Peek());
                if (players_connected <= 3)
                {
                    //players_connected++;
                    byte[] player_index = Encoding.ASCII.GetBytes(players_connected.ToString() + "<EOF>");
                    handler.Send(player_index);
                    Console.WriteLine(players_connected);

                }
                // Echo the data back to the client.
                string byte_str = (string)dataQ.Dequeue();

                if (byte_str.Length > 3)
                {
                    Console.WriteLine("WE ARE HERE");
                    if (byte_str[0] == '0' && byte_str[2] == 'X')
                    {
                        list_of_clients[0].Shutdown(SocketShutdown.Both);
                        list_of_clients[0] = null;
                    }
                    else if (byte_str[0] == '1' && byte_str[2] == 'X')
                    {
                        list_of_clients[1].Shutdown(SocketShutdown.Both);
                        list_of_clients[1] = null;
                    }
                    else if (byte_str[0] == '2' && byte_str[2] == 'X')
                    {
                        list_of_clients[2].Shutdown(SocketShutdown.Both);
                        list_of_clients[2] = null;
                    }
                    else if (byte_str[0] == '3' && byte_str[2] == 'X')
                    {
                        list_of_clients[3].Shutdown(SocketShutdown.Both);
                        list_of_clients[3] = null;
                    }


                }

                byte[] msg = Encoding.ASCII.GetBytes(byte_str); //change data string to bytes for the message
                //handler.Send(msg); // send the bytes



                foreach (Socket s in list_of_clients)
                {
                    if (s != null)
                    {
                        s.Send(msg);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}


public class SynchronousSocketListener
{

    public static int ps = 0;
    public static List<Socket> client_list = new List<Socket>();

    public static void StartListening()
    {
        IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
            //int players_connected = -1;
            // Start listening for connections.
            while (true)
            {
                Console.WriteLine("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();

                client_list.Add(handler);

                // An incoming connection needs to be processed.
                ClientConnection cc = new ClientConnection(handler, ps, client_list);
                Thread t = new Thread(new ThreadStart(cc.ReceivingThread));
                t.Start();

                //Data handler thread.
                Thread dh = new Thread(new ThreadStart(cc.DataHandler));
                dh.Start();

                ps++;
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}