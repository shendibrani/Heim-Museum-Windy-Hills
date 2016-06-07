using UnityEngine;
using System.Collections;

[RequireComponent(typeof(StormBehavior))]
public class EventFollowIndicator : MonoBehaviour {

    GameObject UIContainer;
    [SerializeField]
    GameObject indicator;

    StormBehavior behaviour;

    // Use this for initialization
    void Start()
    {
        UIContainer = GameObject.FindGameObjectWithTag("EventFollowUI");
        GameObject i = GameObject.Instantiate(indicator);
        i.transform.parent = UIContainer.transform;
        behaviour = GetComponent<StormBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 targetPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Debug.Log(targetPos);
        //indicator.GetComponent<RectTransform>().position = Vector2.Lerp(indicator.GetComponent<RectTransform>().position, targetPos, 0.5f);
        indicator.GetComponent<RectTransform>().localPosition = targetPos;
    }

    void Destory()
    {
        GameObject.Destroy(indicator);
    }
}
