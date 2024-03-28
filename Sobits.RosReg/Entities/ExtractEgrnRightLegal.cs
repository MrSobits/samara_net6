namespace Sobits.RosReg.Entities
{
    using System;

    using Bars.B4.DataAccess;

    using Sobits.RosReg.Enums;

    public class ExtractEgrnRightLegal : PersistentObject 
    {
        /// <inheritdoc />
        public override long Id { get; set; }
        
        /// <summary>
        /// Резидент или нет
        /// </summary>
        public virtual LegalOwnerType OwnerType { get; set; }

        /// <summary>
        /// Правовая форма
        /// </summary>
        public virtual string IncorporationForm { get; set; }
        /// <summary>
        /// Наименивание организации
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Инн
        /// </summary>
        public virtual long Inn { get; set; }
        /// <summary>
        ///  E-mail
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public virtual string MailingAddress { get; set; }
        /// <summary>
        /// Право из выписки
        /// </summary>
        public virtual ExtractEgrnRight RightId { get; set; }
    }
}