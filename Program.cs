using System;
using System.IO;
using System.Linq;

namespace TestSolution
{
    internal class Program
    {
        private const string ZipName = "Utrom's secrets.zip";

        public static void Main(string[] args)
        {
            var list = args.ToList();
            var srcPath = args[list.IndexOf("--src") + 1];
            var destPath = args[list.IndexOf("--dest") + 1];

            var srcFile = new DirectoryInfo(srcPath);
            var destFile = new FileInfo(destPath);

            if (!srcFile.Exists)
            {
                Console.WriteLine($"Wrong src: {srcFile.FullName}");
                Environment.Exit(1);
            }

            if (!destFile.Exists) Directory.CreateDirectory(destFile.FullName);
            var destZip = new FileInfo(Path.Combine(destFile.FullName, ZipName));

            new Worker(srcFile, destZip).Run();
            Console.WriteLine($"Path to Utrom's secrets.zip: {destFile.FullName}");
        }
    }
}