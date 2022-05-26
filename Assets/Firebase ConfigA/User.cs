
public class User
{
    public int Distancia; // Puntuacio (Score)
    public int Coins; //Diners (Coins)
    public string UserName; // Nom d'usuari
    public string IdMobil; // Identificador Únic del Dispositiu


    public User(int Distancia, int Coins, string UserName, string IdMobil)
    {
        this.Distancia = Distancia; 
        this.Coins = Coins;
        this.UserName = UserName; 
        this.IdMobil = IdMobil; 

    }    
}