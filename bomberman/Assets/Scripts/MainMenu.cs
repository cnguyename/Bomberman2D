using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	private AudioSource source;

	void OnGUI()
	{
		const int buttonWidth = 84;
		const int buttonHeight = 45;

		string username = "";
		string password = "";
		
		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect buttonRect = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			);

		// Draw a button to start the game
		if(GUI.Button(buttonRect,"Start!"))
		{
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("Corey_Scene");
		}
	}


	void Awake(){
		source = GetComponent<AudioSource> ();
		source.PlayDelayed (1);
	}
}
