using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public static float speed = 0.1f;

    Animator anim;

	public AudioClip fire_powerup;
	public AudioClip speed_powerup;
	public AudioClip bomb_powerup;
	public AudioClip step_sound;

	private AudioSource source;

    void Start() {
        anim = GetComponent<Animator>();
    }

	void Awake(){
		source = GetComponent<AudioSource> ();
	}

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
        transform.Translate(dir);
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "PickUpFlame") {
			source.PlayOneShot (fire_powerup);
			other.gameObject.SetActive (false);
			Bomb.count += 1;
		} else if (other.gameObject.tag == "PickUpSpeed") {
			source.PlayOneShot (speed_powerup);
			other.gameObject.SetActive (false);
			speed += 0.03f;
		} else if (other.gameObject.tag == "PickUpLimit") {
			source.PlayOneShot (bomb_powerup);
			other.gameObject.SetActive (false);
			BombDrop.bomb_limit += 1;
		}
	}
}