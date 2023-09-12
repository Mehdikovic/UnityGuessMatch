using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Core {
    public static class BinaryClone {
        public static T Clone<T>(T source) {
            if (!typeof(T).IsSerializable) {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            if (source is null) {
                return default;
            }

            IFormatter formatter = new BinaryFormatter();
            using Stream stream = new MemoryStream();
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);
            return (T) formatter.Deserialize(stream);
        }
    }
}