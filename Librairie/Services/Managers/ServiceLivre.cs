using Librairie.Entities;
using Librairie.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librairie.Services.Managers
{
    public class ServiceLivre : IServiceLivre
    {
        IServiceBD serviceBD = null;
        public ServiceLivre ( IServiceBD ServiceBD )
        {
            serviceBD = ServiceBD;
        }
        public decimal AcheterLivre(Guid IdClient, Guid IdLivre, decimal montant)
        {
            var client = serviceBD.ObtenirClient(IdClient);
            var livre = serviceBD.ObtenirLivre(IdLivre);

            bool clientExiste = client != null;
            if(!clientExiste)
            {
                throw new Exception("Pas de client avec cet Id");
            }
            else if (livre == null)
            {
                throw new Exception("Pas de livre avec cet Id");
            }
            else if (livre.Quantite == 0)
            {
                throw new Exception("Les copie du livre sont tous vendus");
            }
            else if(montant <= livre.Valeur)
            {
                throw new Exception("Montant Insuffisant");
            }
            else
            {

            
                if (client.ListeLivreAchete.ContainsKey(livre.Id))
                {
                    client.ListeLivreAchete[livre.Id]++;
                }
                else
                {
                    client.ListeLivreAchete.Add(livre.Id, 1);
                }

                livre.Quantite--;

                serviceBD.ModifierClient(client);
                serviceBD.ModifierLivre(livre);

                return montant - livre.Valeur;
            }            
        }

        public decimal RembourserLivre(Guid IdClient, Guid idLivre)
        {
            var client = serviceBD.ObtenirClient(IdClient);
            var livre = serviceBD.ObtenirLivre(idLivre);


            bool clientExiste = client != null;
            bool achatFait = false ;

            if (clientExiste)
            {
                achatFait = client.ListeLivreAchete.ContainsKey(idLivre) && client.ListeLivreAchete[idLivre] > 0;
            }

            if (!clientExiste)
            {
                throw new Exception("Pas de client avec cet Id");
            }
            else if(livre == null)
            {
                throw new Exception("Pas de livre avec cet Id");
            }
            else if (!achatFait)
            {
                throw new Exception("Ce client n'a pas acheter le livre sujet");
            }
            else 
            {
                client.ListeLivreAchete[idLivre]--;
                livre.Quantite++;

                serviceBD.ModifierClient(client);
                serviceBD.ModifierLivre(livre);

                return livre.Valeur;
            }
        }
    }
}
