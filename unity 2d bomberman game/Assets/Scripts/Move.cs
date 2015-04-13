using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public static float speed = 0.1f;

    Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
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
			other.gameObject.SetActive (false);
			Bomb.count += 1;
		} else if (other.gameObject.tag == "PickUpSpeed") {
			other.gameObject.SetActive (false);
			speed += 0.1f;
		} else if (other.gameObject.tag == "PickUpLimit") {
			other.gameObject.SetActive (false);
			BombDrop.bomb_limit += 1;
		}
	}
}