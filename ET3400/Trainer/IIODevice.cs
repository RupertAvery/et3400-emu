namespace ET3400.Trainer
{
    public interface IIODevice
    {
        int Read(int address);
        void Write(int address, int value);
    }
}