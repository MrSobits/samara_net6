namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr;
    using Bars.Gkh.InspectorMobile.Extensions;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.GkhCr.Entities;

    using NHibernate.Linq;

    using MobileTypeWorkCr = Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr.TypeWorkCr;
    using TypeWorkCr = Bars.GkhCr.Entities.TypeWorkCr;

    /// <summary>
    /// Сервис для работы с данными об объекте капитального ремонта
    /// </summary>
    public class ObjectCrService : BaseApiService<ObjectCr, object, ObjectCrUpdate>, IObjectCrService
    {
        #region DependencyInjection
        private readonly IStateProvider stateProvider;
        private readonly IDomainService<State> stateDomain;
        private readonly IDomainService<ObjectCr> objectCrDomain;
        private readonly IDomainService<BasePropertyOwnerDecision> propertyOwnerDecisionDomain;
        private readonly IDomainService<TypeWorkCr> typeWorkCrDomain;
        private readonly IDomainService<ProtocolCr> protocolCrDomain;
        private readonly IDomainService<DefectList> defectListDomain;
        private readonly IDomainService<EstimateCalculation> estimateCalculationDomain;
        private readonly IDomainService<Estimate> estimateDomain;
        private readonly IDomainService<BuildContract> buildContractDomain;
        private readonly IDomainService<BuildContractTypeWork> buildContractTypeWorkDomain;
        private readonly IDomainService<PerformedWorkAct> performedWorkActDomain;

        public ObjectCrService(
            IStateProvider stateProvider,
            IDomainService<State> stateDomain,
            IDomainService<ObjectCr> objectCrDomain,
            IDomainService<BasePropertyOwnerDecision> propertyOwnerDecisionDomain,
            IDomainService<TypeWorkCr> typeWorkCrDomain,
            IDomainService<ProtocolCr> protocolCrDomain,
            IDomainService<DefectList> defectListDomain,
            IDomainService<EstimateCalculation> estimateCalculationDomain,
            IDomainService<Estimate> estimateDomain,
            IDomainService<BuildContract> buildContractDomain,
            IDomainService<BuildContractTypeWork> buildContractTypeWorkDomain,
            IDomainService<PerformedWorkAct> performedWorkActDomain)
        {
            this.stateProvider = stateProvider;
            this.stateDomain = stateDomain;
            this.objectCrDomain = objectCrDomain;
            this.propertyOwnerDecisionDomain = propertyOwnerDecisionDomain;
            this.typeWorkCrDomain = typeWorkCrDomain;
            this.protocolCrDomain = protocolCrDomain;
            this.defectListDomain = defectListDomain;
            this.estimateCalculationDomain = estimateCalculationDomain;
            this.estimateDomain = estimateDomain;
            this.buildContractDomain = buildContractDomain;
            this.buildContractTypeWorkDomain = buildContractTypeWorkDomain;
            this.performedWorkActDomain = performedWorkActDomain;
        }
        #endregion

        /// <summary>
        /// Словарь обработанных статусов
        /// </summary>
        private Dictionary<long, State> processedStatesDict = new Dictionary<long, State>();

        /// <inheritdoc />
        public async Task<ObjectCrGet> GetByObjectCr(long objectId)
            => await this.InternalGet(x => x.Id == objectId);

        /// <inheritdoc />
        public async Task<ObjectCrGet> GetByProgramAndAddress(long programId, long addressId)
            => await this.InternalGet(x => x.ProgramCr.Id == programId && x.RealityObject.Id == addressId);

        /// <summary>
        /// Получить данные об объекте капитального ремонта
        /// </summary>
        /// <param name="filterExpression"><see cref="Expression"/> для фильтрация объектов капитального ремонта</param>
        private async Task<ObjectCrGet> InternalGet(Expression<Func<ObjectCr, bool>> filterExpression)
        {
            var objectCrInfo = await this.objectCrDomain.GetAll()
                .Where(filterExpression)
                .Select(x => new
                {
                    x.Id,
                    ProgramId = x.ProgramCr.Id,
                    AddressId = x.RealityObject.Id,
                    RealityObjectId = x.RealityObject.Id,
                    Number = x.GjiNum,
                    DateAdoption = x.DateAcceptCrGji,
                    WorkSum = x.SumSmr,
                    ApprovedWorkSum = x.SumSmrApproved,
                    ExpertiseSum = x.SumDevolopmentPsd,
                    TechnicalSum = x.SumTehInspection,
                })
                .SingleOrDefaultAsync();

            if (objectCrInfo == null)
            {
                return null;
            }

            var resp = objectCrInfo.CopyIdenticalProperties<ObjectCrGet>();

            resp.FormWay = (await this.propertyOwnerDecisionDomain.GetAll()
                    .OrderByDescending(x => x.PropertyOwnerProtocol.DocumentDate)
                    .FirstOrDefaultAsync(y => y.PropertyOwnerProtocol.TypeProtocol != PropertyOwnerProtocolType.SelectManOrg &&
                        y.RealityObject.Id == objectCrInfo.RealityObjectId &&
                        y.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming))?
                .MethodFormFund
                .GetDisplayName();

            resp.Work = await this.typeWorkCrDomain.GetAll()
                .Where(y => y.ObjectCr.Id == resp.Id)
                .Select(y => new MobileTypeWorkCr
                {
                    Work = y.Work.Name,
                    Year = y.YearRepair,
                    Volume = y.Volume,
                    Measure = y.Work.UnitMeasure.Name,
                    Sum = y.Sum,
                    Psd = y.HasPsd
                })
                .ToListAsync();

            resp.Protocols = await this.protocolCrDomain.GetAll()
                .Where(y => y.ObjectCr.Id == resp.Id)
                .Select(y => new Protocol
                {
                    DocumentType = y.TypeDocumentCr.Value,
                    Contragent = y.Contragent.Name,
                    Date = y.DateFrom,
                    Number = y.DocumentNum,
                    Description = y.Description,
                    FileId = y.File.Id
                })
                .ToListAsync();

            resp.DefectiveStatements = await this.defectListDomain.GetAll()
                .Where(y => y.ObjectCr.Id == resp.Id)
                .Select(y => new DefectiveStatementGet
                {
                    Id = y.Id,
                    State = y.State.GetStateInfo(),
                    Date = y.DocumentDate,
                    Work = y.Work.Name,
                    Sum = y.Sum,
                    FileId = y.File.Id
                })
                .ToListAsync();

            resp.Estimates = await this.estimateCalculationDomain.GetAll()
                .Where(y => y.ObjectCr.Id == resp.Id)
                .Select(y => new EstimateGet
                {
                    Id = y.Id,
                    State = y.State.GetStateInfo(),
                    Work = y.TypeWorkCr.Work.Name,
                    FinancingSection = y.TypeWorkCr.FinanceSource.Name,
                    SumTotal = y.TotalEstimate,
                    Sum = this.estimateDomain.GetAll().Where(z => z.EstimateCalculation == y).Sum(z => z.TotalCost),
                    EstimateFileName = y.EstimateDocumentName,
                    EstimateFileNumber = y.EstimateDocumentNum,
                    EstimateFileDate = y.EstimateDateFrom,
                    EstimateFileId = y.EstimateFile.Id,
                    ResourceFileName = y.ResourceStatmentDocumentName,
                    ResourceFileNumber = y.ResourceStatmentDocumentNum,
                    ResourceFileDate = y.ResourceStatmentDateFrom,
                    ResourceFileId = y.ResourceStatmentFile.Id
                })
                .ToListAsync();

            var buildContractTypeWorks = this.buildContractTypeWorkDomain.GetAll()
                .Where(x => x.BuildContract.ObjectCr.Id == resp.Id)
                .GroupBy(x => x.BuildContract.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(z => new WorkCr { Work = z.TypeWork.Work.Name, Sum = z.Sum })
                );

            resp.Contracts = (await this.buildContractDomain.GetAll()
                    .Where(y => y.ObjectCr.Id == resp.Id)
                    .ToListAsync())
                .Select(y => new ContractGet
                {
                    Id = y.Id,
                    State = y.State.GetStateInfo(),
                    Work = buildContractTypeWorks.Get(y.Id),
                    Date = y.DocumentDateFrom,
                    Number = y.DocumentNum,
                    Customer = y.Contragent?.Name,
                    ContractOrganization = y.Builder?.Contragent?.Name,
                    TypeContractBuild = y.TypeContractBuild,
                    Sum = y.Sum,
                    StartDateWork = y.DateStartWork,
                    EndDateWork = y.DateEndWork,
                    FileId = y.DocumentFile?.Id
                });

            resp.Acts = await this.performedWorkActDomain.GetAll()
                .Where(y => y.ObjectCr.Id == resp.Id)
                .Select(y => new ActGet
                {
                    Id = y.Id,
                    State = y.State.GetStateInfo(),
                    Work = y.TypeWorkCr.Work.Name,
                    Date = y.DateFrom,
                    Volume = y.Volume,
                    Sum = y.Sum,
                    FileId = y.DocumentFile.Id
                })
                .ToListAsync();

            return resp;
        }

        /// <inheritdoc />
        protected override long UpdateEntity(long objectId, ObjectCrUpdate updateObject)
        {
            var objectCr = this.objectCrDomain.Get(objectId);

            if (objectCr.IsNull())
                throw new ApiServiceException("Не найден объект для обновления");

            this.UpdateEntities(updateObject.DefectiveStatements, this.DefectiveStatementTransferValues<DefectiveStatementUpdate, StateInfoUpdate>());
            this.UpdateEntities(updateObject.Estimates, this.EstimateTransfer<EstimateUpdate, StateInfoUpdate>());
            this.UpdateEntities(updateObject.Contracts, this.ContractTransfer<ContractUpdate, StateInfoUpdate>());
            this.UpdateEntities(updateObject.Acts, this.ActTransfer<ActUpdate, StateInfoUpdate>());

            return objectCr.Id;
        }

        /// <summary>
        /// Перенос информации для <see cref="DefectList"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TStateInfo">Тип модели с информацией о статусе</typeparam>
        private TransferValues<TModel, DefectList> DefectiveStatementTransferValues<TModel, TStateInfo>()
            where TModel : BaseDefectiveStatement<TStateInfo>
            where TStateInfo : BaseStateInfo =>
            (TModel model, ref DefectList defectList, object mainEntity) => this.ChangeState(defectList, model.State, "дефективной ведомости");

        /// <summary>
        /// Перенос информации для <see cref="EstimateCalculation"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TStateInfo">Тип модели с информацией о статусе</typeparam>
        private TransferValues<TModel, EstimateCalculation> EstimateTransfer<TModel, TStateInfo>()
            where TModel : BaseEstimate<TStateInfo>
            where TStateInfo : BaseStateInfo =>
            (TModel model, ref EstimateCalculation estimateCalculation, object mainEntity) => this.ChangeState(estimateCalculation, model.State, "сметы");

        /// <summary>
        /// Перенос информации для <see cref="BuildContract"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TStateInfo">Тип модели с информацией о статусе</typeparam>
        private TransferValues<TModel, BuildContract> ContractTransfer<TModel, TStateInfo>()
            where TModel : BaseContract<TStateInfo>
            where TStateInfo : BaseStateInfo =>
            (TModel model, ref BuildContract buildContract, object mainEntity) => this.ChangeState(buildContract, model.State, "договора");

        /// <summary>
        /// Перенос информации для <see cref="PerformedWorkAct"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TStateInfo">Тип модели с информацией о статусе</typeparam>
        private TransferValues<TModel, PerformedWorkAct> ActTransfer<TModel, TStateInfo>()
            where TModel : BaseAct<TStateInfo>
            where TStateInfo : BaseStateInfo =>
            (TModel model, ref PerformedWorkAct performedWorkAct, object mainEntity) =>
                this.ChangeState(performedWorkAct, model.State, "акта выполненных работ");

        /// <summary>
        /// Изменить статус
        /// </summary>
        /// <param name="entity">Сущность для смены статуса</param>
        /// <param name="newStateInfo">Информация о новом статусе</param>
        /// <param name="notAvailableMessagePostFix">Постфикс стандартного сообщения о невозможности смены статуса</param>
        /// <param name="notAvailableMessage">Сообщение для подмены стандартного сообщения о невозможности смены статуса</param>
        /// <typeparam name="TEntity">Тип сущности для смены статуса</typeparam>
        /// <typeparam name="TStateInfo">Тип модели с информацией о новом статусе</typeparam>
        private void ChangeState<TEntity, TStateInfo>(
            TEntity entity,
            TStateInfo newStateInfo,
            string notAvailableMessagePostFix = null,
            string notAvailableMessage = null)
            where TEntity : PersistentObject, IStatefulEntity
            where TStateInfo : BaseStateInfo =>
            StateInfoExtensions.ChangeState(
                this.stateProvider,
                this.stateDomain,
                this.processedStatesDict,
                entity,
                newStateInfo,
                notAvailableMessagePostFix,
                notAvailableMessage);
    }
}