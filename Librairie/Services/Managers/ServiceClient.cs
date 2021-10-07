using Librairie.Entities;
using Librairie.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librairie.Services.Managers
{
    public class ServiceClient : IServiceClient
    {
        public List<Client> clients = new List<Client>();
        public Client CreerClient(string nomClient)
        {
            Client newClient = null;
            var registeredClients = clients.Where(x => x.NomUtilisateur == nomClient).ToList();

            if (registeredClients.Count == 0)
            {
                newClient = new Client
                {
                    Id = new Guid(),
                    NomUtilisateur = nomClient,
                };

                clients.Add(newClient);
            }
                
            return newClient;      
        }

        public void RenommerClient(Guid clientId, string nouveauNomClient)
        {
            var client = ChercherClient(clientId);
            var clientExists = clients.Where(x => x.NomUtilisateur == nouveauNomClient).ToList().Count == 0 ;

            if (client !=null && client.NomUtilisateur!= nouveauNomClient && clientExists )
            {
                client.NomUtilisateur = nouveauNomClient;
            }
        }

        public Client ChercherClient(Guid clientId)
        { 
            return clients.FirstOrDefault(x => x.Id == clientId);
        }
    }
}
