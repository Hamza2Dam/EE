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

    int CoinsDataBase = 0;
    int CoinsInGame = 0;

    int DistanceDataBase = 0;
    int DistanceInGame = 0;

    private string userID;
    private string UserName = "Hamza";

    private DatabaseReference dbReference;
    public Button buttonSave;
    public TimerScript timerScript;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier; // Unique Device ID

        FirebaseDatabase database = FirebaseDatabase.GetInstance("https://final-project-406f7-default-rtdb.firebaseio.com");

        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        buttonSave.onClick.Invoke();

        //SearchIfUserExist();
    }

    public void SearchIfUserExist()
    {

        StartCoroutine(GetUser((string IdDB) =>
        {
            String userIdDatabase = IdDB.ToString(); // igualar un string a id de bases de dades

            Debug.Log(userIdDatabase);
            Debug.Log(IdDB);

            // si existeix el compte
            if (userID.Equals(userIdDatabase)) // 
            {
                NameText.text = "User Exist " + userIdDatabase;

            }

            // si no existeix, crear usuari nou amb BD
            else
            {
                NameText.text = "User No Exist " + userIdDatabase;
                CreateUser();
            }

        }));
    }


    //public void SearchIfUserExist2()
    //{
    //    String usID = SystemInfo.deviceUniqueIdentifier; // Unique Device ID

    //    // Primer el que fem es una consulta
    //    dbReference.Child("Users").Child(userID).GetValueAsync().ContinueWith(task =>
    //    {
    //        if (task.IsFaulted)
    //        {
    //            Debug.Log("Base de Datos esta Null: ");
    //            buttonSave.onClick.Invoke();
    //            CreateUser();
    //        }

    //        else if (task.IsCompleted)
    //        {
    //            DataSnapshot snapshot = task.Result;
    //            string x;
    //            x = snapshot.Child("IdMobil").Value.ToString();

    //            Debug.Log("Compara ID si Existe el Usuario: " + x);

    //            if (x != userID)
    //            {
    //                Debug.Log(" NOT Equal: " + x);
    //                Debug.Log(" NOT Equal: " + usID);

    //                buttonSave.onClick.Invoke();
    //                CreateUser();
    //            }

    //        }
    //    });
    //}



    // Crear Usuari nomes un cop i si userid no existeix 
    public void CreateUser()
    {
        User newUser = new User(DistanceInGame, CoinsInGame, UserName, userID);
        string json = JsonUtility.ToJson(newUser);

        dbReference.Child("Users").Child(userID).SetRawJsonValueAsync(json);
    }


    // GET USER
    public IEnumerator GetUser(Action<string> onCallback)
    {
        var IdDB = dbReference.Child("Users").Child(userID).Child("IdMobil").GetValueAsync();

        yield return new WaitUntil(predicate: () => IdDB.IsCompleted);

        if (IdDB != null)
        {
            DataSnapshot snapshot = IdDB.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
    }

    // GET PUNTUACIO
    public IEnumerator GetDistance(Action<int> onCallback)
    {
        var userDistanceData = dbReference.Child("Users").Child(userID).Child("Distancia").GetValueAsync();

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
        var userCoinData = dbReference.Child("Users").Child(userID).Child("Coins").GetValueAsync();

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

            dbReference.Child("Users").Child(userID).Child("Coins").SetValueAsync(CoinsDataBase);

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
                dbReference.Child("Users").Child(userID).Child("Distancia").SetValueAsync(DistanceInGame);

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

