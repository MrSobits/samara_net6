namespace Bars.Gkh.Regions.Voronezh.Controllers
{
    using System.Linq;

    using B4;
    using B4.DataAccess;

    using Microsoft.AspNetCore.Mvc;

    using Modules.ClaimWork.Entities;

    using System;
    using System.Text.RegularExpressions;

    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;


    /// <inheritdoc />
    /// <summary>
    /// Контроллер для генерации номеров заявлений
    /// </summary>
    public class GenNumberController : BaseController
    {
        //TODO: Объединить методы для минимизации повторов кода 
        public ActionResult GenNumberForPetition(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("docId");
            var service = this.Container.ResolveDomain<Petition>();
            var ownerService = this.Container.ResolveDomain<LawsuitOwnerInfo>();
            var debtorDomain = this.Container.ResolveDomain<DebtorClaimWork>();

            
            try
            {
                Petition entity = service.GetAll().FirstOrDefault(x => x.Id == id);
                var typeOwner = debtorDomain.Get(entity.ClaimWork.Id).DebtorType;
                //Автогенерация номера заявления
                var stringBidNumberList
                    = service.GetAll()
                        .Where(x => x.BidNumber != null
                            && x.BidNumber != entity.BidNumber
                            && x.ObjectCreateDate.Year == DateTime.Now.Year)
                        .Select(x => x.BidNumber)
                        .ToList();


                string resultBidNum = "";

                if (typeOwner == Gkh.Enums.ClaimWork.DebtorType.Legal)
                {
                    var pretensionId = this.Container.ResolveDomain<DocumentClw>().GetAll()
                        .Where(x => x.ClaimWork == entity.ClaimWork && x.DocumentType == Modules.ClaimWork.Enums.ClaimWorkDocumentType.Pretension)
                        .Select(x => x.Id)
                        .FirstOrDefault();
                    var pretensionNumber = this.Container.ResolveDomain<PretensionClw>().Get(pretensionId).NumberPretension;
                    resultBidNum = pretensionNumber ?? "";
                }
                else
                {
                    long autoGenNum = 0;
                    foreach (string stringBidNumber in stringBidNumberList)
                    {
                        string tempStringBidNumber = Regex.Replace(stringBidNumber, @"(\D*)$", ""); //Срезаем хвост из символов
                        int intBid;
                        int.TryParse(tempStringBidNumber, out intBid);
                        if (intBid > autoGenNum)
                        {
                            autoGenNum = intBid;
                        }
                    }
                    autoGenNum++;
                    resultBidNum = autoGenNum.ToString() + "/и";
                }


                if (string.IsNullOrEmpty(entity.BidNumber))
                {
                    entity.BidNumber = resultBidNum;
                    entity.BidDate = DateTime.Today;
                    service.Save(entity);
                }

                var ownerList = ownerService.GetAll().Where(x => x.Lawsuit.Id == id).ToList();
                foreach (LawsuitOwnerInfo lawsuitOwnerInfo in ownerList)
                {
                    if (lawsuitOwnerInfo.ClaimNumber!=null&&lawsuitOwnerInfo.ClaimNumber.StartsWith("/"))
                    {
                        lawsuitOwnerInfo.ClaimNumber = entity.BidNumber + lawsuitOwnerInfo.ClaimNumber;
                        ownerService.Save(lawsuitOwnerInfo);
                    }
                }

                ActionResult result = new BaseDataResult(new { resBidNum = entity.BidNumber, resBidDate = entity.BidDate }).ToJsonResult();
                return result;
            }
            finally
            {
                this.Container.Release(ownerService);
                this.Container.Release(service);
            }
        }

        public ActionResult GenNumberForPretention(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("docId");
            var service = this.Container.ResolveDomain<PretensionClw>();
            try
            {
                PretensionClw entity = service.Get(id);

                //Автогенерация номера претензии
                var stringNumberList
                    = service.GetAll()
                        .Where(x => x.NumberPretension != null
                            && x.NumberPretension != entity.NumberPretension
                            && x.ObjectCreateDate.Year == DateTime.Now.Year)
                        .Select(x => x.NumberPretension)
                        .ToList();

                long autoGenNum = 0;
                foreach (string stringNumber in stringNumberList)
                {
                    string tempStringNumber = Regex.Replace(stringNumber, @"(\D*)$", ""); //Срезаем хвост из символов
                    int intNumber;
                    int.TryParse(tempStringNumber, out intNumber);
                    if (intNumber > autoGenNum)
                    {
                        autoGenNum = intNumber;
                    }
                }

                autoGenNum++;
                string resultNum = autoGenNum.ToString() + "/а";
                if (string.IsNullOrEmpty(entity.NumberPretension))
                {
                    entity.NumberPretension = resultNum;                    
                    service.Save(entity);
                }


                ActionResult result = new BaseDataResult(new { resNum = entity.NumberPretension }).ToJsonResult();
                return result;
            }
            finally
            {                
                this.Container.Release(service);
            }
        }

        public ActionResult GenNumberForLawsuit(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("docId");
            var service = this.Container.ResolveDomain<Lawsuit>();
            var ownerService = this.Container.ResolveDomain<LawsuitOwnerInfo>();
            try
            {
                Lawsuit entity = service.GetAll().FirstOrDefault(x => x.Id == id);

                //Автогенерация номера заявления
                var stringBidNumberList
                    = service.GetAll()
                        .Where(x => x.BidNumber != null 
                            && x.BidNumber != entity.BidNumber
                            && x.ObjectCreateDate.Year==DateTime.Now.Year)
                        .Select(x => x.BidNumber)
                        .ToList();

                long autoGenNum = 0;
                foreach (string stringBidNumber in stringBidNumberList)
                {
                    string tempStringBidNumber = Regex.Replace(stringBidNumber, @"(\D*)$", ""); //Срезаем хвост из символов
                    int intBid;
                    int.TryParse(tempStringBidNumber, out intBid);
                    if (intBid > autoGenNum)
                    {
                        autoGenNum = intBid;
                    }
                }

                autoGenNum++;
                string resultBidNum = autoGenNum.ToString();

                if (string.IsNullOrEmpty(entity.BidNumber))
                {
                    entity.BidNumber = resultBidNum;
                    entity.BidDate = DateTime.Today;
                    service.Save(entity);
                }

                var ownerList = ownerService.GetAll().Where(x => x.Lawsuit.Id == id).ToList();
                foreach (LawsuitOwnerInfo lawsuitOwnerInfo in ownerList)
                {
                    if (lawsuitOwnerInfo.ClaimNumber!=null&&lawsuitOwnerInfo.ClaimNumber.StartsWith("/"))
                    {
                        lawsuitOwnerInfo.ClaimNumber = entity.BidNumber + lawsuitOwnerInfo.ClaimNumber;
                        ownerService.Save(lawsuitOwnerInfo);
                    }
                }

                ActionResult result = new BaseDataResult(new { resBidNum = entity.BidNumber, resBidDate = entity.BidDate }).ToJsonResult();
                return result;
            }
            finally
            {
                this.Container.Release(ownerService);
                this.Container.Release(service);
            }
        }
    }
}