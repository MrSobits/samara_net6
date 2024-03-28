namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.DomainService.PersonalAccount.Debtor.Impl
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Impl;
    using Debtor;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.Debtors;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using NHibernate.Linq;

    using Dapper;
    using Gkh.Enums;
    using System;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor;
    using Bars.GkhCr.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Modules.ClaimWork.Entity;

    /// <summary>
    /// Сервис по должникам
    /// </summary>
    public class DebtorService : IDebtorService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Домен-сервис Объект капитального ремонта <see cref="ObjectCr"/>
        /// </summary>
        public IDomainService<ObjectCr> ObjectCrDomainService { get; set; }

        /// <summary>
        /// Формирование реестра должников
        /// </summary>
        /// <param name="baseParams"> Базовый параметр </param>
        /// <returns></returns>
        public IDataResult Create(BaseParams baseParams)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var session = sessionProvider.GetCurrentSession();
            var query1 = session.CreateSQLQuery(@"delete from regop_debtor;
        drop table if exists tmpcharges2;
        create table tmpcharges2 as
        select ac.id account_id, penalty_debt, saldo_out debt_sum, base_tariff_debt debt_base_tariff_sum, round(saldo_out/(10*area_share*gr.carea),0)*30 days_count, round(saldo_out/(10*area_share*gr.carea),0) month_count, 10 court_type, 0 debt_decision_tariff_sum,
        case when t1.pers_acc_id >0 then 10 else 20 end PROCESSED_BY_AGENT
        from regop_pers_acc_period_summ psum 
        join regop_pers_acc ac on ac.id = psum.account_id --and ac.id = 619432
        join regop_period rp on rp.id = period_id and cis_closed = false
        join gkh_room gr on gr.id = room_id
        join gkh_reality_object gro on gro.id = gr.ro_id
        join gkh_dict_municipality gdm on gdm.id = gro.municipality_id
        join b4_state st on st.id = ac.state_id and st.start_state
        left join (select distinct AGENT_PIR_DEBTOR_PA_ID pers_acc_id from AGENT_PIR_DEBTOR) t1 on t1.pers_acc_id = ac.id
        where gr.carea >0 and area_share>0 and (base_tariff_debt - tariff_payment)>(10*2*area_share*gr.carea) and (base_tariff_debt - tariff_payment)> 500 
        and installmentplan = false and is_not_debtor = false;");

            var query1_2 = session.CreateSQLQuery(@"drop table if exists tmp_lastdoc;
        create table tmp_lastdoc as
        select cad.account_id, max(cd.id) maxdoc from clw_claim_work_acc_detail cad 
        join clw_document cd on cd.CLAIMWORK_ID = cad.claim_work_id group by 1
        ");

            var query2 = session.CreateSQLQuery(@"drop table if exists tmppir;
        create table tmppir as
        select distinct cad.account_id, max(l.id), l.DEBT_START_DATE, l.DEBT_END_DATE, l.DEBT_BASE_TARIFF_SUM, cad.claim_work_id from clw_claim_work_acc_detail cad 
        join clw_document cd on cd.CLAIMWORK_ID = cad.claim_work_id
        join tmp_lastdoc ld on ld.maxdoc = cd.id
        join CLW_LAWSUIT l on l.id = cd.id
        where DEBT_START_DATE is not null and DEBT_END_DATE is not null
        group by 1,3,4,5,6;");

            var query3 = session.CreateSQLQuery(@"drop table if exists tmppirpayments;
        create table tmppirpayments as
        select account_id, sum(t.amount) paymentssum from regop_transfer t
        join regop_money_operation o on o.id = t.op_id and is_cancelled = false
        join regop_period p on p.id = t.period_id
        join tmppir tp on tp.account_id = t.owner_id and DEBT_END_DATE < p.cstart
        group by 1;");
            var query4 = session.CreateSQLQuery(@"drop table if exists debtordata;
        create table debtordata as
        select 0 object_version, current_date object_create_date, current_date object_edit_date, t2.account_id, penalty_debt,debt_sum, days_count, current_date - interval '1' day*days_count start_date, 
        month_count, court_type, t2.debt_base_tariff_sum, debt_decision_tariff_sum,PROCESSED_BY_AGENT,
        coalesce(p.paymentssum,0) PAYMENTS_SUM, t.DEBT_START_DATE, t.DEBT_END_DATE, t.DEBT_BASE_TARIFF_SUM LASTCLW_DEBT_SUM, claim_work_id
        from tmpcharges2 t2
        left join tmppirpayments p on p.account_id = t2.account_id
        left join tmppir t on t.account_id = t2.account_id;");

            var query5 = session.CreateSQLQuery(@"INSERT INTO public.regop_debtor(
            object_version, object_create_date, object_edit_date, account_id, 
            penalty_debt, debt_sum, days_count, start_date, month_count, 
            court_type, debt_base_tariff_sum, debt_decision_tariff_sum, PROCESSED_BY_AGENT, claim_work_id, LASTCLW_DEBT_SUM, PAYMENTS_SUM, NEW_CLAIM_DEBT, LAST_DEBT_PERIOD)
    select 0, current_date, current_date, account_id, penalty_debt,debt_sum, days_count, current_date - interval '1' day*days_count start_date, month_count, court_type, debt_base_tariff_sum, debt_decision_tariff_sum,PROCESSED_BY_AGENT,
    claim_work_id, coalesce(LASTCLW_DEBT_SUM,0), coalesce(PAYMENTS_SUM,0), (coalesce(debt_base_tariff_sum,0)-coalesce(LASTCLW_DEBT_SUM,0) + coalesce(PAYMENTS_SUM,0)), 
    case when DEBT_START_DATE is not null then concat(to_char(DEBT_START_DATE, 'dd.MM.yyyy'), '-',to_char(DEBT_END_DATE, 'dd.MM.yyyy')) else '' end
    from debtordata;");
            var query6 = session.CreateSQLQuery(@"update regop_debtor set NEW_CLAIM_DEBT = debt_base_tariff_sum where NEW_CLAIM_DEBT>debt_base_tariff_sum;");

            query1.ExecuteUpdate();
         query1_2.ExecuteUpdate();
            query2.ExecuteUpdate();
            query3.ExecuteUpdate();
            query4.ExecuteUpdate();
            query5.ExecuteUpdate();
            query6.ExecuteUpdate();
            return new BaseDataResult();

            //var taskEntryDomain = this.Container.ResolveDomain<TaskEntry>();
            //try
            //{
            //    if (taskEntryDomain.GetAll()
            //        .Any(
            //            x => x.Status != TaskStatus.Error
            //                && x.Status != TaskStatus.Succeeded
            //                && x.Parent.TaskCode == DebtorsTaskProvider.Code))
            //    {
            //        throw new ValidationException("Формирование реестра неплательщиков уже запущено");
            //    }

            //    return this.TaskManager.CreateTasks(new DebtorsTaskProvider(), baseParams);
            //}
            //finally
            //{
            //    this.Container.Release(taskEntryDomain);
            //}
        }

        /// <summary>
        /// Удаление неплательщиков
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <returns></returns>
        public IDataResult Clear(BaseParams baseParams)
        {
            var sessions = this.Container.Resolve<ISessionProvider>();

            using (this.Container.Using(sessions))
            {
                sessions.GetCurrentSession().CreateQuery("delete from Debtor").ExecuteUpdate();
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <param name="paging">Постраничная навигация</param>
        /// <param name="totalCount">Общее количество записей</param>
        /// <returns></returns>
        public IList GetList(BaseParams baseParams, bool paging, out int totalCount)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var claimWorkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            try
            {
                var loadParam = baseParams.GetLoadParam();

                var filter = new BaseParamsDebtorFilter(baseParams);

                filter.RealityObject = this.GetRealityObjectIdByProgramCr(filter.ProgramCrIds);

                var data = this.FilterByBaseParams(this.GetSelectDebtor(), filter)
                    .Filter(loadParam, this.Container);

                totalCount = data.Count();

                data = paging ? data.Order(loadParam).Paging(loadParam) : data.Order(loadParam);

                return data.ToList();
            }
            finally
            {
                this.Container.Release(debtorDomain);
                this.Container.Release(claimWorkAccountDetailDomain);
            }
        }

        private IQueryable<DebtorDto> FilterByBaseParams(IQueryable<DebtorDto> debtorDtoQuery, BaseParamsDebtorFilter filter)
        {
            return debtorDtoQuery
                .WhereIf(filter.MuncipalityIds.Length > 0, x => filter.MuncipalityIds.Contains(x.MunicipalityId))
                .WhereIf(filter.OwnerIds.Length > 0, x => filter.OwnerIds.Contains(x.AccountOwnerId))
                .WhereIf(filter.StateIds.Length > 0, x => filter.StateIds.Contains(x.State.Id))
                .WhereIf(filter.RealityObject.Length > 0, x => filter.RealityObject.Contains(x.RealityObjectId));
        }

        /// <summary>
        /// Список
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <param name="paging">Постраничная навигация</param>
        /// <param name="totalCount">Общее количество записей</param>
        /// <returns></returns>
        public IQueryable<ViewDebtorExport> GetListQuery(BaseParams baseParams, out int totalCount)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var claimWorkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            try
            {
                var loadParam = baseParams.GetLoadParam();

                var filter = new BaseParamsDebtorFilter(baseParams);

                filter.RealityObject = this.GetRealityObjectIdByProgramCr(filter.ProgramCrIds);

                var data = this.FilterByBaseParams(this.GetSelectDebtor(), filter)
                    .Filter(loadParam, this.Container);

                totalCount = data.Count();

                return data.OrderBy(x => x.Id);
            }
            finally
            {
                this.Container.Release(debtorDomain);
                this.Container.Release(claimWorkAccountDetailDomain);
            }
        }

        private IQueryable<DebtorDto> GetSelectDebtorDto(IQueryable<Debtor> debtorQuery, IQueryable<ClaimWorkAccountDetail> claimWorkAccountDetailQuery)
        {
            var persAccSerivce = Container.ResolveDomain<IndividualAccountOwner>();
            return debtorQuery.Select(
                 x => new DebtorDto
                 {
                     Id = x.Id,
                     PersonalAccountId = x.PersonalAccount.Id,
                     MunicipalityId = x.PersonalAccount.Room.RealityObject.Municipality.Id,
                     Municipality = x.PersonalAccount.Room.RealityObject.Municipality.Name,
                     Settlement = x.PersonalAccount.Room.RealityObject.MoSettlement.Name,
                     RoomAddress = x.PersonalAccount.Room.RealityObject.Address + ", кв. " + x.PersonalAccount.Room.RoomNum,
                     State = x.PersonalAccount.State,
                     PersonalAccountNum = x.PersonalAccount.PersonalAccountNum,
                     AccountOwnerId = x.PersonalAccount.AccountOwner.Id,
                     AccountOwner = x.PersonalAccount.AccountOwner.OwnerType == PersonalAccountOwnerType.Legal
                         ? (x.PersonalAccount.AccountOwner as LegalAccountOwner).Contragent.Name
                         : x.PersonalAccount.AccountOwner.Name,
                     OwnerType = x.PersonalAccount.AccountOwner.OwnerType,
                     DebtSum = x.DebtSum,
                     DebtBaseTariffSum = x.DebtBaseTariffSum,
                     DebtDecisionTariffSum = x.DebtDecisionTariffSum,
                     ExpirationDaysCount = x.ExpirationDaysCount,
                     ExpirationMonthCount = x.ExpirationMonthCount,
                     PenaltyDebt = x.PenaltyDebt,
                     HasClaimWork = claimWorkAccountDetailQuery.Any(y => y.PersonalAccount.Id == x.PersonalAccount.Id && !y.ClaimWork.State.FinalState),
                     CourtType = x.CourtType,
                     JurInstitution = x.JurInstitution.ShortName,
                     UserName = claimWorkAccountDetailQuery.Where(y => y.PersonalAccount.Id == x.PersonalAccount.Id && !y.ClaimWork.State.FinalState).Select(y => ((DebtorClaimWork)y.ClaimWork).User.Name).FirstOrDefault() ?? string.Empty,
                     RealityObjectId = x.PersonalAccount.Room.RealityObject.Id,
                     ExtractExists = x.ExtractExists,
                     ExtractDate = x.ExtractDate,
                     AccountRosregMatched = x.AccountRosregMatched,
                     Underage = ((x.PersonalAccount.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                                && (persAccSerivce.Get(x.PersonalAccount.AccountOwner.Id) != null) && (persAccSerivce.Get(x.PersonalAccount.AccountOwner.Id).BirthDate != null))
                                ? (DateTime.Now - persAccSerivce.Get(x.PersonalAccount.AccountOwner.Id).BirthDate) < TimeSpan.FromDays(6574) //365 дней * 18 лет + 4 дня на високосные года
                                : false,
                     OwnerArea = x.PersonalAccount.Room.Area * x.PersonalAccount.AreaShare,
                     RoomArea = x.PersonalAccount.Room.Area,
                     ProcessedByTheAgent = x.ProcessedByTheAgent,
                     Separate = ((x.PersonalAccount.Room.Area * x.PersonalAccount.AreaShare) != x.PersonalAccount.Room.Area) ? YesNo.Yes : YesNo.No,
                     ClaimworkId = x.ClaimworkId,
                     LastClwDebt = x.LastClwDebt,
                     PaymentsSum = x.PaymentsSum,
                     MewClaimDebt = x.MewClaimDebt,
                     LastPirPeriod = x.LastPirPeriod
                 });
        }

        private class DebtorDto
        {
            public long Id { get; set; }
            public long? PersonalAccountId { get; set; }
            public long MunicipalityId { get; set; }
            public string Municipality { get; set; }
            public string Settlement { get; set; }
            public string RoomAddress { get; set; }
            public State State { get; set; }
            public string PersonalAccountNum { get; set; }
            public long AccountOwnerId { get; set; }
            public string AccountOwner { get; set; }
            public PersonalAccountOwnerType OwnerType { get; set; }
            public decimal DebtSum { get; set; }
            public decimal DebtBaseTariffSum { get; set; }
            public decimal DebtDecisionTariffSum { get; set; }
            public int ExpirationDaysCount { get; set; }
            public int? ExpirationMonthCount { get; set; }
            public decimal PenaltyDebt { get; set; }
            public bool HasClaimWork { get; set; }
            public Gkh.Enums.ClaimWork.CourtType CourtType { get; set; }
            public string JurInstitution { get; set; }
            public string UserName { get; set; }
            public long RealityObjectId { get; set; }
            public string ProgramCr { get; set; }
            public decimal OwnerArea { get; set; }
            public bool Underage { get; set; }
            public YesNo? ExtractExists { get; set; }
            public DateTime? ExtractDate { get; set; }
            public YesNo? AccountRosregMatched { get; set; }
            public YesNo? ProcessedByTheAgent { get; set; }
            public decimal RoomArea { get; set; }
            public YesNo Separate { get; set; }
            public long? ClaimworkId { get; set; }
            public decimal LastClwDebt { get; set; }
            public decimal PaymentsSum { get; set; }
            public decimal MewClaimDebt { get; set; }
            public string LastPirPeriod { get; set; }
        }

        /// <summary>
        /// Создание работы
        /// </summary>
        /// <param name="baseParams">Базовый параметр</param>
        /// <returns></returns>
        public IDataResult CreateClaimWorks(BaseParams baseParams)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var claimWorkAccountDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var debtorClaimWorkUpdateService = this.Container.Resolve<IDebtorClaimWorkUpdateService>();

            try
            {
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
                //Если нет отмеченых должников в списке, смотрим на массив AccNum
                if (ids.Length == 0 || ids == null)
                {
                    var acc_nums_temp = baseParams.Params.GetAs<string>("AccNum");

                    var ids_from_acc_num = GetIdsFromAccNum(acc_nums_temp);

                    //Роняем задачу при кривом вводе т.к. пустой массив id запустит формирование ПИР по всему реестру должников
                    if ((acc_nums_temp != null || acc_nums_temp != "") && (ids_from_acc_num.Length == 0))
                    {
                        return new BaseDataResult(false, "Строка со списком ЛС была пустая, либо по всем указанным лс нет записей в реестре должников");
                    }

                    //Передаем id, если найдены
                    if (ids.Length == 0 && ids_from_acc_num.Length > 0)
                    {
                        ids = ids_from_acc_num;
                    }
                }
                

                var loadParams = baseParams.GetLoadParam();

                var filter = new BaseParamsDebtorFilter(baseParams);

                filter.RealityObject = this.GetRealityObjectIdByProgramCr(filter.ProgramCrIds);

                var filterExist = (loadParams.DataFilter != null
                        && (!string.IsNullOrEmpty(loadParams.DataFilter.DataIndex) || loadParams.DataFilter.Filters.IsNotEmpty())
                        || loadParams.Filter.IsNotEmpty())
                    || filter.HasFilter;

                IQueryable<BasePersonalAccount> persAccQuery;

                if (filterExist && ids.IsEmpty())
                {
                    var debtorList = this.FilterByBaseParams(GetSelectDebtor(), filter)
                        .WhereIf(ids.IsNotEmpty(), x => ids.Contains(x.Id))
                        .Filter(loadParams, this.Container)
                        .Where(x => x.PersonalAccountId.HasValue)
                        .Select(x => x.PersonalAccountId.Value)
                        .AsEnumerable()
                        .Distinct()
                        .ToList();

                    persAccQuery = persAccDomain.GetAll()
                        .WhereContainsBulked(x => x.Id, debtorList);
                }
                else
                {
                    persAccQuery = debtorDomain.GetAll()
                        .WhereIf(ids.IsNotEmpty(), x => ids.Contains(x.Id))
                        .Select(x => new BasePersonalAccount
                        {
                         Id = x.PersonalAccount.Id
                        });
                }

                return debtorClaimWorkUpdateService.CreateClaimWorks(persAccQuery);
            }
            finally
            {
                this.Container.Release(claimWorkAccountDetailDomain);
                this.Container.Release(debtorClaimWorkUpdateService);
                this.Container.Release(debtorDomain);
                this.Container.Release(persAccDomain);
            }
        }

        public IDomainService<Lawsuit> LawsuitDomain { get; set; }
        public IDomainService<ClaimWorkAccountDetail> AccountDetailDomain { get; set; }

        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        public Bars.Gkh.DataResult.ListDataResult<DebtorPaymentsDetail> GetPaymentsOperationDetail(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var docId = baseParams.Params.GetAs<long>("docId");
            if (docId == 0)
            {
                return new Bars.Gkh.DataResult.ListDataResult<DebtorPaymentsDetail>();
            }
            Lawsuit lawsuit = this.LawsuitDomain.Get(docId);
            if (!lawsuit.DateLimitationOfActions.HasValue)
            {
                return new Bars.Gkh.DataResult.ListDataResult<DebtorPaymentsDetail>();
            }
            List<long> claimWorkAccountDetail = this.AccountDetailDomain.GetAll()
             .Where(x => x.ClaimWork == lawsuit.ClaimWork)
             .Select(x=> x.PersonalAccount.Id)
             .ToList();
            List<DebtorPaymentsDetail> result = new List<DebtorPaymentsDetail>();
            foreach (long acId in claimWorkAccountDetail)
            {
                //тест
                //result.Add(new DebtorPaymentsDetail
                //{
                //    TransferId = 23433,
                //    Amount = 100,
                //    Date = DateTime.Now,
                //    Name = "Оплата по базовому тарифу",
                //    PaymentSource = "Реестр платежного агента",
                //    Period = "Июль"
                //});

                //result.Add(new DebtorPaymentsDetail
                //{
                //    TransferId = 23436,
                //    Amount = 130,
                //    Date = DateTime.Now.AddMonths(-2),
                //    Name = "Оплата по базовому тарифу",
                //    PaymentSource = "УФССП",
                //    Period = "Май"
                //});
                //
                var persAccAllTransfers = this.Container.Resolve<IDomainService<PersonalAccountPaymentTransfer>>().GetAll()
                 .Where(x => x.Owner.Id == acId && x.PaymentDate > lawsuit.DateLimitationOfActions.Value)
                 .Where(x => x.Operation.IsCancelled != true)
                 .OrderBy(x => x.PaymentDate)
                 .Select(x => new DebtorPaymentsDto
                 {
                     Id = x.Id,
                     PaymentDate = x.PaymentDate,
                     Reason = x.Reason ?? x.Operation.Reason,
                     Amount = x.Amount,
                     Name = x.ChargePeriod.Name,
                     OriginatorGuid = x.Operation.OriginatorGuid
                 }).ToList();
                result.AddRange(persAccAllTransfers.Select(x => new DebtorPaymentsDetail
                {
                    TransferId = x.Id,
                    Date = x.PaymentDate,
                    Name = x.Reason,
                    Amount = x.Amount,
                    Period = x.Name,
                    PaymentSource = GetSource(x.OriginatorGuid)
                }));

            }   
            var totalCount = result.Count();

            return new Bars.Gkh.DataResult.ListDataResult<DebtorPaymentsDetail>(result.ToList(), totalCount);
        }

        private class DebtorPaymentsDto
        {
            public long Id { get; set; }
            public DateTime PaymentDate { get; set; }
            public string Reason { get; set; }
            public decimal Amount { get; set; }
            public string Name { get; set; }
            public string OriginatorGuid { get; set; }
        }


            private string GetSource(string op_guid)
        {
            var moneyOperationDomain = this.Container.ResolveDomain<MoneyOperation>();
            var bankDocImportDomain = this.Container.ResolveDomain<BankDocumentImport>();
            var bankAccStDomain = this.Container.ResolveDomain<BankAccountStatement>();
            var importedDomain = this.Container.ResolveDomain<ImportedPayment>();
            try
            {
                var bankDocumentImport = bankDocImportDomain.GetAll().FirstOrDefault(x => x.TransferGuid == op_guid);
                if (bankDocumentImport != null && bankDocumentImport.DocumentDate.HasValue)
                {
                    return $"Реестр банка {bankDocumentImport.DocumentNumber} от {bankDocumentImport.DocumentDate.Value.ToString("dd.MM.yyyy")}";
                }
                var bankAccSt = bankAccStDomain.GetAll().FirstOrDefault(x => x.TransferGuid == op_guid);
                if (bankAccSt != null)
                {
                    return bankAccSt.PaymentDetails;
                }
                return "Не установлен";
            }
            catch
            {
                return "";
            }
            finally
            {
                Container.Release(moneyOperationDomain);
                Container.Release(bankDocImportDomain);
                Container.Release(bankAccStDomain);
                Container.Release(importedDomain);
            }
        }

        private long[] GetIdsFromAccNum(string accNumString)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            long[] res = new long[0];
            try
            {
                //Чистим строку от пробелов и разбиваем по запятым
                string trimmed = new string(accNumString.ToCharArray()
                                .Where(c => !Char.IsWhiteSpace(c))
                                .ToArray());
                string[] accNums = trimmed.Split(',');

                //var accDict = persAccDomain.GetAll().Select(x => new { x.Id, x.PersonalAccountNum }).ToDictionary(x=>x.PersonalAccountNum,x=>x.Id);

                var persAccIds = persAccDomain.GetAll().Where(x => accNums.Contains(x.PersonalAccountNum)).Select(x => x.Id);

                List<long> result = new List<long>();
                debtorDomain.GetAll().Where(x => persAccIds.Contains(x.PersonalAccount.Id)).Select(x => x.Id).ForEach(x => result.Add(x));
                res = result.ToArray();
            }
            catch
            {
                //Catch&Ignore
            }
            finally
            {
                Container.Release(debtorDomain);
                Container.Release(persAccDomain);
            }
            return res;
        }

        /// <summary>
        /// Обновление учреждения в судебной практике
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public IDataResult UpdateJurInstitution(BaseParams baseParams)
        {
            var debtorDomain = this.Container.ResolveDomain<Debtor>();
            var debtorJurInstitutionCache = this.Container.Resolve<IDebtorJurInstitutionCache>();

            try
            {
                var debtorsCount = debtorDomain.GetAll().Count();

                var step = 10000;

                for (int startIndex = 0; startIndex <= debtorsCount; startIndex += step)
                {
                    var debtors = debtorDomain.GetAll()
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.Room)
                        .ThenFetch(x => x.RealityObject)
                        .OrderBy(x => x.Id)
                        .Skip(startIndex)
                        .Take(step);

                    debtorJurInstitutionCache.InitCache(debtors.Select(x => x.PersonalAccount.Room.RealityObject.Id).ToArray());

                    var listToSave = new List<Debtor>();

                    foreach (var debtor in debtors)
                    {
                        if (debtorJurInstitutionCache.SetJurInstitution(debtor, debtor.PersonalAccount))
                        {
                            listToSave.Add(debtor);
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(this.Container, listToSave, listToSave.Count, true, true);
                }
            }
            finally
            {
                this.Container.Release(debtorDomain);
                this.Container.Release(debtorJurInstitutionCache);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Получаем список жилых домов отфильтрованных по МО и программе КР
        /// </summary>
        /// <param name="muncipalityIds">список "Муниципальное образование"</param>
        /// <param name="programCrIds">список "Программа капитального ремонта"</param>
        /// <returns>Список ID Жилых домов</returns>
        private long[] GetRealityObjectIdByProgramCr(long[] programCrIds)
        {
            if (programCrIds.Length > 0)
            {
                return this.ObjectCrDomainService.GetAll()
                    .Where(x => x.ProgramCr != null)
                    .Where(x => x.RealityObject != null)
                    .WhereNotNull(x => x.RealityObject.Municipality)
                    .WhereIf(programCrIds.IsNotEmpty(), x => programCrIds.Contains(x.ProgramCr.Id))
                    .Select(x => x.RealityObject.Id)
                    .ToArray();
            }
                
            return new long[0];
        }

        private IQueryable<ViewDebtorExport> GetSelectDebtor()
        {
            var viewDebtorsDomain = this.Container.ResolveDomain<ViewDebtorExport>();

            var data = viewDebtorsDomain.GetAll();

            return data;
        }

        private IQueryable<ViewDebtorExport> FilterByBaseParams(IQueryable<ViewDebtorExport> debtorDtoQuery, BaseParamsDebtorFilter filter)
        {
            return debtorDtoQuery
                .WhereIf(filter.MuncipalityIds.Length > 0, x => filter.MuncipalityIds.Contains(x.MunicipalityId))
                .WhereIf(filter.OwnerIds.Length > 0, x => filter.OwnerIds.Contains(x.AccountOwnerId))
                .WhereIf(filter.StateIds.Length > 0, x => filter.StateIds.Contains(x.StateId))
                .WhereIf(filter.RealityObject.Length > 0, x => filter.RealityObject.Contains(x.RealityObjectId));
        }

        /// <summary>
        /// Класс, который содержит в себе вытащенные из параметра запроса
        /// </summary>
        private class BaseParamsDebtorFilter
        {
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="baseParams">Параметры запроса</param>
            public BaseParamsDebtorFilter(BaseParams baseParams)
            {
                this.MuncipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
                this.StateIds = baseParams.Params.GetAs<long[]>("stateIds");
                this.OwnerIds = baseParams.Params.GetAs<long[]>("ownerIds");
                this.ProgramCrIds = baseParams.Params.GetAs<long[]>("programCrIds");
            }

            public long[] MuncipalityIds { get; private set; }

            public long[] StateIds { get; private set; }

            public long[] OwnerIds { get; private set; }

            public long[] ProgramCrIds { get; private set; }

            public long[] RealityObject { get; set; }

            public bool HasFilter => (this.MuncipalityIds != null && this.MuncipalityIds.Length > 0)
                || (this.StateIds != null && this.StateIds.Length > 0)
                || (this.OwnerIds != null && this.OwnerIds.Length > 0);
        }
    }
}