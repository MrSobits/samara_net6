namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.GkhCr.Entities;

    public class SpecialBuildContractDomainService : FileStorageDomainService<SpecialBuildContract>
    {
        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");
            this.InTransaction(() =>
            {
                foreach (var id in ids)
                {
                    var filesForDelete = this.GetFileInfoValues(this.Get(id));
                    this.DeleteInternal(id);
                    foreach (var file in filesForDelete.Where(file => file != null))
                    {
                        var value = this.Repository.Get(file.Id);
                        if (value != null)
                        {
                            this.FileInfoService.Delete(file.Id);
                        }
                    }
                }
            });

            return new BaseDataResult(ids);
        }

        public override void Delete(object id)
        {
            this.InTransaction(() =>
            {
                var filesForDelete = this.GetFileInfoValues(this.Get(id));
                this.DeleteInternal(id);
                foreach (var file in filesForDelete.Where(file => file != null))
                {
                    var value = this.Repository.Get(file.Id);
                    if (value != null)
                    {
                        this.FileInfoService.Delete(file.Id);
                    }
                }
            });
        }
    }
}