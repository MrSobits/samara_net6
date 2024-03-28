namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;

    public class MVDLivingPlaceRegistrationFileViewModel : BaseViewModel<MVDLivingPlaceRegistrationFile>
    {
        public override IDataResult List(IDomainService<MVDLivingPlaceRegistrationFile> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = loadParams.Filter.GetAs("smevMVD", 0L);
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);

            var data = domain.GetAll()
             .Where(x => x.MVDLivingPlaceRegistration.Id == id)
            .Select(x => new
            {
                x.Id,
                x.FileInfo,
                x.SMEVFileType
            })
            .AsQueryable()
            .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());        

        }
    }
}