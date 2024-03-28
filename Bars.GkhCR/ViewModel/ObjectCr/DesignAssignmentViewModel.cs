namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using Bars.Gkh.Utils;

    using Entities;
    using Gkh.Domain;

    using NHibernate.Linq;

    /// <summary>
    /// ViewModel for <see cref="DesignAssignment"/>
    /// </summary>
    public class DesignAssignmentViewModel : BaseViewModel<DesignAssignment>
    {
        /// <summary>
        /// Домен-сервис <see cref="DesignAssignmentTypeWorkCr"/>
        /// </summary>
        public IDomainService<DesignAssignmentTypeWorkCr> DesignAssignmentTypeWorkCrDomain { get; set; }

        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<DesignAssignment> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAsId("objectCrId");
            }

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            var dataList = data
                .FetchMany(x => x.TypeWorksCr)
                .ThenFetch(x => x.TypeWorkCr)
                .ThenFetch(x => x.Work)
                .Order(loadParams)
                .Paging(loadParams)
                .ToList();


            var result = dataList.Select(x => new
            {
                x.ObjectCr,
                x.Id,
                x.State,
                x.Document,
                x.Date,
                x.DocumentFile,
                TypeWorksCr = x.TypeWorksCr.Select(y => y.TypeWorkCr.Work.Name).AggregateWithSeparator(", ")
            });

            return new ListDataResult(result, totalCount);
        }

        /// <summary>Получить объект</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult Get(IDomainService<DesignAssignment> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var value = domainService.Get(id);

            if (value == null)
            {
                return new BaseDataResult();
            }

            

            return new BaseDataResult(new
            {
                value.ObjectCr,
                value.Id,
                value.State,
                value.Document,
                value.Date,
                value.DocumentFile,
                value.UsedInExport,
                value.TypeWorkCr,
                TypeWorksCr = value.TypeWorksCr.Select(y => new { y.TypeWorkCr.Id, WorkName = y.TypeWorkCr.Work.Name })
            });
        }
    }
}