﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}