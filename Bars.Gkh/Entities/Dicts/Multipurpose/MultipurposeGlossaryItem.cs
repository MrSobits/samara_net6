namespace Bars.Gkh.Entities.Dicts.Multipurpose
{
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Наполнитель универсального справочника
    /// </summary>
    public class MultipurposeGlossaryItem : BaseImportableEntity
    {
        public MultipurposeGlossaryItem()
        {

        }

        public MultipurposeGlossaryItem(string key, string value, MultipurposeGlossary glossary)
        {
            ArgumentChecker.NotNull(key, "key");
            ArgumentChecker.NotNull(value, "value");
            ArgumentChecker.NotNull(glossary, "glossary");

            this.Key = key;
            this.Value = value;
            this.Glossary = glossary;
        }

        /// <summary>
        /// Ключ
        /// </summary>
        public virtual string Key { get; protected set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; protected set; }

        /// <summary>
        /// Справочник
        /// </summary>
        public virtual MultipurposeGlossary Glossary { get; protected set; }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var that = obj as MultipurposeGlossaryItem;
            if (that == null)
            {
                return false;
            }
            return this.Key.Equals(that.Key)
                && this.Value.Equals(that.Value)
                && this.Glossary.Equals(that.Glossary);
        }

        public override int GetHashCode()
        {
            return (this.Key + this.Value).GetHashCode() ^ this.Glossary.GetHashCode();
        }
    }
}