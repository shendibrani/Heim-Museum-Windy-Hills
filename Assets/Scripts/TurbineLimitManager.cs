using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurbineLimitManager : MonoBehaviour {

    [SerializeField]
    int maxCount = 5;

    [SerializeField]
    Text countText;

    [SerializeField]
    Text maxText;

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
        FindObjectOfType<PlaceObjectOnClick>().OnObjectPlaced += checkCount;
	}
	
	// Update is called once per frame
	void Update () {
        if (countText != null) countText.text = currentCount.ToString();
        if (maxText != null) maxText.text = maxCount.ToString(); 
	
	}

    public void checkCount(GameObject go)
    {
        int c = FindObjectsOfType<TurbineObject>().Length;
        currentCount = c;
        if (c >= maxCount) isCap = true;
        else isCap = false;
    }
}
