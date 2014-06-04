namespace Sharp6800.Common
{
    public class ComboBoxItem
    {
        public string Description { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}