using SondageClient.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SondageClient
{
    class Program
    {
        public static int readEntry()
        {
            int pollId = 0;
            bool secondTry = false;
            String idRead = null;
             do {
                if(secondTry) {
                    Console.WriteLine("Wrong entry");
                }
                Console.WriteLine("Choix du sondage :");
                idRead = Console.ReadLine();
                secondTry = true;
                if (String.IsNullOrEmpty(idRead) == false)
                {
                    pollId = Convert.ToInt32(idRead);
                }
                // Demande du choix du sondage tant que le choix est invalide
            } while (String.IsNullOrEmpty(idRead) || pollId < 1 || pollId > idRead.Length);
            return pollId;
        }

        public static String readAnswer()
        {
            String response;
            // Mise à jour de la réponse
            do
            {
                Console.WriteLine("Votre réponse (a, b, c ou d):");
                response = Console.ReadLine();
            } while (String.IsNullOrEmpty(response) || !(response.Equals("a") || response.Equals("b") || response.Equals("c") || response.Equals("d")));
            return response;
        }

        public static PollQuestion craftFirstQuestion(int idPoll) {
            // Question par défaut pour obtenir la première question du sondage choisi
            PollQuestion firstQuestion = new PollQuestion();
            firstQuestion.PollId = idPoll;
            firstQuestion.QuestionId = -1;
            return firstQuestion;
        }
 
        public static void Main(string[] args)
        {
            SondageServiceClient client = new SondageServiceClient();
            try {
                String mdp = "paul";
                int userID =  client.Connect("paul",mdp);
                // Si l'authenfication a échouée
                if (userID == -1) {
                    throw new Exception("Erreur d'authentification.");
                } else {
                    Console.WriteLine("Connected");
                    Poll[] polls = client.GetAvailablePolls(userID);

                    // Display all available poll
                    if (polls != null) {        
                        Console.WriteLine("Sondages disponibles : {0} \n",polls.Length);
                        foreach (Poll poll in polls)
                        {
                            Console.WriteLine(poll.Id + ". " + poll.Description);
                        }
                    }

                    //ask the user to choose a poll
                    int idPoll = readEntry();
                    Console.WriteLine("You choose {0}", idPoll);

                    // Default question to start the poll
                    PollQuestion firstQuestion = craftFirstQuestion(idPoll);
                    PollQuestion question = client.GetNext(userID, firstQuestion);           
                    String answer = readAnswer();
                    question.Text = answer;

                    while ((question = client.GetNext(userID, question)) != null)
                    {
                        //Display the question
                        Console.WriteLine(question.QuestionId + ". " + question.Text);
                        //Ask for an answer
                        answer = readAnswer();
                        question.Text = answer;
                    }

                }
            } catch(Exception e) {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Step 3: Closing the client gracefully closes the connection and cleans up resources.
                Console.WriteLine("Appuyer sur une touche pour fermer la console...");
                Console.ReadLine();
                client.Close();
            }
   
        }
    }
}
