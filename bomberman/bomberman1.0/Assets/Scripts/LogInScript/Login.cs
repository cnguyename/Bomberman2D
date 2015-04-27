using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour {
	
	//Static
	public static string username = "";
	public static string password = "";
	
	//Public
	public float X; //Test variables
	public float Y; //''
	public float W; //''
	public float H; //''
	
	public string CurrentMenu = "Login";
	
	//Private
	private string CreateAccountUrl = "";
	private string LoginUrl = "";

	private string ConfirmPass = "";
	private string Cusername = ""; //New
	private string Cpassword = ""; //New
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Main GUI function
	void OnGUI(){
		if (CurrentMenu == "Login") {
			LoginGUI();
		} 
		else if (CurrentMenu == "CreateAccount") 
		{
			CreateAccountGUI();
		}
	}
	
	void LoginGUI(){
		GUI.Box (new Rect (280, 120, (Screen.width / 4) + 200, (Screen.height / 4) + 250), "Login");

		if (GUI.Button (new Rect (370, 360, 120, 25), "Create Account")) {
			CurrentMenu = "CreateAccount";
		}
		if (GUI.Button (new Rect (520, 360, 120, 25), "Log In")) {
		}
		GUI.Label (new Rect (390, 200, 220, 23), "Account: ");
		username = GUI.TextField (new Rect (390, 225, 220, 23), username);

		GUI.Label (new Rect (390, 255, 220, 23), "Password: ");
		password = GUI.TextField (new Rect (390, 280, 220, 23), password);

	}
	
	void CreateAccountGUI(){
		GUI.Box (new Rect (280, 120, (Screen.width / 4) + 200, (Screen.height / 4) + 250), "Create Account");

		GUI.Label (new Rect (390, 200, 220, 23), "New Account: ");
		Cusername = GUI.TextField (new Rect (390, 225, 220, 23), Cusername);

		GUI.Label (new Rect (390, 255, 220, 23), "New Password: ");
		Cpassword = GUI.TextField (new Rect (390, 280, 220, 23), Cpassword);

		GUI.Label (new Rect (390, 310, 220, 23), "Confirm Password: ");
		ConfirmPass = GUI.TextField (new Rect (390, 335, 220, 23), ConfirmPass);

		if (GUI.Button (new Rect (370, 460, 120, 25), "Create Account")) {
			if (ConfirmPass == Cpassword && Cusername != null){
				//StartCoroutine(); //Will be changed later
			}
			else{
				//StartCoroutine(); // ''
			}
		}
		if (GUI.Button (new Rect (520, 460, 120, 25), "Back")) {
			CurrentMenu = "Login";
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
}
