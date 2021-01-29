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

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [Button]
        public void MakeGridLevel()
        {
            var levelSize = new Vector2(bg.localScale.x, bg.localScale.y);
            var gridSize = new Vector2(1, 1);
            var halfGridSize = gridSize * 0.5f;
            var offsetPos = Vector2.zero - levelSize * 0.5f;

            var childsT = childs.GetComponentsInChildren<Transform>().Where(x => x != childs).ToList();

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
                    Debug.Log(obj.name + " " + grid, obj);
                }
                else if (obj.name == Constant.CHARACTER)
                {
                    Debug.Log(obj.name + " " + grid, obj);
                }
            }
        }
    }
}