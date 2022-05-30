using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using UnityEngine;
using UnityEngine.UI;

public class GoogleSignInDemo : MonoBehaviour
{
    // Hem estat seguint un tutorials per conectar el compte google.
    // Ja que el firebase mateix ens proporciona unes funcions que hem de utilitzar, per aixo necessitem molts coneixements.
    // Per anar bé hem estat mirant uns tutorial, en ha funcionat tot meny que es guardi la UID en bases de dades

    private string webClientId = "352196159774-d52l815eaqiqvan43po01fe6tj4h9tfu.apps.googleusercontent.com"; // Auth de Google del Firebase

    private string Name;
    private string Email;
    private string UID;

    public Text Emailtxt;

    private GoogleSignInConfiguration configuration;

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
    }

    public void SignInWithGoogle() { OnSignIn(); } // Posem SignInWithGoogle al botó, que cridara la funció de sign in

    public void SignOutFromGoogle() { OnSignOut(); } // Posem SignOutFromGoogle al botó, que cridara la funció de sign out



    // Sign In
    private void OnSignIn()
    {
        Emailtxt.text = "Sign In..."; // mostre sign in

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        // Continuem amb la funció OnAuthenticationFinished Per comprovar possibles errors
        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnAuthenticationFinished);
    }


    // Sign Out
    private void OnSignOut()
    {
        Emailtxt.text = "Sign Out..."; // mostre sign out

        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    // A l'autenticació finalitzada
    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        // Si ha fallat la tasca (sign in) per algun error, doncs mostrem els errors
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }

        // Si ha cancelat la tasca (sign in), aixo seria si fa el back, doncs mostrem Canceled

        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }

        // Si la tasca  (sign in)  ha sigutcompletada o sigui ha pugut fer log in doncs mostrem les dades
        else
        {
            Name = task.Result.DisplayName; // Nom del usuari connectat amb la conta de google
            Email = task.Result.Email; // Email
            UID = task.Result.IdToken; // UID
            Email = task.Result.Email; // Email

            Emailtxt.text = Email.ToString();                  
        }

    }

    // Amb aquesta funcio anem mustran en text totes les dades para saber si tot va bé // ho tenim desactivat,
    // como ja hem comentat ens mostra la info pero no deixar guarda en bases de dadse
    // hem explcat mes detalladament en document
    private void AddToInformation(string str)
    {
        //infoText.text += "\n" + str;
    }


}