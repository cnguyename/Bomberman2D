using UnityEngine;
using System.Collections;
using System.IO;

public class Username : MonoBehaviour {
	string username = "enter username";

	string UserNameFilePath {
		get {
			return Application.persistentDataPath + "/username.txt";
		}
	}

	void Start(){
		Debug.Log (Application.persistentDataPath);
		try{
			username = File.ReadAllText(UserNameFilePath);
		}
		catch(FileNotFoundException){
			username = "Enter a username here.";
		}
	}
	void OnGUI(){
		//username label
		GUILayout.BeginHorizontal ();{
			GUILayout.Label ("Username:", GUILayout.Height (150));
			GUI.changed = false;
			username = GUILayout.TextField (username);
			if (GUI.changed) {
				File.WriteAllText (UserNameFilePath, username);
			}
		}GUILayout.EndHorizontal ();
	}

}
