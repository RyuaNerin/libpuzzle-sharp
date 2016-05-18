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

            var cvec1c = cvec1.ToCompressedPuzzle();
            var cvec1u = cvec1c.ToUncompressedPuzzle();

            Debug.Assert(cvec1.Equals(cvec1c));
            Debug.Assert(cvec1.Equals(cvec1u));
            Debug.Assert(cvec1c.Equals(cvec1u));

            Debug.Assert(786947673   == cvec1.GetHashCode());
            Debug.Assert(-1292227431 == cvec1.GetHashCode());
            Debug.Assert(556554137   == cvec1.GetHashCode());
            Debug.Assert(-1389008455 == cvec1.GetHashCode());
            Debug.Assert(-304630951  == cvec1.GetHashCode());
            Debug.Assert(739909369   == cvec1.GetHashCode());
        }
    }
}
