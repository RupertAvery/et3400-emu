namespace Sharp6800.Trainer
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