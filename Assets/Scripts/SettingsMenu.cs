using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void SetFieldWidth(float width)
    {
        Debug.Log($"Width: {width}");
    }
    
    public void SetFieldHeight(float width)
    {
        Debug.Log($"Height: {width}");
    }
    
    public void SetMinesCount(float width)
    {
        Debug.Log($"MinesCount: {width}");
    }
    
}
