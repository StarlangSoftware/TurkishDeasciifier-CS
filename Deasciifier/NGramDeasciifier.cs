using Corpus;
using Dictionary.Dictionary;
using MorphologicalAnalysis;
using NGram;

namespace Deasciifier
{
    public class NGramDeasciifier : SimpleDeasciifier
    {
        private NGram<string> nGram;

        /**
         * <summary>A constructor of {@link NGramDeasciifier} class which takes an {@link FsmMorphologicalAnalyzer} and an {@link NGram}
         * as inputs. It first calls it super class {@link SimpleDeasciifier} with given {@link FsmMorphologicalAnalyzer} input
         * then initializes nGram variable with given {@link NGram} input.</summary>
         *
         * <param name="fsm">  {@link FsmMorphologicalAnalyzer} type input.</param>
         * <param name="nGram">{@link NGram} type input.</param>
         */
        public NGramDeasciifier(FsmMorphologicalAnalyzer fsm, NGram<string> nGram) : base(fsm)
        {
            this.nGram = nGram;
        }

        /**
         * <summary>The deasciify method takes a {@link Sentence} as an input. First it creates a string {@link List} as candidates,
         * and a {@link Sentence} result. Then, loops i times where i ranges from 0 to words size of given sentence. It gets the
         * current word and generates a candidateList with this current word then, it loops through the candidateList. First it
         * calls morphologicalAnalysis method with current candidate and gets the first item as root word. If it is the first root,
         * it gets its N-gram probability, if there are also other roots, it gets probability of these roots and finds out the
         * best candidate, best root and the best probability. At the nd, it adds the bestCandidate to the bestCandidate {@link List}.</summary>
         *
         * <param name="sentence">{@link Sentence} type input.</param>
         * <returns>Sentence result as output.</returns>
         */
        public new Sentence Deasciify(Sentence sentence)
        {
            Word previousRoot = null;
            var result = new Sentence();
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = sentence.GetWord(i);
                var fsmParses = fsm.MorphologicalAnalysis(word.GetName());
                if (fsmParses.Size() == 0)
                {
                    var candidates = CandidateList(word);
                    var bestCandidate = word.GetName();
                    var bestRoot = word;
                    double bestProbability = 0;
                    foreach (var candidate in candidates) {
                        var fsmParseList = fsm.MorphologicalAnalysis(candidate);
                        var root = fsmParseList.GetFsmParse(0).GetWord();
                        double probability;
                        if (previousRoot != null)
                        {
                            probability = nGram.GetProbability(previousRoot.GetName(), root.GetName());
                        }
                        else
                        {
                            probability = nGram.GetProbability(root.GetName());
                        }

                        if (probability > bestProbability)
                        {
                            bestCandidate = candidate;
                            bestRoot = root;
                            bestProbability = probability;
                        }
                    }
                    previousRoot = bestRoot;
                    result.AddWord(new Word(bestCandidate));
                }
                else
                {
                    result.AddWord(word);
                    previousRoot = fsmParses.GetParseWithLongestRootWord().GetWord();
                }
            }

            return result;
        }
    }
}