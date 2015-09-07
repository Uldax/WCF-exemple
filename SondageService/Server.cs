using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace USherbrooke.ServiceModel.Sondage
{
    class Server
    {  
        static void Main(string[] args)
        {
            // Specify a base address for the service
            Uri httpBaseAddress = new Uri("https://localhost:1234/SondageWS");
            ServiceHost poolServiceHost = null;
            // Création du service
            using (poolServiceHost = new ServiceHost(typeof(SondageService), httpBaseAddress))
            {
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
                //Add Endpoint to Host
                poolServiceHost.AddServiceEndpoint(typeof(ISondageService), binding, httpBaseAddress);
                    
                //Metadata Exchange
                ServiceMetadataBehavior serviceBehavior = new ServiceMetadataBehavior();
                serviceBehavior.HttpsGetEnabled = true;
                serviceBehavior.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                poolServiceHost.Description.Behaviors.Add(serviceBehavior);
                    
                //Open
                try
                {
                    poolServiceHost.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There is an issue with StudentService" + ex.Message);
                }
                Console.WriteLine("Service is live now at : {0}", httpBaseAddress);
                Console.WriteLine("(Appuyez sur <Entrée> pour arrêter le service)");
                Console.WriteLine(String.Empty);
                Console.ReadLine();
                  // Arrêt du service
                try
                {
                    poolServiceHost.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }                
                                 
        }
    }
 
}