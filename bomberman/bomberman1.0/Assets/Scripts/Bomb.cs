using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    // Time after which the bomb explodes
    float time = 3.0f;

	private AudioSource source;

	public AudioClip place_bomb;


    // Explosion Prefab
    public GameObject explosion;
    public GameObject rangeUp;
    public GameObject speedUp;
    public GameObject bombUp;

    public static float count = 1;

    public Vector3 scale;

    public static float scaleX = 0.9F;
    public static float scaleY = 0.9F;

	private bool left = false;
	private bool right = false;
	private bool up = false;
	private bool down = false;

    public GameObject[] destroyables;
    public float rnd;

	
    void Start()
    {
        destroyables = GameObject.FindGameObjectsWithTag("Destroyable");
        // Call the Explode function after a few seconds
        Invoke("Explode", time);
    }

	void Awake(){
		source = GetComponent<AudioSource>();

	}

    void SpawnPowerUp(float x, float y)
    {
        rnd = Random.Range(0.0f, 10.1f);
        for (int i = 0; i < destroyables.Length; ++i)
        {
            if (destroyables[i].transform.position == new Vector3(x, y))
            {
                if (rnd < 1.0f)
                {
                    Instantiate(rangeUp, new Vector2(x, y), Quaternion.identity);
                }
                else if (rnd > 1.0f && rnd < 2.0f)
                {
                    Instantiate(speedUp, new Vector2(x, y), Quaternion.identity);
                }
                else if (rnd > 2.0f && rnd < 3.0f)
                {
                    Instantiate(bombUp, new Vector2(x, y), Quaternion.identity);
                }
            }

        }
    }

    void Explode()
    {
        // Remove Bomb from game
        Destroy(gameObject);
        BombDrop.bombs_on_field -= 1;

        scale = explosion.transform.localScale;
        Vector2 pos = transform.position;
        Instantiate(explosion,transform.position, Quaternion.identity);
        Block.destroyBlockAt(pos.x, pos.y);
        for (int i = 1; i <= count; i++)
        {
            //Debug.Log(count);
			//up,right,down, and then left:
			if(Block.indestructibleBlockAt(pos.x, pos.y + i))
				up = true;
			if(Block.indestructibleBlockAt(pos.x + i, pos.y))
				right = true;
			if(Block.indestructibleBlockAt(pos.x, pos.y - i))
				down = true;
			if(Block.indestructibleBlockAt(pos.x - i, pos.y))
				left = true;

			//up
            if (!up)
            {
                Instantiate(explosion, new Vector2(pos.x, pos.y + i), Quaternion.identity);
                Block.destroyBlockAt(pos.x, pos.y + i);
                SpawnPowerUp(pos.x, pos.y + i);
            }
            //right
            if (!right)
            {
                Instantiate(explosion, new Vector2(pos.x + i, pos.y), Quaternion.identity);
                Block.destroyBlockAt(pos.x + i, pos.y);
                SpawnPowerUp(pos.x + i, pos.y);
            }
            //down
            if (!down)
            {
                Instantiate(explosion, new Vector2(pos.x, pos.y - i), Quaternion.identity);
                Block.destroyBlockAt(pos.x, pos.y - i);
                SpawnPowerUp(pos.x, pos.y - i);
            }
            //left
            if (!left)
            {
                Instantiate(explosion, new Vector2(pos.x - i, pos.y), Quaternion.identity);
                Block.destroyBlockAt(pos.x - i, pos.y);
                SpawnPowerUp(pos.x - i, pos.y);
            }
            destroyables = GameObject.FindGameObjectsWithTag("Destroyable");

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Explosion")
        {
            Explode();
        }
    }
}