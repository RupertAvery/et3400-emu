using Core6800;

namespace Sharp6800
{
    public delegate void OnUpdateDelegate(Cpu6800 emu);
    
    public delegate void OnTimerDelegate(int cyclePerSecond);

    public enum TrainerKeys
    {
        Key0,
        Key1,
        Key2,
        Key3,
        Key4,
        Key5,
        Key6,
        Key7,
        Key8,
        Key9,
        KeyA,
        KeyB,
        KeyC,
        KeyD,
        KeyE,
        KeyF,
        KeyReset
    }
}
