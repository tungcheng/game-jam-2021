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
        public List<Character> listCharacter;
        public Vector2 levelSize;
        public Vector2 gridSize;
        public Vector2 halfGridSize;
        public Vector2 offsetPos;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveCharacter(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveCharacter(Vector2Int.right);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MoveCharacter(Vector2Int.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MoveCharacter(Vector2Int.down);
            }
        }

        [Button]
        public void MakeGridLevel()
        {
            levelSize = new Vector2(bg.localScale.x, bg.localScale.y);
            gridSize = new Vector2(1, 1);
            halfGridSize = gridSize * 0.5f;
            offsetPos = Vector2.zero - levelSize * 0.5f;

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
                var grid = LocalPosToGrid(child.localPosition);
                
                if (obj.name == Constant.WALL)
                {
                    grids[grid.x, grid.y] = Constant.WALL;
                }
                else if (obj.name == Constant.CHARACTER)
                {
                    var character = obj.GetComponent<Character>();
                    character.grid = grid;
                    listCharacter.Add(character);
                    grids[grid.x, grid.y] = Constant.CHARACTER;
                }
            }
        }

        Vector2Int LocalPosToGrid(Vector3 localPos)
        {
            return new Vector2Int
            {
                x = Mathf.RoundToInt((localPos.x - halfGridSize.x - offsetPos.x)),
                y = Mathf.RoundToInt((localPos.y - halfGridSize.y - offsetPos.y))
            };
        }

        Vector3 GridToLocalPos(Vector2Int grid)
        {
            return new Vector3
            {
                x = grid.x + halfGridSize.x + offsetPos.x,
                y = grid.y + halfGridSize.y + offsetPos.y
            };
        }

        void MoveCharacter(Vector2Int direction)
        {
            foreach (var character in listCharacter)
            {
                var charGrid = character.grid;
                charGrid = GetGridMove(charGrid, direction);
                character.transform.localPosition = GridToLocalPos(charGrid);
            }

            for (int x = 0; x < grids.GetLength(0); x++)
            {
                for (int y = 0; y < grids.GetLength(1); y++)
                {
                    if (grids[x, y] == Constant.CHARACTER)
                    {
                        grids[x, y] = Constant.EMPTY;
                    }
                }
            }

            foreach (var character in listCharacter)
            {
                character.grid = LocalPosToGrid(character.transform.localPosition);
                grids[character.grid.x, character.grid.y] = Constant.CHARACTER;
            }
        }

        Vector2Int GetGridMove(Vector2Int from, Vector2Int direction)
        {
            var gridTo = from;
            var countChar = 0;
            while (direction.x != 0)
            {
                var x = gridTo.x + direction.x;
                if (x >= 0 && x < grids.GetLength(0) && grids[x, gridTo.y] != Constant.WALL)
                {
                    gridTo.x = x;
                    if (grids[x, gridTo.y] == Constant.CHARACTER)
                    {
                        countChar++;
                    }
                }
                else
                {
                    break;
                }
            }
            gridTo.x -= direction.x * countChar;
            countChar = 0;
            while (direction.y != 0)
            {
                var y = gridTo.y + direction.y;
                if (y >= 0 && y < grids.GetLength(1) && grids[gridTo.x, y] != Constant.WALL)
                {
                    gridTo.y = y;
                    if (grids[gridTo.x, y] == Constant.CHARACTER)
                    {
                        countChar++;
                    }
                }
                else
                {
                    break;
                }
            }
            gridTo.y -= direction.y * countChar;
            return gridTo;
        }
    }
}