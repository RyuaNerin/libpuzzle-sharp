using LibPuzzle;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Regress1()
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

            Assert.AreEqual(cvec1,  cvec1c);
            Assert.AreEqual(cvec1,  cvec1u);
            Assert.AreEqual(cvec1c, cvec1u);

            Assert.AreEqual(786947673,   cvec1.GetHashCode());
            Assert.AreEqual(-1292227431, cvec2.GetHashCode());
            Assert.AreEqual(556554137,   cvec3.GetHashCode());
            Assert.AreEqual(-1389008455, cvec4.GetHashCode());
            Assert.AreEqual(-304630951,  cvec5.GetHashCode());
            Assert.AreEqual(739909369,   cvec6.GetHashCode());
        }

        [TestMethod]
        public void Regress2()
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

            Assert.AreEqual((int)d1, (int)d2);

            Assert.IsFalse(d1 > IPuzzle.DefaultSimilarityThreshold);
            Assert.IsFalse(d3 > IPuzzle.DefaultSimilarityThreshold);
            Assert.IsFalse(d4 > IPuzzle.DefaultSimilarityThreshold);
            Assert.IsFalse(d5 < IPuzzle.DefaultSimilarityThreshold);
            Assert.IsFalse(d6 < IPuzzle.DefaultSimilarityThreshold);
        }
        
        [TestMethod]
        public void Regress3()
        {
            var context = new PuzzleContext();

            var cvec1 = context.FromPath("pics/pic-a-0.jpg");
            var cvec2 = context.FromPath("pics/pic-a-1.jpg");

            var d1 = cvec2.GetDistanceFrom(cvec1);
            var d2 = cvec1.GetDistanceFrom(cvec2);

            Assert.IsFalse(d1 > IPuzzle.DefaultSimilarityThreshold);
            Assert.IsFalse(d2 > IPuzzle.DefaultSimilarityThreshold);
        }
    }
}
