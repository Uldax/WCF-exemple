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
        static void Main(string[] args)
        {
            SondageServiceClient client = new SondageServiceClient();
            try
            {
                String mdp = "paul";
                int userID =  client.Connect("paul",mdp);
                // Si l'authenfication a échouée
                if (userID == -1) {
                    throw new Exception("Erreur d'authentification.");
                } else {
                    Console.WriteLine("Connected");
                    Poll[] polls = client.GetAvailablePolls(userID);
                    // Affichage des sondages disponibles
                    Console.WriteLine("Sondages disponibles\n");
                    foreach (Poll poll in polls)
                    {
                        Console.WriteLine(poll.Id + ". " + poll.Description);
                    }
                }

            } catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
            // Allway close the client.
            client.Close();


        }
    }
}
