using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private enum LevelTile
    {
        empty,
        floor,
        wall,
        wallRight,
        wallLeft,
        wallTop,
        wallCorner_LT, wallCorner_LB, wallCorner_RT, wallCorner_RB, wallSingular,
        wallBottom,
        decor,
    };

    private LevelTile[,] grid;
    private struct RandomWalker
    {
        public Vector2 dir;
        public Vector2 pos;
    }

    private List<RandomWalker> walkers;

    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] wallRightTiles;
    public GameObject[] wallLeftTiles;
    public GameObject[] wallTopTiles;

    public GameObject[] wallCorner_LT_tiles;
    public GameObject[] wallCorner_LB_tiles;
    public GameObject[] wallCorner_RT_tiles;
    public GameObject[] wallCorner_RB_tiles;
    public GameObject[] wallSingularTiles;

    public GameObject[] bottomWallTiles;
    public GameObject[] decorTiles;

    public GameObject exit;
    public GameObject player;

    public int levelWidth;
    public int levelHeight;
    public float percentToFill = 0.2f;
    public float chanceWalkerChangeDir = 0.5f;
    public float chanceWalkerSpawn = 0.05f;
    public float chanceWalkerDestoy = 0.05f;
    public int maxWalkers = 10;
    public int iterationSteps = 100000;

    private void Awake()
    {
        Setup();
        CreateAndRemoveWalls();
        SpawnLevel();
        SpawnPlayer();
        SpawnExit();
    }

    private void CreateAndRemoveWalls()
    {
        CreateFloors();
        CreateWalls();
        CreateBottomWalls();
        CreateSideWalls();
        CreateCornerWalls();
        CreateSingleWalls();
        RemoveSingleWalls();
    }

    private void CreateSingleWalls()
    {
        //loop though every grid space
        for (int x = 0; x < levelWidth - 1; x++)
        {
            for (int y = 0; y < levelHeight - 1; y++)
            {
                //if theres a wall, check the spaces around it
                bool isWall = wallCheck(x, y);

                if (isWall)
                {
                    //assume all space around wall are floors
                    bool allFloors = true;
                    //check each side to see if they are all floors
                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > levelWidth - 1 ||
                                y + checkY < 0 || y + checkY > levelHeight - 1)
                            {
                                //skip checks that are out of range
                                continue;
                            }
                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                            {
                                //skip corners and center
                                continue;
                            }
                            if (grid[x + checkX, y] != LevelTile.floor)
                            {
                                allFloors = false;
                            }
                        }
                    }
                    if (allFloors)
                    {
                        grid[x, y] = LevelTile.wallSingular;
                    }
                }
            }
        }

    }

    private void Setup()
    {
        // prepare grid
        grid = new LevelTile[levelWidth, levelHeight];
        for (int x = 0; x < levelWidth - 1; x++)
        {
            for (int y = 0; y < levelHeight - 1; y++)
            {
                grid[x, y] = LevelTile.empty;
            }
        }

        //generate first walker
        walkers = new List<RandomWalker>();
        RandomWalker walker = new RandomWalker();
        walker.dir = RandomDirection();
        Vector2 pos = new Vector2(Mathf.RoundToInt(levelWidth / 2.0f), Mathf.RoundToInt(levelHeight / 2.0f));
        walker.pos = pos;
        walkers.Add(walker);
    }

    private void SpawnLevel()
    {
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                switch (grid[x, y])
                {
                    case LevelTile.empty:
                        break;

                    case LevelTile.floor:
                        Spawn(x, y, floorTiles[UnityEngine.Random.Range(0, floorTiles.Length)]);
                        break;

                    case LevelTile.wall:
                        Spawn(x, y, wallTiles[UnityEngine.Random.Range(0, wallTiles.Length)]);
                        break;

                    case LevelTile.wallTop:
                        Spawn(x, y, wallTopTiles[UnityEngine.Random.Range(0, wallTopTiles.Length)]);
                        break;

                    case LevelTile.wallLeft:
                        Spawn(x, y, wallLeftTiles[UnityEngine.Random.Range(0, wallLeftTiles.Length)]);
                        break;

                    case LevelTile.wallRight:
                        Spawn(x, y, wallRightTiles[UnityEngine.Random.Range(0, wallRightTiles.Length)]);
                        break;

                    case LevelTile.wallBottom:
                        Spawn(x, y, bottomWallTiles[UnityEngine.Random.Range(0, bottomWallTiles.Length)]);
                        break;

                    case LevelTile.wallCorner_LB:
                        Spawn(x, y, wallCorner_LB_tiles[UnityEngine.Random.Range(0, wallCorner_LB_tiles.Length)]);
                        break;

                    case LevelTile.wallCorner_LT:
                        Spawn(x, y, wallCorner_LT_tiles[UnityEngine.Random.Range(0, wallCorner_LT_tiles.Length)]);
                        break;

                    case LevelTile.wallCorner_RB:
                        Spawn(x, y, wallCorner_RB_tiles[UnityEngine.Random.Range(0, wallCorner_RB_tiles.Length)]);
                        break;

                    case LevelTile.wallCorner_RT:
                        Spawn(x, y, wallCorner_RT_tiles[UnityEngine.Random.Range(0, wallCorner_RT_tiles.Length)]);
                        break;

                    case LevelTile.wallSingular:
                        Spawn(x, y, wallSingularTiles[UnityEngine.Random.Range(0, wallSingularTiles.Length)]);
                        break;

                    case LevelTile.decor:
                        Spawn(x, y, decorTiles[UnityEngine.Random.Range(0, decorTiles.Length)]);
                        break;
                }
            }
        }
    }

    private Vector2 RandomDirection()
    {
        int choice = Mathf.FloorToInt(UnityEngine.Random.value * 3.99f);
        switch (choice)
        {
            case 0:
                return Vector2.down;

            case 1:
                return Vector2.left;

            case 2:
                return Vector2.up;

            default:
                return Vector2.right;
        }
    }

    private int NumberOfFloors()
    {
        int count = 0;
        foreach (LevelTile space in grid)
        {
            if (space == LevelTile.floor)
            {
                count++;
            }
        }
        return count;
    }

    private void RemoveSingleWalls()
    {
        //loop though every grid space
        for (int x = 0; x < levelWidth - 1; x++)
        {
            for (int y = 0; y < levelHeight - 1; y++)
            {
                //if theres a wall, check the spaces around it
                bool isWall = wallCheck(x, y);

                if (isWall)
                {
                    //assume all space around wall are floors
                    bool allFloors = true;
                    //check each side to see if they are all floors
                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x + checkX < 0 || x + checkX > levelWidth - 1 ||
                                y + checkY < 0 || y + checkY > levelHeight - 1)
                            {
                                //skip checks that are out of range
                                continue;
                            }
                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                            {
                                //skip corners and center
                                continue;
                            }
                            if (grid[x + checkX, y + checkY] != LevelTile.floor)
                            {
                                allFloors = false;
                            }
                        }
                    }
                    if (allFloors)
                    {
                        grid[x, y] = LevelTile.floor;
                    }
                }
            }
        }
    }

    private bool wallCheck(int x, int y)
    {
        if (
        grid[x, y] == LevelTile.wall ||
        grid[x, y] == LevelTile.wallLeft ||
        grid[x, y] == LevelTile.wallRight ||
        grid[x, y] == LevelTile.wallTop ||
        grid[x, y] == LevelTile.wallCorner_LB ||
        grid[x, y] == LevelTile.wallCorner_LT ||
        grid[x, y] == LevelTile.wallCorner_RT ||
        grid[x, y] == LevelTile.wallCorner_RB ||
        grid[x, y] == LevelTile.wallBottom ||
        grid[x, y] == LevelTile.wallSingular)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void CreateFloors()
    {
        int iterations = 0;
        do
        {
            //create floor at position of every Walker
            foreach (RandomWalker walker in walkers)
            {
                grid[(int)walker.pos.x, (int)walker.pos.y] = LevelTile.floor;
            }

            //chance: destroy Walker
            int numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                if (UnityEngine.Random.value < chanceWalkerDestoy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break;
                }
            }

            //chance: Walker pick new direction
            for (int i = 0; i < walkers.Count; i++)
            {
                if (UnityEngine.Random.value < chanceWalkerChangeDir)
                {
                    RandomWalker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }

            //chance: spawn new Walker
            numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                if (UnityEngine.Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    RandomWalker walker = new RandomWalker();
                    walker.dir = RandomDirection();
                    walker.pos = walkers[i].pos;
                    walkers.Add(walker);
                }
            }

            //move Walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                RandomWalker walker = walkers[i];
                walker.pos += walker.dir;
                walkers[i] = walker;
            }

            //avoid boarder of grid
            for (int i = 0; i < walkers.Count; i++)
            {
                RandomWalker walker = walkers[i];
                walker.pos.x = Mathf.Clamp(walker.pos.x, 1, levelWidth - 2);
                walker.pos.y = Mathf.Clamp(walker.pos.y, 1, levelHeight - 2);
                walkers[i] = walker;
            }

            //check to exit loop
            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }
            iterations++;
        } while (iterations < iterationSteps);
    }

    private void CreateWalls()
    {
        for (int x = 0; x < levelWidth - 1; x++)
        {
            for (int y = 0; y < levelHeight - 1; y++)
            {
                if (grid[x, y] == LevelTile.floor)
                {
                    if (grid[x, y + 1] == LevelTile.empty)
                    {
                        grid[x, y + 1] = LevelTile.wall;
                    }

                    if (grid[x, y - 1] == LevelTile.empty)
                    {
                        grid[x, y - 1] = LevelTile.wall;
                    }
                    if (grid[x + 1, y] == LevelTile.empty)
                    {
                        grid[x + 1, y] = LevelTile.wall;
                    }
                    if (grid[x - 1, y] == LevelTile.empty)
                    {
                        grid[x - 1, y] = LevelTile.wall;
                    }

                    if (grid[x - 1, y - 1] == LevelTile.empty)
                    {
                        grid[x - 1, y - 1] = LevelTile.wall;
                    }
                    if (grid[x - 1, y + 1] == LevelTile.empty)
                    {
                        grid[x - 1, y + 1] = LevelTile.wall;
                    }
                    if (grid[x + 1, y + 1] == LevelTile.empty)
                    {
                        grid[x + 1, y + 1] = LevelTile.wall;
                    }
                    if (grid[x + 1, y - 1] == LevelTile.empty)
                    {
                        grid[x + 1, y - 1] = LevelTile.wall;
                    }
                }
            }
        }
    }

    private void CreateBottomWalls()
    {
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 1; y < levelHeight; y++)
            {
                if (grid[x, y] == LevelTile.wall && grid[x, y - 1] == LevelTile.floor)
                {
                    grid[x, y] = LevelTile.wallBottom;
                }
            }
        }
        for (int x = 0; x < levelWidth; x++)
        {
            for (int y = 1; y < levelHeight - 1; y++)
            {
                if (grid[x, y] == LevelTile.wall && grid[x, y + 1] == LevelTile.floor)
                {
                    grid[x, y] = LevelTile.wallTop;
                }
            }
        }
    }

    private void CreateSideWalls()
    {
        for (int x = 1; x < levelWidth; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                if (grid[x, y] == LevelTile.wall && grid[x - 1, y] == LevelTile.floor)
                {
                    grid[x, y] = LevelTile.wallLeft;
                }
            }
        }

        for (int x = 1; x < levelWidth - 1; x++)
        {
            for (int y = 0; y < levelHeight; y++)
            {
                if (grid[x, y] == LevelTile.wall && grid[x + 1, y] == LevelTile.floor)
                {
                    grid[x, y] = LevelTile.wallRight;
                }
            }
        }
    }

    private void CreateCornerWalls()
    {
        // LEFT BOTTOM CORNER
        for (int x = 1; x < levelWidth; x++)
        {
            for (int y = 1; y < levelHeight; y++)
            {
                if (grid[x, y] == LevelTile.wallBottom && grid[x, y - 1] == LevelTile.floor)
                {
                    if (grid[x, y] == LevelTile.wallBottom && grid[x - 1, y] == LevelTile.floor)
                    {
                        grid[x, y] = LevelTile.wallCorner_LB;
                    }
                }
            }
        }

        for (int x = 1; x < levelWidth; x++)
        {
            for (int y = 1; y < levelHeight - 1; y++)
            {
                if (grid[x, y] == LevelTile.wallTop && grid[x, y + 1] == LevelTile.floor)
                {
                    if (grid[x, y] == LevelTile.wallTop && grid[x - 1, y] == LevelTile.floor)
                    {
                        grid[x, y] = LevelTile.wallCorner_LT;
                    }
                }
            }
        }

        for (int x = 1; x < levelWidth - 1; x++)
        {
            for (int y = 1; y < levelHeight; y++)
            {
                if (grid[x, y] == LevelTile.wallBottom && grid[x, y - 1] == LevelTile.floor)
                {
                    if (grid[x, y] == LevelTile.wallBottom && grid[x + 1, y] == LevelTile.floor)
                    {
                        grid[x, y] = LevelTile.wallCorner_RB;
                    }
                }
            }
        }

        for (int x = 1; x < levelWidth - 1; x++)
        {
            for (int y = 1; y < levelHeight - 1; y++)
            {
                if (grid[x, y] == LevelTile.wallTop && grid[x, y + 1] == LevelTile.floor)
                {
                    if (grid[x, y] == LevelTile.wallTop && grid[x + 1, y] == LevelTile.floor)
                    {
                        grid[x, y] = LevelTile.wallCorner_RT;
                    }
                }
            }
        }
    }

    private void SpawnPlayer()
    {
        Vector3 pos = new Vector3(Mathf.RoundToInt(levelWidth / 2.0f),
                                        Mathf.RoundToInt(levelHeight / 2.0f), 0);
        GameObject playerObj = Instantiate(player, pos, Quaternion.identity) as GameObject;
    }

    public void SpawnExit()
    {
        Vector2 playerPos = new Vector2(Mathf.RoundToInt(levelWidth / 2.0f),
                                Mathf.RoundToInt(levelHeight / 2.0f));
        Vector2 exitPos = playerPos;
        float exitDistance = 0f;

        for (int x = 0; x < levelWidth - 1; x++)
        {
            for (int y = 0; y < levelHeight - 1; y++)
            {
                if (grid[x, y] == LevelTile.floor)
                {
                    Vector2 nextPos = new Vector2(x, y);
                    float distance = Vector2.Distance(playerPos, nextPos);
                    if (distance > exitDistance)
                    {
                        exitDistance = distance;
                        exitPos = nextPos;
                    }
                }
            }
        }

        Spawn(exitPos.x, exitPos.y, exit);
    }

    private void Spawn(float x, float y, GameObject toSpawn)
    {
        Instantiate(toSpawn, new Vector3(x, y, 0), Quaternion.identity);
    }
}