using System.Linq;
using System.Threading.Tasks;
using aspnetcore_mementable.Data;
using aspnetcore_mementable.MementoExtension.Interfaces;
using MementoExtension.Interfaces;

namespace aspnetcore_mementable.MementoExtension.Services
{
    public class StateManager<T> where T : class, Mementable
    {
        private StateDbContext _context;
        private string _type;
        public StateManager(StateDbContext context)
        {
            _context = context;
            _type = typeof(T).AssemblyQualifiedName;

        }

        public async Task PushMemento(Mementable obj)
        {
            var state = obj.CreateState();
            state.IsCurrent = true;
            var lastObject = _context.States.FirstOrDefault(s => s.ObjectType == _type &&
                                                            s.IsCurrent == true);
            if (lastObject != null)
            {
                lastObject.IsCurrent = false;
            }
            _context.States.Add(state);
            await _context.SaveChangesAsync();
        }

        public async Task<T> Undo()
        {
            var currentState = _context.States
            .Where(s => s.ObjectType == _type && s.IsCurrent == true)
            .OrderByDescending(s => s.StateId).FirstOrDefault();

            if (currentState == null)
            {
                return null;
            }

            var lastState = _context.States
            .Where(s => s.ObjectType == _type && s.StateId < currentState.StateId)
            .OrderByDescending(s => s.StateId)
            .FirstOrDefault();

            if (lastState != null)
            {
                lastState.IsCurrent = true;
                currentState.IsCurrent = false;
                await _context.SaveChangesAsync();
                return lastState.GetObject() as T;
            }
            else
            {
                return currentState.GetObject() as T;
            }
        }

        public async Task<T> Redo()
        {
            var currentState = _context.States
            .Where(s => s.ObjectType == _type && s.IsCurrent == true)
            .OrderByDescending(s => s.StateId).FirstOrDefault();

            if (currentState == null)
            {
                return null;
            }

            var nextState = _context.States
            .Where(s => s.ObjectType == _type && s.StateId > currentState.StateId)
            .OrderBy(s => s.StateId)
            .FirstOrDefault();

            if (nextState != null)
            {
                nextState.IsCurrent = true;
                currentState.IsCurrent = false;
                await _context.SaveChangesAsync();
                return nextState.GetObject() as T;
            }
            else
            {
                return currentState.GetObject() as T;
            }
        }
    }
}