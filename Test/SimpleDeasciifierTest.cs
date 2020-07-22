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
        public void TestDeasciify() {
            var fsm = new FsmMorphologicalAnalyzer();
            var simpleDeasciifier = new SimpleDeasciifier(fsm);
            var simpleAsciifier = new SimpleAsciifier();
            for (var i = 0; i < fsm.GetDictionary().Size(); i++){
                var word = (TxtWord) fsm.GetDictionary().GetWord(i);
                var count = 0;
                for (var j = 0; j < word.GetName().Length; j++){
                    switch (word.GetName()[j]){
                        case 'ç':
                        case 'ö':
                        case 'ğ':
                        case 'ü':
                        case 'ş':
                        case 'ı':
                            count++;
                            break;
                    }
                }
                if (count > 0 && !word.GetName().EndsWith("fulü") && (word.IsNominal() || word.IsAdjective() || word.IsAdverb() || word.IsVerb())){
                    var asciified = simpleAsciifier.Asciify(word);
                    if (simpleDeasciifier.CandidateList(new Word(asciified)).Count == 1){
                        var deasciified = simpleDeasciifier.Deasciify(new Sentence(asciified)).ToString();
                        Assert.AreEqual(word.GetName(), deasciified);
                    }
                }
            }
        }
        
    }
}