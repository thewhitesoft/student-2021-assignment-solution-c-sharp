using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestSolution.Item;
using TestSolution.Util;

namespace TestSolution
{
    public class Worker
    {
        private readonly FileInfo _destZip;
        private readonly SortedSet<CustomFileInfo> _fileInfos = new SortedSet<CustomFileInfo>();
        private readonly DirectoryInfo _src;
        private Dictionary<string, List<CustomDirInfo>> _dirsByName;

        public Worker(DirectoryInfo src, FileInfo destZip)
        {
            _src = src;
            _destZip = destZip;
        }

        public void Run()
        {
            Init();
            DetermineNames();
            Zip();
        }

        private void Init()
        {
            ProcessDir(_src, true);
            foreach (var dir in _src.GetDirectories()) ProcessDir(dir, false);

            _dirsByName = _fileInfos
                .GroupBy(x => x.DirInitialName)
                .Select(x => new CustomDirInfo(x.Key, x.ToList()))
                .OrderBy(x => -x.Size)
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key,
                    x => x.ToList());
        }

        private void ProcessDir(DirectoryInfo dir, bool root)
        {
            foreach (var file in dir.GetFiles()) _fileInfos.Add(new CustomFileInfo(file, root));
        }

        private void DetermineNames()
        {
            //Проставляем имена директориям
            _dirsByName.Values.ForEach(DetermineItemNames);

            //Проставляем имена файлам
            _dirsByName.Values
                .SelectMany(x => x)
                .Select(x => x.Files)
                .ForEach(files =>
                {
                    files.GroupBy(x => x.Name)
                        .ForEach(infos => DetermineItemNames(infos.ToList()));
                });
        }

        private void DetermineItemNames<T>(List<T> items) where T : ItemInfo
        {
            var fileName = items[0].Name;
            for (var j = 0; j < items.Count; j++)
            {
                var itemInfo = items[j];
                itemInfo.ResultName = j == 0 ? fileName : $"{itemInfo.BaseName} ({j}){itemInfo.Extension}";
            }
        }

        private void Zip()
        {
            using var zipArchive = new Zip(_destZip.FullName);
            _dirsByName.Values
                .SelectMany(x => x)
                .ForEach(zipArchive.AddDir);
        }
    }
}