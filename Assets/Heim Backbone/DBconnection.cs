using UnityEngine;
using System.Collections;

public class DBconnection : MonoBehaviour {
	private Arguments argumentsScript;
	private string connectionURL;
	private string scoreURL = "insertScore.php?";

	void Awake() {
		argumentsScript = FindObjectOfType<Arguments>();
	}
	void Start() {
		connectionURL = argumentsScript.getConURL();
	}

	/// <summary>
	/// Uploads the score to the museum database.
	/// Has to be called as a Coroutine.
	/// Has to be called only once per game, or we risk spamming the database.
	/// </summary>
	public IEnumerator UploadScore(int userID, int gameID, int score) {
		string full_url = connectionURL + scoreURL + "userID=" + userID + "&gameID=" + gameID + "&score=" + score;
		WWW post = new WWW(full_url);
		yield return post;
		Application.Quit();
	}
}
