using Corpus;
using Dictionary.Dictionary;

namespace Deasciifier
{
    public class SimpleAsciifier : Asciifier
    {
        /**
         * <summary>The asciify method takes a {@link Word} as an input and converts it to a char {@link java.lang.reflect.Array}. Then,
         * loops i times where i ranges from 0 to length of the char {@link java.lang.reflect.Array} and substitutes Turkish
         * characters with their corresponding Latin versions and returns it as a new {@link string}.</summary>
         *
         * <param name="word">{@link Word} type input to asciify.</param>
         * <returns>string output which is asciified.</returns>
         */
        public string Asciify(Word word)
        {
            var modified = word.GetName().ToCharArray();
            for (var i = 0; i < modified.Length; i++)
            {
                switch (modified[i])
                {
                    case '\u00e7':
                        modified[i] = 'c';
                        break;
                    case '\u00f6':
                        modified[i] = 'o';
                        break;
                    case '\u011f':
                        modified[i] = 'g';
                        break;
                    case '\u00fc':
                        modified[i] = 'u';
                        break;
                    case '\u015f':
                        modified[i] = 's';
                        break;
                    case '\u0131':
                        modified[i] = 'i';
                        break;
                    case '\u00c7':
                        modified[i] = 'C';
                        break;
                    case '\u00d6':
                        modified[i] = 'O';
                        break;
                    case '\u011e':
                        modified[i] = 'G';
                        break;
                    case '\u00dc':
                        modified[i] = 'U';
                        break;
                    case '\u015e':
                        modified[i] = 'S';
                        break;
                    case '\u0130':
                        modified[i] = 'I';
                        break;
                }
            }

            return new string(modified);
        }

        /**
         * <summary>Another asciify method which takes a {@link Sentence} as an input. It loops i times where i ranges form 0 to number of
         * words in the given sentence. First it gets each word and calls asciify with current word and creates {@link Word}
         * with returned string. At the and, adds each newly created ascified words to the result {@link Sentence}.</summary>
         *
         * <param name="sentence">{@link Sentence} type input.</param>
         * <returns>Sentence output which is asciified.</returns>
         */
        public Sentence Asciify(Sentence sentence)
        {
            var result = new Sentence();
            for (var i = 0; i < sentence.WordCount(); i++)
            {
                var word = sentence.GetWord(i);
                var newWord = new Word(Asciify(word));
                result.AddWord(newWord);
            }

            return result;
        }
    }
}