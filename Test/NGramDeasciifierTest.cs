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
        public void TestDeasciify()
        {
            var fsm = new FsmMorphologicalAnalyzer();
            var nGram = new NGram<string>("../../../ngram.txt");
            nGram.CalculateNGramProbabilities(new NoSmoothing<string>());
            var nGramDeasciifier = new NGramDeasciifier(fsm, nGram, true);
            var simpleAsciifier = new SimpleAsciifier();
            var corpus = new Corpus.Corpus("../../../corpus.txt");
            for (var i = 0; i < corpus.SentenceCount(); i++)
            {
                var sentence = corpus.GetSentence(i);
                for (var j = 1; j < sentence.WordCount(); j++)
                {
                    if (fsm.MorphologicalAnalysis(sentence.GetWord(j).GetName()).Size() > 0)
                    {
                        var asciified = simpleAsciifier.Asciify(sentence.GetWord(j));
                        if (!asciified.Equals(sentence.GetWord(j).GetName()))
                        {
                            var deasciified = nGramDeasciifier.Deasciify(
                                new Sentence(sentence.GetWord(j - 1).GetName() + " " + sentence.GetWord(j).GetName()));
                            Assert.AreEqual(sentence.GetWord(j).GetName(), deasciified.GetWord(1).GetName());
                        }
                    }
                }
            }
        }

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
    }
}