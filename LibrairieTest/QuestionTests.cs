using Moq;
using NUnit.Framework;
using Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrairieTest
{
    [TestFixture]
    public class QuestionTests
    {
        [Test]
        public void Traiter_ContenuEnregistree_Succeed()
        {
            var MockCollabUtils = new Mock<ICollaborateurUtils>();
            MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));
            Question obj = new Question(MockCollabUtils.Object);

            MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));
            
            List<string> listeContenu = new List<string> { "Hello", "extra", "Mima" };

            obj.Traiter(listeContenu);

            MockCollabUtils.Verify(x => x.AjouterContenuBD(It.IsAny<string>()), Times.Exactly(3));
            Assert.AreEqual(obj.listeContenuValide.Count, 3);
        }

        [Test]
        public void Traiter_ValidList_Succeed()
        {
            var MockCollabUtils = new Mock<ICollaborateurUtils>();
            MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));
            Question obj = new Question(MockCollabUtils.Object);
            List<string> listeContenu = new List<string> { "Hello", "extra", "Mima" };
            obj.Traiter(listeContenu);
            Assert.AreEqual(obj.listeContenuValide.Count, 3);
        }

        [Test]
        public void Traiter_nullString_Fail()
        {
            var MockCollabUtils = new Mock<ICollaborateurUtils>();
            MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));
            Question obj = new Question(MockCollabUtils.Object);

            List<string> listeContenu = new List<string> { "Hello", null, "Mima" };
            var ex = Assert.Throws<Exception>(() => obj.Traiter(listeContenu));
            Assert.That(ex.Message, Is.EqualTo("Le contenu ne peut être vide"));

        }

        [Test]
        public void Traiter_LongString_Fail()
        {
            var MockCollabUtils = new Mock<ICollaborateurUtils>();
            MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));
            Question obj = new Question(MockCollabUtils.Object);

            List<string> listeContenu = new List<string> { "Hello World ", null, "Mima" };
            var ex = Assert.Throws<Exception>(() => obj.Traiter(listeContenu));
            Assert.That(ex.Message, Is.EqualTo("Le contenu est trop long"));

        }
    }
}
