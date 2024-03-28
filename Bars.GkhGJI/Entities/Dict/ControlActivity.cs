namespace Bars.GkhGji.Entities.Dict
{
    using Bars.Gkh.Entities;

    public class ControlActivity : BaseGkhEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// ЕРКНМ ГУИД
        /// </summary>
        public virtual string ERKNMGuid { get; set; }
    }
}
