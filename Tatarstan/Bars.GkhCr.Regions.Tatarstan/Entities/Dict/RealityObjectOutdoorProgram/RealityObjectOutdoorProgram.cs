namespace Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Enums;

    public class RealityObjectOutdoorProgram : BaseEntity
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код.
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Период.
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Видимость.
        /// </summary>
        public virtual TypeVisibilityProgramCr TypeVisibilityProgram { get; set; }

        /// <summary>
        /// Государственный заказчик.
        /// </summary>
        public virtual Contragent GovernmentCustomer { get; set; }

        /// <summary>
        /// Состояние.
        /// </summary>
        public virtual TypeProgramStateCr TypeProgramState { get; set; }

        /// <summary>
        /// Тип КР.
        /// </summary>
        public virtual TypeProgramCr TypeProgram { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа.
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Орган, принявший документ.
        /// </summary>
        public virtual string DocumentDepartment { get; set; }

        /// <summary>
        /// Постановление об утверждении КП.
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }

        /// <summary>
        /// Файл.
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Не доступно добавление дворов.
        /// </summary>
        public virtual bool IsNotAddOutdoor { get; set; }

        /// <summary>
        /// Примечание.
        /// </summary>
        public virtual string Description { get; set; }
    }
}
