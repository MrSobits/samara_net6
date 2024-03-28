namespace Bars.Gkh.Entities.Dicts.Multipurpose
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Универсальный классификатор
    /// </summary>
    public class MultipurposeGlossary : BaseImportableEntity
    {
        private readonly List<MultipurposeGlossaryItem> items;

        public MultipurposeGlossary()
        {
            this.items = new List<MultipurposeGlossaryItem>();
            this.Items = this.items;
        }

        public MultipurposeGlossary(string code, string name)
            : this()
        {
            this.Name = name;
            this.Code = code;
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; protected set; }

        /// <summary>
        /// Записи в классификаторе
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<MultipurposeGlossaryItem> Items
        {
            get { return this.items; }
            protected set
            {
                this.items.Clear();
                if (value != null)
                {
                    this.items.AddRange(value);
                }
            }
        }

        /// <summary>
        /// Добавление записи в классификатор
        /// </summary>
        /// <param name="key">Ключ записи</param>
        /// <param name="value">Значение записи</param>
        public virtual void AddItem(string key, string value)
        {
            ArgumentChecker.NotNull(key, "key");
            ArgumentChecker.NotNull(value, "value");
            this.items.Add(new MultipurposeGlossaryItem(key, value, this));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public virtual string GetItemValue(string itemKey)
        {
            ArgumentChecker.NotNull(itemKey, "itemKey");
            return this.items.Single(x => itemKey.Equals(x.Key)).Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public virtual bool Contains(string itemKey)
        {
            ArgumentChecker.NotNull(itemKey, "itemKey");
            return this.items.Any(x => itemKey.Equals(x.Key));
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var that = obj as MultipurposeGlossary;
            if (that == null)
            {
                return false;
            }

            return this.Code.Equals(that.Code) && this.Name.Equals(that.Name);
        }

        public override int GetHashCode()
        {
            return (this.Code + this.Name).GetHashCode();
        }
    }
}