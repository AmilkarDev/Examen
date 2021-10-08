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
        public IServiceBD serviceBD;

        public ServiceClient(IServiceBD ServiceBd)
        {
            serviceBD = ServiceBd;
        }
        public Client CreerClient(string nomClient)
        {
            Client newClient = null;

            if (string.IsNullOrEmpty(nomClient))
            {
                throw new Exception("erreur : Nom client nulle !");
            }

            //var registeredClients = clients.Where(x => x.NomUtilisateur == nomClient).ToList();
            var registeredClient = serviceBD.ObtenirClient(nomClient);
            if (registeredClient != null)
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

                 serviceBD.AjouterClient(newClient);
            }
                
            return newClient;      
        }

        public void RenommerClient(Guid clientId, string nouveauNomClient)
         {
            if (string.IsNullOrEmpty(nouveauNomClient))
            {
                throw new Exception("Nouveau nom non valide :nulle ou vide");
            }
            var client = serviceBD.ObtenirClient(clientId);
            if(client == null)
            {
                throw new Exception("Pas de client avec cet Id");
            }

            if(client.NomUtilisateur == nouveauNomClient)
            {
                throw new Exception("T'as utilisè le meme nom");
            }

            //var nomUtilisee = clients.Where(x => x.NomUtilisateur == nouveauNomClient).ToList().Count > 0;
            var nomUtilisee = serviceBD.ObtenirClient(nouveauNomClient) != null;
            if (nomUtilisee)
            {
                throw new Exception("Nom deja utilisèe");
            }

            client.NomUtilisateur = nouveauNomClient;
            serviceBD.ModifierClient(client);
        }
    }
}
