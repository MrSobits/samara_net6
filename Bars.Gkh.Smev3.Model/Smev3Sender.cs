namespace Bars.Gkh.Smev3
{
    using System;

    /// <summary>
    /// Отправитель сообщения в СМЭВ3
    /// </summary>
    [Serializable]
    public class Smev3Sender
    {
        /// <summary>
        /// Мнемоника
        /// </summary>
        public string Mnemonic { get; set; }

        /// <summary>
        /// Читаемое название
        /// </summary>
        public string HumanReadableName { get; set; }
    }
}