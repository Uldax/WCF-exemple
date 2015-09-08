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
        public Dictionary<string, string> usersList { get; set; }
        //SimpleSondageDao is like the connexion to the DB
        public Dictionary<int, SimpleSondageDAO> usersConnected { get; set; }
        private String token { get; } = "ZRwS7Ve4g1i99MFwsFC43yYSa4RgV1a";

        public SondageService()
        {
            this.usersList = new Dictionary<string, string>();
            this.usersConnected = new Dictionary<int, SimpleSondageDAO>();
            //Password crypted with sha1
            this.usersList.Add("paul", "oCcYSlUhHNI+PzCU8f3HKN9eBQA=");
            this.usersList.Add("coco", "1GTJdTf13UO9SepSg91MguojSJo+");
            this.usersList.Add("admin", "dJE/XNX2HsC8/bd1QUwvs9FhtiA=");
        }

        public int Connect(String name, String password,String token)
        {
            try
            {
                if (token.Equals(this.token))
                {
                    Console.WriteLine("Trying to connect {0}", name);
                    if (this.usersList.ContainsKey(name))
                    {
                        //try to get the password
                        String rightPassword = null;
                        String hashPassword = Sha.HachPassword(password);
                        //out = passage de référence     
                        usersList.TryGetValue(name, out rightPassword);
                        if (hashPassword.Equals(rightPassword))
                        {
                            //create a DAO for the user linked to his id
                            SimpleSondageDAO userDAO = new SimpleSondageDAO();
                            int userId = this.usersConnected.Count + 1;
                            this.usersConnected.Add(userId, userDAO);
                            Console.WriteLine("Utilisateur connected. userId : {0}", userId);
                            return userId;
                        }
                        else
                        {
                            //Invalid id code 
                            throw new InvalidIdException();
                        }
                    }
                    else throw new InvalidIdException();
                }
                else
                {
                    Console.WriteLine("Wrong token");
                    return -2;
                }
            } catch (ArgumentNullException e) {
                Console.WriteLine(e.Message);
            } catch (InvalidIdException e) {
                Console.WriteLine(e.Message);
            }
            return -1;                 
        }

        public IList<Poll> GetAvailablePolls(int userId)
        {
            if (IsUserConnected(userId)) {
                SimpleSondageDAO userDAO = null;
                try {
                    this.usersConnected.TryGetValue(userId, out userDAO);
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
                    this.usersConnected.TryGetValue(userId, out userDAO);
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
            return this.usersConnected.ContainsKey(userId);
        }

        private void DiplayUsersConnected()
        {
            Console.WriteLine("Display");
            this.usersConnected.ToList().ForEach(x => Console.WriteLine(x.Key));
            Console.WriteLine("End display");
        }
    }

}