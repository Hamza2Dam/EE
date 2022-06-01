using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

 // La classe Score Element ens serveix per fer l'ScoreBoard de l'usuari
public class ScoreElement : MonoBehaviour
{
    public TMP_Text UserNameText; // Aix� serveix per guardar el nom de l'usuari
    public TMP_Text HighScoreText; // Aix� seria per guardar la puntuaci� m�xima

    public void NewScoreElement (string _username, int _coins)
    {
        UserNameText.text = _username;
        HighScoreText.text = _coins.ToString();
    }

}
