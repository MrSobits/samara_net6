namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using NHibernate;

    public partial class SubsidyMunicipalityService
    {
        private bool TryGetVersion(long municipalityId, out ProgramVersion version)
        {
            version = VersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == municipalityId);

            return version != null;
        }

        private bool TryGetSubsidy(long municipalityId, out SubsidyMunicipality subsidyMu)
        {
            subsidyMu = SubsidyMuService.GetAll().FirstOrDefault(x => x.Municipality.Id == municipalityId);

            return subsidyMu != null;
        }

        private static decimal GetSumSubsidy(SubsidyMunicipalityRecord subsidy)
        {
            return subsidy.BudgetRegion + subsidy.BudgetMunicipality + subsidy.BudgetFcr + subsidy.OwnerSource;
        }

        private IEnumerable<SubsidyMunicipalityRecord> GetSubsidyMuRecords(long subsidyMuId)
        {
            return SubsidyRecordService.GetAll()
                .Where(x => x.SubsidyMunicipality.Id == subsidyMuId)
                .AsEnumerable();
        }

        private void CorrectTypeResult(DpkrCorrectionStage2 record, Dictionary<long, int> shortRecordsDict, int startYear, int endYear)
        {
            /*
             * Тут идея такая что необходимо проставить записи корректировки нужный тип
               1. Тип = НоваяЗапись - проставляется в том случае если краткосрочки еще не было вообще
                        этот тип проставляется по умолчанию
                        (Поскольку поумочанию он уже проставлен с ним вообще не паримся и не проверяем на него)
             
               2. Тип = ЗаписьКраткосрочнойПрограммы - проставляется тогда когда запись существовала в периоде краткосрочки
                        и там же и осталась (То есть смещения не произошло, просто проверяем есть ли в краткосрочке такая запись)
             
               3. Тип = ЗаписьДолгосрочнойПрограммы - сначала думали, что это будет иметь смысл, но разницы между НоваяЗапись и этим типом никакого
                        Тоесть НоваяЗапись = ЗаписьДолгосрочнойпрограммы
               
               4. Тип = ЗаписьДобавляетсяВКраткосрочку - проставляется когда записи в краткосрочке не было, 
                        но в результате смещения года она вдруг попала в краткосрочный период
               
               5. Тип = ЗаписьУдаляетсяИзКраткосрочки -  проставляется когда запись была в краткосрочке, 
                        но в результате корректировки произошло смещение она ушла на год позже
             */

            //если год в пределах краткосрочной программы
            if (shortRecordsDict.ContainsKey(record.Stage2.Id))
            {
                var shortYear = shortRecordsDict[record.Stage2.Id];

                //Если год корректировки равен тому году краткосрочке где она уже есть то выставляем что она уже в краткосрочке
                if (record.PlanYear == shortYear)
                {
                    record.TypeResult = TypeResultCorrectionDpkr.InShortTerm;
                        // Это значит чт озапись никуда несдвинулась относительно краткосрочки 
                }
                else
                {
                    // Если год корректировки не равен тому году кратскосрочке где эта запись уже есть тогда говорим чтоона удалится
                    record.TypeResult = TypeResultCorrectionDpkr.RemoveFromShortTerm;
                        // Это значит что записи в этом году уже нет что она сдвинулась и что она удалится из краткосрочки
                }
            }
            else
            {
                // если записи невбло ни водной из краткосрочке но по скорректированному году она попадает в период актуализации тогда
                // выставляем записи чт оона добавляется к краткосрочку
                if (record.PlanYear >= startYear && record.PlanYear <= endYear)
                {
                    record.TypeResult = TypeResultCorrectionDpkr.AddInShortTerm;
                }
            }
        }

        private void SaveOrUpdate(IEnumerable<PersistentObject> entities, IDomainService domainService)
        {
            if (entities != null)
            {
                using (var tr = OpenTransaction())
                {
                    try
                    {
                        foreach (var obj in entities)
                        {
                            if (obj.Id > 0)
                                domainService.Update(obj);
                            else
                                domainService.Save(obj);
                        }

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
        }

        private void InTransaction(Action<IStatelessSession> action)
        {
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    action(session);

                    tr.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        tr.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format("Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        private IDataTransaction OpenTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }
    }
}