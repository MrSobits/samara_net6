namespace Sobits.RosReg.Entities
{
    using System;

    public class ExtractEgrnRightLegalNotResident : ExtractEgrnRightLegal
    {
        
        /// <summary>
        /// Страна региcтстрации
        /// </summary>
        public virtual string IncorporationCountry { get; set; }

        /// <summary>
        /// Регистрационный номер 
        /// </summary>
        public virtual string RegistrationNumber { get; set; }

        /// <summary>
        /// Дата государственной регистрации
        /// </summary>
        public virtual DateTime DateStateReg { get; set; }

        /// <summary>
        /// Наименование регистрирующего органа
        /// </summary>
        public virtual string RegistrationOrgan { get; set; }

        /// <summary>
        /// Адрес местонахождения
        /// </summary>
        public virtual string RegAddresSubject { get; set; }
    }
}