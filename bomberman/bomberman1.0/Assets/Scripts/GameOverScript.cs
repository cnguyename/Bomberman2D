using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {
		void OnGUI()
		{
			const int buttonWidth = 120;
			const int buttonHeight = 60;
			
			if (
				GUI.Button(
				// Center in X, 1/3 of the height in Y
				new Rect(
				Screen.width / 2 - (buttonWidth / 2),
				(1 * Screen.height / 3) - (buttonHeight / 2),
				buttonWidth,
				buttonHeight
				),
				"Play a New Game!"
				)
				)
			{
				// Reload the level
				Application.LoadLevel("Corey_Scene");
			}
			
			if (
				GUI.Button(
				// Center in X, 2/3 of the height in Y
				new Rect(
				Screen.width / 2 - (buttonWidth / 2),
				(2 * Screen.height / 3) - (buttonHeight / 2),
				buttonWidth,
				buttonHeight
				),
				"Back to Main Menu"
				)
				)
			{
				// Reload the level
				Application.LoadLevel("main_menu");
			}
		}
	}
