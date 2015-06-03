using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour
{
	//Static IP
	public static string ip;

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
	private string CreateAccountUrl = "http://" + ip + "/CreateAccountT.php";
	private string LoginUrl = "http://" + ip + "/LoginAccountT.php";

    private string ConfirmPass = "";
    private string Cusername = ""; //New
    private string Cpassword = ""; //New

    // Use this for initialization
    void Start()
    {

    }

    // Main GUI function
    void OnGUI()
    {
        if (CurrentMenu == "Login")
        {
            LoginGUI();
        }
        else if (CurrentMenu == "CreateAccount")
        {
            CreateAccountGUI();
        }
    }

    void LoginGUI()
    {
		GUI.Box(new Rect(Screen.width/5, Screen.height/5, Screen.width/2, Screen.height/2), "Login");

		if (GUI.Button(new Rect(Screen.width/5 + 20, Screen.height/2 + 50, 120, 25), "Create Account"))
        {
            CurrentMenu = "CreateAccount";
        }
		if (GUI.Button(new Rect(Screen.width/5 + 160, Screen.height/2 + 50, 120, 25), "Log In"))
        {
            StartCoroutine(LoginAccount());
        }
		GUI.Label(new Rect(Screen.width/5 + 20, Screen.height/5 + 20, Screen.width/2, Screen.height/2), "Account: ");
		username = GUI.TextField(new Rect(Screen.width/5 + 20, Screen.height/5 + 40, 220, 23), username);

		GUI.Label(new Rect(Screen.width/5 + 20, Screen.height/5 + 65, Screen.width/2, Screen.height/2), "Password: ");
		password = GUI.TextField(new Rect(Screen.width/5 + 20, Screen.height/5 + 85, 220, 23), password);

    }

    void CreateAccountGUI()
    {
		GUI.Box(new Rect(Screen.width/5, Screen.height/5, Screen.width/2, Screen.height/2), "Create Account");

		GUI.Label(new Rect(Screen.width/5 + 20, Screen.height/5 + 20, Screen.width/2, Screen.height/2), "New Account: ");
		Cusername = GUI.TextField(new Rect(Screen.width/5 + 20, Screen.height/5 + 40, 220, 23), Cusername);

		GUI.Label(new Rect(Screen.width/5 + 20, Screen.height/5 + 65, Screen.width/2, Screen.height/2), "New Password: ");
		Cpassword = GUI.TextField(new Rect(Screen.width/5 + 20, Screen.height/5 + 85, 220, 23), Cpassword);

		GUI.Label(new Rect(Screen.width/5 + 20, Screen.height/5 + 110, Screen.width/2, Screen.height/2), "Confirm Password: ");
		ConfirmPass = GUI.TextField(new Rect(Screen.width/5 + 20, Screen.height/5 + 130, 220, 23), ConfirmPass);

		if (GUI.Button(new Rect(Screen.width/5 + 20, Screen.height/2 + 50, 120, 25), "Create Account"))
        {
            if (ConfirmPass == Cpassword && Cusername != null)
            {
                StartCoroutine("CreateAccount"); //Will be changed later
            }
            else
            {
                //StartCoroutine(); // ''
            }
        }
		if (GUI.Button(new Rect(Screen.width/5 + 160, Screen.height/2 + 50, 120, 25), "Back"))
        {
            CurrentMenu = "Login";
        }
    }


    //Create Account
    IEnumerator CreateAccount()
    {
        //sends message to php
        WWWForm Form = new WWWForm();
        // the fields are the variables are sending
        Form.AddField("Account", Cusername);
        Form.AddField("Password", Cpassword);

        WWW CreateAccountWWW = new WWW(CreateAccountUrl, Form);
        // Wait for php to send searching back to Unity
        yield return CreateAccountWWW;

        if (CreateAccountWWW.error != null)
        {
            Debug.LogError("Cannot Connect to Account Server");
        }
        else
        {
            string CreateAccountReturn = CreateAccountWWW.text;
            if (CreateAccountReturn == "Success")
            {
                Debug.Log("Success: Account Created");
                CurrentMenu = "Login";
            }
        }
    }


    //LOG IN function
    IEnumerator LoginAccount()
    {
        // Add values that will go into the php script
        WWWForm Form = new WWWForm();
        Form.AddField("Account", username);
        Form.AddField("Password", password);

        WWW LoginAccountWWW = new WWW(LoginUrl, Form);

        yield return LoginAccountWWW;

        if (LoginAccountWWW.error != null)
        {
            Debug.LogError("Cannot connect to Login");
        }
        else
        {
            string LogText = LoginAccountWWW.text;
            Debug.Log(LogText);
            string[] LogTextSplit = LogText.Split(':');
            if (LogTextSplit[0] == "Success")
            {
				SynchronousClient.PlayerName = username;
                Application.LoadLevel("GameSession"); // SELECT LEVEL after successfully login
            }
        }
    }

}
