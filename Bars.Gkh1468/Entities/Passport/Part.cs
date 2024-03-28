using System.Linq;
using Bars.B4.Utils;

namespace Bars.Gkh1468.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Раздел структуры паспорта
    /// </summary>
    public class Part : BaseGkhEntity
    {
        /// <summary>
        /// Структура паспорта, которому принадлежит раздел
        /// </summary>
        public virtual PassportStruct Struct { get; set; }

        /// <summary>Номер для сортировки, берется последнее целое число из свойства Code</summary>
        public virtual int SortNumber
        {
            get { return string.IsNullOrEmpty(Code) ? 0 : Code.Replace(',', '.').TrimEnd('.').Split('.').Last().ToInt(); }
        }

        /// <summary>
        /// Код раздела
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Порядок, задаваемый вручную
        /// </summary>
        public virtual int OrderNum { get; set; }

        /// <summary>
        /// Наименование раздела
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Родительский раздел
        /// </summary>
        public virtual Part Parent { get; set; }

        /// <summary>
        /// Заполняется УО
        /// </summary>
        public virtual bool Uo { get; set; }

        /// <summary>
        /// Заполняется ПКУ
        /// </summary>
        public virtual bool Pku { get; set; }

        /// <summary>
        /// Заполняется ПР
        /// </summary>
        public virtual bool Pr { get; set; }

        /// <summary>
        /// Код интеграции
        /// </summary>
        public virtual string IntegrationCode { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Parent: {1}", Name, Parent.Return(x => x.Name));
        }

        public virtual Part Clone()
        {
            return MemberwiseClone() as Part;
        }
    }
}
