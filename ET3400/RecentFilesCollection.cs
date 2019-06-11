using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ET3400
{
    public class RecentFilesCollection
    {
        private readonly ToolStripMenuItem _toolStripMenuItem;
        private readonly Action<string> _loadAction;
        private string _configurationPath;
        private readonly int _maxItems;

        public IEnumerable<ToolStripMenuItem> RecentItems
        {
            get { return _toolStripMenuItem.DropDownItems.Cast<ToolStripMenuItem>().Where(x => x.Tag != null); }
        }

        public RecentFilesCollection(ToolStripMenuItem toolStripMenuItem, string configurationPath, int maxItems, Action<string> loadAction)
        {
            _toolStripMenuItem = toolStripMenuItem;
            _configurationPath = configurationPath;
            _maxItems = maxItems;
            if (!File.Exists(_configurationPath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(_configurationPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_configurationPath));
                }
                File.WriteAllText(_configurationPath, "; ET3400 Recent Files List");
            }
            _loadAction = loadAction;
            Load();
        }

        public void AddItem(string fileName)
        {
            if (RecentItems.Any(x => ((string)x.Tag).ToLower() == fileName.ToLower()))
            {
                return;
            }

            if (RecentItems.Count() == _maxItems)
            {
                _toolStripMenuItem.DropDownItems.Remove(RecentItems.Last());
            }


            var item = new ToolStripMenuItem()
            {
                Text = Path.GetFileName(fileName),
                Tag = fileName,
            };

            item.Click += ItemOnClick;

            _toolStripMenuItem.DropDownItems.Insert(1, item);

            Save();
        }

        public void Load()
        {
            var lines = File.ReadAllLines(_configurationPath);
            foreach (var line in lines)
            {
                if (!line.Trim().StartsWith(";"))
                {
                    AddItem(line);
                }
            }
        }

        public void Save()
        {
            var recentFiles = new StringBuilder();
            recentFiles.AppendLine("; ET3400 Recent Files List");

            foreach (var item in RecentItems)
            {
                recentFiles.AppendLine((string)item.Tag);
            }

            if (!Directory.Exists(Path.GetDirectoryName(_configurationPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_configurationPath));
            }

            File.WriteAllText(_configurationPath, recentFiles.ToString());
        }

        private void ItemOnClick(object sender, EventArgs e)
        {
            _loadAction((string)((ToolStripMenuItem)sender).Tag);
        }

        public void Clear()
        {
            foreach (var item in RecentItems.ToList())
            {
                _toolStripMenuItem.DropDownItems.Remove(item);
            }
        }
    }
}