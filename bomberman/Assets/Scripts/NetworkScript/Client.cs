using UnityEngine;
using System.Collections;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

public class Client : MonoBehaviour {
	
	// State object for receiving data from remote device.
	public class StateObject {
		// Client socket.
		public Socket workSocket = null;
		// Size of receive buffer.
		public const int BufferSize = 256;
		// Receive buffer.
		public byte[] buffer = new byte[BufferSize];
		// Received data string.
		public StringBuilder sb = new StringBuilder();
		    // ManualResetEvent instances signal completion.
	    public ManualResetEvent connectDone =
	        new ManualResetEvent(false);

	    public ManualResetEvent receiveDone =
	        new ManualResetEvent(false);

	    public ManualResetEvent sendDone =
	        new ManualResetEvent(false);

		// The response from the remote device.
		public String response = String.Empty;
	}
	
	public class AsynchronousClient {

		//Empty Constructor
		public AsynchronousClient(){
		}

		// The port number for the remote device.
		public int port = 11000;
		static string[] stringSeparators = new string[] { "<EOF>" };
		
		public void StartClient() {
			// Connect to a remote device.
			try {
				// Establish the remote endpoint for the socket.
				// The name of the 
				// remote device is "host.contoso.com".
				IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
				IPAddress ipAddress = ipHostInfo.AddressList[0];
				IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
				
				// Create a TCP/IP socket.
				Socket client = new Socket(AddressFamily.InterNetwork,
				                           SocketType.Stream, ProtocolType.Tcp);

				StateObject send_state_object = new StateObject();
				send_state_object.workSocket = client;
				// Connect to the remote endpoint.
				client.BeginConnect( remoteEP, 
				                    new AsyncCallback(ConnectCallback), send_state_object);

				//Thread wait/halt
				send_state_object.connectDone.WaitOne(1000);

				// Send test data to the remote device.
				Send(client,"SentMsg", send_state_object);
				// Receive the response from the remote device.
				send_state_object.connectDone.WaitOne(1000);

				StateObject recv_state_object = new StateObject();
				recv_state_object.workSocket = client;

				//Receive(client);
				Receive(recv_state_object);
				recv_state_object.receiveDone.WaitOne(1000);
				// Write the response to the console.

				print("Response received : {0}"+ recv_state_object.response);

				/*
				// Release the socket.
				try {
					client.Shutdown(SocketShutdown.Both);
				}
				catch (SocketException e) {
					Console.WriteLine ("Socket closed remotely");
				}
				client.Close();
				*/

			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}
		
		public void ConnectCallback(IAsyncResult ar) {
			try {
				// Retrieve the socket from the state object.
				Socket client = (Socket) ar.AsyncState;
				
				// Complete the connection.
				client.EndConnect(ar);
				
				Console.WriteLine("Socket connected to {0}",
				                  client.RemoteEndPoint.ToString());
				
				// Signal that the connection has been made.

			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}
		
		public void Receive(StateObject state) {
			try {
				Socket client = state.workSocket;
				
				// Begin receiving the data from the remote device.
				client.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,
				                    new AsyncCallback(ReceiveCallback), state);
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}
		
		public void ReceiveCallback( IAsyncResult ar ) {
			 try {
            // Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject state = (StateObject) ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0) {
                // Found a 
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                string content = state.sb.ToString();
                
                String[] message = content.Split(stringSeparators, StringSplitOptions.None);
                if (message.Length == 2)
                {
                    state.receiveDone.Set();
                    state.response = message[0];

                    state.workSocket.Shutdown(SocketShutdown.Both);
                    state.workSocket.Close();
                    
                }
                else
                {
                    // Get the rest of the data.
                    //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                      //  new AsyncCallback(ReceiveCallback), state);
                }
            } else {
                Console.WriteLine("Connection close has been requested.");
                // Signal that all bytes have been received.
                
            }
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
		}
		
		public void Send(Socket client, String data, StateObject so) {
			// Convert the string data to byte data using ASCII encoding.
			byte[] byteData = Encoding.ASCII.GetBytes(data);


			
			// Begin sending the data to the remote device.
			client.BeginSend(byteData, 0, byteData.Length, 0,
			                 new AsyncCallback(SendCallback), so);
		}
		
		public void SendCallback(IAsyncResult ar) {

			try {
				// Retrieve the socket from the state object.
				Socket client = (Socket) ar.AsyncState;
				
				// Complete sending the data to the remote device.
				int bytesSent = client.EndSend(ar);
				Console.WriteLine("Sent {0} bytes to server.", bytesSent);
				
				// Signal that all bytes have been sent.
			} catch (Exception e) {
				Console.WriteLine(e.ToString());
			}
		}



	}

	void Start(){
		AsynchronousClient AsynchClient = new AsynchronousClient ();
		Thread.Sleep(3000);
		AsynchClient.StartClient ();
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
