using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class another : MonoBehaviour
{

    public GoogleSignInDemo gsd;
    public Text sd;

    private string X;
    private string Y;
    private string Z;

    void Update()
    {
        X = gsd.Email;
        Y = gsd.Name;
        Z = gsd.UID;

        sd.text = X + " / " + Y + " / " + Z;
    }

}
