using System;
using System.IO;
using TestSolution.Util;

namespace TestSolution.Item
{
    public class CustomFileInfo : ItemInfo, IComparable<CustomFileInfo>
    {
        public FileInfo Src { get; }
        public int Size { get; }
        private readonly DirNameInfo _dirNameInfo;
        public string DirInitialName => _dirNameInfo.InitialName;

        public CustomFileInfo(FileInfo src, bool rootDir)
            : base(src.Name)
        {
            Src = src;
            Size = GetFileSize(src);

            _dirNameInfo = rootDir ? 
                new DirNameInfo("") : 
                new DirNameInfo(src.Directory.Name);
        }

        public int CompareTo(CustomFileInfo that)
        {
            if (that.Equals(this)) return 0;

            return Size > that.Size ? -1 : 1;
        }

        public int GetFileSize(FileInfo fileInfo)
        {
            // Здесь можно поменять метод подсчета размера файла
            return (int) fileInfo.Length;
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            if (obj == null || obj.GetType() != GetType()) return false;
            var that = (CustomFileInfo) obj;

            return _dirNameInfo.Name.Equals(that._dirNameInfo.Name) &&
                   Name.Equals(that.Name) &&
                   Size == that.Size &&
                   Src.CompareContent(that.Src);
        }

        public override int GetHashCode()
        {
            var result = Name.GetHashCode();
            result = 31 * result + Size;
            result = 31 * result + _dirNameInfo.Name.GetHashCode();
            return result;
        }

        private class DirNameInfo
        {
            public DirNameInfo(string initialName)
            {
                InitialName = initialName;
                Name = initialName.GetNameWithoutUUID();
            }

            public string InitialName { get; }
            public string Name { get; }
        }
    }
}