using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public float speed = 0.075f;

    public GameObject ClientObject;

    public SynchronousClient client;
    

    void Start() {
        client = ClientObject.GetComponent<SynchronousClient>();
    }

    //void Awake()
    //{
    //    source = bombermans[client.PlayerIndex].GetComponent<AudioSource>();
    //}

	void FixedUpdate () {
		if (client.bombermans[client.PlayerIndex] != null) {
			Vector2 dir = Vector2.zero;
			if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
				client.anim.SetInteger ("Direction", 0); // up
				dir.y = speed;
			} else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
				client.anim.SetInteger ("Direction", 1); // right
				dir.x = speed;
			} else if (Input.GetKey (KeyCode.DownArrow) || Input.GetKey (KeyCode.S)) {
				client.anim.SetInteger ("Direction", 2); // down
				dir.y = -speed;
			} else if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
				client.anim.SetInteger ("Direction", 3); // left
				dir.x = -speed;
			} else {
				// idle
				client.anim.SetInteger ("Direction", 4);
			}
			client.bombermans [client.PlayerIndex].transform.Translate (dir);
		}
    }

}