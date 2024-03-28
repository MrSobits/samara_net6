namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Действие заполнения истории изменений протоколов решений
    /// </summary>
    public class FillDecisionHistoryAction : BaseExecutionAction
    {
        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "РегОператор - Заполнение истории изменений протоколов решений";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Действие заполнения истории изменений протоколов решений. Запускается если история изменений пуста";

        /// <summary>
        /// Проверка необходимости выполнения действия
        /// </summary>
        //public override bool IsNeedAction()
        //{
        //    var entityHistory = this.Container.ResolveRepository<EntityHistoryInfo>();
        //    var govDecision = this.Container.ResolveRepository<GovDecision>();

        //    using (this.Container.Using(entityHistory, govDecision))
        //    {
        //        var isGovDecisionExists = govDecision.GetAll().Any();
        //        if (isGovDecisionExists)
        //        {
        //            var isExistsHistory = entityHistory.GetAll()
        //                .Where(x => x.ParentEntityName == typeof(RealityObject).FullName)
        //                .Any(x => x.EntityName == typeof(RealityObjectDecisionProtocol).FullName
        //                    || x.EntityName == typeof(GovDecision).FullName);

        //            return !isExistsHistory;
        //        }

        //        return isGovDecisionExists;
        //    }
        //}

        private BaseDataResult Execute()
        {
            var ownerHistoryCount = this.InsertOwnerHistory();
            var fovHistoryCount = this.InsertGovHistory();

            return new BaseDataResult
            {
                Message = $"Протоколы решения собственников: {ownerHistoryCount.Item1}. Протоколы решения органов гос. власти: {fovHistoryCount.Item1}"
            };
        }

        private Tuple<int, int> InsertOwnerHistory()
        {
            var crFundFormationDecisionRepository = this.Container.ResolveRepository<CrFundFormationDecision>();
            var accountOwnerDecisionRepository = this.Container.ResolveRepository<AccountOwnerDecision>();
            var realityObjectDecisionProtocolRepository = this.Container.ResolveRepository<RealityObjectDecisionProtocol>();

            using (this.Container.Using(crFundFormationDecisionRepository,
                accountOwnerDecisionRepository,
                realityObjectDecisionProtocolRepository))
            {
                var ownerDecisions = realityObjectDecisionProtocolRepository.GetAll()
                    .Where(x => x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.DocumentNum,
                        x.ProtocolDate,
                        x.DateStart
                    })
                    .ToList();

                var ownerIds = ownerDecisions.Select(x => x.Id).ToArray();

                var crFundFormationDict = crFundFormationDecisionRepository.GetAll()
                    .WhereContainsBulked(x => x.Protocol.Id, ownerIds)
                    .Select(x => new
                    {
                        x.Protocol.Id,
                        x.Decision
                    })
                    .ToDictionary(x => x.Id, x => (CrFundFormationDecisionType?)x.Decision);

                var accountOwnerDict = accountOwnerDecisionRepository.GetAll()
                        .WhereContainsBulked(x => x.Protocol.Id, ownerIds)
                        .Where(x => x.DecisionType != 0)
                        .Select(x => new
                        {
                            x.Protocol.Id,
                            x.DecisionType
                        })
                         .AsEnumerable().GroupBy(x=> x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(z=> (AccountOwnerDecisionType?)z.DecisionType).First());

                var ownerDecisionList = ownerDecisions
                    .Select(x => new DecisionProxy
                    {
                        EntityId = x.Id,
                        EntityName = typeof(RealityObjectDecisionProtocol).FullName,
                        ParentEntityId = x.RoId,
                        EditDate = x.ProtocolDate,
                        Number = x.DocumentNum,
                        Date = x.ProtocolDate.ToShortDateString(),
                        StartDate = x.DateStart == DateTime.MinValue ? x.ProtocolDate.ToShortDateString() : x.DateStart.ToShortDateString(),
                        FundFormationType = crFundFormationDict.Get(x.Id, CrFundFormationDecisionType.RegOpAccount).GetDisplayName(),
                        Owner = accountOwnerDict.Get(x.Id, AccountOwnerDecisionType.RegOp).GetDisplayName(),
                        ProtocolType = CoreDecisionType.Owners.GetDisplayName()
                    })
                    .ToList();

                return this.InsertHistory(ownerDecisionList);
            }
        }

        private Tuple<int, int> InsertGovHistory()
        {
            var govDecisionRepository = this.Container.ResolveRepository<GovDecision>();

            using (this.Container.Using(govDecisionRepository))
            {
                var govDecisionList = govDecisionRepository.GetAll()
                    .Where(x => x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id,
                        x.ProtocolNumber,
                        x.ProtocolDate,
                        x.DateStart,
                        x.FundFormationByRegop
                    })
                    .AsEnumerable()
                    .Select(x => new DecisionProxy
                    {
                        EntityId = x.Id,
                        EntityName = typeof(GovDecision).FullName,
                        ParentEntityId = x.RoId,
                        EditDate = x.ProtocolDate,
                        Number = x.ProtocolNumber,
                        Date = x.ProtocolDate.ToShortDateString(),
                        StartDate = x.DateStart == DateTime.MinValue ? x.ProtocolDate.ToShortDateString() : x.DateStart.ToShortDateString(),
                        FundFormationType = x.FundFormationByRegop
                            ? CrFundFormationType.RegOpAccount.GetDisplayName()
                            : CrFundFormationType.NotSelected.GetDisplayName(),
                        Owner = x.FundFormationByRegop
                            ? "Региональный оператор"
                            : "Не выбран",
                        ProtocolType = CoreDecisionType.Government.GetDisplayName()
                    })
                    .ToList();

                return this.InsertHistory(govDecisionList);
            }
        }

        private Tuple<int, int> InsertHistory(IList<DecisionProxy> proxies)
        {
            var infoList = new List<EntityHistoryInfo>(proxies.Count);
            var fieldList = new List<EntityHistoryField>(proxies.Count * 6);
            foreach (var decisionProxy in proxies)
            {
                var info = new EntityHistoryInfo
                {
                    ActionKind = ActionKind.Insert,
                    EditDate = decisionProxy.EditDate,
                    EntityId = decisionProxy.EntityId,
                    ParentEntityId = decisionProxy.ParentEntityId,
                    Username = this.GetType().Name,
                    User = this.User
                };
                infoList.Add(info);
                this.AddFields(info, fieldList, decisionProxy);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, infoList, useStatelessSession: true);
            TransactionHelper.InsertInManyTransactions(this.Container, fieldList, useStatelessSession: true);

            return Tuple.Create(infoList.Count, fieldList.Count);
        }

        private void AddFields(EntityHistoryInfo info, List<EntityHistoryField> fieldList, DecisionProxy decisionProxy)
        {
            fieldList.Add(new EntityHistoryField
            {
                EntityHistoryInfo = info,
                FieldName = "Номер протокола",
                NewValue = decisionProxy.Number
            });
            fieldList.Add(new EntityHistoryField
            {
                EntityHistoryInfo = info,
                FieldName = "Дата протокола",
                NewValue = decisionProxy.Date
            });
            fieldList.Add(new EntityHistoryField
            {
                EntityHistoryInfo = info,
                FieldName = "Дата вступления в силу",
                NewValue = decisionProxy.StartDate
            });
            fieldList.Add(new EntityHistoryField
            {
                EntityHistoryInfo = info,
                FieldName = "Способ формирования фонда",
                NewValue = decisionProxy.FundFormationType
            });
            fieldList.Add(new EntityHistoryField
            {
                EntityHistoryInfo = info,
                FieldName = "Владелец специального счета",
                NewValue = decisionProxy.Owner
            });
            fieldList.Add(new EntityHistoryField
            {
                EntityHistoryInfo = info,
                FieldName = "Тип протокола",
                NewValue = decisionProxy.ProtocolType
            });
        }

        private class DecisionProxy
        {
            public long EntityId { get; set; }
            public string EntityName { get; set; }
            public long ParentEntityId { get; set; }
            public string ParentEntityName { get; set; } = typeof(RealityObject).FullName;
            public DateTime EditDate { get; set; }
            public string Number { get; set; }
            public string Date { get; set; }
            public string StartDate { get; set; }
            public string FundFormationType { get; set; }
            public string Owner { get; set; }
            public string ProtocolType { get; set; }
        }
    }
}