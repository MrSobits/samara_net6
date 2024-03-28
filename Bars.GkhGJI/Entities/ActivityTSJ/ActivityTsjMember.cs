namespace Bars.GkhGji.Entities
{
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    public class ActivityTsjMember : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Деятельность ТСЖ
        /// </summary>
        public virtual ActivityTsj ActivityTsj { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Файл реестра
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }
    }
}