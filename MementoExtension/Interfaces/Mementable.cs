using MementoExtension.Data;

namespace MementoExtension.Interfaces
{
    public interface Mementable
    {
        State CreateState();
        void RestoreState(State memento);

    }
}