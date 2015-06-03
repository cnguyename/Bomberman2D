using UnityEngine;
using System.Collections;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


public class Death : MonoBehaviour {

    public AudioSource source;

    public AudioClip fire_powerup;
    public AudioClip speed_powerup;
    public AudioClip bomb_powerup;
    public AudioClip step_sound;

    public GameObject mover;

    public Move Character;

    public SynchronousClient camera_sc;

    public BombDrop bombtracker;

	// Use this for initialization
	void Start () {
        Character = mover.GetComponent<Move>();
        source = GetComponent<AudioSource>();
//        bombtracker = mover.bombermans[Character.client.PlayerIndex].GetComponent<BombDrop>();
        //bombtracker.bomb_limit += 1;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnDestroy(){
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Explosion")
        {
            string name = gameObject.name;
            byte[] msg = Encoding.ASCII.GetBytes(name[10].ToString() + "," + "D" + "," + "<EOF>");
			print (name[10].ToString() + "," + "D" + "," + "<EOF>");
            camera_sc.synch_client.sender.Send(msg);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "PickUpFlame")
        {
            source.PlayOneShot(fire_powerup);
            other.gameObject.SetActive(false);
            Debug.Log("before");
            Debug.Log(Bomb.count);
            Bomb.count += 1;
            Debug.Log("after");
            Debug.Log(Bomb.count);
        }
        else if (other.gameObject.tag == "PickUpSpeed")
        {
            source.PlayOneShot(speed_powerup);
            other.gameObject.SetActive(false);
            Character.speed += 0.015f;
        }
        else if (other.gameObject.tag == "PickUpLimit")
        {
            source.PlayOneShot(bomb_powerup);
            other.gameObject.SetActive(false);
			bombtracker.bomb_limit += 1;
        }
    }
  
}
