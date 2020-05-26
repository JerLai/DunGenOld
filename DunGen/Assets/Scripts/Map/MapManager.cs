using System.Collections;
using System.Collections.Generic;
namespace DunGen
{
    using System;
    using UnityEngine;

    public class MapManager : MonoBehaviour
    {
        // Render specific variables
        [SerializeField] GameObject wallTile;
        [SerializeField] GameObject floorTile;
        [SerializeField] GameObject corridorTile;

        // For generic map parameters
        [SerializeField] int mapHeight = 100, mapWidth = 100;
        [SerializeField] int maxRoom = 20, minRoom = 8;
        [SerializeField] int mapType = 0;
        private GameObject[,] mapPositionsFloor;
        private IMapGen<Map> generator;
        private IMap map;

        // For BSP generation parameters
        [SerializeField] int minSize, maxSize;
        [SerializeField] double tolerance = 1.25;
        // Cave params
        // Room and Maze params

        private void Awake()
        {

        }
        // Start is called before the first frame update
        void Start()
        {
            GenerateMap();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    for (int j = 0; j < mapHeight; j++)
                    {
                        if (mapPositionsFloor[i, j] != null)
                        {
                            Destroy(mapPositionsFloor[i, j]);
                        }
                    }
                }
                GenerateMap();
            }
        }
        void GenerateMap()
        {
            mapPositionsFloor = new GameObject[mapWidth, mapHeight];
            if (mapType == 0)
            {
                //BSP
                generator = new BSPTree<Map>(mapWidth, mapHeight, maxSize, minSize, maxRoom, minRoom, new System.Random(DateTime.Now.Millisecond));
                map = Map.Create(generator);
            } else if (mapType == 1)
            {
                //Cave
            } else if (mapType == 2)
            {
                //Maze
            }
            DrawMap();
        }

        private void DrawMap()
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    switch (map.GetTile(x, y).type)
                    {
                        case Tile.Type.Block:
                            {
                                GameObject newTile = GameObject.Instantiate(corridorTile, new Vector2(x, y), Quaternion.identity);
                                newTile.transform.SetParent(transform);
                                mapPositionsFloor[x, y] = newTile;
                                break;
                            }
                        case Tile.Type.Floor:
                            {
                                GameObject newTile = GameObject.Instantiate(floorTile, new Vector2(x, y), Quaternion.identity);
                                newTile.transform.SetParent(transform);
                                mapPositionsFloor[x, y] = newTile;
                                break;
                            }
                        case Tile.Type.Hall:
                            {
                                GameObject newTile = GameObject.Instantiate(floorTile, new Vector2(x, y), Quaternion.identity);
                                newTile.transform.SetParent(transform);
                                mapPositionsFloor[x, y] = newTile;
                                break;
                            }
                    }
                }
            }
        }
        void BSPDraw(SubDungeon subDungeon)
        {
            if (subDungeon == null)
            {
                return;
            }
            if (subDungeon.IsLeaf())
            {
                for (int i = (int)subDungeon.room.x; i < subDungeon.room.xMax; i++)
                {
                    for (int j = (int)subDungeon.room.y; j < subDungeon.room.yMax; j++)
                    {
                        GameObject instance = Instantiate(floorTile, new Vector2(i, j), Quaternion.identity) as GameObject;
                        instance.transform.SetParent(transform);
                        mapPositionsFloor[i, j] = instance;
                    }
                }
            }
            else
            {
                BSPDraw(subDungeon.left);
                BSPDraw(subDungeon.right);
            }
        }
        void BSPCorDraw(SubDungeon subDungeon)
        {
            if (subDungeon == null)
            {
                return;
            }

            BSPCorDraw(subDungeon.left);
            BSPCorDraw(subDungeon.right);

            foreach (Rect corridor in subDungeon.corridors)
            {
                for (int i = (int)corridor.x; i < corridor.xMax; i++)
                {
                    for (int j = (int)corridor.y; j < corridor.yMax; j++)
                    {
                        if (mapPositionsFloor[i, j] == null)
                        {
                            GameObject instance = Instantiate(corridorTile, new Vector2(i, j), Quaternion.identity) as GameObject;
                            instance.transform.SetParent(transform);
                            mapPositionsFloor[i, j] = instance;
                        }
                    }
                }
            }
        }
    }

}