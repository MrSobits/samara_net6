namespace Bars.Gkh.RegOperator.Interceptors.Debtor
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Modules.ClaimWork.Interceptors;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories;

    using System.Collections.Generic;

    using Bars.Gkh.Modules.ClaimWork.Entities;

    using Dapper;

    public class DebtorClaimWorkInterceptor<T> : BaseClaimWorkInterceptor<T>
        where T : DebtorClaimWork
    {
        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            if (entity.IsDebtPaid && entity.DebtPaidDate.HasValue && entity.DebtPaidDate <= DateTime.Today)
            {
                var stateRepo = this.Container.Resolve<IStateRepository>();
                using (this.Container.Using(stateRepo))
                {
                    var finalState = stateRepo.GetAllStates<DebtorClaimWork>(x => x.FinalState).Take(2).ToList();
                    if (finalState.Count != 1)
                    {
                        return BaseDataResult.Error("Не найден конечный статус");
                    }

                    entity.State = finalState.First();
                }
            }

            return this.Success();
        }
        /*
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            //Crutch-driven development ahead
            var detailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var archiveDomain = this.Container.ResolveDomain<FlattenedClaimWork>();
            var statelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            dynamic res = statelessSession.Connection.Query<dynamic>(this.RegBookSql(entity.Id));
            foreach (dynamic re in res)
            {
                DateTime dateTime;
                var flattenedClaimWork = new FlattenedClaimWork();
                flattenedClaimWork.DebtorFullname = re.debtor_fullname;
                flattenedClaimWork.DebtorRoomAddress = re.debtor_room_address;
                flattenedClaimWork.DebtorRoomType = re.debtor_room_type;
                flattenedClaimWork.DebtorRoomNumber = re.debtor_room_number;
                flattenedClaimWork.DebtorDebtPeriod = re.debtor_debt_period;
                flattenedClaimWork.DebtorDebtAmount = re.debtor_debt_amount;
                flattenedClaimWork.DebtorDutyAmount = re.debtor_duty_amount;
                if (DateTime.TryParse(re.debtor_duty_payment_date, out dateTime)) flattenedClaimWork.DebtorDebtPaymentDate = dateTime;
                flattenedClaimWork.DebtorDutyPaymentAssignment = re.debtor_duty_payment_assignment_num;
                if (DateTime.TryParse(re.debtor_claim_delivery_date, out dateTime)) flattenedClaimWork.DebtorClaimDeliveryDate = dateTime;
                flattenedClaimWork.DebtorPaymentsAfterCourtOrder = re.debtor_payments_after_court_order;
                flattenedClaimWork.DebtorJurInstType = re.debtor_jur_inst_type;
                flattenedClaimWork.DebtorJurInstName = re.debtor_jur_inst_name;
                flattenedClaimWork.CourtClaimNum = re.court_claim_num;
                flattenedClaimWork.DebtorPaymentsAfterCourtOrder = re.debtor_payments_after_court_order;
                flattenedClaimWork.DebtorJurInstType = re.debtor_jur_inst_type;
                flattenedClaimWork.DebtorJurInstName = re.debtor_jur_inst_name;
                flattenedClaimWork.CourtClaimNum = re.court_claim_num;
                if (DateTime.TryParse(re.court_claim_date, out dateTime)) flattenedClaimWork.CourtClaimDate = dateTime;
                flattenedClaimWork.CourtClaimConsiderationResult = re.court_claim_consideration_result;
                if (DateTime.TryParse(re.court_claim_cancellation_date, out dateTime)) flattenedClaimWork.CourtClaimCancellationDate = dateTime;
                flattenedClaimWork.CourtClaimRospName = re.court_claim_rosp_name;
                if (DateTime.TryParse(re.court_claim_rosp_date, out dateTime)) flattenedClaimWork.CourtClaimRospDate = dateTime;
                flattenedClaimWork.CourtClaimEnforcementProcNum = re.court_claim_enf_proc_num;
                if (DateTime.TryParse(re.court_claim_enf_proc_date, out dateTime)) flattenedClaimWork.CourtClaimEnforcementProcDate = dateTime;
                flattenedClaimWork.CourtClaimPaymentAssignmentNum = re.court_claim_payment_assignment_num;
                if (DateTime.TryParse(re.court_claim_payment_assignment_date, out dateTime)) flattenedClaimWork.CourtClaimPaymentAssignmentDate = dateTime;
                flattenedClaimWork.CourtClaimRospDebtExact = re.court_claim_rosp_debt_exact;
                flattenedClaimWork.CourtClaimRospDutyExact = re.court_claim_rosp_duty_exact;
                flattenedClaimWork.CourtClaimEnforcementProcActEndNum = re.court_claim_enf_proc_act_end_num;
                if (DateTime.TryParse(re.court_claim_detirmination_turn_date, out dateTime)) flattenedClaimWork.CourtClaimDeterminationTurnDate = dateTime;
                flattenedClaimWork.FkrRospName = re.fkr_rosp_name;
                flattenedClaimWork.FkrEnforcementProcDecisionNum = re.fkr_enf_proc_decision_num;
                if (DateTime.TryParse(re.fkr_enf_proc_date, out dateTime)) flattenedClaimWork.FkrEnforcementProcDate = dateTime;
                flattenedClaimWork.FkrPaymentAssignementNum = re.fkr_payment_assignment_num;
                if (DateTime.TryParse(re.fkr_payment_assignment_date, out dateTime)) flattenedClaimWork.FkrPaymentAssignmentDate = dateTime;
                flattenedClaimWork.FkrDebtExact = re.fkr_debt_exact;
                flattenedClaimWork.FkrDutyExact = re.fkr_duty_exact;
                flattenedClaimWork.FkrEnforcementProcActEndNum = re.fkr_enf_proc_act_end_num;
                if (DateTime.TryParse(re.lawsuit_court_delivery_date, out dateTime)) flattenedClaimWork.LawsuitCourtDeliveryDate = dateTime;
                flattenedClaimWork.LawsuitDocNum = re.lawsuit_doc_num;
                if (DateTime.TryParse(re.lawsuit_consideration_date, out dateTime)) flattenedClaimWork.LawsuitConsiderationDate = dateTime;

                string lcr = null;
                switch (re.lawsuit_consideration_result)
                {
                    case 0:
                        lcr = "Отказано";
                        break;
                    case 10:
                        lcr = "Удовлетворено";
                        break;
                    case 20:
                        lcr = "Частично удовлетворено";
                        break;
                    case 40:
                        lcr = "Вынесен судебный приказ";
                        break;
                }

                flattenedClaimWork.LawsuitConsiderationResult = lcr;

                flattenedClaimWork.LawsuitDebtExact = re.lawsuit_debt_exact;
                flattenedClaimWork.LawsuitDutyExact = re.lawsuit_duty_exact;
                flattenedClaimWork.ListListNum = re.list_list_num;
                if (DateTime.TryParse(re.list_list_rosp_date, out dateTime)) flattenedClaimWork.ListListRopsDate = dateTime;
                flattenedClaimWork.ListRospName = re.list_rosp_name;
                if (DateTime.TryParse(re.list_rosp_date, out dateTime)) flattenedClaimWork.ListRospDate = dateTime;
                flattenedClaimWork.ListEnfProcDecisionNum = re.list_enf_proc_decision_num;
                if (DateTime.TryParse(re.list_enf_proc_date, out dateTime)) flattenedClaimWork.ListEnfProcDate = dateTime;
                flattenedClaimWork.ListPaymentAssignmentNum = re.list_payment_assignment_num;
                if (DateTime.TryParse(re.list_payment_assignment_date, out dateTime)) flattenedClaimWork.ListPaymentAssignmentDate = dateTime;
                flattenedClaimWork.ListRospDebtExacted = re.list_rosp_debt_exacted;
                flattenedClaimWork.ListRospDutyExacted = re.list_rosp_duty_exacted;
                flattenedClaimWork.ListEnfProcActEndNum = re.list_enf_proc_act_end_num;
                flattenedClaimWork.Note = re.note;
                flattenedClaimWork.RloiId = re.rloi_id;
                var check = archiveDomain.GetAll().FirstOrDefault(x => x.RloiId == re.rloi_id);
                if (check.RloiId == flattenedClaimWork.RloiId)
                {
                    flattenedClaimWork.Id = check.Id;
                    archiveDomain.Update(flattenedClaimWork);
                }
                else
                {
                    archiveDomain.Save(flattenedClaimWork);
                }
            }

            this.Container.Release(archiveDomain);

            using (this.Container.Using())
            {
                List<ClaimWorkAccountDetail> clwAccDetails = detailDomain.GetAll()
                    .Where(x => x.ClaimWork.Id == entity.Id).ToList();

                foreach (var detail in clwAccDetails)
                {
                    detailDomain.Delete(detail.Id);
                }
            }

            statelessSession.Close();
            this.Container.Release(statelessSession);
            return base.BeforeDeleteAction(service, entity);
        }

        private string RegBookSql(long id)
        {
            return $@"/*Owner Count
with own_count as (SELECT cdoc.claimwork_id ccw_id, count(rloi.id) owner_count
                   from regop_lawsuit_owner_info rloi
                          join clw_document cdoc on cdoc.id = rloi.lawsuit_id
                   GROUP by ccw_id
                   ORDER by owner_count),
/*Court Claim
    base_info_and_court_claim as (Select
                                      --Filter for num
                                         case
                                           when law.bid_number ~ E'^\\d+$' then law.bid_number :: integer
                                           else 0 end                                  f1,
                                         case
                                           when rloi.claim_number ~ E'^\\d+/'
                                                   then regexp_replace(rloi.claim_number, '^(\d+?)/(\d+?)', '\2') :: INTEGER
                                           else 0 end                                  f2,
                                         ccw.id                                        f3,
                                         oc.owner_count                                f4,
                                         law.bid_number                                n1,
                                         rloi.id                                       rloi_id,
                                         rloi.claim_number                             n2,
                                         rloi.name                                     p2,
                                         ro.address                                    p3,
                                      --Разбор типов помещений
                                         CASE
                                           WHEN r.type = 10 THEN 'Квартира'
                                           WHEN r.type = 20 THEN 'Нежилое помещение'
                                           ELSE 'Не указано'
                                             END                                       p4,
                                         r.croom_num                                   p5,
                                         to_char(law.debt_start_date, 'dd.MM.yyyy') || ' - ' ||
                                         to_char(law.debt_end_date, 'dd.MM.yyyy')      p6,
                                         round(law.debt_sum * case
                                                                when rloi.area_share_den <= 1 then 1
                                                                else rloi.area_share_num :: decimal / rloi.area_share_den :: decimal
                                             end, 2)                                   p7,
                                         round(law.duty, 2)                            p8,
                                         t1.doc_date as                                p9,
                                         t1.doc_number                                 p10,
                                         to_char(law.bid_date, 'dd.MM.yyyy')           p11,
                                         CASE
                                           WHEN law.WHO_CONSIDERED = 0 THEN 'Не задано'
                                           WHEN law.WHO_CONSIDERED = 10 THEN 'Арбитражный суд'
                                           WHEN law.WHO_CONSIDERED = 20 THEN 'Мировой суд'
                                           WHEN law.WHO_CONSIDERED = 30 THEN 'Районный суд'
                                           ELSE 'Не указано'
                                             END                                       p13,
                                         replace(ji.name, 'Судебный участок ', '')     p14,
                                         law.COURT_BUISNESS_NUMBER                     p15,
                                         to_char(law.DATE_JUDICAL_ORDER, 'dd.MM.yyyy') p16,
                                         case
                                           when IS_DETERMINATION_RETURN then 'Определение о возвращении ЗВСП'
                                           when IS_DETERMINATION_RENOUNCEMENT then 'Определение об отказе принятия ЗВСП'
                                           when IS_DETERMINATION_CANCEL then 'Определение об отмене судебного приказа'
                                           when IS_DETERMINATION_TURN then 'Определение о повороте СП'
                                           when DATE_JUDICAL_ORDER is not null then 'Вынесен судебный приказ'
                                           else 'Не задано' end                        p17,
                                         law.DATE_DETERMINATION_CANCEL                 p18,
                                         cji.name                                      p19,
                                         law.CB_SSP_DATE                               p20,
                                         cld_start.number                              p21,
                                         cld_start.date                                p22,
                                         cld_pay_doc.doc_number                        p23,
                                         cld_pay_doc.doc_date                          p24,
                                         law.CB_SUM_STEP                               p25,
                                         cld_end.number                                p27,
                                         law.DATE_DETERMINATION_TURN                   p28,
                                      --Вспомогательные

                                         ccw.id                                        ccw_id,
                                         cdoc.type_document                            type_document
                                  from clw_claim_work ccw
                                         join own_count oc ON oc.ccw_id = ccw.id
                                         join clw_document cdoc on cdoc.claimwork_id = ccw.id
                                         join clw_lawsuit law on law.id = cdoc.id
                                         left join clw_jur_institution ji on ji.id = law.jinst_id
                                         join clw_debtor_claim_work cw on cw.id = ccw.id
                                         join clw_claim_work_acc_detail cacc on cacc.claim_work_id = ccw.id
                                         join regop_pers_acc ac on ac.id = cacc.account_id
                                         left join CLW_JUR_INSTITUTION cji on cji.id = law.CB_SSP_JINST_ID
                                         join regop_pers_acc_owner ow on ow.id = cw.owner_id
                                         LEFT JOIN regop_lawsuit_owner_info rloi on rloi.lawsuit_id = law.id
                                         join gkh_room r on r.id = room_id
                                         join gkh_reality_object ro on ro.id = r.ro_id
                                         left join REGOP_INDIVIDUAL_ACC_OWN riao on riao.id = ow.id
                                         left join clw_lawsuit_documentation cld_start
                                           on cld_start.lawsuit_id = law.id and cld_start.type_doc = 10
                                         left join clw_lawsuit_documentation cld_end
                                           on cld_end.lawsuit_id = law.id and cld_end.type_doc = 50
                                         left join clw_lawsuit_doc cld_pay_doc on cld_pay_doc.document_id = law.id and
                                                                                  cld_pay_doc.doc_name = 'Платежное поручение'
                                         left join (select document_id, doc_number, doc_date
                                                    from clw_lawsuit_doc
                                                    order by id desc
                                                    limit 1) t1 on t1.document_id = cdoc.id
                                  where type_document = 60),
/*Lawsuit
    lawsuit as (Select law.debt_start_date                           p6,
                       round(law.debt_sum, 2)                        p7,
                       round(law.duty, 2)                            p8,
                       t1.doc_date as                                p9,
                       t1.doc_number                                 p10,
                       to_char(law.bid_date, 'dd.MM.yyyy')           p11,
                       CASE
                         WHEN law.WHO_CONSIDERED = 0 THEN 'Не задано'
                         WHEN law.WHO_CONSIDERED = 10 THEN 'Арбитражный суд'
                         WHEN law.WHO_CONSIDERED = 20 THEN 'Мировой суд'
                         WHEN law.WHO_CONSIDERED = 30 THEN 'Районный суд'
                         ELSE 'Не указано'
                           END                                       p13,
                       replace(ji.name, 'Судебный участок ', '')     p14,
                       law.COURT_BUISNESS_NUMBER                     p15,
                       to_char(law.DATE_JUDICAL_ORDER, 'dd.MM.yyyy') p16,
                       case
                         when IS_DETERMINATION_RETURN then 'Определение о возвращении ЗВСП'
                         when IS_DETERMINATION_RENOUNCEMENT then 'Определение об отказе принятия ЗВСП'
                         when IS_DETERMINATION_CANCEL then 'Определение об отмене судебного приказа'
                         when IS_DETERMINATION_TURN then 'Определение о повороте СП'
                         when DATE_JUDICAL_ORDER is not null then 'Вынесен судебный приказ'
                         else 'Не задано' end                        p17,
                       law.DATE_DETERMINATION_CANCEL                 p18,
                       law.DATE_DETERMINATION_TURN                   p28,
                       law.DATE_OF_ADOPTION                          p37,
                       law.BID_NUMBER                                p38,
                       law.DATE_OF_REWIEW                            p39,
                       law.RESULT_CONSIDERATION                      p40,
                       law.DEBT_SUM_APPROV                           p41,
                       law.CB_NUMBER_DOC                             p43,
                       law.CB_SSP_DATE                               p44,
                       cji.name                                      p45,
                       law.CB_SSP_DATE                               p46,  --p44

                       cld_start.number                              p47,
                       cld_start.date                                p48,
                       cld_pay_doc.doc_number                        p49,
                       cld_pay_doc.doc_date                          p50,
                       law.CB_SUM_STEP                               p51,
                       cld_end.number                                p53,
                                                                           --Вспомогательные

                       ccw.id                                        ccw_id,
                       cdoc.type_document                            type_document
                from clw_claim_work ccw
                       join clw_document cdoc on cdoc.claimwork_id = ccw.id
                       join clw_lawsuit law on law.id = cdoc.id
                       left join clw_jur_institution ji on ji.id = law.jinst_id
                       join clw_debtor_claim_work cw on cw.id = ccw.id
                       join clw_claim_work_acc_detail cacc on cacc.claim_work_id = ccw.id
                       join regop_pers_acc ac on ac.id = cacc.account_id
                       left join CLW_JUR_INSTITUTION cji on cji.id = law.CB_SSP_JINST_ID
                       join regop_pers_acc_owner ow on ow.id = cw.owner_id
                       join gkh_room r on r.id = room_id
                       join gkh_reality_object ro on ro.id = r.ro_id
                       left join REGOP_INDIVIDUAL_ACC_OWN riao on riao.id = ow.id
                       left join clw_lawsuit_documentation cld_start
                         on cld_start.lawsuit_id = law.id and cld_start.type_doc = 10
                       left join clw_lawsuit_documentation cld_end
                         on cld_end.lawsuit_id = law.id and cld_end.type_doc = 50
                       left join clw_lawsuit_doc cld_pay_doc
                         on cld_pay_doc.document_id = law.id and cld_pay_doc.doc_name = 'Платежное поручение'
                       left join (select document_id, doc_number, doc_date
                                  from clw_lawsuit_doc
                                  order by id desc
                                  limit 1) t1 on t1.document_id = cdoc.id
                where type_document = 30),
     payments as (Select
                      --Вспомогательные
                         ccw.id id, sum(rt.amount) p12
                  from clw_claim_work ccw
                         join clw_document cdoc on cdoc.claimwork_id = ccw.id
                         join clw_lawsuit law on law.id = cdoc.id
                         join clw_claim_work_acc_detail cacc on cacc.claim_work_id = ccw.id
                         join regop_pers_acc ac on ac.id = cacc.account_id
                         left join regop_transfer rt on ac.id = rt.owner_id
                  where law.bid_date < rt.payment_date
                  GROUP by 1)


Select
    --1)  № п/п
       CASE
         when biacc.f4 = 1 then biacc.n1
         ELSE biacc.n2
           END     num,
       biacc.rloi_id,
       biacc.f1,
       biacc.f2,
--2)  Должник
       biacc.p2    debtor_fullname,
--    Адрес МКД, в т.ч.
    --        3) Адрес
       biacc.p3    debtor_room_address,
--        4) Тип помещения
       biacc.p4    debtor_room_type,
--        5) Номер пом.
       biacc.p5    debtor_room_number,
--6)  Период задолженности
       biacc.p6    debtor_debt_period,
--
    --7)  Сумма долга
       biacc.p7    debtor_debt_amount,
--    Пошлина
    --        8)  Размер (руб.)
       biacc.p8    debtor_duty_amount,
--        9)  Дата оплаты
       biacc.p9    debtor_duty_payment_date,
--        10) № платежного поручения
       biacc.p10   debtor_duty_payment_assignment_num,
--11) Дата подачи заявления в суд
       biacc.p11   debtor_claim_delivery_date,
--12) Сумма уплаченной задолженности после подачи заявления в суд
       case
         when biacc.f2 in (0, 1) then payments.p12
         else null end
                   debtor_payments_after_court_order,
--13) Тип суда
       biacc.p13   debtor_jur_inst_type,
--14) Наименование суда
       biacc.p14   debtor_jur_inst_name,
--
    --Заявление о вынесении судебного приказа (СП)
    --    15) № дела
       biacc.p15   court_claim_num,
--    16) Дата вынесения СП
       biacc.p16   court_claim_date,
--    17) Результат рассмотрения:
    --            - определение о возврате заявления о вынесении  СП,
    --            - определение об отказе в принятии заявления о вынесении СП,
    --            - СП
       biacc.p17   court_claim_consideration_result,
--    18) Определение об отмене СП (дата)
       biacc.p18   court_claim_cancellation_date,
--
    --Исполнительное производство  к должнику по СП
    --    19) Наименование РОСП
       biacc.p19   court_claim_rosp_name,
--    20) Дата направления в РОСП
       biacc.p20   court_claim_rosp_date,
--    21) № Постановления о возбуждении  ИП
       biacc.p21   court_claim_enf_proc_num,
--    22) Дата (ИП)
       biacc.p22   court_claim_enf_proc_date,
--    23) № платежного поручения
       biacc.p23   court_claim_payment_assignment_num,
--    24) Дата (ПП)
       biacc.p24   court_claim_payment_assignment_date,
--    Взыскано РОСП (руб.)
    --        25) Сумма основного долга
       biacc.p25   court_claim_rosp_debt_exact,
--        26) Размер пошлины
       null         court_claim_rosp_duty_exact,
--    27) № Постановления об окончании ИП
       biacc.p27   court_claim_enf_proc_act_end_num,
--
    --28) Определение о повороте исполнения СП (дата)
       biacc.p28   court_claim_detirmination_turn_date,
--
    --Исполнительное производство  к  ФКР [??? 1-to-1]
    --    29) Наименование РОСП
       '-'         fkr_rosp_name,
--    30) № Постановления о возбуждении  ИП
       biacc.p21   fkr_enf_proc_decision_num,
--    31) Дата (ИП)
       biacc.p22   fkr_enf_proc_date,
--    32) № платежного поручения
       biacc.p23   fkr_payment_assignment_num,
--    33) Дата (ПП)
       biacc.p24   fkr_payment_assignment_date,
--    Взыскано РОСП (руб.)
    --        34) Сумма основного долга
       biacc.p25   fkr_debt_exact,
--        35) Размер пошлины
       null         fkr_duty_exact,
--    36) № Постановления об окончании ИП
       biacc.p27   fkr_enf_proc_act_end_num,
--
    --37) Дата подачи иска в суд
       lawsuit.p37 lawsuit_court_delivery_date,
--Исковое заявление (ИЗ)
    --    38) № дела
       lawsuit.p38 lawsuit_doc_num,
--    39) Дата рассмотрения
       lawsuit.p39 lawsuit_consideration_date,
--    40) Результат рассмотрения:
    --        - Определение об оставлении ИЗ без движения,
    --        - Определение о возврате ИЗ,
    --        - Определение об отказе в принятии ИЗ,
    --        - Определение о направлении ИЗ по подсудности,
    --        - Определение о прекращении производства по делу,
    --        - Решение (с удовлетворением требований),
    --        - Решение (с частичным удовлетворением требований)
    --        - Решение (отказано в удовлетворении требований).
       lawsuit.p40 lawsuit_consideration_result,
--    Взыскано (руб.)
    --        41) Сумма основного долга
       lawsuit.p41 lawsuit_debt_exact,
--        42) Размер пошлины
       null        lawsuit_duty_exact,
--
    --Исполнительное производство  к должнику по исполнительному листу
    --    Исполнительный лист
    --        43) Серия №
       lawsuit.p43 list_list_num,
--        44) Дата направления в РОСП
       lawsuit.p44 list_list_rosp_date,
--    45) Наименование РОСП
       lawsuit.p45 list_rosp_name,
--    46) Дата направления в РОСП
       lawsuit.p46 list_rosp_date,
--    47) № Постановления о возбуждении  ИП
       lawsuit.p47 list_enf_proc_decision_num,
--    48) Дата (ИП)
       lawsuit.p48 list_enf_proc_date,
--    49) № платежного поручения
       lawsuit.p49 list_payment_assignment_num,
--    50) Дата (ПП)
       lawsuit.p50 list_payment_assignment_date,
--    Взыскано РОСП (руб.)
    --        51) Сумма основного долга
       lawsuit.p51 list_rosp_debt_exacted,
--        52) Размер пошлины
       null         list_rosp_duty_exacted,
--    53) № Постановления об окончании ИП
       lawsuit.p53 list_enf_proc_act_end_num,
--
    --54) Примечание
       ''          note
from base_info_and_court_claim biacc
       left join lawsuit on lawsuit.ccw_id = biacc.ccw_id
       left join payments on payments.id = biacc.ccw_id
where biacc.ccw_id={id}
order by f1, f2";
        }*/
    }
}