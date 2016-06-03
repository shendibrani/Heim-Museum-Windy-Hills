using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurbineLimitManager : MonoBehaviour {

    public static TurbineLimitManager instance;
    public static TurbineLimitManager Instance
    {
        get
        {
            if (instance == null)
            {
                if (FindObjectsOfType<TurbineLimitManager>().Length > 1)
                {
                    Debug.LogError("Multiple instances of TurbineLimitManager detected");
                }
                instance = FindObjectOfType<TurbineLimitManager>();
                Debug.Log("TurbineLimitManager Set");
            }
            return instance;
        }
    }


    [SerializeField]
    int maxCount = 6;

    [SerializeField]
    Text countText;

    [SerializeField]
    Text maxText;

    [SerializeField]
    bool debug;

    public int currentCount
    {
        get;
        private set;
    }

    public bool isCap
    {
        get;
        private set;
    }

	// Use this for initialization
	void Start () {
        //FindObjectOfType<PlaceObjectOnClick>().OnObjectPlaced += checkCount;
	}
	
	// Update is called once per frame
	void Update () {
        if (countText != null) countText.text = (maxCount - currentCount).ToString();
        if (maxText != null && debug) maxText.text = (maxCount - currentCount).ToString();
        checkCount(); 
	
	}
    public void IncreaseMax()
    {
        maxCount += 1;
    }
    public void checkCount()
    {
        int c = TurbineObject.all.Count;
        currentCount = c;
        if (c >= maxCount) isCap = true;
        else isCap = false;
    }
}
