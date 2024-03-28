namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Количественный параметр очередности
    /// </summary>
    public class QuantPriorityParam : BaseImportableEntity
    {
        public virtual string Code { get; set; }

        public virtual string MinValue { get; set; }

        public virtual string MaxValue { get; set; }

        public virtual decimal Point { get; set; }
    }
}
