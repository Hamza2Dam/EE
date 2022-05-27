using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text UserNameText; 
    public TMP_Text HighScoreText;

    public void NewScoreElement (string _username, int _coins)
    {
        UserNameText.text = _username;
        HighScoreText.text = _coins.ToString();
    }

}
