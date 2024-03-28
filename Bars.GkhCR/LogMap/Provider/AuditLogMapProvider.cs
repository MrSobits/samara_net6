namespace Bars.GkhCr.LogMap.Provider
{
    using Bars.B4.Modules.NHibernateChangeLog;
    using Entities;

    public class AuditLogMapProvider : IAuditLogMapProvider
    {
        public void Init(IAuditLogMapContainer container)
        {
            container.Add<BankStatementLogMap>();
            container.Add<BasePaymentOrderLogMap>();
            container.Add<EstimateLogMap>();
            container.Add<ObjectCrLogMap>();
            container.Add<PerformedWorkActLogMap>();
            container.Add<PerformedWorkActPaymentLogMap>();
            container.Add<ContractCrLogMap>();
            container.Add<BuildContractLogMap>();
            container.Add<FinanceSourceResourceLogMap>();
            container.Add<DefectListLogMap>();
            container.Add<TypeWorkCrLogMap>();
            container.Add<DocumentWorkCrLogMap>();
            container.Add<WorksCrInspectionLogMap>();
            container.Add<SpecialTypeWorkCrLogMap>();

#warning после Того как будет реализаована доработка модуля Логирования чтобы по одной сущности можно было создать несколько Лог мапов, то эти лог мапы раскомментирвоать из класса TypeWorkCrLogMap
            //container.Add<ScheduleExecutionWorkLogMap>();
            //container.Add<ProgressExecutionWorkLogMap>();
            //container.Add<WorkersCountLogMap>();
        }
    }
}