using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    public GameObject LoginCanvas;
    public GameObject SettingCanvas;
    public GameObject LeaderBoardCanvas;

    private void Start()
    {
        LoginCanvas.SetActive(false); // Desactivar el canvas de Login
        SettingCanvas.SetActive(false); // Desactivar el canvas de Setting
        LeaderBoardCanvas.SetActive(false); // Desactivar el canvas de ScoreBoard/LeaderBoard

    }

    public void Back()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }


    // Log In Board
    public void OpenLoginWindow()
    {
        LoginCanvas.SetActive(true); // Activar el canvas de Login
    }

    public void CloseLoginWindow()
    {
        LoginCanvas.SetActive(false); // Desactivar el canvas de Login
    }

    // Settings Board
    public void OpenSettingWindow()
    {
        SettingCanvas.SetActive(true); // Activar el canvas de Login
    }
    public void CloseSettingWindow()
    {
        SettingCanvas.SetActive(false); // Desactivar el canvas de Login
    }

    // Leader Board
    public void OpenLeaderWindow()
    {
        LeaderBoardCanvas.SetActive(true); // Activar el canvas de Login
    }

    public void CloseLeaderWindow()
    {
        LeaderBoardCanvas.SetActive(false); // Desactivar el canvas de Login
    }




}
