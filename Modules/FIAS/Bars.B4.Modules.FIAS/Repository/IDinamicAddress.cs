namespace Bars.B4.Modules.FIAS
{
    /// <summary>
    /// Динамический адрес ФИАС
    /// </summary>
    public interface IDinamicAddress
    {
        /// <summary>
        /// Код
        /// </summary>
        string Code { get; set; }

        /// <summary>
        /// AOGuid записи ФИАС
        /// </summary>
        string GuidId { get; set; }

        string Name { get; set; }

        /// <summary>
        /// Наименование полного адреса
        /// </summary>
        string AddressName { get; set; }

        /// <summary>
        /// AOGuid всех составляющих данный адрес элементов
        /// </summary>
        string AddressGuid { get; set; }
        
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        string PostCode { get; set; }

        /// <summary>
        /// Уровень
        /// </summary>
        FiasLevelEnum Level { get; set; }

        /// <summary>
        /// AOGuid родительской записи
        /// </summary>
        string ParentGuidId { get; set; }

        /// <summary>
        /// Наименвоание родительской записи
        /// </summary>
        string ParentName { get; set; }

        /// <summary>
        /// Уровень родительской записи
        /// </summary>
        FiasLevelEnum ParentLevel { get; set; }

        /// <summary>
        /// AOguid зеркальной записи
        /// </summary>
        string MirrorGuid { get; set; }
    }
}