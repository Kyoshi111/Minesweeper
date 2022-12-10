using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public int FieldWidth { get; private set; }
    [field: SerializeField] public int FieldHeight { get; private set; }
    [field: SerializeField] public int MinesCount { get; private set; }

    private Field field;
    
    private void Start()
    {
        field = new Field(FieldWidth, FieldHeight, MinesCount);
    }
}
