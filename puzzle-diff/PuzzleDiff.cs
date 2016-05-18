using System;
using LibPuzzle;

namespace PuzzleDiff
{
    static class PuzzleDiff
    {
        private static void Usage()
        {
            Console.WriteLine("\nUsage: puzzle-diff [-b <contrast barrier for cropping>] [-c]");
            Console.WriteLine("   [-C <max cropping ratio>] [-e] [-E <similarity threshold>] [-h]");
            Console.WriteLine("   [-H <max height>] [-l <lambdas>] [-n <noise cutoff>]");
            Console.WriteLine("   [-p <p ratio>] [-t] [-W <max width>] <file 1> <file 2>");
            Console.WriteLine();
            Console.WriteLine("Visually compares two images and returns their distance.");
            Console.WriteLine();
            Console.WriteLine("-b <contrast barrier for cropping>");
            Console.WriteLine("-c : disable autocrop");
            Console.WriteLine("-C <max cropping ratio>");
            Console.WriteLine("-e : exit with 10 (images are similar) or 20 (images are not)");
            Console.WriteLine("-E <similarity threshold> : for -e");
            Console.WriteLine("-h : show help");
            Console.WriteLine("-H <width> : set max height");
            Console.WriteLine("-l <lambdas> : change lambdas");
            Console.WriteLine("-n <noise cutoff> : change noise cutoff");
            Console.WriteLine("-p <ratio> : set p ratio");
            Console.WriteLine("-t disable fix for texts");
            Console.WriteLine("-W <width> : set max width");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        private static int Main(string[] args)
        {
            PuzzleContext context = new PuzzleContext();

            string file1 = null, file2 = null;
            bool useExitCode = false;
            double threshold = IPuzzle.DefaultSimilarityThreshold;
            bool fixForTexts = true;

            try
            {            
                int i = 0;
                while (i < args.Length)
                {
                    switch (args[i])
                    {
                        case "-b":
                            context.ContrastBarrierForCropping = int.Parse(args[++i]);
                            break;

                        case "-c":
                            context.AutoCrop = false;
                            break;

                        case "-C":
                            context.MaxCroppingRatio = int.Parse(args[++i]);
                            break;

                        case "-e":
                            useExitCode = true;
                            break;

                        case "-E":
                            threshold = double.Parse(args[++i]);
                            break;

                        case "-h":
                            Usage();
                            return 0;

                        case "-H":
                            context.MaxHeight = int.Parse(args[++i]);
                            break;

                        case "-W":
                            context.MaxHeight = int.Parse(args[++i]);
                            break;

                        case "-l":
                            context.Lambdas = int.Parse(args[++i]);
                            break;

                        case "-n":
                            context.NoiseCutOff = double.Parse(args[++i]);
                            break;

                        case "-p":
                            context.PRatio = double.Parse(args[++i]);
                            break;

                        case "-t":
                            fixForTexts = false;
                            break;

                        default:
                            if (file1 == null)
                                file1 = args[i];
                            else if (file2 == null)
                                file2 = args[i];
                            else
                            {
                                Usage();
                                return 0;
                            }
                            break;
                    }

                    ++i;
                }

                if (file1 == null || file2 == null)
                {
                    Usage();
                    return 0;
                }

                Puzzle cvec1, cvec2;
                try
                {
                    cvec1 = context.FromPath(file1);
                }
                catch
                {
                    Console.Error.WriteLine("Unable to read [{0}]\n", file1);
                    return 1;
                }
                try
                {
                    cvec2 = context.FromPath(file2);
                }
                catch
                {
                    Console.Error.WriteLine("Unable to read [{0}]\n", file2);
                    return 1;
                }
                
                double d = cvec1.GetDistanceFrom(cvec2, fixForTexts);

                if (!useExitCode)
                {
                    Console.WriteLine(d);
                    return 0;
                }

                return d >= threshold ? 20 : 10;
            }
            catch
            {
                Usage();
            }
            
            return 0;
        }
    }
}
