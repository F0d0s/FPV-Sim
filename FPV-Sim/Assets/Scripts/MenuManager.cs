using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{
    
    //Menus
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject playMenu;
    [SerializeField] private GameObject pauseMenu;

    //Input fields
    [SerializeField] private TMP_InputField MaxRR;
    [SerializeField] private TMP_InputField CenterRR;
    [SerializeField] private TMP_InputField ExpoR;
    [SerializeField] private TMP_InputField MaxRP;
    [SerializeField] private TMP_InputField CenterRP;
    [SerializeField] private TMP_InputField ExpoP;
    [SerializeField] private TMP_InputField MaxRY;
    [SerializeField] private TMP_InputField CenterRY;
    [SerializeField] private TMP_InputField ExpoY;
    [SerializeField] private TMP_InputField DronePower;
    [SerializeField] private TMP_InputField camAngle;
    [SerializeField] private Slider volumeSlider;
    private string readString;

    //Pausing
    public bool isPaused = false;

    //Other
    public Transform camTranform;
    
    //Audio volume
    public AudioMixer audioMixer;
    void Start()
    {

        SetValues();
        UpdateValues();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            if (isPaused == false)
            {
                pauseMenu.SetActive(true);
                Cursor.visible = true;
                isPaused = true;
                Time.timeScale = 0;
                SetValues();
            }
            else if (isPaused == true)
            {
                pauseMenu.SetActive(false);
                Cursor.visible = false;
                isPaused = false;
                Time.timeScale = 1;
            }
        }

    }



    public void SettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        SetValues();
    }
    
    public void PlayMenu()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void Back()
    {
        if (settingsMenu.activeSelf == true)
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
        if (playMenu.activeSelf == true)
        {
            playMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    public void Exit()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ForestMap()
    {
        SceneManager.LoadScene("Forest");
        Cursor.visible = false;
    }
    public void PrototypeMap()
    {
        SceneManager.LoadScene("SciFI");
        Cursor.visible = false;
    }

    public void Volume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        GameValues.Volume = volume;
    }



    
    public void SetValues()
    {
        MaxRR.text = GameValues.MaxRateRoll.ToString();
        CenterRR.text = GameValues.CenterSensitivityRoll.ToString();
        ExpoR.text = GameValues.ExpoRoll.ToString();
        MaxRP.text = GameValues.MaxRatePitch.ToString();
        CenterRP.text = GameValues.CenterSensitivityPitch.ToString();
        ExpoP.text = GameValues.ExpoPitch.ToString();
        MaxRY.text = GameValues.MaxRateYaw.ToString();
        CenterRY.text = GameValues.CenterSensitivityYaw.ToString();
        ExpoY.text = GameValues.ExpoYaw.ToString();
        DronePower.text = GameValues.DronePower.ToString();
        volumeSlider.value = GameValues.Volume;
        camAngle.text = GameValues.CamAngle.ToString();
    }

    public void UpdateValues()
    {
        camTranform.localRotation = Quaternion.Euler(-GameValues.CamAngle, 0,0);
        audioMixer.SetFloat("Volume", GameValues.Volume);
    }
    
    public void CamAngle(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.CamAngle = Mathf.Clamp(value, 0f, 90f);
        camTranform.localRotation = Quaternion.Euler(-GameValues.CamAngle,0,0);
    }
    
    public void MaxRRValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.MaxRateRoll = value;
    }
    public void CenterRRValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.CenterSensitivityRoll = value;
    }
    public void ExpoRValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.ExpoRoll = value;
    }
    public void MaxRPValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.MaxRatePitch = value;
    }
    public void CenterRPValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.CenterSensitivityPitch = value;
    }
    public void ExpoPValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.ExpoPitch = value;
    }
    public void MaxRYValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.MaxRateYaw = value;
    }
    public void CenterRYValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.CenterSensitivityYaw = value;
    }
    public void ExpoYValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.ExpoYaw = value;
    }
    public void DronePowerValue(string input)
    {
        if (float.TryParse(input, out float value) == false)
        {
            SetValues();
        }
        GameValues.DronePower = value;
    }
}
