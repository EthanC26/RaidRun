using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.Pool;

public class TileLevelGen : MonoBehaviour
{
    [Header("Level Settings")]
    public float scrollSpeed = 5f;

    [Header("Tile Settings")]
    public Tile groundTile;
    public Tile DirtTile;

    public GameObject obstaclePreFab;

    private ObjectPool<Tile> GroundPool;
    private ObjectPool<GameObject> ObstaclePool;

   private List<GameObject> ActiveObstacle = new List<GameObject>();

    [Range(0, 1)]
    [Tooltip("Probability of placing an obstacle tile (0 = no obstacles, 1 = all tiles are obstacles)")]
    public float obstacleProbability = 0.05f;

    [Header("Generation Settings")]
    public int initTileColums = 5;

    private Tilemap tilemap;
    private Camera mainCamera;

    //tracking position of tiles
    private int leftmostTileColumnXPos = 0;
    private int rightmostTileColumnXPos = 0;
    private int groundlevel = -4;

    private float screenLeftEdgeX;
    private float screenRightEdgeX;
    private float scrollAccumulator = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap component not found in children of TileLevelGen.");
            this.enabled = false;
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found.");
            this.enabled = false;
            return;
        }

        GroundPool = new ObjectPool<Tile>(
            createFunc: CreateGroundTiles,
            actionOnGet: PositionAndActivate,
            actionOnRelease: DeactivateTile,
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
            );

        ObstaclePool = new ObjectPool<GameObject>(
            createFunc: () =>
            {
                GameObject obj = Instantiate(obstaclePreFab);
                return obj;
            },
            actionOnGet: obj => obj.SetActive(true),
            actionOnRelease: obj => obj.SetActive(false),
            actionOnDestroy: null,
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 100
        );

        CalculateScreenBounds();

        
        Vector3 tileWorldSize = tilemap.cellSize;
        float tilesNeededToFillScreen = Mathf.Ceil((screenRightEdgeX - screenLeftEdgeX) / tileWorldSize.x);
        int totalColumns = Mathf.CeilToInt(tilesNeededToFillScreen) + initTileColums * 2;
        int startPositionX = Mathf.FloorToInt(screenLeftEdgeX - initTileColums);

        for (int i = 0; i < totalColumns; i++)
            GenerateTileColum(startPositionX + i);
        
        leftmostTileColumnXPos = startPositionX;
        rightmostTileColumnXPos = startPositionX + totalColumns - 1;
    }

    Tile CreateGroundTiles()
    {
        Tile newTile = Instantiate(groundTile);
        return newTile;
    }

    void PositionAndActivate(Tile tile)
    {
        Vector3Int tilePosition = new Vector3Int(rightmostTileColumnXPos, groundlevel, 0);
        tilemap.SetTile(tilePosition, tile);

        Vector3Int titlePosition = new Vector3Int(rightmostTileColumnXPos, groundlevel - 1, 0);
        tilemap.SetTile(titlePosition, DirtTile);
    }

    void DeactivateTile(Tile tile)
    {
        tile.gameObject.SetActive(false);
    }
    private void GenerateTileColum(int colXPos)
    {
        rightmostTileColumnXPos = colXPos;
        GroundPool.Get();

        if(Random.value < obstacleProbability)
        {
            Vector3Int objectPos = new Vector3Int(colXPos, groundlevel + 1, 0); // Place obstacle one tile above ground
            Vector3 worldPosition = tilemap.CellToWorld(objectPos) + tilemap.cellSize / 2f;

            GameObject obs = ObstaclePool.Get();
            obs.transform.position = worldPosition;
            ActiveObstacle.Add(obs);
        }
    }

    private void CalculateScreenBounds()
    {
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        screenLeftEdgeX = mainCamera.transform.position.x - screenWidth / 2f;
        screenRightEdgeX = mainCamera.transform.position.x + screenWidth / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateScreenBounds();

        float scrollAmount = scrollSpeed * Time.deltaTime;
        scrollTitles(scrollAmount);
        MoveObstacles(scrollAmount);
        RemoveOffScreenObstacles();
        ManageTiles();
    }

    private void MoveObstacles(float scrollAmount)
    {
        Vector3 movement = Vector3.left * scrollAmount;
        foreach (var obsList in ActiveObstacle)
           obsList.transform.position += movement;

    }

    private void scrollTitles(float scrollAmount)
    {
        scrollAccumulator += scrollAmount;
        float tileWidth = tilemap.cellSize.x;

        if (scrollAccumulator >= tileWidth)
        {
            int shiftAmount = Mathf.FloorToInt(scrollAccumulator / tileWidth);

            //define range to shift tiles
            int minY = groundlevel - 5;
            int maxY = groundlevel + 5;

            int height = maxY - minY;

            //calculate the bounds for the tiles to shift
            BoundsInt soruceBounds = new BoundsInt(leftmostTileColumnXPos + shiftAmount
                , minY, 0, rightmostTileColumnXPos - leftmostTileColumnXPos + 1 - shiftAmount, height, 1);

            //get the tiles in the source bounds
            TileBase[] tilesToShift = tilemap.GetTilesBlock(soruceBounds);

            //shift the tiles to the left
            BoundsInt TargetBounds = new BoundsInt(leftmostTileColumnXPos, minY, 0, soruceBounds.size.x,
                height, 1);

            //clear original area and set new tiles to a avoid overlapping tiles
            TileBase[] nullTiles = new TileBase[tilesToShift.Length];
            tilemap.SetTilesBlock(soruceBounds, nullTiles);

            //set the shifted tiles to the target bounds
            tilemap.SetTilesBlock(TargetBounds, tilesToShift);

            leftmostTileColumnXPos -= shiftAmount;
            rightmostTileColumnXPos -= shiftAmount;

            scrollAccumulator -= shiftAmount * tileWidth;
        }
        //implementation shifts tiles directly to the left. tilemap itself should remain stationary.
        //ensure that you are not moving the tilemap gameobject itself in the same hierarchy,
        //as this will cause the tiles to appear to move instead of scrolling
    }

    private void RemoveOffScreenObstacles()
    {
        // Check if any obstacles are off-screen and remove them
        for (int i = ActiveObstacle.Count - 1; i >= 0; i--)
        {
            GameObject obs = ActiveObstacle[i];
            if (obs.transform.position.x + tilemap.cellSize.x / 2f < screenLeftEdgeX)
            {
                ObstaclePool.Release(obs);
                ActiveObstacle.RemoveAt(i);
            }
        }
    }
    private void ManageTiles()
    {
        //convert screen edges to tile coordinates
        Vector3Int leftTile = tilemap.WorldToCell(new Vector3(screenLeftEdgeX, 0, 0));
        Vector3Int rightTile = tilemap.WorldToCell(new Vector3(screenRightEdgeX, 0, 0));

        while (leftmostTileColumnXPos < leftTile.x - initTileColums)
        {
            RemoveLeftColumn();
        }
        while (rightmostTileColumnXPos < rightTile.x + initTileColums)
        {
            AddRightColumn();
        }
    }
    private void RemoveLeftColumn()
    {

        for (int y = groundlevel; y <= groundlevel + 5; y++)
        {
            Vector3Int tilePosition = new Vector3Int(leftmostTileColumnXPos, y, 0);
            Tile tile = tilemap.GetTile(tilePosition) as Tile;
            if (tile != null)
            {
                tilemap.SetTile(tilePosition, null);
                GroundPool.Release(tile);
            }
        }

        leftmostTileColumnXPos++;
    }

   

    private void AddRightColumn()
    {
        rightmostTileColumnXPos++;
        GenerateTileColum(rightmostTileColumnXPos);
    }
}
