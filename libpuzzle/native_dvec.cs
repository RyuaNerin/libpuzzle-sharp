using System;
using System.Drawing;
using System.IO;

namespace LibPuzzle
{
    internal static partial class PuzzleNative
    {
        public static void puzzle_autocrop_axis(PuzzleContext context, PuzzleView view, out int crop0, out int crop1, int axisn, int axiso, int omaptrinc, int nmaptrinc)
        {
            int chunk_n1 = axisn - 1;
            int chunk_o1 = axiso - 1;
            crop0 = 0;
            crop1 = chunk_n1;
            if (axisn < PuzzleNative.PUZZLE_MIN_SIZE_FOR_CROPPING ||
                axiso < PuzzleNative.PUZZLE_MIN_SIZE_FOR_CROPPING)
                return;

            double[] chunk_contrasts = new double[chunk_n1 + 1];
            if (axisn >= int.MaxValue || axiso >= int.MaxValue)
                throw new Exception();

            double chunk_contrast = 0.0, total_contrast = 0.0, barrier_contrast;
            int chunk_n = chunk_n1;
            byte level = 0;
            byte previous_level = 0;
            int chunk_o;
            int maptr = 0;
            do
            {
                chunk_contrast = 0.0;
                chunk_o = chunk_o1;
                do
                {
                    level = view.map[maptr];
                    if (previous_level > level)
                        chunk_contrast += previous_level - level;
                    else
                        chunk_contrast += level - previous_level;
                    maptr += omaptrinc;
                } while (chunk_o-- != 0);
                chunk_contrasts[chunk_n] = chunk_contrast;
                total_contrast += chunk_contrast;
                maptr += nmaptrinc;
            } while (chunk_n-- != 0);
            barrier_contrast = total_contrast * context.puzzle_contrast_barrier_for_cropping;
            total_contrast = 0.0;
            crop0 = 0;
            do
            {
                total_contrast += chunk_contrasts[crop0];
                if (total_contrast >= barrier_contrast)
                    break;
            } while (crop0++ < chunk_n1);
            total_contrast = 0.0;    
            crop1 = chunk_n1;
            do
            {
                total_contrast += chunk_contrasts[crop1];
                if (total_contrast >= barrier_contrast)
                    break;
            } while (crop1-- > 0);
            chunk_contrasts = null;
            if (crop0 > chunk_n1 || crop1 > chunk_n1)
                throw new Exception();

            int max_crop = (int)Math.Round(chunk_n1 * context.puzzle_max_cropping_ratio);
            if (max_crop > chunk_n1)
                throw new Exception();
            crop0 = Math.Min(crop0, max_crop);
            crop1 = Math.Max(crop1, chunk_n1 - max_crop);
        }

        public static void puzzle_autocrop_view(PuzzleContext context, ref PuzzleView view)
        {
            int cropx0, cropx1;
            int cropy0, cropy1;

            puzzle_autocrop_axis(context, view, out cropx0, out cropx1, view.width,  view.height, (int)view.width, 1 - (int)(view.width * view.height));
            puzzle_autocrop_axis(context, view, out cropy0, out cropy1, view.height, view.width,  1,               0);

            if (cropx0 > cropx1 || cropy0 > cropy1)
                throw new Exception();

            int maptr = 0;
            int x;
            int y = cropy0;
            do
            {
                x = cropx0;
                do
                {
                    view.map[maptr++] = view.map[view.width * y + x];
                } while (x++ != cropx1);
            } while (y++ != cropy1);
            view.width  = cropx1 - cropx0 + 1;
            view.height = cropy1 - cropy0 + 1;

            byte[] newArr = new byte[view.width * view.height];
            Array.Copy(view.map, newArr, newArr.Length);
            view.map = newArr;
            if (view.width <= 0 || view.height <= 0)
                throw new Exception();
        }

        public static PuzzleView puzzle_getview_from_gdimage(PuzzleContext context, Bitmap bitmap)
        {
            PuzzleView view = new PuzzleView();
            view.map = null;    
            view.width = bitmap.Width;
            view.height = bitmap.Height;
            if (view.width > context.puzzle_max_width || view.height > context.puzzle_max_height)
                throw new Exception();
            if (view.width == 0 || view.height == 0)
                throw new Exception();

            int x1 = view.width - 1;
            int y1 = view.height - 1;
            if (view.width <= 0 || view.height <= 0)
                throw new Exception();
            if (x1 > int.MaxValue || y1 > int.MaxValue)
                throw new Exception();

            view.map = new byte[view.width * view.height];

            Color pixel;
            int maptr = 0;// view.map;
            int x = (int)x1;
            int y;
            do
            {
                y = (int)y1;
                do
                {
                    pixel = bitmap.GetPixel(x, y);

                    view.map[maptr++] = (byte)((pixel.R * 77 + pixel.G * 151 + pixel.B * 28 + 128) / 256);
                } while (y-- != 0);
            } while (x-- != 0);

            return view;
        }

        public static double puzzle_softedgedlvl(ref PuzzleView view, int x, int y)
        {
            int xlimit = x + PuzzleNative.PUZZLE_PIXEL_FUZZ_SIZE;
            int ylimit = y + PuzzleNative.PUZZLE_PIXEL_FUZZ_SIZE;

            if (x >= view.width || y >= view.height || xlimit <= x || ylimit <= y)
                throw new Exception();
            
            uint lvl = 0;
            int count = 0;
            int ax = x > PuzzleNative.PUZZLE_PIXEL_FUZZ_SIZE ? x - PuzzleNative.PUZZLE_PIXEL_FUZZ_SIZE : 0;
            int ay;
            do
            {
                if (ax >= view.width)
                    break;

                ay = y > PuzzleNative.PUZZLE_PIXEL_FUZZ_SIZE ? y - PuzzleNative.PUZZLE_PIXEL_FUZZ_SIZE : 0;

                do
                {
                    if (ay >= view.height)
                        break;
                    count++;
                    lvl += (uint)view.map[view.width * ay + ax];
                } while (ay++ < ylimit);
            } while (ax++ < xlimit);

            if (count <= 0)
                return 0.0;

            return lvl / (double) count;
        }

        public static double puzzle_get_avglvl(ref PuzzleView view, int x, int y, int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new Exception();

            int xlimit = x + width - 1;
            int ylimit = y + height - 1;

            if (xlimit < x || ylimit < y)
                throw new Exception();

            double lvl = 0.0;
            int ax = x;
            int ay;
            do
            {
                if (ax >= view.width)
                    throw new Exception();

                ay = y;
                do
                {
                    if (ay >= view.height)
                        throw new Exception();

                    lvl += puzzle_softedgedlvl(ref view, ax, ay);
                } while (ay++ < ylimit);
            } while (ax++ < xlimit);
    
            return lvl / (double)(width * height);
        }

        public static PuzzleAvgLvls puzzle_fill_avglgls(PuzzleContext context, PuzzleView view, int lambdas)
        {
            double width = (double)view.width;
            double height = (double)view.height;


            PuzzleAvgLvls avglvls = new PuzzleAvgLvls();
            avglvls.lambdas = lambdas;
            avglvls.lvls = new double[lambdas * lambdas];
            double xshift = (width  - width  * lambdas / (lambdas + 1)) / 2;
            double yshift = (height - height * lambdas / (lambdas + 1)) / 2;
            int p = (int)Math.Round(Math.Min(width, height) / ((lambdas + 1) * context.puzzle_p_ratio));
            if (p < PuzzleNative.PUZZLE_MIN_P)
                p = PuzzleNative.PUZZLE_MIN_P;

            double x, y;
            int xd, yd;
            int px, py;
            int lwidth, lheight;
            int lx = 0;
            int ly;
            do
            {
                ly = 0;
                do
                {
                    x = xshift + (double) lx * (width  - 1) / (lambdas + 1);
                    y = yshift + (double) ly * (height - 1) / (lambdas + 1);
                    lwidth  = (int)Math.Round(xshift + (lx + 1) * (width  - 1) / (lambdas + 1) - x);
                    lheight = (int)Math.Round(yshift + (ly + 1) * (height - 1) / (lambdas + 1) - y);
                    
                    xd = p < lwidth  ? (int)Math.Round(x + (lwidth  - p) / 2.0) : (int)Math.Round(x);
                    yd = p < lheight ? (int)Math.Round(y + (lheight - p) / 2.0) : (int)Math.Round(y);

                    px = view.width  - xd < p ? 1 : p;
                    py = view.height - yd < p ? 1 : p;
                    
                    avglvls.lvls[avglvls.lambdas * ly + lx] = (px > 0 && py > 0) ? puzzle_get_avglvl(ref view, xd, yd, px, py) : 0;
                } while (++ly < lambdas);
            } while (++lx < lambdas);

            return avglvls;
        }

        public static int puzzle_add_neighbors(double[] vecur_, ref int vecur, int max_neighbors, ref PuzzleAvgLvls avglvls, int lx, int ly)
        {
            if (max_neighbors != 8)
                throw new Exception();

    
            int xlimit = lx >= avglvls.lambdas - 1 ? avglvls.lambdas - 1 : lx + 1;
            int ylimit = ly >= avglvls.lambdas - 1 ? avglvls.lambdas - 1 : ly + 1;

            int ax = lx <= 0 ? 0 : lx - 1;
            int ay;
            int neighbors = 0;
            double @ref = avglvls.lvls[avglvls.lambdas * ly + lx];
            do
            {
                ay = ly <= 0 ? 0 : ly - 1;
                do
                {
                    if (ax == lx && ay == ly)
                        continue;

                    vecur_[vecur++] = @ref - avglvls.lvls[avglvls.lambdas * ay + ax];
                    neighbors++;
                } while (ay++ < ylimit);
            } while (ax++ < xlimit);

            if (neighbors > max_neighbors)
                throw new Exception();

            return neighbors;
        }

        public static PuzzleDvec puzzle_fill_dvec(PuzzleAvgLvls avglvls)
        {
            int lambdas = avglvls.lambdas;
            
            PuzzleDvec dvec = new PuzzleDvec();
            dvec.vec = new double[(lambdas * lambdas * PuzzleNative.PUZZLE_NEIGHBORS)];
            int vecur = 0;
            int lx = 0;
            int ly;
            do
            {
                ly = 0;
                do
                {
                    puzzle_add_neighbors(dvec.vec, ref vecur, PuzzleNative.PUZZLE_NEIGHBORS, ref avglvls, lx, ly);
                } while (++ly < lambdas);
            } while (++lx < lambdas);
            dvec.sizeof_compressed_vec = vecur;

            return dvec;
        }

        public static PuzzleDvec puzzle_fill_dvec_from_file(PuzzleContext context, Bitmap bitmap)
        {
            PuzzleView view = puzzle_getview_from_gdimage(context, bitmap);

            if (context.puzzle_enable_autocrop)
                puzzle_autocrop_view(context, ref view);

            return puzzle_fill_dvec(puzzle_fill_avglgls(context, view, context.puzzle_lambdas));
        }
    }
}
