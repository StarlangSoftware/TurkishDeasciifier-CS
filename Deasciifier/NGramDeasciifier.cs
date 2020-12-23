using Corpus;
using Dictionary.Dictionary;
using MorphologicalAnalysis;
using NGram;

namespace Deasciifier
{
    public class NGramDeasciifier : SimpleDeasciifier
    {
        private NGram<string> nGram;
        private bool rootNGram;
        private double threshold = 0.0;

        /**
         * <summary>A constructor of {@link NGramDeasciifier} class which takes an {@link FsmMorphologicalAnalyzer} and an {@link NGram}
         * as inputs. It first calls it super class {@link SimpleDeasciifier} with given {@link FsmMorphologicalAnalyzer} input
         * then initializes nGram variable with given {@link NGram} input.</summary>
         *
         * <param name="fsm">  {@link FsmMorphologicalAnalyzer} type input.</param>
         * <param name="nGram">{@link NGram} type input.</param>
         * <param name="rootNGram">True if the NGram is root nGram</param>
         */
        public NGramDeasciifier(FsmMorphologicalAnalyzer fsm, NGram<string> nGram, bool rootNGram) : base(fsm)
        {
            this.nGram = nGram;
            this.rootNGram = rootNGram;
        }

        /**
        * <summary>Checks the morphological analysis of the given word in the given index. If there is no misspelling, it returns
        * the longest root word of the possible analyses.</summary>
        * <param name="sentence"> Sentence to be analyzed.</param>
        * <param name="index"> Index of the word</param>
        * <returns> If the word is misspelled, null; otherwise the longest root word of the possible analyses.</returns>
        */
        private Word CheckAnalysisAndSetRoot(Sentence sentence, int index)
        {
            if (index < sentence.WordCount())
            {
                var fsmParses = fsm.MorphologicalAnalysis(sentence.GetWord(index).GetName());
                if (fsmParses.Size() != 0)
                {
                    if (rootNGram)
                    {
                        return fsmParses.GetParseWithLongestRootWord().GetWord();
                    }

                    return sentence.GetWord(index);
                }
            }

            return null;
        }

        public void SetThreshold(double threshold)
        {
            this.threshold = threshold;
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
            Word previousRoot = null, root, nextRoot;
            FsmParseList fsmParses;
            double previousProbability, nextProbability, bestProbability;
            var result = new Sentence();
            root = CheckAnalysisAndSetRoot(sentence, 0);
            nextRoot = CheckAnalysisAndSetRoot(sentence, 1);
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = sentence.GetWord(i);
                if (root == null)
                {
                    var candidates = CandidateList(word);
                    var bestCandidate = word.GetName();
                    var bestRoot = word;
                    bestProbability = threshold;
                    foreach (var candidate in candidates)
                    {
                        fsmParses = fsm.MorphologicalAnalysis(candidate);
                        if (rootNGram)
                        {
                            root = fsmParses.GetParseWithLongestRootWord().GetWord();
                        }
                        else
                        {
                            root = new Word(candidate);
                        }

                        if (previousRoot != null)
                        {
                            previousProbability = nGram.GetProbability(previousRoot.GetName(), root.GetName());
                        }
                        else
                        {
                            previousProbability = 0.0;
                        }

                        if (nextRoot != null)
                        {
                            nextProbability = nGram.GetProbability(root.GetName(), nextRoot.GetName());
                        }
                        else
                        {
                            nextProbability = 0.0;
                        }

                        if (System.Math.Max(previousProbability, nextProbability) > bestProbability)
                        {
                            bestCandidate = candidate;
                            bestRoot = root;
                            bestProbability = System.Math.Max(previousProbability, nextProbability);
                        }
                    }

                    root = bestRoot;
                    result.AddWord(new Word(bestCandidate));
                }
                else
                {
                    result.AddWord(word);
                }

                previousRoot = root;
                root = nextRoot;
                nextRoot = CheckAnalysisAndSetRoot(sentence, i + 2);
            }

            return result;
        }
    }
}