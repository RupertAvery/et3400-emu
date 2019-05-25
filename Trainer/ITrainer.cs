using System.Collections.Generic;
using Core6800;

namespace Sharp6800.Trainer
{
    public interface ITrainer : IMemory, IMemoryMap
    {
        List<int> Breakpoints { get; }
        Cpu6800State State { get; }
        bool Running { get; }
    }
}