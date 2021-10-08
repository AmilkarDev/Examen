using Librairie.Entities;
using Librairie.Services.Managers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibrairieTest
{
    [TestFixture]
    public class ServiceClientTests
    {
        [Test]
        public void CreerClient_NomDejaExiste_Failure()
        {
            //var MockCollabUtils = new Mock<ICollaborateurUtils>();
            //MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));
            //Question obj = new Question(MockCollabUtils.Object);

            //MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));

            ServiceClient sut = new ServiceClient();
            sut.clients.Add(new Client { Id = new Guid(), NomUtilisateur = "Malek" });

            var ex = Assert.Throws<Exception>(() => sut.CreerClient("Malek"));
            Assert.That(ex.Message, Is.EqualTo("erreur : Nom déja utilisé !"));

        }

        [Test]
        public void CreerClient_NomVide_Failure()
        {
            ServiceClient sut = new ServiceClient();

            var ex = Assert.Throws<Exception>(() => sut.CreerClient(""));
            Assert.That(ex.Message, Is.EqualTo("erreur : Nom client nulle !"));
        }

        [Test]
        public void CreerClient_NomNulle_Failure()
        {
            ServiceClient sut = new ServiceClient();

            var ex = Assert.Throws<Exception>(() => sut.CreerClient(null));
            Assert.That(ex.Message, Is.EqualTo("erreur : Nom client nulle !"));
        }

        [Test]
        public void CreerClient_NomValide_Success()
        {
            ServiceClient sut = new ServiceClient();

            sut.CreerClient("Mirna");
            var result = sut.CreerClient("Alex");

            Assert.AreEqual(sut.clients.Count, 2);
            Assert.AreEqual(result.NomUtilisateur, "Alex");
        }

        [Test]
        public void RenommerClient_NomValide_Success()
        {
            ServiceClient sut = new ServiceClient();
            var result = sut.CreerClient("Alex");

            sut.RenommerClient(result.Id,"Mirna");;
            

            Assert.AreEqual(sut.clients.Count, 1);
            Assert.AreEqual(sut.clients.First().NomUtilisateur, "Mirna");
        }

        [Test]
        public void RenommerClient_NomReutilisee_Failure()
        {
            ServiceClient sut = new ServiceClient();
            var result = sut.CreerClient("Alex");

            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(result.Id, "Alex"));
            Assert.That(ex.Message, Is.EqualTo("T'as utilisè le meme nom"));
        }

        [Test]
        public void RenommerClient_NomVide_Failure()
        {
            ServiceClient sut = new ServiceClient();
            var result = sut.CreerClient("Alex");

            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(result.Id, ""));
            Assert.That(ex.Message, Is.EqualTo("Nouveau nom non valide :nulle ou vide"));
        }

        [Test]
        public void RenommerClient_NomDejaUtilisee_Failure()
        {
            ServiceClient sut = new ServiceClient();
            var result = sut.CreerClient("Alex");
            sut.CreerClient("Mirna");

            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(result.Id, "Mirna"));
            Assert.That(ex.Message, Is.EqualTo("Nom deja utilisèe"));
        }
        
        [Test]
        public void RenommerClient_IdNonValide_Failure()
        {
            ServiceClient sut = new ServiceClient();
            var result = sut.CreerClient("Alex");

            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(Guid.NewGuid(), "Mirna"));
            Assert.That(ex.Message, Is.EqualTo("Pas de client avec cet Id"));
        }
    }
}
