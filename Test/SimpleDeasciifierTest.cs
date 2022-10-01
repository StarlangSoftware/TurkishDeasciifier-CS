using Corpus;
using Deasciifier;
using Dictionary.Dictionary;
using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class SimpleDeasciifierTest
    {
        [Test]
        public void TestDeasciify()
        {
            var fsm = new FsmMorphologicalAnalyzer();
            var simpleDeasciifier = new SimpleDeasciifier(fsm);
            Assert.AreEqual("hakkında", simpleDeasciifier.Deasciify(new Sentence("hakkinda")).ToString());
            Assert.AreEqual("küçük", simpleDeasciifier.Deasciify(new Sentence("kucuk")).ToString());
            Assert.AreEqual("karşılıklı", simpleDeasciifier.Deasciify(new Sentence("karsilikli")).ToString());
        }
    }
}