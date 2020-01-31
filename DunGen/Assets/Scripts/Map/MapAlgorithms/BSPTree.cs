namespace DunGen
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class BSPTree
    {
        //TODO: make this a generator object to then have parameters to save
        //Current workaround is running method inside, in MapManager
        public void GenerateBSPMap(SubDungeon subDungeon, int minSize, int maxSize)
        {
            Debug.Log("Splitting sub-dungeon " + subDungeon.debugId + ": " + subDungeon.rect);
            if (subDungeon.IsLeaf())
            {
                // if the sub-dungeon is too large
                if (subDungeon.rect.width > maxSize
                  || subDungeon.rect.height > maxSize
                  || Random.Range(0.0f, 1.0f) > 0.25)
                {

                    if (subDungeon.Cut(minSize, maxSize, 1.25))
                    {
                        Debug.Log("Splitted sub-dungeon " + subDungeon.debugId + " in "
                          + subDungeon.left.debugId + ": " + subDungeon.left.rect + ", "
                          + subDungeon.right.debugId + ": " + subDungeon.right.rect);

                        GenerateBSPMap(subDungeon.left, minSize, maxSize);
                        GenerateBSPMap(subDungeon.right, minSize, maxSize);
                    }
                }
            }
        }
    }
}
