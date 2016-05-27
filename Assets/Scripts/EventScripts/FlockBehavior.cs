using UnityEngine;
using System.Collections;

public class FlockBehavior : MonoBehaviour {

    float WindRadius = 60.0f;
    float DamageRadius = 40.0f;

    public float speed;
    int randomed;
    TurbineObject selectedTurbined;
    public bool debug = true;
    // Update is called once per frame
    void Start() {
        randomed = Random.Range(0, TurbineObject.all.Count);
        selectedTurbined = TurbineObject.all[randomed];
    }

    void Update()
    {

        Move();
        
        WindEffect();
        DestroyCloud();

        Destroy(this, 10.0f);
    }

    void WindEffect()
    {
       
            float distance = Vector3.Distance(new Vector3(selectedTurbined.transform.position.x, 30, transform.position.z), this.transform.position);

            
            if (distance <= DamageRadius) selectedTurbined.BreakTurbine();
        

    }

    void Move()
    {
        
        Vector3 target = TurbineObject.all[randomed].transform.position;
       // transform
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x,target.y + 30, target.z), 0.2f );
    }

    void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, WindRadius);
            Gizmos.DrawSphere(transform.position, DamageRadius);
        }
    }

    private void DestroyCloud()
    {
        if (transform.position.x < -300) Destroy(gameObject);
    }

    
    public void OnEnterWindzone()
    {
        

    }

    public void OnExitWindzone()
    {
        
    }

    public void OnStayWindzone()
    {

    }
}
