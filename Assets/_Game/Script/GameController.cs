﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC
{
    public class GameController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var level = FindObjectOfType<Level>();
            level.MakeGridLevel();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}