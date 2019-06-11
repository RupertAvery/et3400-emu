namespace ET3400.Common
{
    public class MemoryRange
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