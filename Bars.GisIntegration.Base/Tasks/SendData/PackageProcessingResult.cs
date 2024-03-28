namespace Bars.GisIntegration.Base.Tasks.SendData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Результат обработки пакета данных
    /// </summary>
    public class PackageProcessingResult
    {
        /// <summary>
        /// Статус
        /// </summary>
        public PackageState State { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Результаты обработки объектов
        /// </summary>
        public List<ObjectProcessingResult> Objects { get; set; }

        /// <summary>
        /// Общее количество объектов
        /// </summary>
        public int ObjectsCount {
            get
            {
                if (this.State == PackageState.SuccessProcessed || this.State == PackageState.ProcessedWithErrors)
                {
                    return this.Objects.Count;
                }

                return 0;
            }
        }

        /// <summary>
        /// Количество успешно обработанных объектов
        /// </summary>
        public int SuccessProcessedObjectsCount {
            get
            {
                if (this.State == PackageState.SuccessProcessed || this.State == PackageState.ProcessedWithErrors)
                {
                    return this.Objects.Count(x => x.State == ObjectProcessingState.Success);
                }

                return 0;
            }
        }
    }
}
