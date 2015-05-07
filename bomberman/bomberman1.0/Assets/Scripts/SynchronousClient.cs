using UnityEngine;
using System.Collections;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;

public class SynchronousClient : MonoBehaviour {

	//Player Index (of array)
	//Player 1, 2, 3, 4 = index 0, 1, 2, 3 respectively.
    public int PlayerIndex;

	public static string PlayerName;
	//what needs to be sent over unity?
	//players movement + position, bomb placements, map updates, powerups, score

	public class SynchronousSocketClient {
		public Socket sender;
		public SynchronousSocketClient(){

		}
		
		public void StartClient() {
			// Data buffer for incoming data.
			byte[] bytes = new byte[1024];
			
			// Connect to a remote device.
			try {
				// Establish the remote endpoint for the socket.
				// This example uses port 11000 on the local computer.
				IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
				IPAddress ipAddress = ipHostInfo.AddressList[0];
				IPEndPoint remoteEP = new IPEndPoint(ipAddress,11000);
				
				// Create a TCP/IP  socket.
				sender = new Socket(AddressFamily.InterNetwork, 
				                           SocketType.Stream, ProtocolType.Tcp );
				
				// Connect the socket to the remote endpoint. Catch any errors.
				try {
					sender.Connect(remoteEP);
					print("Socket connected to {0}" + sender.RemoteEndPoint.ToString());
					
					// Encode the data string into a byte array.
					byte[] msg = Encoding.ASCII.GetBytes(PlayerName + " has successfully connected<EOF>");
					
					// Send the data through the socket.
					int bytesSent = sender.Send(msg);
					
					// Receive the response from the remote device.
					int bytesRec = sender.Receive(bytes);
					print("Echoed test = {0}"+
					                  Encoding.ASCII.GetString(bytes,0,bytesRec));

					MessageListener ml = new MessageListener(sender);
					Thread t = new Thread(new ThreadStart(ml.ReceivingThread));
               		t.Start();
					

				} catch (ArgumentNullException ane) {
					print("ArgumentNullException : {0}" + ane.ToString());
				} catch (SocketException se) {
					print("SocketException : {0}" + se.ToString());
				} catch (Exception e) {
					print("Unexpected exception : {0}" + e.ToString());
				}
				
			} catch (Exception e) {
				print( e.ToString());
			}
		}		
	}

	public class MessageListener{
		public MessageListener(){

		}

		byte[] bytes = new Byte[1024];
	    Socket sender;
	    string data;

	    public MessageListener(Socket s){
	        sender = s;
	    }

	    public void ReceivingThread(){
	        // Data buffer for incoming data.
	        while (true) {
	            int bytesRec = sender.Receive(bytes);
	            data = Encoding.ASCII.GetString(bytes,0,bytesRec);
	            
	            //Data handler thread.
	            Thread dh = new Thread(new ThreadStart(DataHandler));
	            dh.Start();
	        }
    	}

    	public void DataHandler(){
        	// Show the data on the console.
        	print( "Echoed Text received : {0}"+ data);
    	}
	}

	public SynchronousSocketClient synch_client = new SynchronousSocketClient ();

	// Use this for initialization
	void Start () {		
		synch_client.StartClient ();
	}

	public int timer = 1;

	void Update() {
		if (timer % 5 == 0){
			byte[] msg = Encoding.ASCII.GetBytes("Scooby Doo<EOF>");
			synch_client.sender.Send(msg);
			timer = 1;
		}
		timer++;
	}	
}
