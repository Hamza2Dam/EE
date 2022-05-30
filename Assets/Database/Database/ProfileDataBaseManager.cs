using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;


public class ProfileDataBaseManager : MonoBehaviour
{
    private string userID;
    private int DistanceInGame = 0;
    private int CoinsInGame = 0;

    public Text TotalCoins;
    public Text Usertxt;

    public InputField UserName;

    private DatabaseReference dbReference; 

    public GameObject LeaderBoardCanvas;
    public GameObject scoreElement;

    public Transform scoreboardContent;

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

    // Crear un nou usuari en bases de dades
    public void CreateUser()
    {
        // tenim una classe user i li enviem els quatres apartats que volem guardar
        User newUser = new User(DistanceInGame, CoinsInGame, UserName.text.ToString(), userID);

        // guardem el user en format json 
        string json = JsonUtility.ToJson(newUser);

        // amb la referencia de bases de dades crear el User, amb l'UID i amb el setrawjson que son les dads que volem guardar
        dbReference.Child("Users").Child(userID).SetRawJsonValueAsync(json);
        SceneManager.LoadScene("MenuPrincipal");
        
    }


    //GET USER NAME
    public void GetUserName()
    {
        // Amb GetUserName aconseguim el nom del usuari creat
        StartCoroutine(GetUserName((string nam) =>
        {
            string name = nam; // varaible name

            Usertxt.text = "UserName:" + name; // Mostar el nom del user

        }));
    }

    // Diners Total que te l'usuari
    public void GetTotalCoins()
    {
        // Fem get per aconseguir les monedes totals que tenim guarddes en bases de dades

        StartCoroutine(GetTotalGold((int gold) =>
        {
            string coin = gold.ToString(); // igualar una variable amb els diners total que tenim guardat (es podria eviat aquesta linia de codi, pero per fer ho mes facil de entendre fem aixi)

            TotalCoins.text = "Total:" + coin; // Mostar les monedes total que te un usuari

        }));
    }

    // boto que crida aquesta funci, surt que no te referencia pero si que utilitzem aquesta funcio
    public void ScoreBoardButton() // el boto de ScoreBoard
    {
        StartCoroutine(LoadScoreBoard()); // Cridem la funció LoadScoreBoard
    }


    // Cargar Dades en Score Board (Ranking by HighScore)
    private IEnumerator LoadScoreBoard()
    {
        var DBtask = dbReference.Child("Users").OrderByChild("Distancia").GetValueAsync(); // Agafem tots els usuaris i ordenem per la distancia major a menor i fem un getValue
         
        yield return new WaitUntil(predicate: () => DBtask.IsCompleted); // esperem fins que acabi la tasca 

        if (DBtask.Exception != null) // si la tasca ha donat error no fem res (per veure hem fet un debug que seria per unity)
        {
            Debug.Log("Failed" + DBtask.Exception);
        }
        else
        {
            // Si la tasca ha estat completada donç guardem el resultat en Datasnapshot
            DataSnapshot snapshot = DBtask.Result;

            foreach (Transform child in scoreboardContent.transform) // Per cada Fila(Child) que surti en scoreboard
            {
                Destroy(child.gameObject); // Destruis/Netejar el ScoreBoard 
            }

            // Recorre cada UID d'usuari
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("UserName").Value.ToString(); // Nom usuari
                int highscore = int.Parse(childSnapshot.Child("Distancia").Value.ToString()); // puntuació més alta


                // Instanciar nous elements del marcador
                // scoreElement es un prefab que tenim creat
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);

                // Cridem la funcion NewScoreElement i enviem l'usuari i la puntuació
                // amb l'ajuda del prefab que tenim ja creat posarem aquell prefab amb aquests textos
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, highscore);
            }

            LeaderBoardCanvas.SetActive(true); // Activar el canvas de Ranking

        }
    }


    // GET USER ID
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
