using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Slider fieldWidthSlider;
    [SerializeField] private Slider fieldHeightSlider;
    [SerializeField] private Slider minesCountSlider;
    [SerializeField] private TextMeshProUGUI fieldWidthTextMeshPro;
    [SerializeField] private TextMeshProUGUI fieldHeightTextMeshPro;
    [SerializeField] private TextMeshProUGUI minesCountTextMeshPro;

    private void OnEnable()
    {
        fieldWidthSlider.onValueChanged.AddListener(SetFieldWidth);
        fieldHeightSlider.onValueChanged.AddListener(SetFieldHeight);
        minesCountSlider.onValueChanged.AddListener(SetMinesCount);
    }

    private void OnDisable()
    {
        fieldWidthSlider.onValueChanged.RemoveListener(SetFieldWidth);
        fieldHeightSlider.onValueChanged.RemoveListener(SetFieldHeight);
        minesCountSlider.onValueChanged.RemoveListener(SetMinesCount);
    }

    private void Start()
    {
        SetFieldWidth(PlayerPrefs.GetInt("Width", 9));
        SetFieldHeight(PlayerPrefs.GetInt("Height", 16));
        SetMinesCount(PlayerPrefs.GetInt("MinesCount", 20));
    }

    private void SetFieldWidth(float width)
    {
        Debug.Log($"Width: {width}");
        PlayerPrefs.SetInt("Width", (int)width);
        fieldWidthTextMeshPro.text = $"Width of field: {fieldWidthSlider.value}";
        UpdateMinesCountSliderMaxValue();
    }
    
    private void SetFieldHeight(float height)
    {
        Debug.Log($"Height: {height}");
        PlayerPrefs.SetInt("Height", (int)height);
        fieldHeightTextMeshPro.text = $"Height of field: {fieldHeightSlider.value}";
        UpdateMinesCountSliderMaxValue();
    }
    
    private void SetMinesCount(float minesCount)
    {
        Debug.Log($"MinesCount: {minesCount}");
        PlayerPrefs.SetInt("MinesCount", (int)minesCount);
        minesCountTextMeshPro.text = $"Count of mines: {minesCountSlider.value}";
    }

    private void UpdateMinesCountSliderMaxValue()
    {
        minesCountSlider.maxValue = fieldWidthSlider.value * fieldHeightSlider.value - 9;
        minesCountTextMeshPro.text = $"Count of mines: {minesCountSlider.value}";
    }
    
}
