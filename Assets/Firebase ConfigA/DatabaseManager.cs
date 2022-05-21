using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using System;
using Google;
using Firebase.Auth;

public class DatabaseManager : MonoBehaviour
{
    public Text Distancia;
    public Text Coins;
    public Text HightScore;
    public Text NameText;

    public Text AnotherText;

    int CoinsDataBase = 0;
    int CoinsInGame = 0;

    int DistanceDataBase = 0;
    int DistanceInGame = 0;

    private string userID;
    private DatabaseReference dbReference;

    public TimerScript timerScript;
    public GoogleSignInDemo googleScript;

    private string UserGoogleEmail = "hamza@gmail.com"; // EMAIL DE GOOGLE FALTA CONFIGURAR
    private string UserGoogleUID = "123456789";  // ID DE GOOGLE FALTA CONFIGURAR


    // Varaibles para recibit datos del usuario cuando inicia sesion de google
    private string UserName;
    private string UserEmail;
    private string UserUID;

    void Update()
    {
        UserName = googleScript.Email;
        UserEmail = googleScript.Name;
        UserUID = googleScript.UID;

        AnotherText.text = UserName + " // " + UserEmail + " // " + UserUID;
    }


    void Start()
    {    
        FirebaseDatabase database = FirebaseDatabase.GetInstance("https://final-project-406f7-default-rtdb.firebaseio.com");

        //userID = SystemInfo.deviceUniqueIdentifier; // Unique Device ID

        userID = UserGoogleUID;
        //userID = UserName;

        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    public void SearchUserExist()
    {    
        StartCoroutine(GetUser((string name) =>
        {
            // si existeix el compte
            String ida = name.ToString();

            if (userID == ida) // 
            {
                NameText.text = "Exist " + ida;

            }
            // si no existeix, crear usuari nou
            else
            {
                NameText.text = "No Exist " + ida;
            }

        }));
    }

    // Crear Usuari nomes un cop i si userid no existeix || falta funcionalitat
    public void CreateUser()
    {
        User newUser = new User(DistanceInGame, CoinsInGame, UserGoogleEmail, userID);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);

    }


    // GET USER
    public IEnumerator GetUser(Action<string> onCallback)
    {
        var userNameData = dbReference.Child("users").Child(userID).Child("UID_Google").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
    }


    // GET PUNTUACIO
    public IEnumerator GetDistance(Action<int> onCallback)
    {
        var userDistanceData = dbReference.Child("users").Child(userID).Child("distancia").GetValueAsync();

        yield return new WaitUntil(predicate: () => userDistanceData.IsCompleted);

        if (userDistanceData != null)
        {
            DataSnapshot snapshot = userDistanceData.Result;

            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }


    // GET GOLD
    public IEnumerator GetGold(Action<int> onCallback)
    {
        var userCoinData = dbReference.Child("users").Child(userID).Child("coins").GetValueAsync();

        yield return new WaitUntil(predicate: () => userCoinData.IsCompleted);

        if (userCoinData != null)
        {
            DataSnapshot snapshot = userCoinData.Result;

            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
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

            dbReference.Child("users").Child(userID).Child("coins").SetValueAsync(CoinsDataBase);

        }));

        StartCoroutine(GetDistance((int distancia) =>
        {
            // la puntuacio que tenim guardat en BD
            DistanceDataBase = distancia;

            // la puntuacio que anem fent durant la partida que agafem del script TimeScript
            DistanceInGame = timerScript.dbTimeCompare;


            if (DistanceDataBase < DistanceInGame)
            {
                //Update en BD si la puntuacio que hem fet es mes gran que el que tenim guardat
                dbReference.Child("users").Child(userID).Child("distancia").SetValueAsync(DistanceInGame);

                // Canvas HighScore
                HightScore.text = "New HighScore: " + DistanceInGame.ToString() + "m";

            }

            else
            {
                // Canvas HighScore
                HightScore.text = "HighScore: " + DistanceDataBase.ToString() + "m";
            }

        }));

    }



}

