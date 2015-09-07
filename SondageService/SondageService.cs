using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections;

namespace USherbrooke.ServiceModel.Sondage
{

    public class SondageService : ISondageService
    {
        // Couple login, mot de passe pour authentifier l'utilisateur
        public Dictionary<string, string> UsersList { get; set; }
        //SimpleSondageDao is like the connexion to the DB
        public Dictionary<int, SimpleSondageDAO> UsersConnected { get; set; }

        public SondageService()
        {
            this.UsersList = new Dictionary<string, string>();
            this.UsersConnected = new Dictionary<int, SimpleSondageDAO>();
            // Liste des utilisateurs autoris�s � se connecter
            //TODO protect password
            this.UsersList.Add("paul", "paul");
            this.UsersList.Add("test", "admin");

            Console.WriteLine("******************");
            Console.WriteLine("SondageWS - SERVER");
            Console.WriteLine("******************");
            Console.WriteLine(String.Empty);
        }

        public int Connect(String name, String password)
        {
            Console.WriteLine("Trying to connect {0}",name);
            if (this.UsersList.ContainsKey(name))
            {
                //try to get the password
                String rightPassword = null;
                //out = passage de r�f�rence
                UsersList.TryGetValue(name, out rightPassword);
                if(password.Equals(rightPassword))
                {
                    // Instanciation d'un sondage pour l'utilisateur
                    SimpleSondageDAO sondage = new SimpleSondageDAO();
              
                    // Instanciation d'un identifiant pour l'utilisateur
                    int userId = this.UsersConnected.Count + 1;
                        
                    // Ajout de l'utilisateur en m�moire
                    this.UsersConnected.Add(userId, sondage);
                    Console.WriteLine("Utilisateur connect� et authentifi�. userId (token): {0}", userId);
                    return userId;
                }                 
            }
            return -1;           
        }

        public IList<Poll> GetAvailablePolls(int userId)
        {
            throw new NotImplementedException();
        }

        public PollQuestion GetNext(int userId, PollQuestion answer)
        {
            throw new NotImplementedException();
        }
    }

}