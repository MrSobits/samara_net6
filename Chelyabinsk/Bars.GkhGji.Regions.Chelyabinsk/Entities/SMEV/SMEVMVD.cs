namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.B4.Modules.FileStorage;

    public class SMEVMVD : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// дата рождения
        /// </summary>
        public virtual DateTime BirthDate { get; set; }

        /// <summary>
        /// СНИЛС
        /// </summary>
        public virtual string SNILS { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public virtual string Surname { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public virtual string PatronymicName { get; set; }

        /// <summary>
        /// Тип адреса основной 
        /// </summary>
        public virtual MVDTypeAddress MVDTypeAddressPrimary { get; set; }

        /// <summary>
        /// Код региона основной
        /// </summary>
        public virtual RegionCodeMVD RegionCodePrimary { get; set; }

        /// <summary>
        /// Адрес основной
        /// </summary>
        public virtual string AddressPrimary { get; set; }

        /// <summary>
        /// Тип адреса дополнительный 
        /// </summary>
        public virtual MVDTypeAddress MVDTypeAddressAdditional { get; set; }

        /// <summary>
        /// Код региона дополнительный
        /// </summary>
        public virtual RegionCodeMVD RegionCodeAdditional { get; set; }

        /// <summary>
        /// Адрес дополнительный
        /// </summary>
        public virtual string AddressAdditional { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string AnswerInfo { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }


    }
}
