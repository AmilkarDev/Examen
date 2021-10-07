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
        IServiceClient serviceClient = null;
        List<Livre> livres = null;
        public ServiceLivre (IServiceClient ServiceClient)
        {
            serviceClient = ServiceClient;

            livres = new List<Livre>
            {
                new Livre { Id = new Guid(), Quantite = 150, Valeur=200.5M },
                new Livre { Id = new Guid(), Quantite= 69 , Valeur =50.6M },
                new Livre { Id = new Guid(), Quantite= 65 , Valeur =140.6M },
                new Livre { Id = new Guid(), Quantite= 10 , Valeur =80.6M },
                new Livre { Id = new Guid(), Quantite= 8 , Valeur =42.6M },
                new Livre { Id = new Guid(), Quantite= 5 , Valeur =50.6M },
                new Livre { Id = new Guid(), Quantite= 43 , Valeur =99.6M },
                new Livre { Id = new Guid(), Quantite= 20 , Valeur =25.6M },
                new Livre { Id = new Guid(), Quantite= 12 , Valeur =10.6M },
                new Livre { Id = new Guid(), Quantite= 4 , Valeur =120.6M },

            };
        }
        public decimal AcheterLivre(Guid IdClient, Guid IdLivre, decimal montant)
        {
            var client = ((ServiceClient)serviceClient).ChercherClient(IdClient);
            var livre = livres.FirstOrDefault(x => x.Id == IdLivre);

            bool clientExiste = client != null;
            bool livreExiste = livre != null && livre.Quantite > 0  ;
            bool montantSuffisant = montant >= livre.Valeur; 


            if( clientExiste && livreExiste && montantSuffisant)
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

                return montant - livre.Valeur;
            }

            return -1;
        }
           
        public decimal RembourserLivre(Guid IdClient, Guid idLivre)
        {
            var client = ((ServiceClient)serviceClient).ChercherClient(IdClient);
            var livre = livres.FirstOrDefault(x => x.Id == idLivre);


            bool clientExiste = client != null;
            bool livreExiste = livre != null && livre.Quantite > 0;

            bool achatFait = client.ListeLivreAchete.ContainsKey(idLivre) && client.ListeLivreAchete[idLivre] > 0 ;

            if(clientExiste && livreExiste && achatFait)
            {
                client.ListeLivreAchete[idLivre]--;
                livre.Quantite++;

                return livre.Valeur;
            }

            return -1;
        }
    }
}
