using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Collections;

namespace USherbrooke.ServiceModel.Sondage
{
    //Juste for 1 instance ! Keep a track of all user connected
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
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
            // Liste des utilisateurs autorisés à se connecter
            //TODO protect password
            this.UsersList.Add("paul", "paul");
            this.UsersList.Add("test", "admin");
        }

        public int Connect(String name, String password)
        {
            Console.WriteLine("Trying to connect {0}",name);
            if (this.UsersList.ContainsKey(name))
            {
                //try to get the password
                String rightPassword = null;
                //out = passage de référence
                UsersList.TryGetValue(name, out rightPassword);
                if(password.Equals(rightPassword))
                {
                    // Instanciation d'un sondage pour l'utilisateur
                    SimpleSondageDAO sondage = new SimpleSondageDAO();
              
                    // Instanciation d'un identifiant pour l'utilisateur
                    int userId = this.UsersConnected.Count + 1;
                    this.UsersConnected.Add(userId, sondage);
                    this.DiplayUsers();
                    Console.WriteLine("Utilisateur connecté et authentifié. userId (token): {0}", userId);
                    return userId;
                }                 
            }
            return -1;           
        }

        public IList<Poll> GetAvailablePolls(int userId)
        {
            if (IsUserConnected(userId)) {
                SimpleSondageDAO userDAO = null;
                try {
                    this.UsersConnected.TryGetValue(userId, out userDAO);
                    return userDAO.GetAvailablePolls();
                } catch (ArgumentNullException e) {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("User need to be auth before using this function");
            return null;
        }

        public PollQuestion GetNext(int userId, PollQuestion answer)
        {
            if (IsUserConnected(userId))
            {
                SimpleSondageDAO userDAO = null;
                try{
                    //get the user DAO
                    this.UsersConnected.TryGetValue(userId, out userDAO);
                    //Save the answer
                    userDAO.SaveAnswer(userId, answer);
                    //Then return the next question 
                    return userDAO.GetNextQuestion(answer.PollId, answer.QuestionId);
                }
                catch (ArgumentNullException e) {
                    Console.WriteLine(e.Message);
                } catch (InvalidIdException e)  {
                    Console.WriteLine(e.Message);
               }                     
            }
            return null;
        }

        private bool IsUserConnected(int userId)
        {
            return this.UsersConnected.ContainsKey(userId);
        }

        private void DiplayUsers()
        {
            Console.WriteLine("Display");
            this.UsersConnected.ToList().ForEach(x => Console.WriteLine(x.Key));
            Console.WriteLine("End display");
        }
    }

}