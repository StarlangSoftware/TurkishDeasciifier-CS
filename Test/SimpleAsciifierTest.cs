using Corpus;
using Deasciifier;
using Dictionary.Dictionary;
using NUnit.Framework;

namespace Test
{
    public class SimpleAsciifierTest
    {
        SimpleAsciifier simpleAsciifier;

        [SetUp]
        public void Setup()
        {
            simpleAsciifier = new SimpleAsciifier();
        }

        [Test]
        public void TestWordAsciify()
        {
            Assert.AreEqual("cogusiCOGUSI", simpleAsciifier.Asciify(new Word("çöğüşıÇÖĞÜŞİ")));
            Assert.AreEqual("sogus", simpleAsciifier.Asciify(new Word("söğüş")));
            Assert.AreEqual("uckagitcilik", simpleAsciifier.Asciify(new Word("üçkağıtçılık")));
            Assert.AreEqual("akiskanlistiricilik", simpleAsciifier.Asciify(new Word("akışkanlıştırıcılık")));
            Assert.AreEqual("citcitcilik", simpleAsciifier.Asciify(new Word("çıtçıtçılık")));
            Assert.AreEqual("duskirikligi", simpleAsciifier.Asciify(new Word("düşkırıklığı")));
            Assert.AreEqual("yuzgorumlugu", simpleAsciifier.Asciify(new Word("yüzgörümlüğü")));
        }

        [Test]
        public void TestSentenceAsciify()
        {
            Assert.AreEqual(new Sentence("cogus iii COGUSI").ToString(), simpleAsciifier.Asciify(new Sentence("çöğüş ııı ÇÖĞÜŞİ")).ToString());
            Assert.AreEqual(new Sentence("uckagitcilik akiskanlistiricilik").ToString(), simpleAsciifier.Asciify(new Sentence("üçkağıtçılık akışkanlıştırıcılık")).ToString());
            Assert.AreEqual(new Sentence("citcitcilik duskirikligi yuzgorumlugu").ToString(), simpleAsciifier.Asciify(new Sentence("çıtçıtçılık düşkırıklığı yüzgörümlüğü")).ToString());
        }

    }
}