using Librairie.Entities;
using Librairie.Services.Interfaces;
using Librairie.Services.Managers;
using Moq;
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
        ServiceClient sut = null;
        Mock<IServiceBD> MockServiceBD = null;
        Client result = null;

        [Test]
        [SetUp]
        public void Initializer()
        {
            MockServiceBD = new Mock<IServiceBD>();
            sut = new ServiceClient(MockServiceBD.Object);
            result = sut.CreerClient("Alex");
        }

        [Test]
        public void CreerClient_NomDejaExiste_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<string>()))
                .Returns(new Client() { NomUtilisateur = "Malek", Id = Guid.NewGuid() }) ;

            var ex = Assert.Throws<Exception>(() => sut.CreerClient("Malek"));
            Assert.That(ex.Message, Is.EqualTo("erreur : Nom déja utilisé !"));

        }

        [Test]
        public void CreerClient_NomVide_Failure()
        {
            var ex = Assert.Throws<Exception>(() => sut.CreerClient(""));
            Assert.That(ex.Message, Is.EqualTo("erreur : Nom client nulle !"));
        }

        [Test]
        public void CreerClient_NomNulle_Failure()
        {
            var ex = Assert.Throws<Exception>(() => sut.CreerClient(null));
            Assert.That(ex.Message, Is.EqualTo("erreur : Nom client nulle !"));
        }

        [Test]
        public void CreerClient_NomValide_Success()
        {
            sut.CreerClient("Mirna");

            MockServiceBD.Verify(x => x.AjouterClient(It.IsAny<Client>()), Times.Exactly(2));
            Assert.AreEqual(result.NomUtilisateur, "Alex");
        }

        [Test]
        public void RenommerClient_NomValide_Success()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(result);

            sut.RenommerClient(result.Id,"Mirna");;

            MockServiceBD.Verify(x => x.ModifierClient(It.IsAny<Client>()), Times.Exactly(1));
        }

        [Test]
        public void RenommerClient_NomReutilisee_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(result);

            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(result.Id, "Alex"));

            Assert.That(ex.Message, Is.EqualTo("T'as utilisè le meme nom"));
        }

        [Test]
        public void RenommerClient_NomVide_Failure()
        {            
            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(result.Id, ""));
            Assert.That(ex.Message, Is.EqualTo("Nouveau nom non valide :nulle ou vide"));
        }

        [Test]
        public void RenommerClient_NomDejaUtilisee_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(result);
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<string>()))
                .Returns(result);

            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(result.Id, "Mirna"));
            Assert.That(ex.Message, Is.EqualTo("Nom deja utilisèe"));
        }
        
        [Test]
        public void RenommerClient_IdNonValide_Failure()
        {
            var ex = Assert.Throws<Exception>(() => sut.RenommerClient(Guid.NewGuid(), "Mirna"));
            Assert.That(ex.Message, Is.EqualTo("Pas de client avec cet Id"));
        }
    }
}
