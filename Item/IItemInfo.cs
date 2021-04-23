using System.IO;
using TestSolution.Util;

namespace TestSolution.Item
{
    public abstract class ItemInfo
    {
        public string ResultName { get; set; }
        public string Name { get; }
        public string BaseName { get; }
        public string Extension { get; }
        
        protected ItemInfo(string name)
        {
            Name = name.GetNameWithoutUUID();
            Extension = Path.GetExtension(Name);
            BaseName = Path.GetFileNameWithoutExtension(Name);
        }
    }
}