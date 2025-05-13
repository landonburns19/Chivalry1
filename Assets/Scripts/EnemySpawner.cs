using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private float minSpawnTime;

    [SerializeField]
    private float maxSpawnTime;


    [SerializeField]
    private Tilemap Ground;
    [SerializeField]
    private Sprite GroundSprite;
    private float timeUntilSpawn;

    private int randomSpawn;

    private List<int[]> result = new List<int[]>(); 

    void Start()
    {
        setMap();
        SetTimeUntilSpawn(); 
    }



    // Update is called once per frame
    void Update()
    {
        


        //int randomX = Random.Range(0 , bounds.size.x);
        //int randomY = Random.Range(0 , bounds.size.y);
        int randIndex = Random.Range(0, result.Count - 1);

        //GameObject[] Grounds = GameObject.FindGameObjectsWithTag("Ground");
        //Debug.Log(Grounds.Length);
        //randomSpawn = Random.Range(0, Grounds.Length-1);

        timeUntilSpawn -= Time.deltaTime;

        if(timeUntilSpawn <= 0)
        {
            Instantiate(enemy, new Vector2(result[randIndex][0], result[randIndex][1]), Quaternion.identity);

            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        timeUntilSpawn = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void setMap()
    {
        BoundsInt bounds = Ground.cellBounds;
        //TileBase[] allTiles = Ground.GetTilesBlock(bounds);
        for (int x = bounds.min.x; x < bounds.size.x; x++) {
                    for (int y = bounds.min.y; y < bounds.size.y; y++) {
                        var cellPos = new Vector3Int(x,y,0);
                        var sprite = Ground.GetSprite(cellPos);
                        var tile = Ground.GetTile(cellPos);
                        if (tile != null && sprite != GroundSprite) {
                            result.Add(new int[] {x, y});
                            
                        } 
                    }
                }        
            }  
}
