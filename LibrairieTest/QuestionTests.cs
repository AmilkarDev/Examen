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
        Mock<ICollaborateurUtils> MockCollabUtils = null;
        Question question = null;

        [Test]
        [SetUp]
        public void Initializer()
        {
            MockCollabUtils = new Mock<ICollaborateurUtils>();
            question = new Question(MockCollabUtils.Object);

            MockCollabUtils.Setup(t => t.AjouterContenuBD(It.IsAny<string>()));
            
        }

        [Test]
        public void Traiter_ContenuEnregistree_Succeed()
        {          
            List<string> listeContenu = new List<string> { "Hello", "extra", "Mima" };

            question.Traiter(listeContenu);

            MockCollabUtils.Verify(x => x.AjouterContenuBD(It.IsAny<string>()), Times.Exactly(3));
        }

        [Test]
        public void Traiter_nullString_Fail()
        {
            List<string> listeContenu = new List<string> { "Hello", null, "Mima" };

            var ex = Assert.Throws<Exception>(() => question.Traiter(listeContenu));

            Assert.That(ex.Message, Is.EqualTo("Le contenu ne peut être vide"));

        }

        [Test]
        public void Traiter_LongString_Fail()
        {
            List<string> listeContenu = new List<string> { "Hello World ", null, "Mima" };

            var ex = Assert.Throws<Exception>(() => question.Traiter(listeContenu));

            Assert.That(ex.Message, Is.EqualTo("Le contenu est trop long"));

        }
    }
}
