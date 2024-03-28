namespace Bars.Gkh.RegOperator.Entities.Owner
{
    /// <summary>
    /// Собственник юр. лицо в исковом заявлении
    /// </summary>
    public class LawsuitLegalOwnerInfo : LawsuitOwnerInfo
    {
        /// <summary>
        /// Наименование контрагента
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public virtual string Kpp { get; set; }
    }
}