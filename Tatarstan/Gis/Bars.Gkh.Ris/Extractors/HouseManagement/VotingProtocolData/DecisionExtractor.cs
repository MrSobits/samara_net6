namespace Bars.Gkh.Ris.Extractors.HouseManagement.VotingProtocolData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Экстрактор принятых решений по протоколу общего собрания собственников
    /// </summary>
    public class DecisionExtractor : BaseDataExtractor<RisDecisionList, BasePropertyOwnerDecision>
    {
        private List<RisVotingProtocol> protocols;
        private Dictionary<long, RisVotingProtocol> protocolsById;
        private int index = 0;
        private IDictionary decisionsTypeDict;
        private IDictionary formingFundDict;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.protocols = parameters.GetAs<List<RisVotingProtocol>>("selectedProtocols");

            this.protocolsById = this.protocols?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

            this.decisionsTypeDict = this.DictionaryManager.GetDictionary("OwnersMeetingDecisionTypeDictionary");

            this.formingFundDict = this.DictionaryManager.GetDictionary("FormingFondTypeDictionary");
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<BasePropertyOwnerDecision> GetExternalEntities(DynamicDictionary parameters)
        {
            var basePropertyOwnerDecisionDomain = this.Container.ResolveDomain<BasePropertyOwnerDecision>();

            try
            {
                var selectedProtocolIds = this.protocols?.Select(x => x.ExternalSystemEntityId).ToList();

                return basePropertyOwnerDecisionDomain.GetAll()
                    .Where(x => selectedProtocolIds.Contains(x.PropertyOwnerProtocol.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(basePropertyOwnerDecisionDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(BasePropertyOwnerDecision externalEntity, RisDecisionList risEntity)
        {
            var protocolTypeSelectManOrg = externalEntity.PropertyOwnerProtocol.TypeProtocol == PropertyOwnerProtocolType.SelectManOrg;
            var decisionsType = protocolTypeSelectManOrg
                ? this.decisionsTypeDict.GetDictionaryRecord((long)PropertyOwnerDecisionType.SetMinAmount)
                : this.decisionsTypeDict.GetDictionaryRecord((long)externalEntity.PropertyOwnerDecisionType);
            var formingFund = this.formingFundDict.GetDictionaryRecord((long)(externalEntity.MethodFormFund ?? 0));

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.QuestionNumber = ++this.index;
            risEntity.QuestionName = externalEntity.PropertyOwnerDecisionType.ToString();
            risEntity.DecisionsTypeCode = decisionsType.ReturnSafe(x => x.GisCode);
            risEntity.DecisionsTypeGuid = decisionsType.ReturnSafe(x => x.GisGuid);
            risEntity.VotingProtocol = this.protocolsById.Get(externalEntity.PropertyOwnerProtocol.Id);
            risEntity.Agree = externalEntity.PropertyOwnerProtocol.NumberOfVotes;
            risEntity.FormingFundCode = formingFund?.GisCode;
            risEntity.FormingFundGuid = formingFund?.GisGuid;
            risEntity.VotingResume = RisVotingResume.M;
        }
    }
}
