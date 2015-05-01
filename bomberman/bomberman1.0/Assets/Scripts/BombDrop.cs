using UnityEngine;
using System.Collections;

public class BombDrop : MonoBehaviour {
    public GameObject bombPrefab;
       
    public float bomb_limit = 1;
	public static float bombs_on_field = 0;

    public GameObject mover;
    public Move Character;

    void Start()
    {
        Character = mover.GetComponent<Move>();
    }

	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && bombs_on_field < bomb_limit ) {
			bombs_on_field += 1;
            Vector2 pos = Character.bombermans[Character.client.PlayerIndex].transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            Instantiate(bombPrefab, pos, Quaternion.identity);
        }
	}

}
