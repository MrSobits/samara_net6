namespace Bars.Gkh1468
{
    using B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.Gkh1468.Entities;

    public class BaseProviderPassportRow : BaseEntity
    {
        public virtual string Value { get; set; }

        public virtual MetaAttribute MetaAttribute { get; set; }

        public virtual int GroupKey { get; set; }

        /// <summary>
        /// Ссылка на родительскую запись
        /// </summary>
        public virtual long? ParentValue { get; set; }

        /// <summary>Не использовать в запросах к БД</summary>
        public virtual BaseProviderPassport Passport { get; set; }


        public override string ToString()
        {
            return string.Format("Meta: {0}, Value: {1}", MetaAttribute.Return(x => x.Name), Value);
        }
    }

    public class BaseProviderPassportRow<T> : BaseProviderPassportRow where T : BaseProviderPassport
    {
        public virtual T ProviderPassport { get; set; }

        /// <summary>Не использовать в запросах к БД</summary>
        public override BaseProviderPassport Passport
        {
            get { return ProviderPassport; }
            set { ProviderPassport = (T)value; }
        }
    }
}