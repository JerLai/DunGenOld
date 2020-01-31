using System.Collections;
using System.Collections.Generic;
namespace DunGen
{

    using UnityEngine;

    public class MapManager : MonoBehaviour
    {
        // Render specific variables
        [SerializeField] Sprite wall = null;
        [SerializeField] GameObject floor;

        // For generic map parameters
        [SerializeField] int mapHeight, mapWidth;
        [SerializeField] int mapType = 0;
        private GameObject[,] mapPositionsFloor;

        // For BSP generation parameters
        [SerializeField] int minSize, maxSize;
        [SerializeField] double tolerance = 1.25;
        // Cave params
        // Room and Maze params

        // Start is called before the first frame update
        void Start()
        {
            GenerateMap();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void GenerateMap()
        {
            if (mapType == 0)
            {
                //BSP
                SubDungeon rootDun = new SubDungeon(new Rect(0, 0, mapWidth, mapHeight));
                BSPMapGen(rootDun, minSize, maxSize);
                rootDun.CreateRoom();
                mapPositionsFloor = new GameObject[mapHeight, mapWidth];
                BSPDraw(rootDun);
            } else if (mapType == 1)
            {
                //Cave
            } else if (mapType == 2)
            {
                //Maze
            }
        }
        void BSPMapGen(SubDungeon subDungeon, int minSize, int maxSize)
        {
            Debug.Log("Splitting sub-dungeon " + subDungeon.debugId + ": " + subDungeon.rect);
            if (subDungeon.IsLeaf())
            {
                // if the sub-dungeon is too large
                if (subDungeon.rect.width > maxSize
                  || subDungeon.rect.height > maxSize
                  || Random.Range(0.0f, 1.0f) > 0.25)
                {

                    if (subDungeon.Cut(minSize, maxSize, tolerance))
                    {
                        Debug.Log("Splitted sub-dungeon " + subDungeon.debugId + " in "
                          + subDungeon.left.debugId + ": " + subDungeon.left.rect + ", "
                          + subDungeon.right.debugId + ": " + subDungeon.right.rect);

                        BSPMapGen(subDungeon.left, minSize, maxSize);
                        BSPMapGen(subDungeon.right, minSize, maxSize);
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
                        GameObject instance = Instantiate(floor, new Vector2(i, j), Quaternion.identity);
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
    }

}