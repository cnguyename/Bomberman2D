using UnityEngine;
using System.Collections;

public class Login : MonoBehaviour
{

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
	private string CreateAccountUrl = "http://169.234.58.129/CreateAccountT.php";
	private string LoginUrl = "http://169.234.58.129/LoginAccountT.php";

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
        GUI.Box(new Rect(280, 120, (Screen.width / 4) + 200, (Screen.height / 4) + 250), "Login");

        if (GUI.Button(new Rect(370, 360, 120, 25), "Create Account"))
        {
            CurrentMenu = "CreateAccount";
        }
        if (GUI.Button(new Rect(520, 360, 120, 25), "Log In"))
        {
            StartCoroutine(LoginAccount());
        }
        GUI.Label(new Rect(390, 200, 220, 23), "Account: ");
        username = GUI.TextField(new Rect(390, 225, 220, 23), username);

        GUI.Label(new Rect(390, 255, 220, 23), "Password: ");
        password = GUI.TextField(new Rect(390, 280, 220, 23), password);

    }

    void CreateAccountGUI()
    {
        GUI.Box(new Rect(280, 120, (Screen.width / 4) + 200, (Screen.height / 4) + 250), "Create Account");

        GUI.Label(new Rect(390, 200, 220, 23), "New Account: ");
        Cusername = GUI.TextField(new Rect(390, 225, 220, 23), Cusername);

        GUI.Label(new Rect(390, 255, 220, 23), "New Password: ");
        Cpassword = GUI.TextField(new Rect(390, 280, 220, 23), Cpassword);

        GUI.Label(new Rect(390, 310, 220, 23), "Confirm Password: ");
        ConfirmPass = GUI.TextField(new Rect(390, 335, 220, 23), ConfirmPass);

        if (GUI.Button(new Rect(370, 460, 120, 25), "Create Account"))
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
        if (GUI.Button(new Rect(520, 460, 120, 25), "Back"))
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
                Application.LoadLevel("scene_main"); // SELECT LEVEL after successfully login
            }
        }
    }













}
