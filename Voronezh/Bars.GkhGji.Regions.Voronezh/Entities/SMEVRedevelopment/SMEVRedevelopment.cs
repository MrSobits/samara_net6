namespace Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVRedevelopment : BaseEntity
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
        /// Наименование органа, выдавшего акт о завершении переустройства 
        /// </summary>
        public virtual string GovermentName { get; set; }

        /// <summary>
        /// дата акта
        /// </summary>
        public virtual DateTime? ActDate { get; set; }

        /// <summary>
        /// номер акта
        /// </summary>
        public virtual string ActNum { get; set; }

        /// <summary>
        /// имя объекта
        /// </summary>
        public virtual string ObjectName { get; set; }

        /// <summary>
        /// кадастровый
        /// </summary>
        public virtual string Cadastral { get; set; }

        /// <summary>
        /// RO
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

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
        /// 
        /// </summary>
        public virtual FileInfo AnswerFile { get; set; }

        /// <summary>
        /// муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
