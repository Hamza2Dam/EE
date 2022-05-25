using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{

    public float speed;// Velocitat 
    private Vector3 StartPosition; // Poscio incial



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation: Vector3.left * speed * Time.deltaTime); // Moviment lateral de l'objecte
        transform.Translate(translation: Vector3.down * (speed  - 1) * Time.deltaTime); // Scroll del obejcte cap avall amb un speed
    }
}
