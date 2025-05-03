using System.Collections.Generic;
using System.Linq;
using BasicGameStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Toggle invertMouseY;
    public Toggle invertMouseX;
    public TMP_Dropdown resolutionDropdown;
    private bool invertX;
    private bool invertY;
    private readonly List<Resolution> newResolutions = new();

    public bool disableOnStart = true;

    private Resolution res;
    private Resolution[] resolutions;

    private double sensitivity = 0.2;
    public static PauseMenu Instance { get; private set; }

    public void Awake()
    {
        Instance = this;

        if (disableOnStart)
        {
            gameObject.SetActive(false);
        }

        sensitivitySlider.onValueChanged.AddListener(value =>
        {
            sensitivity = value;
            PlayerPrefs.SetFloat("sensitivity", value);
            UpdateSensitivity();
        });

        invertMouseX.onValueChanged.AddListener(b =>
        {
            invertX = b;
            PlayerPrefs.SetInt("invertX", invertX ? 1 : 0);
            UpdateSensitivity();
        });

        invertMouseY.onValueChanged.AddListener(b =>
        {
            invertY = b;
            PlayerPrefs.SetInt("invertY", invertY ? 1 : 0);
            UpdateSensitivity();
        });
        
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity");
        invertMouseX.isOn = PlayerPrefs.GetInt("invertX") == 1;
        invertMouseY.isOn = PlayerPrefs.GetInt("invertY") == 1;
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        var currentResIndex = 0;

        foreach (var resolution in resolutions)
        {
            if (resolution.width / 16 == resolution.height / 9)
            {
                options.Add($"{resolution.width}x{resolution.height}@{resolution.refreshRateRatio}");
                newResolutions.Add(resolution);
            }

            var currentRes = Screen.currentResolution;
            if (resolution.width == currentRes.width &&
                resolution.height == currentRes.height &&
                resolution.refreshRateRatio.value == currentRes.refreshRateRatio.value)
                currentResIndex = resolutions.ToList().IndexOf(resolution);
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void UpdateSensitivity()
    {
        var sensitivityX = invertX ? -sensitivity : sensitivity;
        var sensitivityY = invertY ? -sensitivity : sensitivity;

        PlayerController.xSensitivity = (float)sensitivityX;
        PlayerController.ySensitivity = (float)sensitivityY;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

    public void SetResolution(int index)
    {
        res = newResolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRateRatio);
    }
}