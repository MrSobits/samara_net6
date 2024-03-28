namespace Bars.Gkh.Entities.Licensing
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Licensing;

    /// <summary>
    /// Тип показателя
    /// </summary>
    public class GovernmenServiceDetailGroup : BaseImportableEntity
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="rowNumber">Номер строки</param>
        /// <param name="name">Наименование</param>
        /// <param name="serviceDetailSectionType">Раздел</param>
        /// <param name="groupName">Название группы (для отображения в fieldset)</param>
        public GovernmenServiceDetailGroup(int rowNumber, string name, ServiceDetailSectionType serviceDetailSectionType, string groupName = null)
        {
            this.Name = name;
            this.RowNumber = rowNumber;
            this.ServiceDetailSectionType = serviceDetailSectionType;
            this.GroupName = groupName;
        }

        /// <summary>
        /// .nh ctor
        /// </summary>
        protected GovernmenServiceDetailGroup()
        {
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public virtual int RowNumber { get; set; }

        /// <summary>
        /// Раздел
        /// </summary>
        public virtual ServiceDetailSectionType ServiceDetailSectionType { get; set; }

        /// <summary>
        /// Название группы (для отображения в fieldset)
        /// </summary>
        public virtual string GroupName { get; set; }
    }
}