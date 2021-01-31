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
        public Character characterOnMove;
        public List<Target> listTarget;
        public Vector2 levelSize;
        public Vector2 gridSize;
        public Vector2 halfGridSize;
        public Vector2 offsetPos;
        public bool isSolve;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }

            if (isSolve) return;

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
                    if (character.isMoveFirst)
                    {
                        characterOnMove = character;
                        characterOnMove.SetCanMove(true);
                    }
                }
                else if (obj.name == Constant.TARGET)
                {
                    var target = obj.GetComponent<Target>();
                    target.grid = grid;
                    listTarget.Add(target);
                }
            }

            isSolve = false;
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
            var charGrid = characterOnMove.grid;
            var isChangeCharacter = false;
            charGrid = GetGridMove(charGrid, direction, out isChangeCharacter);
            characterOnMove.transform.localPosition = GridToLocalPos(charGrid);

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

            var count = 0;
            foreach (var character in listCharacter)
            {
                character.grid = LocalPosToGrid(character.transform.localPosition);
                grids[character.grid.x, character.grid.y] = Constant.CHARACTER;

                foreach (var target in listTarget)
                {
                    if (character.grid == target.grid)
                    {
                        count++;
                        character.isFinish = true;
                        break;
                    }
                }
            }

            if (characterOnMove.isFinish)
            {
                isChangeCharacter = true;
            }

            if (count == listCharacter.Count)
            {
                isSolve = true;
            }

            if (isChangeCharacter && !isSolve)
            {
                characterOnMove.SetCanMove(false);
                characterOnMove = listCharacter.Where(x => x != characterOnMove && !x.isFinish).FirstOrDefault();
                characterOnMove?.SetCanMove(true);
            }
        }

        bool IsCanMoveGrid(int x, int y)
        {
            if (x < 0 || x >= grids.GetLength(0)) return false;
            if (y < 0 || y >= grids.GetLength(1)) return false;
            if (grids[x, y] == Constant.WALL) return false;
            if (grids[x, y] == Constant.CHARACTER) return false;
            return true;
        }

        Vector2Int GetGridMove(Vector2Int from, Vector2Int direction, out bool isChangeCharacter)
        {
            var gridTo = from;
            isChangeCharacter = false;
            var count = 0;
            while (direction.x != 0)
            {
                var x = gridTo.x + direction.x;
                if (IsCanMoveGrid(x, gridTo.y))
                {
                    gridTo.x = x;
                    count++;
                }
                else
                {
                    if (grids[x, gridTo.y] == Constant.CHARACTER)
                    {
                        isChangeCharacter = true;
                    }
                    break;
                }
            }
            while (direction.y != 0)
            {
                var y = gridTo.y + direction.y;
                if (IsCanMoveGrid(gridTo.x, y))
                {
                    gridTo.y = y;
                    count++;
                }
                else
                {
                    if (grids[gridTo.x, y] == Constant.CHARACTER)
                    {
                        isChangeCharacter = true;
                    }
                    break;
                }
            }
            return gridTo;
        }
    }
}