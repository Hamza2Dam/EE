using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCotxe : MonoBehaviour
{
    public float speed;// Velocitat del scroll
    private Vector3 StartPosition; // Poscio incial


    void Start()
    {
        //StartPosition = transform.position; // Posici� inicial del objecte (Primer objecte que se spawneja)

    }



    void Update()
    {
        transform.Translate(translation: Vector3.down * speed * 2 * Time.deltaTime); // Scroll del obejcte cap avall amb un speed
   
        //if (transform.position.y < limitycr1)
        //{

        //    transform.position = StartPosition;
        //}
    }

    void OnTriggerEnter2D(Collider2D other) // Colisi� Trigger 
    {

        if (other.gameObject.CompareTag("npc")) // Si l'objecte amb el qual colisionem te un tag == "";
        {
            Debug.Log("choca");
            Destroy(other.gameObject);


        }
    }




}
