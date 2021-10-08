using System;
using System.Collections.Generic;

namespace Questions
{
    //Améliorer le code de cette classe ainsi que sa relation avec la classe Collaborateur.

   
    public class Question
    {
        ICollaborateurUtils collaborateurUtils = null;
        public Question(ICollaborateurUtils CollaborateurUtils)
        {
            collaborateurUtils = CollaborateurUtils;
        }
        public List<string> listeContenuValide = new List<string>();
        public void Traiter(List<string> listeContenu)
        {
            
            string message = null;
            bool estValide = true;

            foreach (var contenu in listeContenu)
            {
                if (estValide)
                {
                    estValide = Valider(contenu, out message);
                }              

                if (estValide && !listeContenuValide.Contains(contenu))
                {
                    //listeContenuValide.Add(contenu.Substring(0,10));
                    listeContenuValide.Add(contenu);
                }
            }

            if (!estValide)
            {
                throw new Exception(message);
            }

            if(listeContenuValide.Count > 0)
            {
                listeContenuValide.ForEach(x => collaborateurUtils.AjouterContenuBD(x));
            }

        }

        private bool Valider(string contenu, out string message)
        {
            message = "";
            if ( string.IsNullOrEmpty(contenu))
            {
                message = "Le contenu ne peut être vide";

                return false;
            }

            if ( contenu.Length > 10)
            {
                message = "Le contenu est trop long";

                return false;
            }

            return true;

        }
    }
}
