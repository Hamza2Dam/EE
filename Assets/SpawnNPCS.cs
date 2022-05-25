using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNPCS : MonoBehaviour
{



    // NPC
    public GameObject[] npc;


    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {

        InvokeRepeating("SpawnNext", 1f, 5f); // Cada x temps (xf) cridarem a la funció SpawnNext per spawnejar un nou objecte


    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnNext()
    {
        int rand = Random.Range(0, 2); // Número random per escollir els carrils on spawnejar l'objecte de manera random
        GameObject generate = Instantiate(npc[rand]); // Objecte a spawnejar (Instanciar)




        generate.transform.position = new Vector3(4, 43, 0); // Igualem la posicio x del objecte amb la del carril corresponent.




    }


    void OnTriggerEnter2D(Collider2D other) // Colisió Trigger 
    {

        if (other.gameObject.CompareTag("car")) // Si l'objecte amb el qual colisionem te un tag == "";
        {
            Debug.Log("choca");
            Destroy(gameObject);
            

        }
    }
}
