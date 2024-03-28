/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
///     using NHibernate.Mapping.ByCode;
/// 
///     public class PersonalAccountOwnerMap : BaseImportableEntityMap<PersonalAccountOwner>
///     {
///         public PersonalAccountOwnerMap() : base("REGOP_PERS_ACC_OWNER")
///         {
///             Map(x => x.Name, "NAME", false, 500);
///             Map(x => x.OwnerType, "OWNER_TYPE", true);
///             Map(x => x.BillingAddressType, "BILLING_ADDRESS_TYPE", true);
///             References(x => x.PrivilegedCategory, "PRIVILEGED_CATEGORY");
/// 
///             Bag(x => x.Accounts, mapper =>
///             {
///                 mapper.Access(Accessor.NoSetter);
///                 mapper.Fetch(CollectionFetchMode.Select);
///                 mapper.Lazy(CollectionLazy.Lazy);
///                 mapper.Key(x => x.Column("ACC_OWNER_ID"));
///                 mapper.Cascade(Cascade.All);
///             }, action => action.OneToMany());
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Абонент"</summary>
    public class PersonalAccountOwnerMap : BaseImportableEntityMap<PersonalAccountOwner>
    {
        
        public PersonalAccountOwnerMap() : 
                base("Абонент", "REGOP_PERS_ACC_OWNER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Наименование").Column("NAME").Length(500);
            Property(x => x.OwnerType, "Тип абонента").Column("OWNER_TYPE").NotNull();
            Reference(x => x.PrivilegedCategory, "Льготная категория").Column("PRIVILEGED_CATEGORY");
            Property(x => x.BillingAddressType, "Какой адрес использовать для отправки корреспонденции").Column("BILLING_ADDRESS_TYPE").NotNull();
            Property(x => x.TotalAccountsCount, "Количество ЛС (всего)").Column("TOTAL_ACCOUNTS_COUNT").NotNull();
            Property(x => x.ActiveAccountsCount, "Количество ЛС (открытые)").Column("ACTIVE_ACCOUNTS_COUNT").NotNull();
            this.Reference(x => x.FactAddrDoc, "Документ - основание фактического адреса").Column("FACT_ADDR_DOC").Fetch();
        }
    }

    public class PersonalAccountOwnerNHibernateMapping : ClassMapping<PersonalAccountOwner>
    {
        public PersonalAccountOwnerNHibernateMapping()
        {
            Bag(
                x => x.Accounts,
                mapper =>
                    {
                        mapper.Access(Accessor.NoSetter);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Lazy);
                        mapper.Key(x => x.Column("ACC_OWNER_ID"));
                        mapper.Cascade(Cascade.None);
                    },
                action => action.OneToMany());
        }
    }
}
