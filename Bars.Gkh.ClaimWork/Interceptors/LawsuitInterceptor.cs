namespace Bars.Gkh.ClaimWork.Interceptors
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.Repositories;

    using Modules.ClaimWork.DomainService;
    using Modules.ClaimWork.DomainService.Lawsuit;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;
    using Domain;
    using System.Collections.Generic;

    /// <summary>
    /// Интерцептор для исковых заявлений
    /// </summary>
    /// <typeparam name="TLawsuit">Тип искового заявления</typeparam>
    public class LawsuitInterceptor<TLawsuit> : EmptyDomainInterceptor<TLawsuit>
        where TLawsuit : Lawsuit
    {
        public IDomainService<DebtorClaimWork> DebtorClaimWorkDomain { get; set; }
        /// <summary>Метод вызывается перед созданием объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeCreateAction(IDomainService<TLawsuit> service, TLawsuit entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);
            // //Автогенерация номера заявления
            // var stringBidNumberList = service.GetAll().Where(x=>x.BidNumber!=null).Select(x => x.BidNumber).ToList();
            // //var intBidNumberList = new List<int>();
            // long autoGenNum = 0;
            // foreach (string str in stringBidNumberList)
            // {
            //     int.TryParse(str, out int intBid);
            //     if (intBid>autoGenNum)
            //     {
            //         autoGenNum = intBid;
            //     }
            // }
            // autoGenNum++;
            // string resultBidNum = autoGenNum.ToString();
            // entity.BidNumber = resultBidNum;
            // entity.BidDate = DateTime.Today;
            return this.Success();
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<TLawsuit> service, TLawsuit entity)
        {
            var selectors = this.Container.ResolveAll<ILawsuitAutoSelector>();

            var jurInstRealObjService = this.Container.Resolve<IDomainService<JurInstitutionRealObj>>();
            //Запись дома в подсудную территорию судебного участка
            try
            {
                var ji_ro = new List<JurInstitutionRealObj>();
                var cur_ji_ro = new JurInstitutionRealObj
                {
                    RealityObject = entity.ClaimWork.RealityObject,
                    JurInstitution = entity.JurInstitution
                };
                ji_ro.Add(cur_ji_ro);

                var checkForCollisions = jurInstRealObjService.GetAll().Where(x => x.JurInstitution.Id == entity.JurInstitution.Id && x.RealityObject.Id == entity.ClaimWork.RealityObject.Id);
                if (checkForCollisions.Count() == 0)
                {
                    TransactionHelper.InsertInManyTransactions(this.Container, ji_ro);
                }
            }
            catch
            {
                //Бесшумно завершаем попытку внести связь при непредвиденных ошибках
                //TODO: Логгирование/Сообщение об ошибке?
            }

            try
            {
                selectors.ForEach(x => x.TrySetAll(entity));
            }
            finally
            {
                selectors.ForEach(x => this.Container.Release(x));
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>Метод вызывается после обновления объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterUpdateAction(IDomainService<TLawsuit> service, TLawsuit entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateRepository = this.Container.Resolve<IStateRepository>();
            try
            {
                var debtor = this.GetDebtor(entity);

                var startState = stateRepository.GetAllStates<DebtorClaimWork>()
                    .FirstOrDefault(x => x.StartState);
                if (startState == null)
                {
                    return this.Failure("Не найден начальный статус");
                }

                var finalState = stateRepository.GetAllStates<DebtorClaimWork>()
                    .FirstOrDefault(x => x.FinalState);
                if (finalState == null)
                {
                    return this.Failure("Не найден конечный статус");
                }

                if (debtor != null && debtor.DebtorState != DebtorState.PaidDebt && debtor.DebtorState!= DebtorState.StartedEnforcement)
                {
                    var canUpdate = false;

                    if (entity.ClaimWork.ClaimWorkTypeBase == ClaimWorkTypeBase.Debtor && (entity.CbSize == LawsuitCollectionDebtType.FullRepaid ||
                        entity.CbReasonStoppedType == LawsuitCollectionDebtReasonStoppedType.FactPerformed))
                    {
                        debtor.DebtPaidDate = DateTime.Today;
                        debtor.SetDebtorState(DebtorState.PaidDebt, finalState);
                        canUpdate = true;
                    }
                    else if(entity.DocumentType == ClaimWorkDocumentType.CourtOrderClaim)
                    {
                        if (entity.CbFactInitiated == LawsuitFactInitiationType.Initiated && debtor.DebtorState != DebtorState.StartedEnforcement)
                        {
                            debtor.SetDebtorState(DebtorState.StartedEnforcement, startState);
                            canUpdate = true;
                        }
                        else if (entity.CbFactInitiated != LawsuitFactInitiationType.Initiated && debtor.DebtorState == DebtorState.StartedEnforcement)
                        {
                            debtor.SetDebtorState(DebtorState.LawsuitNeeded, startState);
                            canUpdate = true;
                        }
                        else if (entity.ResultConsideration == LawsuitResultConsideration.NotSet && !entity.IsDeterminationCancel)
                        {
                            var oldState = debtor.DebtorState;
                            debtor.DebtorState = DebtorState.CourtOrderClaimFormed;
                            debtor.DebtorStateHistory = oldState;
                        }
                        else if (entity.ResultConsideration == LawsuitResultConsideration.Denied && entity.DocumentType == ClaimWorkDocumentType.CourtOrderClaim)
                        {
                            var oldState = debtor.DebtorState;
                            debtor.DebtorState = DebtorState.LawsuitNeeded;
                            debtor.DebtorStateHistory = oldState;
                        }
                        else if (entity.ResultConsideration == LawsuitResultConsideration.Denied && entity.DocumentType == ClaimWorkDocumentType.Lawsuit)
                        {
                            var oldState = debtor.DebtorState;
                            debtor.DebtorState = DebtorState.LawSueenDenied;
                            debtor.DebtorStateHistory = oldState;
                        }
                        else if (entity.IsDeterminationCancel && entity.DateDeterminationCancel.HasValue)
                        {
                            if (entity.DateDeterminationCancel.Value.AddMonths(2) <= DateTime.Now)
                            {
                                var oldState = debtor.DebtorState;
                                debtor.DebtorState = DebtorState.LawsuitNeeded;
                                debtor.DebtorStateHistory = oldState;
                            }
                            else
                            {
                                var oldState = debtor.DebtorState;
                                debtor.DebtorState = DebtorState.CourtOrderCancelled;
                                debtor.DebtorStateHistory = oldState;
                            }
                        }
                        else if (entity.IsDeterminationCancel && !entity.DateDeterminationCancel.HasValue)
                        {
                            var oldState = debtor.DebtorState;
                            debtor.DebtorState = DebtorState.CourtOrderCancelled;
                            debtor.DebtorStateHistory = oldState;
                        }
                        else if (entity.ResultConsideration == LawsuitResultConsideration.CourtOrderIssued && !entity.IsDeterminationCancel)
                        {
                            if (entity.ConsiderationDate.HasValue && entity.ConsiderationDate.Value.AddMonths(2) <= DateTime.Now)
                            {
                                var oldState = debtor.DebtorState;
                                debtor.DebtorState = DebtorState.ROSPStartRequired;
                                debtor.DebtorStateHistory = oldState;
                              
                            }
                            else
                            {
                                var oldState = debtor.DebtorState;
                                debtor.DebtorState = DebtorState.CourtOrderApproved;
                                debtor.DebtorStateHistory = oldState;
                             
                            }
                        }
                    }

                    if (canUpdate)
                    {
                        this.DebtorClaimWorkDomain.Update(debtor);
                    }
                }

                return this.Success();
            }
            finally
            {
                this.Container.Release(stateProvider);
                this.Container.Release(stateRepository);
            }
        }

        /// <summary>Метод вызывается перед удалением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<TLawsuit> service, TLawsuit entity)
        {
            var courtDomain = this.Container.Resolve<IDomainService<LawsuitClwCourt>>();
            var documentDomain = this.Container.Resolve<IRepository<LawsuitClwDocument>>();
            var restructDebtDomain = this.Container.ResolveDomain<RestructDebt>();
            var ownerInfoDomain = this.Container.ResolveDomain<LawsuitOwnerInfo>();
            var refCalcDomain = this.Container.ResolveDomain<LawsuitReferenceCalculation>();

            try
            {
                var restructExists = restructDebtDomain.GetAll()
                    .Where(x => x.DocumentType == ClaimWorkDocumentType.RestructDebtAmicAgr)
                    .Any(x => x.ClaimWork.Id == entity.ClaimWork.Id);

                if (restructExists)
                {
                    return this.Failure("Невозможно удалить запись, т.к. создана реструктуризация по мировому соглашению");
                }

                var idsCourt = courtDomain.GetAll().Where(x => x.DocumentClw.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var id in idsCourt)
                {
                    courtDomain.Delete(id);
                }

                var idsDoc = documentDomain.GetAll().Where(x => x.DocumentClw.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var id in idsDoc)
                {
                    documentDomain.Delete(id);
                }

                var idsOwner = ownerInfoDomain.GetAll().Where(x => x.Lawsuit.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var id in idsOwner)
                {
                    ownerInfoDomain.Delete(id);
                }

                var refCalc = refCalcDomain.GetAll().Where(x => x.Lawsuit.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var id in refCalc)
                {
                    refCalcDomain.Delete(id);
                }

                return this.Success();
            }
            finally
            {
                this.Container.Release(courtDomain);
                this.Container.Release(documentDomain);
                this.Container.Release(restructDebtDomain);
                this.Container.Release(ownerInfoDomain);
                this.Container.Release(refCalcDomain);
            }
        }

        /// <summary>Метод вызывается после удаления объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterDeleteAction(IDomainService<TLawsuit> service, TLawsuit entity)
        {
            var clwService = this.Container.ResolveAll<IBaseClaimWorkService>()
                .FirstOrDefault(x => x.ClaimWorkTypeBase == entity.ClaimWork.ClaimWorkTypeBase);

            if (clwService != null)
            {
                var newParams = new DynamicDictionary
                                {
                                    { "id", entity.ClaimWork.Id }
                                };
                var baseParams = new BaseParams { Params = newParams };

          //      clwService.UpdateStates(baseParams);
            }

            return this.Success();
        }

        private DebtorClaimWork GetDebtor(TLawsuit entity)
        {
            return this.DebtorClaimWorkDomain.Load(entity.ClaimWork.Id);
        }
    }
}