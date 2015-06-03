using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using System.Collections;
using System.Collections.Generic;

public class ClientConnection
{
    static Dictionary<String,List<ClientConnection>> sessionDict = new Dictionary<String,List<ClientConnection>>();
    byte[] bytes = new Byte[1024];
    Socket handler;
    List<Socket> list_of_clients;
    string data;
    string playerId;
    Queue dataQ = new Queue();
    AutoResetEvent lock_thread = new AutoResetEvent(false);
    //int players_connected;
    String session;

    public ClientConnection(Socket s, int ps_connected, List<Socket> a)
    {
        handler = s;
        //players_connected = ps_connected;
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
            // Show the data on the console.
            lock_thread.WaitOne();
            Console.WriteLine("Text received: {0}", dataQ.Peek());

            // Echo the data back to the client.
            if (!dataQ.Peek().ToString().StartsWith("L")) // if (message == update) - > forward
            {
                string byte_str = (string)dataQ.Dequeue();
				foreach (ClientConnection c in ClientConnection.sessionDict[this.session])
				{
					
					Console.WriteLine(byte_str);	
					byte[] msg = Encoding.ASCII.GetBytes(byte_str);
					c.handler.Send(msg);
				}


            }
            else if (dataQ.Peek().ToString().StartsWith("L"))  // if (message == login) set playerid and session
            {
                string byte_str = (string)dataQ.Dequeue();
                session = byte_str.Split(',')[1];
				playerId = byte_str.Split(',')[2];
				if (!ClientConnection.sessionDict.ContainsKey (session))
                    ClientConnection.sessionDict.Add (session, new List<ClientConnection>());
                ClientConnection.sessionDict[session].Add(this);
				int count = 0;
                foreach (ClientConnection c in ClientConnection.sessionDict[this.session])
                {
					Console.WriteLine(c.playerId);
					byte[] msg = Encoding.ASCII.GetBytes("P," + count.ToString() + "," + playerId);
					c.handler.Send(msg);
					count++;
                }
            }
 			//change data string to bytes for the message
            // if ( message == update) -> forward
           
            // if (message == login) -> playerId = msg[0]; session = msg[1]; 
            // if ClientConnections.sessions[session] == null; -> new List<CLientConnection>();
            //  ClientConnections.sessions[session].Add(this)


            // foreach (ClientConnection c in ClientConncetions.sessions[this.session])
            // c.handler.Send(msg)

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

        Console.WriteLine(ipAddress);

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