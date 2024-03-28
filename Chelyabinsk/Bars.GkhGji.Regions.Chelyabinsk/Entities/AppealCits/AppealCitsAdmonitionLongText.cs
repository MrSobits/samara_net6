namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    ///Предостережение текстовки для печатки
    /// </summary>
    public class AppealCitsAdmonitionLongText : BaseGkhEntity
    {
        /// <summary>
        /// Предостережение
        /// </summary>
        public virtual AppealCitsAdmonition AppealCitsAdmonition { get; set; }

        public virtual byte[] Violations { get; set; }

        public virtual byte[] Measures { get; set; }
    }
}