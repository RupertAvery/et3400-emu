namespace Sharp6800.Trainer
{
    public interface IIODevice
    {
        int Read(int address);
        void Write(int address, int value);
    }
}