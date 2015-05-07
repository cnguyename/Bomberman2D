using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ClientConnection
{
    byte[] bytes = new Byte[1024];
    Socket handler;
    string data;

    public ClientConnection(Socket s){
        handler = s;
    }

    public void ReceivingThread(){
        // Data buffer for incoming data.
        while (true) {
            int bytesRec = handler.Receive(bytes);
            data = Encoding.ASCII.GetString(bytes,0,bytesRec);

            //Data handler thread.
            Thread dh = new Thread(new ThreadStart(DataHandler));
            dh.Start();
        }
    }

    public void DataHandler(){
        // Show the data on the console.
        Console.WriteLine( "Text received : {0}", data);

        // Echo the data back to the client.
        byte[] msg = Encoding.ASCII.GetBytes(data); //change data string to bytes for the message
        handler.Send(msg); // send the bytes

    }
}


public class SynchronousSocketListener {

    public static void StartListening() {
        
        // Establish the local endpoint for the socket.
        // Dns.GetHostName returns the name of the 
        // host running the application.
        IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        // Create a TCP/IP socket.
        Socket listener = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp );

        // Bind the socket to the local endpoint and 
        // listen for incoming connections.
        try {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.
            while (true) {
                Console.WriteLine("Waiting for a connection...");
                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();
                
                // An incoming connection needs to be processed.
                ClientConnection cc = new ClientConnection(handler);
                Thread t = new Thread(new ThreadStart(cc.ReceivingThread));
                t.Start();
            }
            
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    public static int Main(String[] args) {
        StartListening();
        return 0;
    }
}