using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public float speed = 0.075f;

    public GameObject[] bombermans;

    Animator anim;

	public AudioClip fire_powerup;
	public AudioClip speed_powerup;
	public AudioClip bomb_powerup;
	public AudioClip step_sound;

    public GameObject bman1;
    public GameObject bman2;
    public GameObject bman3;
    public GameObject bman4;

    public GameObject ClientObject;

    public SynchronousClient client;
    

	private AudioSource source;

    void Start() {
        bombermans = new GameObject[4];
        bombermans[0] = bman1;
        bombermans[1] = bman2;
        bombermans[2] = bman3;
        bombermans[3] = bman4;
        client = ClientObject.GetComponent<SynchronousClient>();
        anim = bombermans[client.PlayerIndex].GetComponent<Animator>();
        source = bombermans[client.PlayerIndex].GetComponent<AudioSource>();
    }

    //void Awake()
    //{
    //    source = bombermans[client.PlayerIndex].GetComponent<AudioSource>();
    //}

	void FixedUpdate () {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            anim.SetInteger("Direction", 0); // up
            dir.y = speed;
		} else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
            anim.SetInteger("Direction", 1); // right
            dir.x = speed;
		} else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) {
            anim.SetInteger("Direction", 2); // down
            dir.y = -speed;
		} else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
            anim.SetInteger("Direction", 3); // left
            dir.x = -speed;
        } else {
            // idle
            anim.SetInteger("Direction", 4);
        }
        bombermans[client.PlayerIndex].transform.Translate(dir);
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "PickUpFlame") {
			source.PlayOneShot (fire_powerup);
			other.gameObject.SetActive (false);
            Debug.Log("before");
            Debug.Log(Bomb.count);
			Bomb.count += 1;
            Debug.Log("after");
            Debug.Log(Bomb.count);
		} else if (other.gameObject.tag == "PickUpSpeed") {
			source.PlayOneShot (speed_powerup);
			other.gameObject.SetActive (false);
			speed += 0.015f;
		} else if (other.gameObject.tag == "PickUpLimit") {
			source.PlayOneShot (bomb_powerup);
			other.gameObject.SetActive (false);
			BombDrop.bomb_limit += 1;
		}
	}
}