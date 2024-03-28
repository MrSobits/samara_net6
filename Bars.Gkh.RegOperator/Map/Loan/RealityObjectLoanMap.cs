namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Займ дома"</summary>
    public class RealityObjectLoanMap : BaseImportableEntityMap<RealityObjectLoan>
    {
        public RealityObjectLoanMap() : 
                base("Займ дома", "REGOP_RO_LOAN")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LoanTaker, "Объект недвижимости").Column("RO_ACC_ID").NotNull().Fetch();
            Reference(x => x.Source, "Источник займа (расчетный счет регопа)").Column("SOURCE_CALC_ACC_ID");
            Property(x => x.LoanDate, "Дата займа").Column("LOAN_DATE").NotNull();
            Property(x => x.OperationDate, "Дата операции внутри периода").Column("OP_DATE");
            Property(x => x.PlanLoanMonthCount, "Планируемая количество месяцев на возврат займа").Column("PLAN_MONTH_COUNT");
            Property(x => x.FactEndDate, "Фактическая дата возврата").Column("FACT_END_DATE");
            Reference(x => x.ProgramCr, "Программа кап.ремонта").Column("PROGRAM_CR_ID").NotNull().Fetch();
            Property(x => x.LoanSum, "Сумма займа").Column("LOAN_SUM").NotNull();
            Property(x => x.LoanReturnedSum, "Погашенная сумма займа").Column("LOAN_RETURNED_SUM").NotNull();
            Reference(x => x.Document, "Документ").Column("DOC_ID");
            Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUMBER");
            Property(x => x.CommonEstateObjectNames, "Список ООИ").Column("CEO_NAMES").Length(250);
            Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();
            Property(x => x.TransferGuid, "Уникальный идентификатор").Column("C_GUID").Length(40);
        }
    }

    public class RealityObjectLoanNHibernateMapping : ClassMapping<RealityObjectLoan>
    {
        public RealityObjectLoanNHibernateMapping()
        {
            Bag(
                x => x.Operations,
                mapper =>
                    {
                        mapper.Access(Accessor.NoSetter);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Lazy);
                        mapper.Key(
                            k =>
                                {
                                    k.Column("ORIGINATOR_GUID");
                                    k.PropertyRef(x => x.TransferGuid);
                                });
                        mapper.Cascade(Cascade.All);
                        mapper.Inverse(true);
                    },
                action => action.OneToMany());

            Bag(
                x => x.LoansFromWallets,
                mapper =>
                    {
                        mapper.Access(Accessor.NoSetter);
                        mapper.Fetch(CollectionFetchMode.Select);
                        mapper.Lazy(CollectionLazy.Lazy);
                        mapper.Key(k => k.Column("REGOP_RO_LOAN_ID"));
                        mapper.Cascade(Cascade.DeleteOrphans);
                        mapper.Inverse(true);
                    },
                action => action.OneToMany());
        }
    }
}
