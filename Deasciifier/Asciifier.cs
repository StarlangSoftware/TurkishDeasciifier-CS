using Corpus;

namespace Deasciifier
{
    public interface Asciifier
    {
        /**
         * <summary>The asciify method which takes a {@link Sentence} as an input and also returns a {@link Sentence} as the output.</summary>
         *
         * <param name="sentence">{@link Sentence} type input.</param>
         * <returns>Sentence result.</returns>
         */
        Sentence Asciify(Sentence sentence);
        
    }
}