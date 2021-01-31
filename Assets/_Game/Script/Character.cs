using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC
{
    public class Character : MonoBehaviour
    {
        public Vector2Int grid;
        public bool isMoveFirst;
        public bool isFinish;
        public GameObject objFeet;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetCanMove(bool isCanMove)
        {
            objFeet.SetActive(isCanMove);
        }
    }
}