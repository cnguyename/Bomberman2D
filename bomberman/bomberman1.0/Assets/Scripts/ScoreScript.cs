using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreScript : MonoBehaviour {

	//Bottom Left -> Player 1, Upper Left Player 2, Upper Right Player 3, Bottom Right Player 4
	public int player1_score = 0;
	public int player2_score = 0;
	public int player3_score = 0;
	public int player4_score = 0;
	public int round_count = 1;

	public Text player1_text;
	public Text player2_text;
	public Text player3_text;
	public Text player4_text;
	public Text round_text;

	public string player1_username;
	public string player2_username;
	public string player3_username;
	public string player4_username;

	// Use this for initialization
	void Start () {
		updateScoreTexts ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	//Updates the scores of all players and also the round count
	public void updateScoreTexts(){
		player1_text.text = "Player 1 (White) \n Score: " + player1_score.ToString ();
		player2_text.text = "Player 2 (Purple) \n Score: " + player2_score.ToString ();
		player3_text.text = "Player 3 (Black) \n Score: " + player3_score.ToString ();
		player4_text.text = "Player 4 (Yellow) \n Score: " + player4_score.ToString ();
		round_text.text = "Round : " + round_count.ToString();
	}
}
