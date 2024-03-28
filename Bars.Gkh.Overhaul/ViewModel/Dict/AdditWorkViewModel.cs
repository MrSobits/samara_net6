namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using B4;
    using Gkh.Entities.Dicts;
    using Entities;

    /// <summary>
    /// View model работы
    /// </summary>
    public class AdditWorkViewModel : Gkh.ViewModel.AdditWorkViewModel
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<AdditWork> domainService, BaseParams baseParams)
        {
            
            var id = baseParams.Params.GetAs<long>("workId");
            var addWork = domainService.GetAll().FirstOrDefault(x => x.Work.Id == id);

            if(addWork != null)
            {
                return new BaseDataResult(new
                {
                    addWork.Id,
                    addWork.Name,
                    addWork.Code,
                    addWork.Percentage,
                    addWork.Description,
                    addWork.Work
                });
            }

            return new BaseDataResult();
        }
    }
}
