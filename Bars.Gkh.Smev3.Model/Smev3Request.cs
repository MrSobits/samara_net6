namespace Bars.Gkh.Smev3
{
    using System;

    /// <summary>
    /// Сообщение, передаваемое через СМЭВ 3.0
    /// </summary>
    [Serializable]
    public class Smev3Request : Smev3Message
    {
        /// <summary>
        /// Сообщение для передачи
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Подпись пакета
        /// </summary>
        public string PersonalSignature { get; set; }

        /// <summary>
        /// Отправитель
        /// </summary>
        public Smev3Sender Sender { get; set; }
    }
}
