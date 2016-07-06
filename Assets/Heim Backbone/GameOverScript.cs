using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(DBconnection),typeof(Arguments))]
public class GameOverScript : MonoBehaviour {
    private int score;
    private DBconnection connection;
    private Arguments arguments;

	[SerializeField]
	private GameObject defaultHUD;
	[SerializeField]
	private GameObject GameOverHUD;
	[SerializeField]
	private Text scoreText;
    //private GameObject sceneManager;
	private float timer;
	private float targetTime;
	private bool isTimeOut;


    void Awake() {
        //sceneManager = GameObject.Find("SceneManager")

    }

    // Use this for initialization
    void Start() {
		connection = GetComponent<DBconnection> ();
		arguments = GetComponent<Arguments> ();
		Debug.Log ("start");
        DebugScriptResults();
		isTimeOut = false;
		SetTime(172f);
    }

	void Update(){
		timer += Time.deltaTime;
		if  (timer > targetTime && !isTimeOut)
		{
			isTimeOut = true;
			EndGame();

		}
	}
		


	public void SetTime(float pTime)
	{
		targetTime = (pTime);
		timer = 0;
	}


    public void EndGame() {
		score = Mathf.RoundToInt(FindObjectOfType<ScoreManager>().totalScore);
        StartCoroutine(connection.UploadScore(arguments.getUserID(), arguments.getGameID(), GetScore));

    }

	public void EndScreen()
	{
		TutorialProgression.Instance.ProgressPause = true;
		if (defaultHUD != null) defaultHUD.SetActive(false);
		if (GameOverHUD != null) GameOverHUD.SetActive(true);
		if (scoreText != null) scoreText.text = score.ToString();
	}

    public int GetScore
    {
        get { return score; }
        set { score = value; }
    }

    private void LoadScripts() {
        connection = GetComponent<DBconnection>();
        arguments = GetComponent<Arguments>();
    }

    private void DebugScriptResults()
    {
        if (connection != null) { Debug.Log("Database Connection Script found and is loaded!"); }
        else Debug.Log("Database script not found!");
        if (arguments != null) { Debug.Log("Arguments Script found and is loaded!"); }
        else Debug.Log("Arguments script not found!");
    }
}
