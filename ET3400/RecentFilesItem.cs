namespace ET3400
{
    public class RecentFilesItem
    {
        public string FileName { get; set; }
        public string Path { get; set; }

        public RecentFilesItem(string path)
        {
            Path = path;
            FileName = System.IO.Path.GetFileName(path);
        }
    }
}