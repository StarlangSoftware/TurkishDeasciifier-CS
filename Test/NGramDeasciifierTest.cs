using Corpus;
using Deasciifier;
using MorphologicalAnalysis;
using NGram;
using NUnit.Framework;

namespace Test
{
    public class NGramDeasciifierTest
    {

        [Test]
        public void TestDeasciify2()
        {
            var fsm = new FsmMorphologicalAnalyzer();
            var nGram = new NGram<string>("../../../ngram.txt");
            nGram.CalculateNGramProbabilities(new NoSmoothing<string>());
            var nGramDeasciifier = new NGramDeasciifier(fsm, nGram, false);
            Assert.AreEqual("noter hakkında", nGramDeasciifier.Deasciify(new Sentence("noter hakkinda")).ToString());
            Assert.AreEqual("sandık medrese", nGramDeasciifier.Deasciify(new Sentence("sandik medrese")).ToString());
            Assert.AreEqual("kuran'ı karşılıklı", nGramDeasciifier.Deasciify(new Sentence("kuran'ı karsilikli")).ToString());
        }
        
        [Test]
        public void TestDeasciify3()
        {
            var fsm = new FsmMorphologicalAnalyzer();
            var nGram = new NGram<string>("../../../ngram.txt");
            nGram.CalculateNGramProbabilities(new LaplaceSmoothing<string>());
            var nGramDeasciifier = new NGramDeasciifier(fsm, nGram, true);
            Assert.AreEqual("dün akşam yeni aldığımız çam ağacını süsledik", nGramDeasciifier.Deasciify(new Sentence("dün aksam yenı aldıgımız cam ağacını susledık")).ToString());
            Assert.AreEqual("ünlü sanatçı tartışmalı konu hakkında demeç vermekten kaçındı", nGramDeasciifier.Deasciify(new Sentence("unlu sanatçı tartismali konu hakkinda demec vermekten kacindi")).ToString());
            Assert.AreEqual("köylü de durumdan oldukça şikayetçiydi", nGramDeasciifier.Deasciify(new Sentence("koylu de durumdan oldukca şikayetciydi")).ToString());
        }

    }
}