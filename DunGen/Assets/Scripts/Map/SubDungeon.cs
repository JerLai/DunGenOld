namespace DunGen
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    // Used in BSP to split a map into smaller areas until they are the size
    // of given 
    public class SubDungeon
    {
        public SubDungeon left, right;
        public Rect rect;
        public Rect room = new Rect(-1, -1, 0, 0); //(xpos, ypos, width, height)
        public int debugId;

        private static int debugCounter = 0;

        // Constructor for the SubDungeon class, based on the section of the Map/Dungeon
        // to be used for the split
        public SubDungeon(Rect mrect)
        {
            rect = mrect;
            debugId = debugCounter;
            debugCounter++;
        }

        // Checks to see if there are no child SubDungeons confined in this SubDungeon
        public bool IsLeaf()
        {
            return left == null && right == null;
        }

        // Splits area considered for the sub dungeon based on given room size limitations
        public bool Cut(int minSize, int maxSize, double tolerance)
        {
            if (!IsLeaf())
            {
                return false;
            }

            bool hCut;
            // If wider than a particular tolerance, split vertically or horizontally
            if (rect.width / rect.height >= tolerance)
            {
                hCut = false;
            }
            else if (rect.height / rect.width >= 1.25)
            {
                hCut = true;
            }
            else // Assuming the dimensions are roughly the same, split at random
            {
                hCut = Random.Range(0.0f, 1.0f) > 0.5; //0.5 for squares
            }

            if (Mathf.Min(rect.height, rect.width)/2 < minSize)
            {
                Debug.Log("Sub-Dungeon " + debugId + " is leaf");
                return false;
            }

            if (hCut)
            {
                // Split so that the resulting sub-dungeons widths are not too small
                // (since we are splitting horizontally)
                int split = Random.Range(minSize, (int)(rect.width - minSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, rect.width, split));
                right = new SubDungeon(
                  new Rect(rect.x, rect.y + split, rect.width, rect.height - split));
            }
            else
            {
                int split = Random.Range(minSize, (int)(rect.height - minSize));

                left = new SubDungeon(new Rect(rect.x, rect.y, split, rect.height));
                right = new SubDungeon(
                  new Rect(rect.x + split, rect.y, rect.width - split, rect.height));
            }

            return true;
        }
    
        public void CreateRoom()
        {
            if (left != null)
            {
                left.CreateRoom();
            }
            if (right != null)
            {
                right.CreateRoom();
            }
            if (IsLeaf())
            {
                int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
                int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

                // room position will be absolute in the board, not relative to the sub-dungeon
                room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
                Debug.Log("Created room " + room + " in sub-dungeon " + debugId + " " + rect);
            }
        }
    }

}
