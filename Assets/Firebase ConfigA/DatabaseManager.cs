using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using System;

public class DatabaseManager : MonoBehaviour
{
    public Text Distancia;
    public Text Coins;
    public Text HightScore;

    int CoinsDataBase = 0; 
    int CoinsInGame = 0;

    int DistanceDataBase = 0;
    int DistanceInGame = 0;

    private string userID;

    private DatabaseReference dbReference;
    public TimerScript timerScript; // script de Timer

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier; // Identificador Únic del Dispositiu

        FirebaseDatabase database = FirebaseDatabase.GetInstance("https://final-project-406f7-default-rtdb.firebaseio.com"); // el nostre base de dades

        dbReference = FirebaseDatabase.DefaultInstance.RootReference; // fem referencia el nostre bases de dades

    }

    // GET PUNTUACIO
    public IEnumerator GetDistance(Action<int> onCallback)
    {
        // fem una variable de tipus implícita = referncia de base de dades, els usuaris que existeixen, el usuari amb la id (Identificador Únic del Dispositiu), agafem las seves monedes
        var userDistanceData = dbReference.Child("Users").Child(userID).Child("Distancia").GetValueAsync();

        // esperar fins que es completar las tasca 
        yield return new WaitUntil(predicate: () => userDistanceData.IsCompleted);

        // si la variable no es null
        if (userDistanceData != null)
        {
            DataSnapshot snapshot = userDistanceData.Result; // fem un DataSnapshot 

            onCallback.Invoke(int.Parse(snapshot.Value.ToString())); // returnem el valor que busquem
        }
    }

    // GET GOLD
    public IEnumerator GetGold(Action<int> onCallback)
    {
        // fem una variable de tipus implícita = referncia de base de dades, els usuaris que existeixen, el usuari amb la id (Identificador Únic del Dispositiu), agafem las seves monedes
        var userCoinData = dbReference.Child("Users").Child(userID).Child("Coins").GetValueAsync();

        // si ha completat las tasca 
        yield return new WaitUntil(predicate: () => userCoinData.IsCompleted);

        // si la variable no es null
        if (userCoinData != null)
        {
            DataSnapshot snapshot = userCoinData.Result; // fem un DataSnapshot 

            onCallback.Invoke(int.Parse(snapshot.Value.ToString())); // returnem el valor que busquem
        }
    }


    // Update Database 
    public void UpdateAllData()
    {
        CoinsInGame = int.Parse(Coins.text); // Diners en la partida

        // GET COINS from DB 
        StartCoroutine(GetGold((int gold) =>
        {
            CoinsDataBase = gold + CoinsInGame; // els diners de DB + els diners recollits en la partida

            dbReference.Child("Users").Child(userID).Child("Coins").SetValueAsync(CoinsDataBase); // Fem un update que seria SetValue

        }));

        StartCoroutine(GetDistance((int distancia) =>
        {
            // la puntuacio que tenim guardat en BD
            DistanceDataBase = distancia;

            // la puntuacio que anem fent durant la partida que agafem del script TimeScript
            DistanceInGame = timerScript.dbTimeCompare;


            if (DistanceDataBase < DistanceInGame) // si la distancia que tenim guardat en base de es mes petit que el que hem fet actual
            {
                //Update en BD si la puntuacio que hem fet es mes gran que el que tenim guardat
                dbReference.Child("Users").Child(userID).Child("Distancia").SetValueAsync(DistanceInGame);

                // Mostrem HighScore -> Canvas HighScore
                HightScore.text = "New HighScore: " + DistanceInGame.ToString() + "m";

            }

            else 
            {
                // Si la Puntuacio es que tenim guardat en Base de dades es mes gran que actual
                // Mostrem la puntuacio maxima -> Canvas HighScore
                HightScore.text = "HighScore: " + DistanceDataBase.ToString() + "m";
            }

        }));

    }



}

