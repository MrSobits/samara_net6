namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVPremises;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using Bars.B4.Modules.FileStorage;

    public class SMEVPremisesViewModel : BaseViewModel<SMEVPremises>
    {
        public override IDataResult List(IDomainService<SMEVPremises> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    ReqId = x.Id,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.CalcDate,
                    x.OKTMO,
                    x.ActDate,
                    x.ActDepartment,
                    x.ActName,
                    x.ActNumber,
                    x.Answer
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<SMEVPremises> domain, BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var loadParams = baseParams.GetLoadParam();
            long id = Convert.ToInt64(baseParams.Params.Get("id"));

            var file = domain.Get(id).AnswerFile;
            if (file == null)
            {
                var data = domain.GetAll()
                    .Where(x => x.Id == id)
                    .AsQueryable()
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                Stream docStream = fileManager.GetFile(file);
                var xDoc = LoadFromStream(docStream);
                var premises = DeSerializer(xDoc);

                var data = domain.GetAll()
                      .Where(x => x.Id == id)
                      .Select(x => new
                      {
                          x.Id,
                          x.ActDate,
                          x.ActDepartment,
                          x.ActName,
                          x.ActNumber,
                          x.Answer,
                          x.AnswerFile,
                          x.CalcDate,
                          x.Inspector,
                          x.MessageId,
                          x.ObjectCreateDate,
                          x.ObjectEditDate,
                          x.ObjectVersion,
                          x.OKTMO,
                          x.RequestState,
                          premises.Employee.EmployeeName,
                          premises.Employee.EmployeePost,
                          premises.Employee.Department,
                          premises.PremisesDetails.PremisesInfo,
                          premises.PremisesDetails.PremisesAddress.Region,
                          premises.PremisesDetails.PremisesAddress.District,
                          premises.PremisesDetails.PremisesAddress.City,
                          premises.PremisesDetails.PremisesAddress.Locality,
                          premises.PremisesDetails.PremisesAddress.Street,
                          premises.PremisesDetails.PremisesAddress.House,
                          premises.PremisesDetails.PremisesAddress.Housing,
                          premises.PremisesDetails.PremisesAddress.Building,
                          premises.PremisesDetails.PremisesAddress.Apartment,
                          premises.PremisesDetails.PremisesAddress.Index,
                          premises.PremisesDetails.CadastralNumber,
                          premises.PremisesDetails.PropertyRightsDate,
                          DocRightNumber = premises.PremisesDetails.DocumentRightsDetails.Number,
                          DocRightDate = premises.PremisesDetails.DocumentRightsDetails.Date,
                          premises.PremisesDetails.RightholderInfo,
                          SupervisionDetails = string.Join("; ", premises.PremisesDetails.SupervisionDetails.Select(y => $"{y.Number}, {y.Date}")),
                          InsNumber = premises.PremisesDetails.InspectionDetails.Number,
                          InsDate = premises.PremisesDetails.InspectionDetails.Date,
                          ConNumber = premises.PremisesDetails.ConclusionDetails.Number,
                          ConDate = premises.PremisesDetails.ConclusionDetails.Date
                      })
                      .AsQueryable()
                      .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }

        static MstrUnfitPremisesResponse DeSerializer(XDocument document)
        {
            var serializer = new XmlSerializer(typeof(MstrUnfitPremisesResponse));
            return (MstrUnfitPremisesResponse)serializer.Deserialize(document.CreateReader());
        }

        static XDocument LoadFromStream(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                return XDocument.Load(reader);
            }
        }
    }
}

