namespace Bars.Gkh.Smev3
{
    using System;
	
    [Serializable]
    public class Smev3Response : Smev3Request
    {
        /// <summary>
        /// Статус передачи сообщения
        /// </summary>
        public TransferState State { get; set; }

        /// <summary>
        /// Идентификатор сообщения СМЭВ 3.0
        /// </summary>
        public string MessageId { get; set; }
    }
}