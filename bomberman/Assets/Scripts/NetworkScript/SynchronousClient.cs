using UnityEngine;
using System.Collections;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class SynchronousClient : MonoBehaviour {

	
	public class SynchronousSocketClient {

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
				Socket sender = new Socket(AddressFamily.InterNetwork, 
				                           SocketType.Stream, ProtocolType.Tcp );
				
				// Connect the socket to the remote endpoint. Catch any errors.
				try {
					sender.Connect(remoteEP);
					
					print("Socket connected to {0}"+
					                  sender.RemoteEndPoint.ToString());
					
					// Encode the data string into a byte array.
					byte[] msg = Encoding.ASCII.GetBytes("Today is 04/29/2015!<EOF>");
					
					// Send the data through the socket.
					int bytesSent = sender.Send(msg);
					
					// Receive the response from the remote device.
					int bytesRec = sender.Receive(bytes);
					print("Echoed test = {0}"+
					                  Encoding.ASCII.GetString(bytes,0,bytesRec));
					
					// Release the socket.
					//sender.Shutdown(SocketShutdown.Both);
					//sender.Close();
					
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
	// Use this for initialization
	void Start () {
		SynchronousSocketClient synch_client = new SynchronousSocketClient ();
		synch_client.StartClient ();
	
	}
}
