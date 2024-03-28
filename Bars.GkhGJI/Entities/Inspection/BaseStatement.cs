namespace Bars.GkhGji.Entities
{
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Основание обращение граждан ГЖИ
    /// </summary>
    public class BaseStatement : InspectionGji
    {
        /// <summary>
        /// Форма проверки
        /// </summary>
        public virtual TypeFormInspection TypeForm { get; set; }

        /// <summary>
        /// Тип запроса
        /// </summary>
        public virtual BaseStatementRequestType RequestType { get; set; }
        
         /// <summary>
         /// Форма проверки
         /// </summary>
        public virtual FormCheck FormCheck { get; set; }
         
        /// <summary>
        /// ИНН физ./долж. лица
        /// </summary>
        public virtual string Inn { get; set; }

        public BaseStatement()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.RequestType = BaseStatementRequestType.AppealCits;
        }
    }
}