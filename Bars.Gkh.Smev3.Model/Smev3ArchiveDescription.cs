namespace Bars.Gkh.Smev3.Attachments
{
    using System;

    /// <summary>
    /// Описание архива
    /// </summary>
    [Serializable]
    public class Smev3ArchiveDescription
    {
        /// <summary>
        /// Наименование файла
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Пространство имен
        /// </summary>
        public string Namespace { get; set; }
    }
}