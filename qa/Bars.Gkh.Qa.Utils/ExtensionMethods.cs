namespace Bars.Gkh.Qa.Utils
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    public static class ExtensionMethods
    {
        /// <summary>
        /// Полное копирование сущности
        /// </summary>
        /// <typeparam name="T">
        /// тип копируемого объекта
        /// </typeparam>
        /// <param name="a">
        /// копируемый объект
        /// </param>
        /// <returns>
        /// </returns>
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
