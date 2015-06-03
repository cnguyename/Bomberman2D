using UnityEngine;
using System.Collections;

public class GameSession : MonoBehaviour {

	public string gamename = "";
	public static int num_games = 0;
	
	void OnGUI () {
		// Make a text field that modifies stringToEdit.
		//GUI.contentColor = Color.black;
		GUI.Label( new Rect (170, 120, 250, 25), "ENTER GAME NAME (one word) BELOW");
		gamename = GUI.TextField ( new Rect (150, 150, 250, 25), gamename, 40);

		Rect buttonRect = new Rect (150, 210, 250, 25);
		if (GUI.Button(buttonRect, "Join Game"))
		{
			if(gamename == ""){
				gamename = ("default" + num_games.ToString());
				num_games++;
			}
			SynchronousClient.game_name = gamename;
			Application.LoadLevel("scene_main");
		}
	}

}
