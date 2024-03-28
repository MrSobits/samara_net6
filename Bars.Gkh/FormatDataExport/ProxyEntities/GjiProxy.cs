namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using Bars.B4.DataModels;

    /// <summary>
    /// Контролирующий орган
    /// </summary>
    public class GjiProxy : IHaveId
    {
        /// <inheritdoc />
        public long Id { get; set; }

        /// <summary>
        /// 2. Вид контролирующего органа
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 3. Идентификатор контролирующего органа в системе «Федеральный реестр государственных и муниципальных услуг»
        /// </summary>
        public string FrguId { get; set; }
    }
}