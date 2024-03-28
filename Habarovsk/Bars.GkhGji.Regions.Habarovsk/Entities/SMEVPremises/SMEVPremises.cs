namespace Bars.GkhGji.Regions.Habarovsk.Entities.SMEVPremises
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Enums;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVPremises : BaseEntity
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
        /// ОКТМО
        /// </summary>
        public virtual string OKTMO { get; set; }

        /// <summary>
        /// Номер акта
        /// </summary>
        public virtual string ActNumber { get; set; }

        /// <summary>
        /// Дата принятия акта
        /// </summary>
        public virtual DateTime ActDate { get; set; }

        /// <summary>
        /// Наименование акта
        /// </summary>
        public virtual string ActName { get; set; }

        /// <summary>
        /// Орган выдавший акт
        /// </summary>
        public virtual string ActDepartment { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// xml
        /// </summary>
        public virtual FileInfo AnswerFile { get; set; }
    }
}
