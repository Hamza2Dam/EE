using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using System;

public class ProfileDataBaseManager : MonoBehaviour
{
    public Text TotalCoins;
    public Text UserNam;

    public InputField UserName;

    private DatabaseReference dbReference;

    private string userID;
    private int DistanceInGame = 0;
    private int CoinsInGame = 0;

    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier; // Identificador Únic del Dispositiu

        FirebaseDatabase database = FirebaseDatabase.GetInstance("https://final-project-406f7-default-rtdb.firebaseio.com"); // bases de dades

        dbReference = FirebaseDatabase.DefaultInstance.RootReference; // referecia a la nostra base de dades

        //SearchIfUserExist();

        GetTotalCoins();
        GetUserName();

    }

    // Comprovar si l'usari existeix o no (NO ens ha acabat de funcion aquesta funció)
    // el que volem aconseguir es un usuari ja ha registart un cop i ja surt en bases de dades 
    // doncs que no pugui crear un altre cop perquè si ho fa, és substitueixen les dades 

    // El que he fet es agafar les ID de bases de dades i compara si aquella ID de BD es la mateixa que 
    // Identificador Únic del Dispositiu si es així doncs l'usuari ja existeix, i si no doncs que cridi la funcio de crear usuari, molt simple, pero per alguna rao no funciona
    public void SearchIfUserExist()
    {
        StartCoroutine(GetUserID((string IdDB) =>
        {
            // amb la funcion getuserid aconseguim las id guardades en bases de dades

            String userIdDatabase = IdDB.ToString(); // igualar un string a id de bases de dades

            // si existeix el compte
            if (userID.Equals(userIdDatabase)) // 
            {
                Debug.Log("User Exist " + userIdDatabase);
            }

            // si no existeix, crear usuari nou amb BD
            else
            {
                Debug.Log("User No Exist " + userIdDatabase);
                CreateUser();
            }
        }));
    }


    // GET USER
    public IEnumerator GetUserID(Action<string> onCallback)
    {
        var IdDB = dbReference.Child("Users").Child(userID).Child("IdMobil").GetValueAsync();

        yield return new WaitUntil(predicate: () => IdDB.IsCompleted);

        if (IdDB != null)
        {
            DataSnapshot snapshot = IdDB.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
    }


    // Crear un nou usuari en bases de dades
    public void CreateUser()
    {
        // tenim una classe user i li enviem els quatres apartats que volem guardar
        User newUser = new User(DistanceInGame, CoinsInGame, UserName.text.ToString(), userID);

        // guardem el user en format json 
        string json = JsonUtility.ToJson(newUser);

        // amb la referencia de bases de dades crear el User, amb l'UID i amb el setrawjson que son les dads que volem guardar
        dbReference.Child("Users").Child(userID).SetRawJsonValueAsync(json);
    }


    public void GetUserName()
    {
        // Amb GetUserName aconseguim el nom del usuari creat
        StartCoroutine(GetUserName((string nam) =>
        {
            string name = nam; // varaible name

            UserNam.text = "UserName:" + name; // Mostar el nom del user

        }));
    }



    public void GetTotalCoins()
    {
        // Fem get per aconseguir les monedes totals que tenim guarddes en bases de dades

        StartCoroutine(GetTotalGold((int gold) =>
        {
            string coin = gold.ToString(); // igualar una variable amb els diners total que tenim guardat (es podria eviat aquesta linia de codi, pero per fer ho mes facil de entendre fem aixi)

            TotalCoins.text = "Total:" + coin; // Mostar les monedes total que te un usuari

        }));
    }


    // GET Total GOLD
    public IEnumerator GetTotalGold(Action<int> onCallback)
    {
        var userCoinData = dbReference.Child("Users").Child(userID).Child("Coins").GetValueAsync();

        yield return new WaitUntil(predicate: () => userCoinData.IsCompleted);

        if (userCoinData != null)
        {
            DataSnapshot snapshot = userCoinData.Result;

            onCallback.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }

    // GET USERNAME
    public IEnumerator GetUserName(Action<string> onCallback)
    {
        var IdDB = dbReference.Child("Users").Child(userID).Child("UserName").GetValueAsync();

        yield return new WaitUntil(predicate: () => IdDB.IsCompleted);

        if (IdDB != null)
        {
            DataSnapshot snapshot = IdDB.Result;

            onCallback.Invoke(snapshot.Value.ToString());
        }
    }
}
