using System.Diagnostics;
using LibPuzzle;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new PuzzleContext();

            var cvec1 = context.FromPath("pics/luxmarket_tshirt01.jpg");
            var cvec2 = context.FromPath("pics/luxmarket_tshirt01_black.jpg");
            var cvec3 = context.FromPath("pics/luxmarket_tshirt01_sal.jpg");
            var cvec4 = context.FromPath("pics/luxmarket_tshirt01_sheum.jpg");
            var cvec5 = context.FromPath("pics/duck.gif");
            var cvec6 = context.FromPath("pics/pic-a-0.jpg");

            var d1 = cvec2.GetDistanceFrom(cvec1);
            var d2 = cvec1.GetDistanceFrom(cvec2);
            var d3 = cvec1.GetDistanceFrom(cvec3);
            var d4 = cvec1.GetDistanceFrom(cvec4);
            var d5 = cvec1.GetDistanceFrom(cvec5);
            var d6 = cvec1.GetDistanceFrom(cvec6);

            Debug.Assert((int)d1 == (int)d2);

            Debug.Assert(d1 < IPuzzle.DefaultSimilarityThreshold);
            Debug.Assert(d3 < IPuzzle.DefaultSimilarityThreshold);
            Debug.Assert(d4 < IPuzzle.DefaultSimilarityThreshold);
            Debug.Assert(d5 > IPuzzle.DefaultSimilarityThreshold);
            Debug.Assert(d6 > IPuzzle.DefaultSimilarityThreshold);
        }
    }
}
