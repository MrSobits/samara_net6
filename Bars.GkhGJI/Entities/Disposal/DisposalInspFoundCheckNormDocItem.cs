namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Требования НПА проверки
    /// </summary>
    public class DisposalInspFoundCheckNormDocItem : BaseGkhEntity
    {
        /// <summary>
        /// НПА проверки приказа ГЖИ 
        /// </summary>
        public virtual DisposalInspFoundationCheck DisposalInspFoundationCheck { get; set; }

        /// <summary>
        /// Пункт нормативного документа
        /// </summary>
        public virtual NormativeDocItem NormativeDocItem { get; set; }

        /// <summary>
        /// Распоряжение ГЖИ
        /// </summary>
        public virtual Disposal Disposal { get; set; }
    }
}