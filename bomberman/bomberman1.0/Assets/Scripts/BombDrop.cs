using UnityEngine;
using System.Collections;


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;



public class BombDrop : MonoBehaviour {
    public GameObject bombPrefab;
       
    public float bomb_limit = 1;
	public static float bombs_on_field = 0;

    public GameObject mover;
    public Move Character;
	public SynchronousClient camera_sc;
	public GameObject camera_object;

    void Start()
    {
        Character = mover.GetComponent<Move>();
		camera_sc = camera_object.GetComponent<SynchronousClient> ();
    }

	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && bombs_on_field < bomb_limit ) {
			bombs_on_field += 1;
            Vector2 pos = Character.client.bombermans[Character.client.PlayerIndex].transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            Instantiate(bombPrefab, pos, Quaternion.identity);

			byte[] msg = Encoding.ASCII.GetBytes( camera_sc.PlayerIndex.ToString() +"," + "B" + pos.ToString() + "<EOF>");
			camera_sc.synch_client.sender.Send(msg);
        }
	}

}
