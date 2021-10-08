using Librairie.Entities;
using Librairie.Services.Interfaces;
using Librairie.Services.Managers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrairieTest
{
    [TestFixture]
    public class ServiceLivreTests
    {
        Mock<IServiceBD> MockServiceBD = null;
        ServiceLivre sut = null;
        Client client = null;
        Livre livre = null;

        [Test]
        [SetUp]
        public void Initializer()
        {
             MockServiceBD = new Mock<IServiceBD>();
             sut = new ServiceLivre(MockServiceBD.Object);
             client = new Client() { NomUtilisateur = "Malek", Id = Guid.NewGuid() };
             livre = new Livre() { Valeur = 50.5M, Quantite = 100, Id = Guid.NewGuid() };
        }

        [Test]
        public void AcheterLivre_ValidData_Success()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);
            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                .Returns(livre);

            var resteMontant = sut.AcheterLivre(client.Id, livre.Id,100);

            Assert.AreEqual(resteMontant, 100-livre.Valeur);
            Assert.AreEqual(client.ListeLivreAchete[livre.Id], 1);
              
            MockServiceBD.Verify(x => x.ModifierClient(It.IsAny<Client>()), Times.Exactly(1));
            MockServiceBD.Verify(x => x.ModifierLivre(It.IsAny<Livre>()), Times.Exactly(1));
        }

        [Test]
        public void AcheterLivre_MontantInsuffisant_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);
            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                .Returns(livre);

            var ex = Assert.Throws<Exception>(() => sut.AcheterLivre(client.Id, livre.Id, 20));
            Assert.That(ex.Message, Is.EqualTo("Montant Insuffisant"));

        }

        [Test]
        public void AcheterLivre_ClientNulle_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);
            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                .Returns(livre);

            var ex = Assert.Throws<Exception>(() => sut.AcheterLivre(client.Id, livre.Id, 20));
            Assert.That(ex.Message, Is.EqualTo("Montant Insuffisant"));
        }

        [Test]
        public void AcheterLivre_IdLivreNonValide_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);

            var ex = Assert.Throws<Exception>(() => sut.AcheterLivre(client.Id, Guid.NewGuid(), 20));
            Assert.That(ex.Message, Is.EqualTo("Pas de livre avec cet Id"));

        }

        [Test]
        public void AcheterLivre_IdClientNonValide_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                .Returns(livre);

            var ex = Assert.Throws<Exception>(() => sut.AcheterLivre(Guid.NewGuid(), livre.Id, 20));
            Assert.That(ex.Message, Is.EqualTo("Pas de client avec cet Id"));

        }

        [Test]
        public void AcheterLivre_QuantiteNonSuffisante_Failure()
        {
            livre.Quantite = 0;

            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);
            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                .Returns(livre);

            var ex = Assert.Throws<Exception>(() => sut.AcheterLivre(client.Id, livre.Id, 20));
            Assert.That(ex.Message, Is.EqualTo("Les copie du livre sont tous vendus"));

        }


        [Test]
        public void RembourserLivre_ValidData_Success()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);
            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                .Returns(livre);

            sut.AcheterLivre(client.Id, livre.Id, 100);
            Assert.AreEqual(client.ListeLivreAchete[livre.Id], 1);

            decimal montantRemboursee = sut.RembourserLivre(client.Id, livre.Id);

            Assert.AreEqual(montantRemboursee, livre.Valeur);
            Assert.AreEqual(client.ListeLivreAchete[livre.Id], 0);
            MockServiceBD.Verify(x => x.ModifierClient(It.IsAny<Client>()), Times.Exactly(2));
            MockServiceBD.Verify(x => x.ModifierLivre(It.IsAny<Livre>()), Times.Exactly(2));

        }

        [Test]
        public void RembourserLivre_ClientNonValide_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                .Returns(livre);

            var ex = Assert.Throws<Exception>(() => sut.RembourserLivre(Guid.NewGuid(), livre.Id));
            Assert.That(ex.Message, Is.EqualTo("Pas de client avec cet Id"));
        }

        [Test]
        public void RembourserLivre_LivreNonValide_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);

            var ex = Assert.Throws<Exception>(() => sut.RembourserLivre(client.Id, Guid.NewGuid()));
            Assert.That(ex.Message, Is.EqualTo("Pas de livre avec cet Id"));
        }

        [Test]
        public void RembourserLivre_LivreNonAchetee_Failure()
        {
            MockServiceBD.Setup(t => t.ObtenirClient(It.IsAny<Guid>()))
                .Returns(client);

            MockServiceBD.Setup(t => t.ObtenirLivre(It.IsAny<Guid>()))
                            .Returns(livre);

            var ex = Assert.Throws<Exception>(() => sut.RembourserLivre(client.Id, livre.Id));
            Assert.That(ex.Message, Is.EqualTo("Ce client n'a pas acheter le livre sujet"));
        }

    }
}
