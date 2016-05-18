using System;
using System.Drawing;
using System.IO;

namespace LibPuzzle
{
    [Serializable]
    public class PuzzleContext : ICloneable, IEquatable<PuzzleContext>
    {
        internal int puzzle_max_width = PuzzleNative.PUZZLE_DEFAULT_MAX_WIDTH;
        internal int puzzle_max_height = PuzzleNative.PUZZLE_DEFAULT_MAX_HEIGHT;
        internal int puzzle_lambdas = PuzzleNative.PUZZLE_DEFAULT_LAMBDAS;
        internal double puzzle_p_ratio = PuzzleNative.PUZZLE_DEFAULT_P_RATIO;
        internal double puzzle_noise_cutoff = PuzzleNative.PUZZLE_DEFAULT_NOISE_CUTOFF;
        internal double puzzle_contrast_barrier_for_cropping = PuzzleNative.PUZZLE_DEFAULT_CONTRAST_BARRIER_FOR_CROPPING;
        internal double puzzle_max_cropping_ratio = PuzzleNative.PUZZLE_DEFAULT_MAX_CROPPING_RATIO;
        internal bool puzzle_enable_autocrop = PuzzleNative.PUZZLE_DEFAULT_ENABLE_AUTOCROP;

        public int MaxWidth
        {
            get { return this.puzzle_max_width; }
            set { this.puzzle_max_width = value; }
        }
        public int MaxHeight
        {
            get { return this.puzzle_max_height; }
            set { this.puzzle_max_height = value; }
        }
        public int Lambdas
        {
            get { return this.puzzle_lambdas; }
            set { this.puzzle_lambdas = value; }
        }
        public double NoiseCutOff
        {
            get { return this.puzzle_noise_cutoff; }
            set { this.puzzle_noise_cutoff = value; }
        }
        public double PRatio
        {
            get { return this.puzzle_p_ratio; }
            set { this.puzzle_p_ratio = value; }
        }
        public double ContrastBarrierForCropping
        {
            get { return this.puzzle_contrast_barrier_for_cropping; }
            set { this.puzzle_contrast_barrier_for_cropping = value; }
        }
        public double MaxCroppingRatio
        {
            get { return this.puzzle_max_cropping_ratio; }
            set { this.puzzle_max_cropping_ratio = value; }
        }
        public bool AutoCrop
        {
            get { return this.puzzle_enable_autocrop; }
            set { this.puzzle_enable_autocrop = value; }
        }

        public object Clone()
        {
            var other = new PuzzleContext();
            other.puzzle_max_width = this.puzzle_max_width;
            other.puzzle_max_height = this.puzzle_max_height;
            other.puzzle_lambdas = this.puzzle_lambdas;
            other.puzzle_p_ratio = this.puzzle_p_ratio;
            other.puzzle_noise_cutoff = this.puzzle_noise_cutoff;
            other.puzzle_contrast_barrier_for_cropping = this.puzzle_contrast_barrier_for_cropping;
            other.puzzle_max_cropping_ratio = this.puzzle_max_cropping_ratio;
            other.puzzle_enable_autocrop = this.puzzle_enable_autocrop;

            return other;
        }

        public bool Equals(PuzzleContext other)
        {
            return
                other != null &&
                this.puzzle_max_width == other.puzzle_max_width &&
                this.puzzle_max_height == other.puzzle_max_height &&
                this.puzzle_lambdas == other.puzzle_lambdas &&
                this.puzzle_p_ratio == other.puzzle_p_ratio &&
                this.puzzle_noise_cutoff == other.puzzle_noise_cutoff &&
                this.puzzle_contrast_barrier_for_cropping == other.puzzle_contrast_barrier_for_cropping &&
                this.puzzle_max_cropping_ratio == other.puzzle_max_cropping_ratio &&
                this.puzzle_enable_autocrop == other.puzzle_enable_autocrop;
        }

        public Puzzle FromStream(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            using (var image = Image.FromStream(stream))
                return this.FromImage(image);
        }
        public Puzzle FromPath(string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            using (var image = Image.FromFile(path))
                return this.FromImage(image);
        }
        public Puzzle FromImage(Image image)
        {
            if (image == null) throw new ArgumentNullException("image");
            if (image.Width  > this.puzzle_max_width ||
                image.Height > this.puzzle_max_height)
                throw new NotSupportedException("Image has too large resolution.");
            if (image.Width  <= 0 ||
                image.Height <= 0)
                throw new NotSupportedException("Width or Height of Image is 0.");

            Bitmap bitmap = image as Bitmap;
            if (bitmap != null)
                return new Puzzle(PuzzleNative.puzzle_fill_cvec_from_file(this, bitmap));

            using (bitmap = new Bitmap(image))
                return new Puzzle(PuzzleNative.puzzle_fill_cvec_from_file(this, bitmap));
        }
    }
}
