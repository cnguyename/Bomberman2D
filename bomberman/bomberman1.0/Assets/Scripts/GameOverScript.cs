using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {
		void OnGUI(){


			const int buttonWidth = 120;
			const int buttonHeight = 60;

			GUI.Label( new Rect (170, 120, 250, 25), "GAME OVER!!!");
				
			if (
				GUI.Button(
				// Center in X, 2/3 of the height in Y
				new Rect(
				Screen.width / 2 - (buttonWidth / 2),
				(2 * Screen.height / 3) - (buttonHeight / 2),
				buttonWidth,
				buttonHeight
				),
				"Load Main Menu"
				)
				)
			{


				// Reload the level
				Application.LoadLevel("main_menu");
			}
		}
	}
