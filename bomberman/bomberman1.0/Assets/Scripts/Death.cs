using UnityEngine;
using System.Collections;

public class Death : MonoBehaviour {

    public AudioSource source;

    public AudioClip fire_powerup;
    public AudioClip speed_powerup;
    public AudioClip bomb_powerup;
    public AudioClip step_sound;

    public GameObject mover;

    public Move Character;

	// Use this for initialization
	void Start () {
        Character = mover.GetComponent<Move>();
        source = GetComponent<AudioSource>();
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
            BombDrop.bomb_limit += 1;
        }
    }
  
}
