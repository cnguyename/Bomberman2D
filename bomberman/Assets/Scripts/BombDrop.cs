using UnityEngine;
using System.Collections;

public class BombDrop : MonoBehaviour {
    public GameObject bombPrefab;
       
    public static float bomb_limit = 1;
	public static float bombs_on_field = 0;

	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && bombs_on_field < bomb_limit ) {
			bombs_on_field += 1;
            Vector2 pos = transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            Instantiate(bombPrefab, pos, Quaternion.identity);
        }
	}

}
