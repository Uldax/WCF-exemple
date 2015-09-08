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
        public static int readEntry(int maxValue)
        {
            int pollId = 0;
            bool secondTry = false;
            String idRead = null;
             do {
                if(secondTry) {
                    Console.WriteLine("Please use the right syntaxe");
                }
                Console.WriteLine("Poll choice :");
                idRead = Console.ReadLine();
                secondTry = true;
                if (String.IsNullOrEmpty(idRead) == false) {
                    pollId = Convert.ToInt32(idRead);
                }
                // Ask while there is an error
            } while (String.IsNullOrEmpty(idRead) || pollId < 1 || pollId > maxValue);
            return pollId;
        }

        public static String readAnswer()
        {
            String response;
            do
            {
                Console.WriteLine("Votre réponse (a, b, c ou d):");
                response = Console.ReadLine();
            } while (String.IsNullOrEmpty(response) || !(response.Equals("a") || response.Equals("b") || response.Equals("c") || response.Equals("d")));
            return response;
        }

        public static String readCredentials()
        {
            String credential;
            do
            {
                credential = Console.ReadLine();
            } while (String.IsNullOrEmpty(credential) );
            return credential;
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
            try
            {
                String user = null, pass = null;
                String token = "ZRwS7Ve4g1i99MFwsFC43yYSa4RgV1a";
                int userID = -1;
                Console.Write("Login :");
                user = readCredentials();
                Console.Write("Password :");
                pass = readCredentials();      
                userID = client.Connect(user,pass,token);

                //If auth failed
                if (userID == -1)
                {
                    throw new Exception("Wrong credentials");
                } else if (userID == -2)
                {
                    throw new Exception("Server not certified");
                }
                else
                {
                    Console.WriteLine("User {0}  connected",userID);
                    Poll[] polls = client.GetAvailablePolls(userID);
                    // Display all available poll
                    if (polls != null)
                    {
                        Console.WriteLine("Available poll : {0} \n", polls.Length);
                        foreach (Poll poll in polls)
                        {
                            Console.WriteLine(poll.Id + ". " + poll.Description);
                        }
                    }

                    //Ask the user to choose a poll
                    int idPoll = readEntry(polls.Length);


                    //TODO : write in folder in fanal version
                    Console.WriteLine("User {0} ask for poll {1}", userID, idPoll);

                    // Default question to start the poll
                    PollQuestion firstQuestion = craftFirstQuestion(idPoll);
                    PollQuestion question = client.GetNext(userID, firstQuestion);
                    Console.WriteLine(question.QuestionId + ". " + question.Text);
                    String answer = readAnswer();
                    question.Text = answer;
                    Console.WriteLine("User {0} answer : {1} to question {2}", userID, answer, question.QuestionId);
                    //Display all the poll
                    while ((question = client.GetNext(userID, question)) != null)
                    {
                        //Display the question
                        Console.WriteLine(question.QuestionId + ". " + question.Text);
                        //Ask for an answer
                        answer = readAnswer();
                        question.Text = answer;
                        //TODO : write in folder in fanal version
                        Console.WriteLine("User {0} answer : {1} to question {2}",userID, answer,question.QuestionId);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Closing the client gracefully closes the connection and cleans up resources.
                Console.WriteLine("Thank you, press anny key to close the console...");
                Console.ReadLine();
                client.Close();
            }
   
        }
    }
}
