using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Rigidbody de l'objecte : 
    Rigidbody2D rb;
    
    
    // GameManager : 
    public GameManager gm;

    // Carrils : 

    public Transform carril1;
    public Transform carril2;
    public Transform carril3;


    // DataBase : 
    public DatabaseManager dbscript;


    private int contadorcoins;
    private bool stopGame;

   

    // Swap : 
    private float swipeRange = 50;
    private float tapRange = 10;
    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private Vector2 endTouchPosition;
    private bool stopTouch = false;



    public  Text coins;
    public Text finalcoins;

    public ScrollCarreteras speed;






    // Start is called before the first frame update
     void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody


        rb.transform.position = new Vector3(carril2.transform.position.x, carril2.transform.position.y, 0);
        contadorcoins = 0; // Iniciem el comptador a 0
        stopGame = false;
        Time.timeScale = 1;

        


    }

    // Update is called once per frame
    void Update()
    {

        Swipe();
        movementpc();
       
    }


    // Android Movement

    public void Swipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentPosition = Input.GetTouch(0).position;
            Vector2 Distance = currentPosition - startTouchPosition;

            if (!stopTouch)
            {

                if (Distance.x < -swipeRange)
                {
                    Left();


                    stopTouch = true;
                }
                else if (Distance.x > swipeRange)
                {
                    Right();


                    stopTouch = true;
                }


            }

        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouch = false;

            endTouchPosition = Input.GetTouch(0).position;

            Vector2 Distance = endTouchPosition - startTouchPosition;

            if (Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange)
            {
                //outputText.text = "Tap";
            }

        }


    }





    // Pc Movement


    private void movementpc() 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Left();
        }
        else if (Input.GetKeyDown(KeyCode.D)) 
        {

            Right();
        }



    }







  


    private  void Right() // Girar a la dreta
    {

        if (rb.transform.position == carril2.transform.position) // Si l'objecte está en el carril 2 (igualem les coordenades x) el pasem al carril3.x
        {

            rb.transform.position = new Vector3(carril3.transform.position.x, carril3.transform.position.y, 0);

         

        }
        else if (rb.transform.position == carril1.transform.position) // Si l'objecte está en el carril 1 (igualem les coordenades x) el pasem al carril2.x
        {

            rb.transform.position = new Vector3(carril2.transform.position.x, carril2.transform.position.y, 0);
        }


    }

    private void Left() // Girar a l'esquerra
    {
        if (rb.transform.position == carril2.transform.position)
        {

            // rb.AddForce(Vector2.left);
            //rb.transform.position = new Vector3(limitesq, -1, 0);

            rb.transform.position = new Vector3(carril1.transform.position.x, carril1.transform.position.y, 0);


        }
        else if (rb.transform.position == carril3.transform.position)
        {

            rb.transform.position = new Vector3(carril2.transform.position.x, carril2.transform.position.y, 0);

        }
    }




        //}

        void OnTriggerEnter2D(Collider2D other) // Colisió Trigger 
        {

        if (other.gameObject.CompareTag("car")) // Si l'objecte amb el qual colisionem te un tag == "";
        {

            GameOverFuntion(); // Game Over

        }
      
        else if (other.gameObject.CompareTag("OilSpeed")) // Si l'objecte amb el qual colisionem te un tag == "";
        {
      
            gm.gasoilsound.Play(); // Gasoil SoundEffect
            gm.gasolinascript.Sumargasolina(); // Sumem la gasolina
            Destroy(other.gameObject);
            
    
        }
        else if (other.gameObject.CompareTag("npc")) // Si l'objecte amb el qual colisionem te un tag == "";
        {

          
            Destroy(other.gameObject);
            GameOverFuntion();


        }
        
        else if (other.gameObject.CompareTag("coin")) // Si l'objecte amb el qual colisionem te un tag == "";
        {

            contadorcoins++; // Sumem la moneda
            gm.coinssound.Play();// So de la moneda
         
            coins.text = contadorcoins.ToString(); // Mostrem la suma
            Destroy(other.gameObject); // Destruim la moneda

        }


    }

   



    public void GameOverFuntion() {

        dbscript.UpdateAllData(); // Acutalitzar les dadese de bases de dades, seia la puntuació(HighScore) i els diners(Coins) , aquesta funcion es cridat d'un altre script

        
        gm.gameoversound.Play(); // GameOver soundEffect


        gm.GameOverObject.SetActive(true); // Activem el canvas de GameOver

        finalcoins.text = contadorcoins.ToString() ;

        StopGame();// Parar tot
      
    }

    private void StopGame()
    {
        Time.timeScale = 0;
        stopGame = true;

    
        Destroy(gm.SpawnCotxes); // Parar Cotxes
        Destroy(gm.SpawnMonedes);    

    }








}
