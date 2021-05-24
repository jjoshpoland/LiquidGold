using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileMap : MonoBehaviour
{
    public static TileMap singleton;
    public Tile[,] tiles;
    float[,] values;
    public int size;
    public int seed;
    float perlinSeed;
    public Tile grassPrefab;
    public Tile waterPrefab;
    public Tile rockyPrefab;
    public Tile forestPrefab;

    public int tileSize = 1;

    public float waterMax = .25f;
    public float rockyMax = .5f;
    public float grassMax = .75f;

    SimplexNoiseGenerator simplexNoise;

    public UnityEvent OnGenerateComplete;
    public UnityEvent OnTileReplacement;

    private void Awake()
    {
        singleton = this;
        tiles = new Tile[size, size];
        values = new float[size, size];
    }


    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed);
        perlinSeed = Random.Range(0f, 1f);
        simplexNoise = new SimplexNoiseGenerator(seed.ToString());
        GenerateValues();
        SpawnTiles();

        OnGenerateComplete.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Evaluates (poorly) simplex noise results for all coordinates on the map and stores them
    /// </summary>
    public void GenerateValues()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float noiseValue = (simplexNoise.coherentNoise((float)(x), 0f, (float)(y)) * 10f);
                
                noiseValue = (noiseValue + 1f) / 2f;
                float distanceFromCenter = Mathf.Abs(x - (size / 2f)) + Mathf.Abs(y - (size / 2f));
                float normDistFromCenter = distanceFromCenter / size;
                float invertedDistFromCenter = 1f - normDistFromCenter;
                
                if(noiseValue < waterMax)
                {
                    noiseValue = Mathf.Min( (invertedDistFromCenter), grassMax);
                }


                values[x, y] = noiseValue;
                //Debug.Log(values[x, y]);
            }
        }
    }

    /// <summary>
    /// Evaluates the noise value at each map coordinate and instantiates the proper tile type based on the noise value.
    /// </summary>
    public void SpawnTiles()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Tile newTile;
                float value = values[x, y];
                if(value <= waterMax)
                {
                    newTile = Instantiate(waterPrefab, transform);
                }
                else if(value <= rockyMax && Random.Range(0f, 1f) < .5f)
                {
                    newTile = Instantiate(rockyPrefab, transform);
                }
                else if(value <= grassMax && Random.Range(0f, 1f) <= .8f)
                {
                    newTile = Instantiate(grassPrefab, transform);
                }
                else
                {
                    newTile = Instantiate(forestPrefab, transform);
                }

                newTile.coords = new Vector2Int(x, y);
                newTile.transform.position = new Vector3(x * tileSize, 0, y * tileSize);
                tiles[x, y] = newTile;
            }
        }
    }


    
    /// <summary>
    /// Returns a tile if it exists at the provided coordinates
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    public Tile GetTileAtCoords(Vector2Int coords)
    {
        if (!ValidCoords(coords)) return null;

        return tiles[coords.x, coords.y];
    }

    /// <summary>
    /// Returns true if the coordinates provided are on the tile map
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    public bool ValidCoords(Vector2Int coords)
    {
        return !(coords.x < 0 || coords.x >= size || coords.y < 0 || coords.y >= size);
    }

    /// <summary>
    /// Returns all 8 neighboring tiles for the tile at the coordinates provided
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    public Tile[] Neighbors(Vector2Int coords)
    {
        if (!ValidCoords(coords)) return null;

        Tile[] neighbors = new Tile[8];

        int x = coords.x;
        int y = coords.y;

        Vector2Int n = new Vector2Int(x, y + 1);
        Vector2Int ne = new Vector2Int(x + 1, y + 1);
        Vector2Int e = new Vector2Int(x + 1, y);
        Vector2Int se = new Vector2Int(x + 1, y - 1);
        Vector2Int s = new Vector2Int(x, y - 1);
        Vector2Int sw = new Vector2Int(x - 1, y - 1);
        Vector2Int w = new Vector2Int(x - 1, y);
        Vector2Int nw = new Vector2Int(x - 1, y + +1);

        Vector2Int[] neighborCoords = { n, ne, e, se, s, sw, w, nw };

        for(int i = 0; i < neighborCoords.Length; i++)
        {
            Tile neighbor = GetTileAtCoords(neighborCoords[i]);
            if (neighbor != null)
            {
                neighbors[i] = neighbor;
            }
        }

        return neighbors;
        
    }

    /// <summary>
    /// Replaces the tile at the target coordinates with the provided new tile
    /// </summary>
    /// <param name="coords"></param>
    /// <param name="newTile"></param>
    /// <returns></returns>
    public bool ReplaceTile(Vector2Int coords, Tile newTile)
    {
        if(ValidCoords(coords))
        {
            //Debug.Log("confirming replacement of tile at " + coords);
            Tile oldTile = tiles[coords.x, coords.y];
            Tile newTileInstance = Instantiate(newTile, transform);
            newTileInstance.transform.position = oldTile.transform.position;
            newTileInstance.coords = oldTile.coords;
            tiles[coords.x, coords.y] = newTileInstance;
            Destroy(oldTile.gameObject);
            
            OnTileReplacement.Invoke();
            return true;
        }
        else
        {
            return false;
        }
    }
}

public enum TileType
{
    Water, Rock, Grass, Forest, Structure
}
