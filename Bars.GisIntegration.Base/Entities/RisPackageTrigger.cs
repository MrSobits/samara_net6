namespace Bars.GisIntegration.Base.Entities
{
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Bars.Gkh.Quartz.Scheduler.Entities;

    /// <summary>
    /// Сущность, связывающая пакет и триггер, который его сформировал или отправил
    /// </summary>
    public class RisPackageTrigger: BaseEntity
    {
        /// <summary>
        /// Ссылка на триггер
        /// </summary>
        public virtual Trigger Trigger { get; set; }

        /// <summary>
        /// Ссылка на пакет
        /// </summary>
        public virtual RisPackage Package { get; set; }

        /// <summary>
        /// Статус пакета
        /// </summary>
        public virtual PackageState State { get; set; }

        /// <summary>
        /// Сообщение (нпр, об ошибке обработки пакета)
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// Результат обработки пакета
        /// </summary>
        public virtual byte[] ProcessingResult { get; set; }

        /// <summary>
        /// Идентификатор сообщения для получения результата от асинхронного сервиса
        /// </summary>
        public virtual string AckMessageGuid { get; set; }

        /// <summary>
        /// Десериализовать результат обработки пакета
        /// </summary>
        /// <returns></returns>
        public virtual List<SerializableObjectProcessingResult> GetProcessingResult()
        {
            var result = new List<SerializableObjectProcessingResult>();
            BinaryFormatter formatter = new BinaryFormatter();

            if (this.ProcessingResult != null && this.ProcessingResult.Length > 0)
            {
                using (var memoryStream = new MemoryStream(this.ProcessingResult))
                {
                    result = (List<SerializableObjectProcessingResult>)formatter.Deserialize(memoryStream);
                }
            }

            return result;
        }

        public virtual void SetProcessingResult(List<SerializableObjectProcessingResult> processingResult)
        {
            using (var memoryStream = new MemoryStream())
            {
                if (processingResult != null)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memoryStream, processingResult);
                }

                this.ProcessingResult = memoryStream.ToArray();
            }
        }
    }
}
