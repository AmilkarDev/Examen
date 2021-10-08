using System;
using System.Collections.Generic;
using System.Text;

namespace Questions
{
    public class CollaborateurUtil : ICollaborateurUtils
    {
        public void AjouterContenuBD(string str)
        {
            Collaborateur.AjouterContenuBD(str);
        }
    }
}
