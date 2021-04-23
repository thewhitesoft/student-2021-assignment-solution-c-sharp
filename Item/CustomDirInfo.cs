using System.Collections.Generic;
using System.Linq;

namespace TestSolution.Item
{
    public class CustomDirInfo : ItemInfo
    {
        public List<CustomFileInfo> Files { get; }
        public int Size { get; }

        public CustomDirInfo(string name, List<CustomFileInfo> files)
            : base(name)
        {
            Files = files;
            Size = files.Sum(file => file.Size);
        }
    }
}