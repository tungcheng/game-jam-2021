using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;

namespace TC
{
    public class Level : MonoBehaviour
    {
        public Transform bg;
        public Transform childs;
        
        public string[,] grids;

        [Header("Debug")]
        public Transform character;
        public Vector2Int charGrid;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("GridMove: " + GetGridMove(charGrid, Vector2Int.left));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("GridMove: " + GetGridMove(charGrid, Vector2Int.right));
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("GridMove: " + GetGridMove(charGrid, Vector2Int.up));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Debug.Log("GridMove: " + GetGridMove(charGrid, Vector2Int.down));
            }
        }

        [Button]
        public void MakeGridLevel()
        {
            var levelSize = new Vector2(bg.localScale.x, bg.localScale.y);
            var gridSize = new Vector2(1, 1);
            var halfGridSize = gridSize * 0.5f;
            var offsetPos = Vector2.zero - levelSize * 0.5f;

            var childsT = childs.GetComponentsInChildren<Transform>().Where(x => x != childs).ToList();
            grids = new string[(int)levelSize.x, (int)levelSize.y];

            for (int x = 0; x < grids.GetLength(0); x++)
            {
                for (int y = 0; y < grids.GetLength(1); y++)
                {
                    grids[x, y] = Constant.EMPTY;
                }
            }

            foreach (var child in childsT)
            {
                var obj = child.gameObject;
                var grid = new Vector2Int
                {
                    x = Mathf.RoundToInt((child.localPosition.x - halfGridSize.x - offsetPos.x)),
                    y = Mathf.RoundToInt((child.localPosition.y - halfGridSize.y - offsetPos.y))
                };
                
                if (obj.name == Constant.WALL)
                {
                    grids[grid.x, grid.y] = Constant.WALL;
                }
                else if (obj.name == Constant.CHARACTER)
                {
                    character = child;
                    charGrid = grid;
                }
            }
        }

        Vector2Int GetGridMove(Vector2Int from, Vector2Int direction)
        {
            var gridTo = from;
            while (direction.x != 0)
            {
                var x = gridTo.x + direction.x;
                if (x >= 0 && x < grids.GetLength(0) && grids[x, gridTo.y] != Constant.WALL)
                {
                    gridTo.x = x;
                }
                else
                {
                    break;
                }
            }
            while (direction.y != 0)
            {
                var y = gridTo.y + direction.y;
                if (y >= 0 && y < grids.GetLength(1) && grids[gridTo.x, y] != Constant.WALL)
                {
                    gridTo.y = y;
                }
                else
                {
                    break;
                }
            }
            return gridTo;
        }
    }
}