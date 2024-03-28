namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason;
    using Bars.Gkh.InspectorMobile.Extensions;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сервис для работы с <see cref="HeatSeason"/> 
    /// </summary>
    public class HeatingSeasonService : BaseApiService<HeatSeason, object, HeatingSeasonObjectUpdate>, IHeatingSeasonService
    {
        #region DependencyInjection
        private readonly IStateProvider stateProvider;
        private readonly IDomainService<State> stateDomain;
        private readonly IDomainService<HeatSeason> heatSeasonDomain;
        private readonly IDomainService<HeatSeasonDoc> heatSeasonDocDomain;

        public HeatingSeasonService(
            IStateProvider stateProvider,
            IDomainService<State> stateDomain,
            IDomainService<HeatSeason> heatSeasonDomain,
            IDomainService<HeatSeasonDoc> heatSeasonDocDomain)
        {
            this.stateProvider = stateProvider;
            this.stateDomain = stateDomain;
            this.heatSeasonDomain = heatSeasonDomain;
            this.heatSeasonDocDomain = heatSeasonDocDomain;
        }
        #endregion

        /// <summary>
        /// Словарь обработанных статусов
        /// </summary>
        private Dictionary<long, State> processedStatesDict = new Dictionary<long, State>();

        /// <inheritdoc />
        public HeatingSeasonObjectGet Get(long heatingSeasonPeriodId, long addressId) =>
            this.GetList(heatingSeasonPeriodId, new[] { addressId })?.FirstOrDefault();

        /// <inheritdoc />
        public IEnumerable<HeatingSeasonObjectGet> GetList(long heatingSeasonPeriodId, long[] addressIds)
        {
            return this.heatSeasonDomain.GetAll()
                .Where(x => x.Period != null && x.Period.Id == heatingSeasonPeriodId)
                .Where(x => x.RealityObject != null && addressIds.Contains(x.RealityObject.Id))
                .Join(this.heatSeasonDocDomain.GetAll(),
                    obj => obj.Id,
                    doc => doc.HeatingSeason.Id,
                    (obj, doc) => new { obj, doc })
                .Where(x => x.doc.State != null && x.doc.State.Name != null && x.doc.File != null)
                .AsEnumerable()
                .GroupBy(x => x.obj,
                    (x, y) => new HeatingSeasonObjectGet
                    {
                        Id = x.Id,
                        HeatingSeasonPeriodId = x.Period.Id,
                        AddressId = x.RealityObject.Id,
                        Documents = y.Select(z => new HeatingSeasonDocumentGet
                        {
                            Id = z.doc.Id,
                            State = z.doc.State.GetStateInfo(),
                            DocumentType = z.doc.TypeDocument,
                            Number = z.doc.DocumentNumber,
                            Date = z.doc.DocumentDate,
                            Description = z.doc.Description,
                            FileId = z.doc.File.Id
                        }).ToArray()
                    });
        }

        /// <inheritdoc />
        protected override long UpdateEntity(long objectId, HeatingSeasonObjectUpdate updateObject)
        {
            var heatObject = this.heatSeasonDomain.Get(objectId);

            if (heatObject.IsNull())
                throw new ApiServiceException("Не найден объект для обновления");

            this.UpdateEntities(updateObject.Documents, this.DocumentsTransfer<HeatingSeasonDocumentUpdate, StateInfoUpdate>());

            return heatObject.Id;
        }

        /// <summary>
        /// Перенос информации для <see cref="HeatSeasonDoc"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        /// <typeparam name="TStateInfo">Тип модели с информацией о статусе</typeparam>
        private TransferValues<TModel, HeatSeasonDoc> DocumentsTransfer<TModel, TStateInfo>()
            where TModel : BaseHeatingSeasonDocument<TStateInfo>
            where TStateInfo : BaseStateInfo =>
            (TModel model, ref HeatSeasonDoc heatSeasonDoc, object mainEntity) => StateInfoExtensions.ChangeState(
                this.stateProvider,
                this.stateDomain,
                this.processedStatesDict,
                heatSeasonDoc,
                model.State,
                "документа по отопительному сезону");
    }
}