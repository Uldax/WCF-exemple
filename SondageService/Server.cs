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
            // Service invocation
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
                        
                try {
                    //Open the service
                    poolServiceHost.Open();
                    Console.WriteLine("Service is live now at : {0}", httpBaseAddress);
                    Console.WriteLine("(Appuyez sur <Entr�e> pour arr�ter le service)");
                    Console.WriteLine(String.Empty);
                    Console.ReadLine();
                    //End of service
                    poolServiceHost.Close();
                } catch (TimeoutException ex) {
                    Console.WriteLine("TimeOut :" + ex.Message);
                } catch (ObjectDisposedException ex) {
                    Console.WriteLine("Object closed :" + ex.Message);
                } catch (InvalidOperationException ex) {
                    Console.WriteLine("Not opened :" + ex.Message);
                } catch (CommunicationObjectFaultedException ex) {
                    Console.WriteLine("Fault State" + ex.Message);
                } catch (Exception ex) {
                    Console.WriteLine("There is an issue with ServiceHost" + ex.Message);
                }   
            }                
                                 
        }
    }
 
}