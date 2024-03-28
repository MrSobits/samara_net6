namespace Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Выгружаемая информация
    /// </summary>
    public class ExportData
    {
        /// <summary>
        /// Список выгруженных идентификаторов
        /// </summary>
        public IList<long> Ids { get; }

        /// <summary>
        /// Массив данных
        /// </summary>
        public FileInfo Data { get; protected set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="capacity">Размер выгружаемых данных</param>
        public ExportData(int capacity = 100)
        {
            this.Ids = new List<long>(capacity);
        }

        /// <summary>
        /// Скопировать данные из потока
        /// </summary>
        /// <param name="stream">Поток в памяти</param>
        public void SetStream(FileStream stream)
        {
            this.Data = new FileInfo(stream.Name);
        }
    }
}