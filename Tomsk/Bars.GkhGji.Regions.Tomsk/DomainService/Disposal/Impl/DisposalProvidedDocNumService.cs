namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class DisposalProvidedDocNumService : IDisposalProvidedDocNumService
    {
        public IDomainService<DisposalProvidedDocNum> ServiceProvidedDocument { get; set; }

        public IDataResult AddProvideDocNum(BaseParams baseParams)
        {
            var dispId = baseParams.Params.GetAs<long>("dispId");
            var provideDocumentsNum = baseParams.Params.GetAs<long>("provideDocumentsNum");

            if (dispId == 0)
            {
                throw new Exception("Нет приказа");
            }

            var existingValue = ServiceProvidedDocument.GetAll().FirstOrDefault(x => x.Disposal.Id == dispId);

            if (existingValue != null)
            {
                existingValue.ProvideDocumentsNum = provideDocumentsNum;
            }
            else
            {
                existingValue = new DisposalProvidedDocNum
                {
                    Disposal = new Disposal{ Id = dispId },
                    ProvideDocumentsNum = provideDocumentsNum
                };
            }

            ServiceProvidedDocument.Save(existingValue);

            return new BaseDataResult();
        }
    }
}
