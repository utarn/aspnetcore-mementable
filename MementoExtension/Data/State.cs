using System;
using System.Text.Json;

namespace MementoExtension.Data
{
    public class State
    {
        public int StateId { get; set; }
        public string ObjectType { get; set; }
        public string SerializedData { get; private set; }
        public bool IsCurrent { get; set; }
        public State()
        {

        }

        public State(object serializeObject)
        {
            ObjectType = serializeObject.GetType().AssemblyQualifiedName;
            SerializedData = JsonSerializer.Serialize(serializeObject);
        }

        public object GetObject()
        {
            var type = Type.GetType(ObjectType);
            return JsonSerializer.Deserialize(SerializedData, type);
        }
    }
}