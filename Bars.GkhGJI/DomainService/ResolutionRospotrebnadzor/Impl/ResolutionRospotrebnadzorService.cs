namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;

    using Gkh.Authentification;
    using Entities;
    using Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с постановлением Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorService : IResolutionRospotrebnadzorService
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить постановление Роспотребнадзора по ID документа
        /// </summary>
        /// <param name="documentId">ID ГЖИ документа</param>
        public IDataResult GetInfo(long? documentId)
        {
            try
            {
                var baseName = new StringBuilder();

                // Пробегаемся по документам на основе которого создано постановление
                var parents = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                    .Where(x => x.Children.Id == documentId)
                    .Select(x => new
                    {
                        parentId = x.Parent.Id,
                        x.Parent.TypeDocumentGji,
                        x.Parent.DocumentDate,
                        x.Parent.DocumentNumber
                    })
                    .ToList();

                foreach (var doc in parents)
                {
                    var docName = Utils.GetDocumentName(doc.TypeDocumentGji);

                    if (baseName.Length > 0)
                    {
                        baseName.Append(", ");
                    }

                    baseName.AppendFormat("{0} №{1} от {2}", docName, doc.DocumentNumber, doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                return new BaseDataResult(baseName.ToString());
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

        /// <summary>
        /// Список постановлений Роспотребнадзора
        /// </summary>
        public IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = this.GetViewList(baseParams);
            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        /// <summary>
        /// Список view постановлений Роспотребнадзора
        /// </summary>
        public IQueryable<ViewResolutionRospotrebnadzor> GetViewList(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var viewDomain = this.Container.ResolveDomain<ViewResolutionRospotrebnadzor>();
            var loadParam = baseParams.GetLoadParam();

            /*
            * В качестве фильтров приходят следующие параметры
            * dateStart - Необходимо получить документы больше даты начала
            * dateEnd - Необходимо получить документы меньше даты окончания
            * realityObjectId - Необходимо получить документы по дому
            */
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            try
            {
                var municipalityList = userManager.GetMunicipalityIds();

                return viewDomain.GetAll()
                    .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value))
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                    .Select(x => new ViewResolutionRospotrebnadzor
                    {
                        Id = x.Id,
                        State = x.State,
                        ResolutionId = x.ResolutionId,
                        SumPays = x.SumPays,
                        RealityObjectIds = x.RealityObjectIds,
                        MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                        MoNames = x.MoNames,
                        PlaceNames = x.PlaceNames,
                        MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                        OfficialName = x.OfficialName,
                        OfficialId = x.OfficialId,
                        PenaltyAmount = x.PenaltyAmount,
                        InspectionId = x.InspectionId,
                        TypeBase = x.TypeBase,
                        TypeExecutant = x.TypeExecutant,
                        Sanction = x.Sanction,
                        ContragentMuId = x.ContragentMuId,
                        ContragentMuName = x.ContragentMuName,
                        ContragentName = x.ContragentName,
                        DocumentDate = x.DocumentDate,
                        DocumentNumber = x.DocumentNumber,
                        DocumentNum = x.DocumentNum,
                        TypeDocumentGji = x.TypeDocumentGji,
                        DeliveryDate = x.DeliveryDate,
                        Paided = x.Paided,
                        RoAddress = x.RoAddress
                    })
                    .Filter(loadParam, this.Container);
            }
            finally
            {
                this.Container.Release(viewDomain);
            }
        }

        /// <summary>
        /// Добавить список статей закона к постановлению Роспотребнадзора
        /// </summary>
        public IDataResult AddArticleLawList(BaseParams baseParams)
        {
            var resolutionId = baseParams.Params.GetAsId("documentId");

            var articleLawIds = baseParams.Params.GetAs("articleLawIds", "");

            var articleLaw = !string.IsNullOrEmpty(articleLawIds)
                ? articleLawIds.Split(',').Select(id => id.ToLong()).ToList()
                : new List<long>();

            if (articleLaw.Count == 0)
            {
                return BaseDataResult.Error("Не выбраны статьи закона");
            }

            var gjiArticleLawDomain = this.Container.ResolveDomain<ArticleLawGji>();
            var resolutionArticleLawDomain = this.Container.ResolveDomain<ResolutionRospotrebnadzorArticleLaw>();
            var resolutionDomain = this.Container.ResolveDomain<ResolutionRospotrebnadzor>();
            try
            {

                var resolution = resolutionDomain.GetAll().Single(x => x.Id == resolutionId);
                var articleLawList = gjiArticleLawDomain.GetAll().Where(x => articleLaw.Contains(x.Id)).ToList();

                this.Container.InTransaction(() =>
                {
                    articleLawList.ForEach(x =>
                    {
                        resolutionArticleLawDomain.Save(new ResolutionRospotrebnadzorArticleLaw()
                        {
                            ArticleLaw = x,
                            Resolution = resolution
                        });
                    });
                });

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(gjiArticleLawDomain);
                this.Container.Release(resolutionArticleLawDomain);
                this.Container.Release(resolutionDomain);
            }
        }
    }
}