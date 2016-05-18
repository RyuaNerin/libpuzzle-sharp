using System.Diagnostics;
using LibPuzzle;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PuzzleContext();

            var cvec1 = context.FromPath("pics/pic-a-0.jpg");
            var cvec2 = context.FromPath("pics/pic-a-1.jpg");

            var d1 = cvec2.GetDistanceFrom(cvec1);
            var d2 = cvec1.GetDistanceFrom(cvec2);

            Debug.Assert(d1 < IPuzzle.DefaultSimilarityThreshold);
            Debug.Assert(d2 < IPuzzle.DefaultSimilarityThreshold);
        }
    }
}
