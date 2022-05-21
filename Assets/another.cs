using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class another : MonoBehaviour
{

    public GoogleSignInDemo googleScript;
    public Text AnotherText;

    private string X;
    private string Y;
    private string Z;

    void Update()
    {
        X = googleScript.Email;
        Y = googleScript.Name;
        Z = googleScript.UID;

        AnotherText.text = X + " / " + Y + " / " + Z;
    }

}
