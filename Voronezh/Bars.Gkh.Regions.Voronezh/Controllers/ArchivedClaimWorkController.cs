using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Domain;
using Bars.Gkh.Modules.ClaimWork.Entities;

using Dapper;

using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Bars.Gkh.Regions.Voronezh.Controllers
{
    using System.Collections.Generic;
    using Bars.B4.Utils;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Lawsuit;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.Regions.Voronezh.Helpers;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Castle.Core.Internal;
    using NCalc;

    public class ArchivedClaimWorkController : BaseController
    {
        public ActionResult CreateArchive(BaseParams baseParams)
        {
            long clwId = baseParams.Params.GetAs<long>("ClwId");
            var tmpIds = baseParams.Params.GetAs<string>("RloiId");
            List<long> rloiIds = new List<long>();
            try
            {
                rloiIds = Array.ConvertAll(tmpIds.Split(','), Convert.ToInt64).ToList();
            }
            catch
            {
                // ignored
            }

            if (rloiIds.IsNullOrEmpty())
            {
                return new BaseDataResult()
                {
                    Message = "Не выбрано ни одной записи",
                    Success = false
                }.ToJsonResult();
            }

            try
            {
                this.CreateArchiveEntries(clwId, false, rloiIds);
                return new BaseDataResult()
                {
                    Message = "Выгрузка в архив успешна",
                    Success = true
                }.ToJsonResult();
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, ex.Message).ToJsonResult();
            }
        }

        public ActionResult MoveToArchive(BaseParams baseParams)
        {
            var tmpIds = baseParams.Params.GetAs<string>("recIds");
            List<long> recIds = new List<long>();
            try
            {
                recIds = Array.ConvertAll(tmpIds.Split(','), Convert.ToInt64).ToList();
            }
            catch
            {
                // ignored
            }

            if (recIds.IsNullOrEmpty())
            {
                return new BaseDataResult()
                {
                    Message = "Не выбрано ни одной записи",
                    Success = false
                }.ToJsonResult();
            }
            var archiveDomain = this.Container.ResolveDomain<FlattenedClaimWork>();
            try
            {
                var res = archiveDomain.GetAll().Where(x => recIds.Contains(x.Id)).ToList();
                res.ForEach(x => x.Archived = true);
                res.ForEach(x=>archiveDomain.Update(x));

                return new BaseDataResult()
                {
                    Message = "Выгрузка в архив успешна",
                    Success = true
                }.ToJsonResult();
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, ex.Message).ToJsonResult();
            }
            finally
            {
                this.Container.Release(archiveDomain);
            }
        }

        public void CreateArchiveEntries(long clwId, bool archive = true, List<long> rloiId = null)
        {
            //Crutch-driven development ahead
            var archiveDomain = this.Container.ResolveDomain<FlattenedClaimWork>();
            var ownerInfoDomain = this.Container.ResolveDomain<LawsuitOwnerInfo>();
            var statelessSession = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();
            string sqlRequest = this.RegBookSql(clwId);
            dynamic res = statelessSession.Connection.Query<dynamic>(sqlRequest);

            try
            {
                foreach (dynamic re in res)
                {
                    long? checkId = re.rloi_id;
                    if (rloiId != null 
                        && checkId.HasValue 
                        && rloiId.Contains((long)checkId))
                    {
                        FlattenedClaimWork flattenedClaimWork;
                        var owner = ownerInfoDomain.Get((long)checkId);


                        flattenedClaimWork = archiveDomain.GetAll().FirstOrDefault(x => x.RloiId == checkId && !x.Archived);
                        if (flattenedClaimWork != null)
                        {
                            flattenedClaimWork.Archived = archive;
                            archiveDomain.Update(flattenedClaimWork);
                        }
                        else
                        {
                            flattenedClaimWork = CreateFlattenedClaimWork(re, owner);
                            flattenedClaimWork.Archived = archive;
                            archiveDomain.Save(flattenedClaimWork);
                        }
                    }
                }
            }

            finally
            {
                this.Container.Release(archiveDomain);
                statelessSession.Close();
                this.Container.Release(statelessSession);
            }
        }

        private FlattenedClaimWork CreateFlattenedClaimWork(dynamic re, LawsuitOwnerInfo owner)
        {
            FlattenedClaimWork flattenedClaimWork = new FlattenedClaimWork();
            flattenedClaimWork.DebtorFullname = re.debtor_fullname;
            flattenedClaimWork.Num = re.num;
            flattenedClaimWork.DebtorRoomAddress = re.debtor_room_address;
            flattenedClaimWork.DebtorRoomType = re.debtor_room_type;
            flattenedClaimWork.DebtorRoomNumber = re.debtor_room_number;
            flattenedClaimWork.DebtorDebtPeriod = re.debtor_debt_period;
            flattenedClaimWork.DebtorDebtAmount = re.debtor_debt_amount;
            flattenedClaimWork.DebtorDutyAmount = re.debtor_duty_amount;
            flattenedClaimWork.DebtorPenaltyAmount = re.debtor_penalty_amount;
            if (owner.AreaShareNumerator + owner.AreaShareDenominator > 2)
            {
                flattenedClaimWork.Share = owner.AreaShareNumerator.ToString() + "/" + owner.AreaShareDenominator.ToString();
            }
            else
            {
                flattenedClaimWork.Share = "0";
            }

            if (flattenedClaimWork.DebtorDebtAmount > 0)
            {
                try
                {
                    if (owner.Lawsuit != null && owner.Lawsuit.PetitionType != null)
                    {
                          var petitions = Container.ResolveDomain<StateDutyPetition>().GetAll().ToArray();
                        var stateDuty = petitions
                           .Where(x => x.PetitionToCourtType.Id == owner.Lawsuit.PetitionType.Id)
                           .Select(x => x.StateDuty)
                           .FirstOrDefault(x => x.CourtType == ConvertToCourtType(owner.Lawsuit.WhoConsidered));

                        if (stateDuty != null)
                        {
                            flattenedClaimWork.DebtorDutyAmount = CalculateDuty(stateDuty, flattenedClaimWork.DebtorDebtAmount.HasValue? flattenedClaimWork.DebtorDebtAmount.Value: 0m);
                        }
                    }
                }
                catch (Exception e)
                {

                }
            }
            if (DateTime.TryParse(re.debtor_duty_payment_date, out DateTime dateTime)) flattenedClaimWork.DebtorDebtPaymentDate = dateTime;
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
            if (DateTime.TryParse(re.court_claim_payment_assignment_date, out dateTime))
                flattenedClaimWork.CourtClaimPaymentAssignmentDate = dateTime;
            flattenedClaimWork.CourtClaimRospDebtExact = re.court_claim_rosp_debt_exact;
            flattenedClaimWork.CourtClaimRospDutyExact = re.court_claim_rosp_duty_exact;
            flattenedClaimWork.CourtClaimEnforcementProcActEndNum = re.court_claim_enf_proc_act_end_num;
            if (DateTime.TryParse(re.court_claim_detirmination_turn_date, out dateTime))
                flattenedClaimWork.CourtClaimDeterminationTurnDate = dateTime;
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

            //string lcr = null;
            //switch (re.lawsuit_consideration_result)
            //{
            //    case 0:
            //        lcr = "Отказано";
            //        break;
            //    case 10:
            //        lcr = "Удовлетворено";
            //        break;
            //    case 20:
            //        lcr = "Частично удовлетворено";
            //        break;
            //    case 40:
            //        lcr = "Вынесен судебный приказ";
            //        break;
            //}

            // flattenedClaimWork.LawsuitConsiderationResult = lcr;
            flattenedClaimWork.LawsuitResultConsideration = re.lawsuit_consideration_result != null ? (LawsuitResultConsideration)re.lawsuit_consideration_result : LawsuitResultConsideration.NotSet;

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
            return flattenedClaimWork;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IFormulaParameter ResolveParameter(string code)
        {
            return Container.Resolve<IFormulaParameter>(code);
        }


        /// <summary>
        /// Посчитать гос.пошлину
        /// </summary>
        /// <param name="stateDuty"></param>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        private decimal CalculateDuty(StateDuty stateDuty, decimal debtsum)
        {
            decimal result = 0;
            if (stateDuty.CourtType == CourtType.Magistrate)
            {
                if (debtsum <= 20000)
                {
                    return (debtsum * 4 / 100) / 2 < 200 ? 200 : (debtsum * 4 / 100) / 2;
                }
                else if (debtsum <= 100000)
                {
                    return (800 + (debtsum - 20000) / 100 * 3) / 2;
                }
                else if (debtsum <= 200000)
                {
                    return (3200 + (debtsum - 100000) * 100 * 2) / 2;
                }
                else if (debtsum <= 1000000)
                {
                    return (5200 + (debtsum - 200000) / 100 * 1) / 2;
                }
                else
                {
                    return 0;
                }
            }

            return result;
        }

        /// <summary>
        /// Преобразовать "кем рассмотрено заявление" в "тип суда"
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static CourtType ConvertToCourtType(LawsuitConsiderationType type)
        {
            switch (type)
            {
                case LawsuitConsiderationType.ArbitrationCourt:
                    return CourtType.Arbitration;
                case LawsuitConsiderationType.RaionCourt:
                    return CourtType.District;
                case LawsuitConsiderationType.WorldCourt:
                    return CourtType.Magistrate;
            }

            return 0;
        }

        private string RegBookSql(long id)
        {
            return EmbeddedResourceHelper.GetStringFromEmbedded("RegBook.sql").Replace("{id}",id.ToString());
        }

        public ActionResult PauseState(BaseParams baseParams, Int64 id, DebtorState debtorState)
        {
            string str = "";
            try
            {
                var debtorClaimWorkRepo = this.Container.Resolve<IRepository<DebtorClaimWork>>();
                var debtorClaimWork = debtorClaimWorkRepo.Get(id);

                DebtorState oldState = debtorClaimWork.DebtorState;
                debtorClaimWork.DebtorState = debtorState;
                debtorClaimWork.DebtorStateHistory = oldState;

                debtorClaimWorkRepo.Update(debtorClaimWork);
                return JsSuccess("Статус успешно изменен");
            }
            catch (Exception ex)
            {
                return JsFailure("Ошибка при переводе статуса: " + ex.Message);
            }

            
        }
        public ActionResult ResumeState(BaseParams baseParams, Int64 id)
        {
           
            try
            {
                var debtorClaimWorkRepo = this.Container.Resolve<IRepository<DebtorClaimWork>>();
                var debtorClaimWork = debtorClaimWorkRepo.Get(id);

                DebtorState oldState = debtorClaimWork.DebtorStateHistory;
                if (oldState != DebtorState.PausedChangeAcc && oldState != DebtorState.PausedGetInfoUnderage)
                {
                    debtorClaimWork.DebtorState = oldState;
                    debtorClaimWorkRepo.Update(debtorClaimWork);
                    return JsSuccess("Статус успешно изменен");
                }
                else
                {
                    return JsSuccess("Статус не был изменен, так как отсутствуют данные о предыдущем статусе");
                }

               
                
            }
            catch (Exception ex)
            {
                return JsFailure("Ошибка при переводе статуса: " + ex.Message);
            }


        }
    }
}