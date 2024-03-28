namespace Bars.GkhGji.Entities
{
    /// <summary>
    /// Этап устранения нарушения в акте проверки
    /// Данная таблица служит связтю между нарушением и Актом устранения
    /// Чтобы понимать какие Нарушения были устранены входе проверки
    /// Для устраненных нарушений ставится фактическая дата устранения
    /// </summary>
    public class ActRemovalViolation : InspectionGjiViolStage
    {
        /* /// <summary>
-        /// Нехранимое поле срок устранения, для обновления нарушения проверки
-        /// </summary>
-        public virtual DateTime? DatePlanRemoval { get; set; }*/

        /// <summary>
        /// Описание обстоятельств
        /// </summary>
        public virtual string CircumstancesDescription { get; set; }
    }
}