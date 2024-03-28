namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVOwnershipProperty;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using System.IO;
    using System.Xml;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Regions.Voronezh.SGIO.Ownership;

    public class SMEVOwnershipPropertyViewModel : BaseViewModel<SMEVOwnershipProperty>
    {
        public override IDataResult List(IDomainService<SMEVOwnershipProperty> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ReqId = x.Id,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    x.CalcDate,
                    x.QueryType,
                    x.PublicPropertyLevel,
                    Municipality = x.Municipality != null ? x.Municipality.Name : "",
                    Value = x.QueryType == Enums.QueryTypeType.AddressQuery ? x.RealityObject.Address :
                        x.QueryType == Enums.QueryTypeType.CadasterNumberQuery ? x.CadasterNumber :
                        x.QueryType == Enums.QueryTypeType.RegisterNumberQuery ? x.RegisterNumber : "",
                    x.Answer
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<SMEVOwnershipProperty> domain, BaseParams baseParams)
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
                var excerptType = DeSerializer(xDoc);
                if (excerptType.Type == ExcerptTypeType.Land)
                {

                    var data = GetDataLand(excerptType, loadParams, domain, id);
                    return data;
                }
                else if (excerptType.Type == ExcerptTypeType.Building)
                {

                    var data = GetDataBuilding(excerptType, loadParams, domain, id);
                    return data;
                }
                else if (excerptType.Type == ExcerptTypeType.Premise)
                {

                    var data = GetDataPremise(excerptType, loadParams, domain, id);
                    return data;
                }
                else
                {
                    var data = GetDataDefault(excerptType, loadParams, domain, id);
                    return data;
                }


            }
        }

        private ListDataResult GetDataDefault(ExcerptType excerptType, LoadParam loadParams, IDomainService<SMEVOwnershipProperty> domain, long id)
        {            
            if (excerptType != null)
            {
                ExcerptDocumentType docRemoval = excerptType.RemovalFromTurnoverDocument != null ? excerptType.RemovalFromTurnoverDocument.Length > 0 ? excerptType.RemovalFromTurnoverDocument[0] : null : null;
                ExcerptDocumentType docRight = excerptType.OwnershipRight != null ? excerptType.OwnershipRight.Document != null ? excerptType.OwnershipRight.Document.Length > 0 ? excerptType.OwnershipRight.Document[0] : null : null : null;
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     excerptType.Type,
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                     excerptType.ResidualCost,
                     //excerpt.HasPartOfRights,
                     //excerpt.ShareOfRight.Share,
                     //PName = excerpt.ShareOfRight.Participant.Name,
                     //POGRN = excerpt.ShareOfRight.Participant.OGRN,
                     //PShare = excerpt.ShareOfRight.Participant.Share,
                     //CNum = excerpt.CadasterNumber.Number,
                     //excerpt.CadasterNumber.NumberType,
                     //CDate = excerpt.CadasterNumber.Date,
                     //excerpt.Address.AddressFull,
                     //excerpt.OwnershipRight.RegistrationDate,
                     RegistrationNumber = excerptType.OwnershipRight != null ? excerptType.OwnershipRight.RegistrationNumber : "",
                     Name = docRight != null ? docRight.Name : "",
                     ORDocNum = docRight != null ? docRight.Number : "",
                     ORDocDate = docRight != null ? docRight.Date : DateTime.MinValue,
                     //ORRName = excerpt.OtherRealRight.Name,
                     //ORRRegDate = excerpt.OtherRealRight.RegistrationDate,
                     //ORRRegNum = excerpt.OtherRealRight.RegistrationNumber,
                     //ORRDocName = excerpt.OtherRealRight.Document.Name,
                     //ORRDocNum = excerpt.OtherRealRight.Document.Number,
                     //ORRDocDate = excerpt.OtherRealRight.Document.Date,
                     //ChPartName = excerpt.Charge.PartName,
                     //ChPartType = excerpt.Charge.PartType,
                     //ChArea = excerpt.Charge.Area,
                     //ChType = excerpt.Charge.Type,
                     //ChNumber = excerpt.Charge.Number,
                     //ChDate = excerpt.Charge.Date,
                     //ChStart = excerpt.Charge.Started,
                     //ChEnd = excerpt.Charge.Ended,
                     //ChChType = excerpt.Charge.Charger.Type,
                     //ChChName = excerpt.Charge.Charger.Name,
                     //excerpt.Charge.Charger.CardNumber,
                     //excerpt.Charge.Charger.OGRN,
                     //excerpt.Charge.Charger.OGRNDate,
                     //excerpt.Charge.Charger.INN,
                     //excerpt.Charge.Charger.SNILS,
                     //excerpt.Charge.Charger.LastName,
                     //excerpt.Charge.Charger.FirstName,
                     //excerpt.Charge.Charger.MiddleName,
                     //ChChPassSeries = excerpt.Charge.Charger.Passport.Series,
                     //ChChPassNum = excerpt.Charge.Charger.Passport.Number,
                     //ChChPassIsBy = excerpt.Charge.Charger.Passport.IssuedBy,
                     //ChChPassIsOn = excerpt.Charge.Charger.Passport.IssuedOn,
                     TDName = docRemoval != null ? docRemoval.Name : "",
                     TDNum = docRemoval != null ? docRemoval.Number : "",
                     TDDate = docRemoval != null ? docRemoval.Date : DateTime.MinValue,
                     RhType = excerptType.Rightholder.Type,
                     RhName = excerptType.Rightholder.Name,
                     RhCNum = excerptType.Rightholder.CardNumber,
                     RhOGRN = excerptType.Rightholder.OGRN,
                     RhOGRNDate = excerptType.Rightholder.OGRNDate,
                     RhINN = excerptType.Rightholder.INN,
                     RhSNILS = excerptType.Rightholder.SNILS,
                     RhLastName = excerptType.Rightholder.LastName,
                     RhFirstName = excerptType.Rightholder.FirstName,
                     RhMiddleName = excerptType.Rightholder.MiddleName,
                     RhPassNumber = excerptType.Rightholder.Passport.Number,
                     RhPassIsBy = excerptType.Rightholder.Passport.IssuedBy,
                     RhPassIsOn = excerptType.Rightholder.Passport.IssuedOn,
                     //   RhAddress = excerpt.Rightholder.Address.AddressFull,
                     OtherTypeOfCostName = excerptType.OtherCost != null ? excerptType.OtherCost.OtherTypeOfCostName : "",
                     CostOfOtherTypeOfLand = excerptType.OtherCost != null ? excerptType.OtherCost.CostOfOtherTypeOfLand : 0,
                     //  excerpt.Ship.ShipType,
                     //   ShipName = excerpt.Ship.Name,
                     //excerpt.Ship.PurposeType,
                     //excerpt.Ship.Purpose,
                     //ShipRegNum = excerpt.Ship.RegistrationNumber,
                     //ShipRegDate = excerpt.Ship.RegistrationDate,
                     //excerpt.Ship.ConstructionYear,
                     //excerpt.Ship.ConstructionPlace,
                     //excerpt.Ship.Airport,
                     //excerpt.Ship.Harbor,
                     //excerpt.Ship.InventoryNumber,
                     //excerpt.Ship.SerialNumber
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
        private ListDataResult GetDataPremise(ExcerptType excerptType, LoadParam loadParams, IDomainService<SMEVOwnershipProperty> domain, long id)
        {
            var excerpt = (ExcerptPremiseType)excerptType.Item;
            if (excerpt != null && !string.IsNullOrEmpty(excerpt.Name) && excerpt.Name.Contains("отсутствует") && excerpt.Name.Contains("информация"))
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     Answer = "в запросе отсутствует информация о запрашиваемом объекте",
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     Type = "Помещение",
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else if (excerptType != null && excerptType.OwnershipRight != null)
            {
                ExcerptDocumentType docRemoval = excerptType.RemovalFromTurnoverDocument != null ? excerptType.RemovalFromTurnoverDocument.Length > 0 ? excerptType.RemovalFromTurnoverDocument[0] : null : null;
                ExcerptDocumentType docRight = excerptType.OwnershipRight != null ? excerptType.OwnershipRight.Document != null ? excerptType.OwnershipRight.Document.Length > 0 ? excerptType.OwnershipRight.Document[0] : null : null : null;
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     Type = "Помещение",
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                     excerptType.ResidualCost,
                     //excerpt.HasPartOfRights,
                     //excerpt.ShareOfRight.Share,
                     //PName = excerpt.ShareOfRight.Participant.Name,
                     //POGRN = excerpt.ShareOfRight.Participant.OGRN,
                     //PShare = excerpt.ShareOfRight.Participant.Share,
                     //CNum = excerpt.CadasterNumber.Number,
                     //excerpt.CadasterNumber.NumberType,
                     //CDate = excerpt.CadasterNumber.Date,
                     //excerpt.Address.AddressFull,
                     //excerpt.OwnershipRight.RegistrationDate,
                     RegistrationNumber = excerptType.OwnershipRight != null ? excerptType.OwnershipRight.RegistrationNumber : "",
                     Name = docRight != null ? docRight.Name : "",
                     ORDocNum = docRight != null ? docRight.Number : "",
                     ORDocDate = docRight != null ? docRight.Date : DateTime.MinValue,
                     //ORRName = excerpt.OtherRealRight.Name,
                     //ORRRegDate = excerpt.OtherRealRight.RegistrationDate,
                     //ORRRegNum = excerpt.OtherRealRight.RegistrationNumber,
                     //ORRDocName = excerpt.OtherRealRight.Document.Name,
                     //ORRDocNum = excerpt.OtherRealRight.Document.Number,
                     //ORRDocDate = excerpt.OtherRealRight.Document.Date,
                     //ChPartName = excerpt.Charge.PartName,
                     //ChPartType = excerpt.Charge.PartType,
                     //ChArea = excerpt.Charge.Area,
                     //ChType = excerpt.Charge.Type,
                     //ChNumber = excerpt.Charge.Number,
                     //ChDate = excerpt.Charge.Date,
                     //ChStart = excerpt.Charge.Started,
                     //ChEnd = excerpt.Charge.Ended,
                     //ChChType = excerpt.Charge.Charger.Type,
                     //ChChName = excerpt.Charge.Charger.Name,
                     //excerpt.Charge.Charger.CardNumber,
                     //excerpt.Charge.Charger.OGRN,
                     //excerpt.Charge.Charger.OGRNDate,
                     //excerpt.Charge.Charger.INN,
                     //excerpt.Charge.Charger.SNILS,
                     //excerpt.Charge.Charger.LastName,
                     //excerpt.Charge.Charger.FirstName,
                     //excerpt.Charge.Charger.MiddleName,
                     //ChChPassSeries = excerpt.Charge.Charger.Passport.Series,
                     //ChChPassNum = excerpt.Charge.Charger.Passport.Number,
                     //ChChPassIsBy = excerpt.Charge.Charger.Passport.IssuedBy,
                     //ChChPassIsOn = excerpt.Charge.Charger.Passport.IssuedOn,
                     TDName = docRemoval != null ? docRemoval.Name : "",
                     TDNum = docRemoval != null ? docRemoval.Number : "",
                     TDDate = docRemoval != null ? docRemoval.Date : DateTime.MinValue,
                     RhType = excerptType.Rightholder.Type,
                     RhName = excerptType.Rightholder.Name,
                     RhCNum = excerptType.Rightholder.CardNumber,
                     RhOGRN = excerptType.Rightholder.OGRN,
                     RhOGRNDate = excerptType.Rightholder.OGRNDate,
                     RhINN = excerptType.Rightholder.INN,
                     RhSNILS = excerptType.Rightholder.SNILS,
                     RhLastName = excerptType.Rightholder.LastName,
                     RhFirstName = excerptType.Rightholder.FirstName,
                     RhMiddleName = excerptType.Rightholder.MiddleName,
                     RhPassNumber = excerptType.Rightholder.Passport.Number,
                     RhPassIsBy = excerptType.Rightholder.Passport.IssuedBy,
                     RhPassIsOn = excerptType.Rightholder.Passport.IssuedOn,
                     //   RhAddress = excerpt.Rightholder.Address.AddressFull,
                     OtherTypeOfCostName = excerptType.OtherCost != null ? excerptType.OtherCost.OtherTypeOfCostName : "",
                     CostOfOtherTypeOfLand = excerptType.OtherCost != null ? excerptType.OtherCost.CostOfOtherTypeOfLand : 0,
                     //  excerpt.Ship.ShipType,
                     //   ShipName = excerpt.Ship.Name,
                     //excerpt.Ship.PurposeType,
                     //excerpt.Ship.Purpose,
                     //ShipRegNum = excerpt.Ship.RegistrationNumber,
                     //ShipRegDate = excerpt.Ship.RegistrationDate,
                     //excerpt.Ship.ConstructionYear,
                     //excerpt.Ship.ConstructionPlace,
                     //excerpt.Ship.Airport,
                     //excerpt.Ship.Harbor,
                     //excerpt.Ship.InventoryNumber,
                     //excerpt.Ship.SerialNumber
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else if (excerptType != null)
            {
                var data = domain.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.QueryType,
                    x.PublicPropertyLevel,
                    x.RealityObject,
                    x.CadasterNumber,
                    x.Answer,
                    x.AnswerFile,
                    x.AttachmentFile,
                    x.CalcDate,
                    x.Inspector,
                    x.MessageId,
                    excerptType.Number,
                    Date = excerptType.Date.ToShortDateString(),
                    excerptType.Issuer,
                    //    excerpt.IssuingDepartment,
                    excerptType.SignerFullName,
                    excerptType.SignerPosition,
                    Type = "Помещение",
                    RegNum = excerptType.RegisterNumber,
                    RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                    excerptType.InitialCost,
                    excerptType.ResidualCost,
                    x.Municipality,
                    x.ObjectCreateDate,
                    x.ObjectEditDate,
                    x.ObjectVersion,
                    x.RegisterNumber,
                    x.Room,
                    x.RequestState
                })
                .AsQueryable()
                .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
        private ListDataResult GetDataBuilding(ExcerptType excerptType, LoadParam loadParams, IDomainService<SMEVOwnershipProperty> domain, long id)
        {
            var excerpt = (ExcerptBuildingType)excerptType.Item;
            if (excerpt != null && !string.IsNullOrEmpty(excerpt.Name) && excerpt.Name.Contains("отсутствует") && excerpt.Name.Contains("информация"))
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     Answer = "в запросе отсутствует информация о запрашиваемом объекте",
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     Type = "Здание",
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else if (excerptType != null && excerptType.OwnershipRight != null)
            {
                ExcerptDocumentType docRemoval = excerptType.RemovalFromTurnoverDocument != null ? excerptType.RemovalFromTurnoverDocument.Length > 0 ? excerptType.RemovalFromTurnoverDocument[0] : null : null;
                ExcerptDocumentType docRight = excerptType.OwnershipRight != null ? excerptType.OwnershipRight.Document != null ? excerptType.OwnershipRight.Document.Length > 0 ? excerptType.OwnershipRight.Document[0] : null : null : null;
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     Type = "Здание",
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                     excerptType.ResidualCost,
                     //excerpt.HasPartOfRights,
                     //excerpt.ShareOfRight.Share,
                     //PName = excerpt.ShareOfRight.Participant.Name,
                     //POGRN = excerpt.ShareOfRight.Participant.OGRN,
                     //PShare = excerpt.ShareOfRight.Participant.Share,
                     //CNum = excerpt.CadasterNumber.Number,
                     //excerpt.CadasterNumber.NumberType,
                     //CDate = excerpt.CadasterNumber.Date,
                     //excerpt.Address.AddressFull,
                     //excerpt.OwnershipRight.RegistrationDate,
                     RegistrationNumber = excerptType.OwnershipRight != null ? excerptType.OwnershipRight.RegistrationNumber : "",
                     Name = docRight != null ? docRight.Name : "",
                     ORDocNum = docRight != null ? docRight.Number : "",
                     ORDocDate = docRight != null ? docRight.Date : DateTime.MinValue,
                     //ORRName = excerpt.OtherRealRight.Name,
                     //ORRRegDate = excerpt.OtherRealRight.RegistrationDate,
                     //ORRRegNum = excerpt.OtherRealRight.RegistrationNumber,
                     //ORRDocName = excerpt.OtherRealRight.Document.Name,
                     //ORRDocNum = excerpt.OtherRealRight.Document.Number,
                     //ORRDocDate = excerpt.OtherRealRight.Document.Date,
                     //ChPartName = excerpt.Charge.PartName,
                     //ChPartType = excerpt.Charge.PartType,
                     //ChArea = excerpt.Charge.Area,
                     //ChType = excerpt.Charge.Type,
                     //ChNumber = excerpt.Charge.Number,
                     //ChDate = excerpt.Charge.Date,
                     //ChStart = excerpt.Charge.Started,
                     //ChEnd = excerpt.Charge.Ended,
                     //ChChType = excerpt.Charge.Charger.Type,
                     //ChChName = excerpt.Charge.Charger.Name,
                     //excerpt.Charge.Charger.CardNumber,
                     //excerpt.Charge.Charger.OGRN,
                     //excerpt.Charge.Charger.OGRNDate,
                     //excerpt.Charge.Charger.INN,
                     //excerpt.Charge.Charger.SNILS,
                     //excerpt.Charge.Charger.LastName,
                     //excerpt.Charge.Charger.FirstName,
                     //excerpt.Charge.Charger.MiddleName,
                     //ChChPassSeries = excerpt.Charge.Charger.Passport.Series,
                     //ChChPassNum = excerpt.Charge.Charger.Passport.Number,
                     //ChChPassIsBy = excerpt.Charge.Charger.Passport.IssuedBy,
                     //ChChPassIsOn = excerpt.Charge.Charger.Passport.IssuedOn,
                     TDName = docRemoval != null ? docRemoval.Name : "",
                     TDNum = docRemoval != null ? docRemoval.Number : "",
                     TDDate = docRemoval != null ? docRemoval.Date : DateTime.MinValue,
                     RhType = excerptType.Rightholder.Type,
                     RhName = excerptType.Rightholder.Name,
                     RhCNum = excerptType.Rightholder.CardNumber,
                     RhOGRN = excerptType.Rightholder.OGRN,
                     RhOGRNDate = excerptType.Rightholder.OGRNDate,
                     RhINN = excerptType.Rightholder.INN,
                     RhSNILS = excerptType.Rightholder.SNILS,
                     RhLastName = excerptType.Rightholder.LastName,
                     RhFirstName = excerptType.Rightholder.FirstName,
                     RhMiddleName = excerptType.Rightholder.MiddleName,
                     RhPassNumber = excerptType.Rightholder.Passport.Number,
                     RhPassIsBy = excerptType.Rightholder.Passport.IssuedBy,
                     RhPassIsOn = excerptType.Rightholder.Passport.IssuedOn,
                     //   RhAddress = excerpt.Rightholder.Address.AddressFull,
                     OtherTypeOfCostName = excerptType.OtherCost != null ? excerptType.OtherCost.OtherTypeOfCostName : "",
                     CostOfOtherTypeOfLand = excerptType.OtherCost != null ? excerptType.OtherCost.CostOfOtherTypeOfLand : 0,
                     //  excerpt.Ship.ShipType,
                     //   ShipName = excerpt.Ship.Name,
                     //excerpt.Ship.PurposeType,
                     //excerpt.Ship.Purpose,
                     //ShipRegNum = excerpt.Ship.RegistrationNumber,
                     //ShipRegDate = excerpt.Ship.RegistrationDate,
                     //excerpt.Ship.ConstructionYear,
                     //excerpt.Ship.ConstructionPlace,
                     //excerpt.Ship.Airport,
                     //excerpt.Ship.Harbor,
                     //excerpt.Ship.InventoryNumber,
                     //excerpt.Ship.SerialNumber
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else if (excerptType != null)
            {
                var data = domain.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.QueryType,
                    x.PublicPropertyLevel,
                    x.RealityObject,
                    x.CadasterNumber,
                    x.Answer,
                    x.AnswerFile,
                    x.AttachmentFile,
                    x.CalcDate,
                    x.Inspector,
                    x.MessageId,
                    excerptType.Number,
                    Date = excerptType.Date.ToShortDateString(),
                    excerptType.Issuer,
                    //    excerpt.IssuingDepartment,
                    excerptType.SignerFullName,
                    excerptType.SignerPosition,
                    Type = "Здание",
                    RegNum = excerptType.RegisterNumber,
                    RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                    excerptType.InitialCost,
                    excerptType.ResidualCost,
                    x.Municipality,
                    x.ObjectCreateDate,
                    x.ObjectEditDate,
                    x.ObjectVersion,
                    x.RegisterNumber,
                    x.Room,
                    x.RequestState
                })
                .AsQueryable()
                .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
        private ListDataResult GetDataLand(ExcerptType excerptType, LoadParam loadParams, IDomainService<SMEVOwnershipProperty> domain, long id)
        {
            var excerpt = (ExcerptLandType)excerptType.Item;
            if (excerpt != null && !string.IsNullOrEmpty(excerpt.Name) && excerpt.Name.Contains("отсутствует") && excerpt.Name.Contains("информация"))
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     Answer = "в запросе отсутствует информация о запрашиваемом объекте",
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     Type = "Земельный участок",
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else if (excerpt != null && excerptType.OwnershipRight == null)
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     Answer = excerpt.Name,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     Type = "Земельный участок",
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else if (excerptType != null)
            {
                ExcerptDocumentType docRemoval = excerptType.RemovalFromTurnoverDocument != null? excerptType.RemovalFromTurnoverDocument.Length > 0 ? excerptType.RemovalFromTurnoverDocument[0] : null:null;
                ExcerptDocumentType docRight = excerptType.OwnershipRight != null? excerptType.OwnershipRight.Document != null ? excerptType.OwnershipRight.Document.Length > 0 ? excerptType.OwnershipRight.Document[0] : null:null:null;
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState,
                     excerptType.Number,
                     Date = excerptType.Date.ToShortDateString(),
                     excerptType.Issuer,
                     //    excerpt.IssuingDepartment,
                     excerptType.SignerFullName,
                     excerptType.SignerPosition,
                     Type = "Земельный участок",
                     RegNum = excerptType.RegisterNumber,
                     RegisterNumberDate = excerptType.RegisterNumberDate.ToShortDateString(),
                     excerptType.InitialCost,
                     excerptType.ResidualCost,
                         //excerpt.HasPartOfRights,
                         //excerpt.ShareOfRight.Share,
                         //PName = excerpt.ShareOfRight.Participant.Name,
                         //POGRN = excerpt.ShareOfRight.Participant.OGRN,
                         //PShare = excerpt.ShareOfRight.Participant.Share,
                         //CNum = excerpt.CadasterNumber.Number,
                         //excerpt.CadasterNumber.NumberType,
                         //CDate = excerpt.CadasterNumber.Date,
                         //excerpt.Address.AddressFull,
                         //excerpt.OwnershipRight.RegistrationDate,
                         RegistrationNumber = excerptType.OwnershipRight != null ? excerptType.OwnershipRight.RegistrationNumber : "",
                     Name = docRight != null ? docRight.Name : "",
                     ORDocNum = docRight != null ? docRight.Number : "",
                     ORDocDate = docRight != null ? docRight.Date : DateTime.MinValue,
                         //ORRName = excerpt.OtherRealRight.Name,
                         //ORRRegDate = excerpt.OtherRealRight.RegistrationDate,
                         //ORRRegNum = excerpt.OtherRealRight.RegistrationNumber,
                         //ORRDocName = excerpt.OtherRealRight.Document.Name,
                         //ORRDocNum = excerpt.OtherRealRight.Document.Number,
                         //ORRDocDate = excerpt.OtherRealRight.Document.Date,
                         //ChPartName = excerpt.Charge.PartName,
                         //ChPartType = excerpt.Charge.PartType,
                         //ChArea = excerpt.Charge.Area,
                         //ChType = excerpt.Charge.Type,
                         //ChNumber = excerpt.Charge.Number,
                         //ChDate = excerpt.Charge.Date,
                         //ChStart = excerpt.Charge.Started,
                         //ChEnd = excerpt.Charge.Ended,
                         //ChChType = excerpt.Charge.Charger.Type,
                         //ChChName = excerpt.Charge.Charger.Name,
                         //excerpt.Charge.Charger.CardNumber,
                         //excerpt.Charge.Charger.OGRN,
                         //excerpt.Charge.Charger.OGRNDate,
                         //excerpt.Charge.Charger.INN,
                         //excerpt.Charge.Charger.SNILS,
                         //excerpt.Charge.Charger.LastName,
                         //excerpt.Charge.Charger.FirstName,
                         //excerpt.Charge.Charger.MiddleName,
                         //ChChPassSeries = excerpt.Charge.Charger.Passport.Series,
                         //ChChPassNum = excerpt.Charge.Charger.Passport.Number,
                         //ChChPassIsBy = excerpt.Charge.Charger.Passport.IssuedBy,
                         //ChChPassIsOn = excerpt.Charge.Charger.Passport.IssuedOn,
                         TDName = docRemoval != null ? docRemoval.Name : "",
                     TDNum = docRemoval != null ? docRemoval.Number : "",
                     TDDate = docRemoval != null ? docRemoval.Date : DateTime.MinValue,
                     RhType = excerptType.Rightholder.Type,
                     RhName = excerptType.Rightholder.Name,
                     RhCNum = excerptType.Rightholder.CardNumber,
                     RhOGRN = excerptType.Rightholder.OGRN,
                     RhOGRNDate = excerptType.Rightholder.OGRNDate,
                     RhINN = excerptType.Rightholder.INN,
                     RhSNILS = excerptType.Rightholder.SNILS,
                     RhLastName = excerptType.Rightholder.LastName,
                     RhFirstName = excerptType.Rightholder.FirstName,
                     RhMiddleName = excerptType.Rightholder.MiddleName,
                     RhPassNumber = excerptType.Rightholder.Passport.Number,
                     RhPassIsBy = excerptType.Rightholder.Passport.IssuedBy,
                     RhPassIsOn = excerptType.Rightholder.Passport.IssuedOn,
                         //   RhAddress = excerpt.Rightholder.Address.AddressFull,
                         OtherTypeOfCostName = excerptType.OtherCost != null ? excerptType.OtherCost.OtherTypeOfCostName : "",
                     CostOfOtherTypeOfLand = excerptType.OtherCost != null ? excerptType.OtherCost.CostOfOtherTypeOfLand : 0,
                         //  excerpt.Ship.ShipType,
                         //   ShipName = excerpt.Ship.Name,
                         //excerpt.Ship.PurposeType,
                         //excerpt.Ship.Purpose,
                         //ShipRegNum = excerpt.Ship.RegistrationNumber,
                         //ShipRegDate = excerpt.Ship.RegistrationDate,
                         //excerpt.Ship.ConstructionYear,
                         //excerpt.Ship.ConstructionPlace,
                         //excerpt.Ship.Airport,
                         //excerpt.Ship.Harbor,
                         //excerpt.Ship.InventoryNumber,
                         //excerpt.Ship.SerialNumber
                     })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var data = domain.GetAll()
                 .Where(x => x.Id == id)
                 .Select(x => new
                 {
                     x.Id,
                     x.QueryType,
                     x.PublicPropertyLevel,
                     x.RealityObject,
                     x.CadasterNumber,
                     x.Answer,
                     x.AnswerFile,
                     x.AttachmentFile,
                     x.CalcDate,
                     x.Inspector,
                     x.MessageId,
                     x.Municipality,
                     x.ObjectCreateDate,
                     x.ObjectEditDate,
                     x.ObjectVersion,
                     x.RegisterNumber,
                     x.Room,
                     x.RequestState
                 })
                 .AsQueryable()
                 .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }

        static ExcerptType DeSerializer(XDocument document)
        {
            var serializer = new XmlSerializer(typeof(ExcerptType));
            return (ExcerptType)serializer.Deserialize(document.CreateReader());
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

