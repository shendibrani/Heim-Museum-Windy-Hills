using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {
    private int score;
    private DBconnection connection;
    private Arguments arguments;
    private GameObject sceneManager;

    void Awake() {
        sceneManager = GameObject.Find("SceneManager");
        LoadScripts();
    }

    // Use this for initialization
    void Start() {
        DebugScriptResults();

    }

    // Update is called once per frame
    public void EndGame() {
        StartCoroutine(connection.UploadScore(arguments.getUserID(), arguments.getGameID(), GetScore));

    }

    public int GetScore
    {
        get { return score; }
        set { score = value; }
    }

    private void LoadScripts() {
        connection = sceneManager.GetComponent<DBconnection>();
        arguments = sceneManager.GetComponent<Arguments>();
    }

    private void DebugScriptResults()
    {
        if (connection != null) { Debug.Log("Database Connection Script found and is loaded!"); }
        else Debug.Log("Database script not found!");
        if (arguments != null) { Debug.Log("Arguments Script found and is loaded!"); }
        else Debug.Log("Arguments script not found!");
    }
}
