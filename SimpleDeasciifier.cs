using System;
using System.Collections.Generic;
using Corpus;
using Dictionary.Dictionary;
using MorphologicalAnalysis;

namespace Deasciifier
{
    public class SimpleDeasciifier : Deasciifier
    {
        protected FsmMorphologicalAnalyzer fsm;

        /**
         * <summary>The generateCandidateList method takes an {@link List} candidates, a {@link string}, and an integer index as inputs.
         * First, it creates a {@link string} which consists of corresponding Latin versions of special Turkish characters. If given index
         * is less than the length of given word and if the item of word's at given index is one of the chars of {@link string}, it loops
         * given candidates {@link List}'s size times and substitutes Latin characters with their corresponding Turkish versions
         * and put them to newly created char {@link java.lang.reflect.Array} modified. At the end, it adds each modified item to the candidates
         * {@link List} as a {@link string} and recursively calls generateCandidateList with next index.</summary>
         *
         * <param name="candidates">{@link List} type input.</param>
         * <param name="word">      {@link string} input.</param>
         * <param name="index">     {@link Integer} input.</param>
         */
        private void GenerateCandidateList(List<string> candidates, string word, int index)
        {
            const string s = "ıiougcsİIOUGCS";
            if (index < word.Length)
            {
                if (s.IndexOf(word[index]) != -1)
                {
                    int size = candidates.Count;
                    for (var i = 0; i < size; i++)
                    {
                        var modified = candidates[i].ToCharArray();
                        switch (word[index])
                        {
                            case 'ı':
                                modified[index] = 'i';
                                break;
                            case 'i':
                                modified[index] = '\u0131';
                                break;
                            case 'o':
                                modified[index] = '\u00f6';
                                break;
                            case 'u':
                                modified[index] = '\u00fc';
                                break;
                            case 'g':
                                modified[index] = '\u011f';
                                break;
                            case 'c':
                                modified[index] = '\u00e7';
                                break;
                            case 's':
                                modified[index] = '\u015f';
                                break;
                            case 'I':
                                modified[index] = '\u0130';
                                break;
                            case 'İ':
                                modified[index] = 'I';
                                break;
                            case 'O':
                                modified[index] = '\u00d6';
                                break;
                            case 'U':
                                modified[index] = '\u00dc';
                                break;
                            case 'G':
                                modified[index] = '\u011e';
                                break;
                            case 'C':
                                modified[index] = '\u00c7';
                                break;
                            case 'S':
                                modified[index] = '\u015e';
                                break;
                        }

                        candidates.Add(new string(modified));
                    }
                }

                GenerateCandidateList(candidates, word, index + 1);
            }
        }

        /**
         * <summary>The candidateList method takes a {@link Word} as an input and creates new candidates {@link List}. First it
         * adds given word to this {@link List} and calls generateCandidateList method with candidates, given word and
         * index 0. Then, loops i times where i ranges from 0 to size of candidates {@link List} and calls morphologicalAnalysis
         * method with ith item of candidates {@link List}. If it does not return any analysis for given item, it removes
         * the item from candidates {@link List}.</summary>
         *
         * <param name="word">{@link Word} type input.</param>
         * <returns>List candidates.</returns>
         */
        protected List<string> CandidateList(Word word)
        {
            var candidates = new List<string> {word.GetName()};
            GenerateCandidateList(candidates, word.GetName(), 0);
            for (var i = 0; i < candidates.Count; i++)
            {
                var fsmParseList = fsm.MorphologicalAnalysis(candidates[i]);
                if (fsmParseList.Size() == 0)
                {
                    candidates.RemoveAt(i);
                    i--;
                }
            }

            return candidates;
        }

        /**
         * <summary>A constructor of {@link SimpleDeasciifier} class which takes a {@link FsmMorphologicalAnalyzer} as an input and
         * initializes fsm variable with given {@link FsmMorphologicalAnalyzer} input.</summary>
         *
         * <param name="fsm">{@link FsmMorphologicalAnalyzer} type input.</param>
         */
        public SimpleDeasciifier(FsmMorphologicalAnalyzer fsm)
        {
            this.fsm = fsm;
        }

        /**
         * <summary>The deasciify method takes a {@link Sentence} as an input and loops i times where i ranges from 0 to number of
         * words in the given {@link Sentence}. First it gets ith word from given {@link Sentence} and calls candidateList with
         * ith word and assigns the returned {@link List} to the newly created candidates {@link List}. And if the size of
         * candidates {@link List} is greater than 0, it generates a random number and gets the item of candidates {@link List}
         * at the index of random number and assigns it as a newWord. If the size of candidates {@link List} is 0, it then
         * directly assigns ith word as the newWord. At the end, it adds newWord to the result {@link Sentence}.</summary>
         *
         * <param name="sentence">{@link Sentence} type input.</param>
         * <returns>result {@link Sentence}.</returns>
         */
        public Sentence Deasciify(Sentence sentence)
        {
            var random = new Random();
            var result = new Sentence();
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = sentence.GetWord(i);
                var fsmParseList = fsm.MorphologicalAnalysis(word.GetName());
                Word newWord;
                if (fsmParseList.Size() == 0)
                {
                    var candidates = CandidateList(word);
                    if (candidates.Count > 0)
                    {
                        int randomCandidate = random.Next(candidates.Count);
                        newWord = new Word(candidates[randomCandidate]);
                    }
                    else
                    {
                        newWord = word;
                    }
                }
                else
                {
                    newWord = word;
                }

                result.AddWord(newWord);
            }

            return result;
        }
    }
}