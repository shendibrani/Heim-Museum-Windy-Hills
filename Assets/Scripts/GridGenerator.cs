using UnityEngine;
using System.Collections;

public class GridGenerator : MonoBehaviour {

    Vector3 terrainDimensions;
    Vector2 tileDimensions;
    [SerializeField]
    public int gridSize;
    Terrain myTerrain;
    int[,] gridArray;
    public GameObject tileSprite;
    TerrainData tData;

    // Use this for initialization
	void Start () {
        
        myTerrain = GetComponent<Terrain>();
        tData = myTerrain.terrainData;
        terrainDimensions = myTerrain.terrainData.size;
        tileDimensions = new Vector2(terrainDimensions.x , terrainDimensions.y ) / gridSize;
        gridArray = new int[gridSize, gridSize];
        GenerateGrid();
        
    }
	
	// Update is called once per frame
	void Update () {
 

    }

    void GenerateGrid() {
        Vector3 pos;
        GameObject Instance;
        
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    pos = new Vector3(i * tileDimensions.x, 1, j * tileDimensions.y);

                if (tData.GetHeight((int)pos.x, (int)pos.z) == 0.0f)
                {
                    Instance = (GameObject)Instantiate(tileSprite, pos, Quaternion.Euler(90, 0, 0));
                    Instance.transform.localScale = new Vector3(tileDimensions.x, tileDimensions.y, 1);
                }
                
            }
        }
       
    }
}
