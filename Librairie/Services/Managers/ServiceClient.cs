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
        public List<Client> clients = null;

        public ServiceClient()
        {
            clients = new List<Client>();
        }
        public Client CreerClient(string nomClient)
        {
            Client newClient = null;

            if (string.IsNullOrEmpty(nomClient))
            {
                throw new Exception("erreur : Nom client nulle !");
            }

            var registeredClients = clients.Where(x => x.NomUtilisateur == nomClient).ToList();

            if(registeredClients.Count > 0)
            {
                throw new Exception("erreur : Nom déja utilisé !");
            }
            else
            {
                newClient = new Client
                {
                    Id =  Guid.NewGuid(),
                    NomUtilisateur = nomClient,
                };

                 clients.Add(newClient);
            }
                
            return newClient;      
        }

        public void RenommerClient(Guid clientId, string nouveauNomClient)
         {
            if (string.IsNullOrEmpty(nouveauNomClient))
            {
                throw new Exception("Nouveau nom non valide :nulle ou vide");
            }
            var client = ChercherClient(clientId);
            if(client == null)
            {
                throw new Exception("Pas de client avec cet Id");
            }

            if(client.NomUtilisateur == nouveauNomClient)
            {
                throw new Exception("T'as utilisè le meme nom");
            }

            var nomUtilisee = clients.Where(x => x.NomUtilisateur == nouveauNomClient).ToList().Count > 0;
            if (nomUtilisee)
            {
                throw new Exception("Nom deja utilisèe");
            }

            client.NomUtilisateur = nouveauNomClient;
        }

        public Client ChercherClient(Guid clientId)
        { 
            return clients.FirstOrDefault(x => x.Id == clientId);
        }
    }
}
