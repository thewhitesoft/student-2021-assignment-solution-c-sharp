using System;
using System.IO;
using System.IO.Compression;
using TestSolution.Item;

namespace TestSolution.Util
{
    public class Zip : IDisposable
    {
        private readonly ZipArchive _archive;
        private readonly FileStream _zipFileStream;

        public Zip(string path)
        {
            _zipFileStream = new FileStream(path, FileMode.Create);
            _archive = new ZipArchive(_zipFileStream, ZipArchiveMode.Create);
        }

        public void Dispose()
        {
            _archive.Dispose();
            _zipFileStream.Dispose();
            GC.SuppressFinalize(this);
        }

        public void AddDir(CustomDirInfo dir)
        {
            if (!dir.ResultName.Equals("")) _archive.CreateEntry(dir.ResultName + Path.DirectorySeparatorChar);

            foreach (var fileInfo in dir.Files) AddFile(dir.ResultName, fileInfo);
        }

        public void AddFile(string dirName, CustomFileInfo file)
        {
            var name = dirName.Equals("")
                ? file.ResultName
                : dirName + Path.DirectorySeparatorChar + file.ResultName;

            var entry = _archive.CreateEntry(name);

            Copy(entry, file.Src);
        }

        private void Copy(ZipArchiveEntry entry, FileInfo file)
        {
            using var writer = new StreamWriter(entry.Open());
            using var reader = new StreamReader(file.FullName);
            reader.BaseStream.CopyTo(writer.BaseStream);
        }
    }
}