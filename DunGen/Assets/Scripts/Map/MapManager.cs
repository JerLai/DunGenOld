using System.Collections;
using System.Collections.Generic;
namespace DunGen
{

    using UnityEngine;

    public class MapManager : MonoBehaviour
    {
        // Render specific variables
        [SerializeField] GameObject wallTile;
        [SerializeField] GameObject floorTile;
        [SerializeField] GameObject corridorTile;

        // For generic map parameters
        [SerializeField] int mapHeight, mapWidth;
        [SerializeField] int mapType = 0;
        private GameObject[,] mapPositionsFloor;
        private IMapGen<IMap> generator;

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
            if (mapType == 0)
            {
                //BSP
                
                BSPCorDraw(rootDun);
            } else if (mapType == 1)
            {
                //Cave
            } else if (mapType == 2)
            {
                //Maze
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