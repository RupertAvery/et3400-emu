namespace Sharp6800.Common
{
    public class ComboBoxItem
    {
        public virtual string Description { get; set; }
        public virtual int Start { get; set; }
        public virtual int End { get; set; }
        public override string ToString()
        {
            return Description;
        }
    }
}