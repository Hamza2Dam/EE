using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject DiamondCanvas;


    private void Start()
    {
        DiamondCanvas.SetActive(false); // Desactivar el canvas de Diamond
    }
    public void Back()
    {
        SceneManager.LoadScene("MenuPrincipal"); // Cargar Menu Principal
    }

    public void OpenDiamondWindow()
    {
        DiamondCanvas.SetActive(true); // Activar el canvas de Diamond
    }

    public void CloseDiamondWindow()
    {
        DiamondCanvas.SetActive(false); // Desactivar el canvas de Diamond
    }

}
