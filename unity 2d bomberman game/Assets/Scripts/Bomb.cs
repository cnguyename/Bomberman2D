using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    // Time after which the bomb explodes
    float time = 3.0f;

    // Explosion Prefab
    public GameObject explosion;

	public static float count = 1;

	public Vector3 scale;

	public static float scaleX = 0.9F;
	public static float scaleY = 0.9F;

    void Start () {
        // Call the Explode function after a few seconds
        Invoke("Explode", time);
    }
    
    void Explode() {
        // Remove Bomb from game
        Destroy(gameObject);
		BombDrop.bombs_on_field -= 1;

		scale = explosion.transform.localScale;

        // Spawn Explosion
		if (count == 1) {
			scale.y = scaleX;
			scale.x = scaleY;
			explosion.transform.localScale = scale;
			Instantiate (explosion,
                    transform.position,
                    Quaternion.identity);
		} else if (count > 1) {
			scale.y = scaleY * count;
			scale.x = scaleX * count;
			explosion.transform.localScale = scale;
			Instantiate (explosion,
			                             transform.position,
			                             Quaternion.identity);
		}

        // Destroy stuff
        Vector2 pos = transform.position;
        Block.destroyBlockAt(pos.x,   pos.y);   // same
        Block.destroyBlockAt(pos.x,   pos.y+1); // top
        Block.destroyBlockAt(pos.x+1, pos.y);   // right
        Block.destroyBlockAt(pos.x,   pos.y-1); // bottom
        Block.destroyBlockAt(pos.x-1, pos.y);   // left
        if (count > 1)
        {
            Block.destroyBlockAt(pos.x + count, pos.y);
            Block.destroyBlockAt(pos.x - count, pos.y);
            Block.destroyBlockAt(pos.x, pos.y + count);
            Block.destroyBlockAt(pos.x, pos.y - count);
        }
    }
}