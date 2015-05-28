using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using System.Threading;

using UnityEditor;

public class SynchronousClient : MonoBehaviour
{
	public static string ip;

    //booleans for connected characters
    public static bool player1_disconnect = false;
    public static bool player2_disconnect = false;
    public static bool player3_disconnect = false;
    public static bool player4_disconnect = false;

    //booleans for alive characters
    public static bool player1_alive = true;
    public static bool player2_alive = true;
    public static bool player3_alive = true;
    public static bool player4_alive = true;


    public int PlayerIndex;
    public static string strPlayerIndex;

    public static string PlayerName;

    public GameObject bomberman;

    public GameObject[] bombermans;
    public GameObject bman1; public GameObject bman2; public GameObject bman3; public GameObject bman4;
    public Animator anim;
    public AudioSource source;

    //hold position updates for bomberman clients
    public static Vector3[] positions = new Vector3[4]; 

	//position of new bomb
	
	public static Vector3 bomb_position = new Vector3();

	//for making the bomb from server to client
	public GameObject bomb_prefab;
	public static bool bomb_set_off = false;
	
	public static Queue<Vector2> q_of_bombs = new Queue<Vector2>();



    //what needs to be sent over unity?
    //players movement + position, bomb placements, map updates, powerups, score

    public class SynchronousSocketClient
    {
        public Socket sender;

        public SynchronousSocketClient()
        {
        }

        public void StartClient()
        {
            // Data buffer for incoming data.
            byte[] bytes = new byte[1024];

            // Connect to a remote device.
            try
            {
                // Establish the remote endpoint for the socket.
                // This example uses port 11000 on the local computer.
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = IPAddress.Parse(ip);
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.
                sender = new Socket(AddressFamily.InterNetwork,
                                           SocketType.Stream, ProtocolType.Tcp);

                // Connect the socket to the remote endpoint. Catch any errors.
                try
                {
                    sender.Connect(remoteEP);
                    print("Socket connected to {0}" + sender.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes(PlayerName + " has successfully connected<EOF>");


                    // Send the data through the socket.
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = sender.Receive(bytes);
                    print("Echoed test = {0}" +
                                      Encoding.ASCII.GetString(bytes, 0, bytesRec));
                    strPlayerIndex = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    MessageListener ml = new MessageListener(sender);
                    Thread t = new Thread(new ThreadStart(ml.ReceivingThread));
                    t.Start();

                    //Data handler thread.
                    Thread dh = new Thread(new ThreadStart(ml.DataHandler));
                    dh.Start();


                }
                catch (ArgumentNullException ane)
                {
                    print("ArgumentNullException : {0}" + ane.ToString());
                }
                catch (SocketException se)
                {
                    print("SocketException : {0}" + se.ToString());
                }
                catch (Exception e)
                {
                    print("Unexpected exception : {0}" + e.ToString());
                }

            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }
    }

    public class MessageListener
    {

        public bool msg_received = false;
        byte[] bytes = new Byte[1024];
        Socket sender;
        String data;
        Queue dataQ = new Queue();
        AutoResetEvent lock_thread = new AutoResetEvent(false);
        char playernum;
		Vector3 b_pos;


        public MessageListener(Socket s)
        {
            sender = s;
            playernum = strPlayerIndex[0];
        }

        //blocking collection
        //take
        public void ReceivingThread()
        {
            // Data buffer for incoming data.
            while (true)
            {
                int bytesRec = sender.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                dataQ.Enqueue(data);
                lock_thread.Set();
            }
        }

        //receive data from server
        public void DataHandler()
        {
            Vector3 pos;
            while (true)
            {
                // Show the data on the console.
                 
                lock_thread.WaitOne();
                string msg = (string)dataQ.Dequeue();
                //check if playerindex is not equal to this clients playerindex
				
				string[] split_msg = msg.Split(new char[] { '/', '(', ')', ',' });

                //check player disconnects
                if (msg[0] != playernum && msg[2] == 'X')
                {
                     // check if false??
                    if (msg[0] == '0')
                        player1_disconnect = true;
                    else if (msg[0] == '1')
                        player2_disconnect = true;
                    else if (msg[0] == '2')
                        player3_disconnect = true;
                    else if (msg[0] == '3')
                        player4_disconnect = true;
                }

                //check player death
                if (msg[0] != playernum && msg[2] == 'D')
                {
                    if (msg[0] == '0')
                        player1_alive = false;
                    else if (msg[0] == '1')
                        player2_alive = false;
                    else if (msg[0] == '2')
                        player3_alive = false;
                    else if (msg[0] == '3')
                        player4_alive = false;
                }


				if (msg[0] != playernum && msg != (PlayerName + " has successfully connected<EOF>") && msg[2] != 'B')
                {
                    //split up the message
					
					string[] split = msg.Split(new Char[] { '/', '(', ')', ',' });
                    //get x,y,z to create vector 3
                    pos.x = Single.Parse(split[1].Trim());
                    pos.y = Single.Parse(split[2].Trim());
                    pos.z = Single.Parse(split[3].Trim());
                    //store new position into positions array
                    positions[Convert.ToInt32(split[0])] = pos;
                    
                }

				//obtaining a bomb_position
				if(msg[0] != playernum && msg[2] == 'B'){
					string[] sp = msg.Split(new Char[] { '/', '(', ')', ',' });
					Vector3 bomb_pos = new Vector3(Single.Parse (sp[2].Trim()), Single.Parse (sp[3].Trim()), 0);
					bomb_position = bomb_pos;
					bomb_set_off = true;
				}
            }
        }
    }


	//Synchronous Client class mv's
    public SynchronousSocketClient synch_client;
    public Vector2 self_position;
    public float self_x, self_y, self_z;
	public Animator[] animations;

	public int count = 0;


    // Use this for initialization
    void Start()
    {
		
		bombermans = new GameObject[4];
        bombermans[0] = bman1;
        bombermans[1] = bman2;
        bombermans[2] = bman3;
        bombermans[3] = bman4;


        synch_client = new SynchronousSocketClient();
        synch_client.StartClient();
        //check x, y, z
        // <0
        if (strPlayerIndex.StartsWith("0"))
        {
            bomberman = bombermans[0];
            PlayerIndex = 0;
            anim = bombermans[0].GetComponent<Animator>();
            source = bombermans[0].GetComponent<AudioSource>();
        }
        else if (strPlayerIndex.StartsWith("1"))
        {
            bomberman = bombermans[1];
            PlayerIndex = 1;
            anim = bombermans[1].GetComponent<Animator>();
            source = bombermans[1].GetComponent<AudioSource>();
        }
        else if (strPlayerIndex.StartsWith("2"))
        {
            bomberman = bombermans[2];
            PlayerIndex = 2;
            anim = bombermans[2].GetComponent<Animator>();
            source = bombermans[2].GetComponent<AudioSource>();
        }
        else if (strPlayerIndex.StartsWith("3"))
        {
            bomberman = bombermans[3];
            PlayerIndex = 3;
            anim = bombermans[3].GetComponent<Animator>();
            source = bombermans[3].GetComponent<AudioSource>();
        }
        Vector2 self_position = bomberman.transform.position;
        self_x = bomberman.transform.position.x;
        self_y = bomberman.transform.position.y;

        //initialize bomberman objects positions
        for (int i = 0; i < 4; ++i)
        {
            positions[i] = bombermans[i].transform.position;
        }
		animations = new Animator[4];
		animations[0] = bman1.GetComponent<Animator>();
		animations[1] = bman2.GetComponent<Animator>();
		animations[2] = bman3.GetComponent<Animator>();
		animations[3] = bman4.GetComponent<Animator>();
	}
	
	public int timer = 0;
	public float curr_x, curr_y;
	public float curr_ox, curr_oy;


    void Update()
    {
        //delete disconnected players?
        if (player1_disconnect == true && bombermans[0] != null)
        {
            Destroy(bombermans[0]);
            player1_disconnect = false; //set to false so we don't do the check again on something that doesn't exist
        }
        if (player2_disconnect == true && bombermans[1] != null)
        {
            Destroy(bombermans[1]);
            //player2_disconnect = false;
        }
        if (player3_disconnect == true && bombermans[2] != null)
        {
            Destroy(bombermans[2]);
            //player3_disconnect = false;
        }
        if (player4_disconnect == true && bombermans[3] != null)
        {
            Destroy(bombermans[3]);
            //player4_disconnect = false;
        }

        if (player1_alive == false && bombermans[0] != null)
        {
            Destroy(bombermans[0]);
        }
        if (player2_alive == false && bombermans[1] != null)
        {
            Destroy(bombermans[1]);
        }
        if (player3_alive == false && bombermans[2] != null)
        {
            Destroy(bombermans[2]);
        }
        if (player4_alive == false && bombermans[3] != null)
        {
            Destroy(bombermans[3]);
        }

        if (timer % 2 == 0 && bomberman != null)
        {
            //Vector2 current_position = bomberman.transform.position;
            curr_x = bomberman.transform.position.x;
            curr_y = bomberman.transform.position.y;

            if (Math.Round(self_x,3) == Math.Round(curr_x,3) && Math.Round(self_y,3) == Math.Round(curr_y,3))//Math.Abs(self_x - curr_x) > .1 || Math.Abs(self_y - curr_y) > .1 )
            {
				//count++;
            }
			else{
				
				byte[] msg = Encoding.ASCII.GetBytes(PlayerIndex.ToString() + bomberman.transform.position.ToString() + "<EOF>");
				synch_client.sender.Send(msg);
				//self_position = current_position;
				
				self_x = curr_x;
				self_y = curr_y;

				//count = 0;
			}
            timer = 1;
        }

        if (EditorApplication.isPlaying == false)
        {
            byte[] msg1 = Encoding.ASCII.GetBytes(PlayerIndex.ToString() + "Disconnect <EOF>");
            synch_client.sender.Send(msg1);
        }

        timer++;

		for(int x = 0; x < 4; x++){
			if( x != PlayerIndex && bombermans[x] != null){
				curr_ox = bombermans[x].transform.position.x;
				curr_oy = bombermans[x].transform.position.y;
				
				if ( (positions[x].y > curr_oy )) {
					animations[x].SetInteger ("Direction", 0); // up
				} else if ((positions[x].x > curr_ox )) {
					animations[x].SetInteger ("Direction", 1); // right
				} else if ((positions[x].y < curr_oy)) {
					animations[x].SetInteger ("Direction", 2); // down
				} else if ((positions[x].x < curr_ox)) {
					animations[x].SetInteger ("Direction", 3); // left
				} 
			}
		}
		
		//update positions for other bombermans
        for (int i = 0; i < 4; ++i)
        {
            if (i != PlayerIndex && bombermans[i] != null)
            {
				if(bombermans[i].transform.position != positions[i])
                	bombermans[i].transform.position = positions[i];
			}
        }


		
		if (bomb_set_off) {
			Instantiate (bomb_prefab, bomb_position, Quaternion.identity);
			bomb_set_off = false;
		}
    }
}
