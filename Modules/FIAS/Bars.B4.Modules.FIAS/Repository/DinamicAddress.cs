namespace Bars.B4.Modules.FIAS
{
    /// <summary>
    /// Динамический адрес ФИАС
    /// </summary>
    public class DinamicAddress : IDinamicAddress
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// AOGuid записи ФИАС
        /// </summary>
        public string GuidId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Наименование полного адреса
        /// </summary>
        public string AddressName { get; set; }

        /// <summary>
        /// AOGuid всех составляющих данный адрес элементов
        /// </summary>
        public string AddressGuid { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Уровень
        /// </summary>
        public FiasLevelEnum Level { get; set; }

        /// <summary>
        /// AOGuid родительской записи
        /// </summary>
        public string ParentGuidId { get; set; }

        /// <summary>
        /// Наименвоание родительской записи
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Уровень родительской записи
        /// </summary>
        public FiasLevelEnum ParentLevel { get; set; }

        /// <summary>
        /// AOguid зеркальной записи
        /// </summary>
        public string MirrorGuid { get; set; }
    }
}