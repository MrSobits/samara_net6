// ReSharper disable once CheckNamespace
namespace Bars.Gkh.Regions.Voronezh.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Castle.Windsor;

    using Gkh.Modules.ClaimWork.Entities;

    using RegOperator.Entities;
    using RegOperator.Entities.Owner;

    using Gkh.Entities;

    using System.Numerics;

    using Bars.B4.DataAccess;

    using Sobits.RosReg.Entities;

    /// <inheritdoc />
    /// <summary>
    /// Сервис "Собственник в исковом заявлении"
    /// </summary>
    public class RosRegExtractOperationsService : IRosRegExtractOperationsService
    {
        public IWindsorContainer Container { get; set; }

        private IDomainService<LawsuitIndividualOwnerInfo> LawsuitOwnerIndividualDomain { get; set; }
        
        private IDomainService<LawsuitLegalOwnerInfo> LawsuitOwnerLegalDomain { get; set; }

        private IDomainService<Lawsuit> LawsuitService { get; set; }

        private IDomainService<ChargePeriod> PeriodDomain { get; set; }

        private IDomainService<ClaimWorkAccountDetail> ClaimWorkAcDetailService { get; set; }

        private IDomainService<DocumentClw> DocumentClwService { get; set; }

        private IDomainService<ExtractEgrn> ExtractEgrnService { get; set; }

        private IDomainService<ExtractEgrnRight> ExtractRightService { get; set; }

        private IDomainService<ExtractEgrnRightInd> ExtractRightIndService { get; set; }

        private IDomainService<ExtractEgrnRightLegal> ExtractRightLegalService { get; set; }

        private Lawsuit Lawsuit { get; set; }

        private DocumentClw DocumentClw { get; set; }

        private ClaimWorkAccountDetail ClaimWorkAccountDetail { get; set; }

        private List<LawsuitIndividualOwnerInfo> NewOwnersList { get; set; }
        
        private List<LawsuitLegalOwnerInfo> NewOwnersLegalList { get; set; }

        private ExtractEgrn ExtractEgrn { get; set; }

        private List<ExtractEgrnRight> ExtractRight { get; set; }

        private List<ExtractEgrnRightInd> ExtractRightIndList { get; set; }

        private List<ExtractEgrnRightLegal> ExtractRightLegalList { get; set; }


        /// <inheritdoc />
        /// расчет начала задолженности 
        public IDataResult GetOwners(BaseParams baseParams)
        {
            
            this.LawsuitService = this.Container.Resolve<IDomainService<Lawsuit>>();
            this.PeriodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
            this.ClaimWorkAcDetailService = this.Container.Resolve<IDomainService<ClaimWorkAccountDetail>>();
            this.DocumentClwService = this.Container.Resolve<IDomainService<DocumentClw>>();
            this.ExtractEgrnService = this.Container.ResolveDomain<ExtractEgrn>();
            this.ExtractRightService = this.Container.ResolveDomain<ExtractEgrnRight>();
            this.ExtractRightIndService = this.Container.ResolveDomain<ExtractEgrnRightInd>();
            this.ExtractRightLegalService = this.Container.ResolveDomain<ExtractEgrnRightLegal>();
            this.LawsuitOwnerLegalDomain = this.Container.Resolve<IDomainService<LawsuitLegalOwnerInfo>>(); 
            this.LawsuitOwnerIndividualDomain = this.Container.Resolve<IDomainService<LawsuitIndividualOwnerInfo>>();
            this.NewOwnersList = new  List<LawsuitIndividualOwnerInfo>();
            this.NewOwnersLegalList = new  List<LawsuitLegalOwnerInfo>();
            
            var docId = baseParams.Params.GetAs<long>("docId");

            if (docId == 0)
            {
                return BaseDataResult.Error("Не найдена информация о документе");
            }


            this.Lawsuit = this.LawsuitService.Get(docId);
            this.DocumentClw = this.DocumentClwService.Get(docId);
            this.ClaimWorkAccountDetail = ClaimWorkAcDetailService
                .GetAll().FirstOrDefault(x => x.ClaimWork == this.DocumentClw.ClaimWork);

            this.ExtractEgrn = this.ExtractEgrnService.GetAll()
                .OrderByDescending(x => x.Id)
                .FirstOrDefault(x => x.RoomId == this.ClaimWorkAccountDetail.PersonalAccount.Room);
            if (this.ExtractEgrn == null)
            {
                return BaseDataResult.Error("Не найдена выписка, связанная с комнатой");
            }

            this.ExtractRight = this.ExtractRightService.GetAll().Where(x => x.EgrnId == this.ExtractEgrn).ToList();
            this.ExtractRightIndList = this.ExtractRightIndService.GetAll().Where(x => x.RightId.EgrnId == this.ExtractEgrn).ToList();
            this.ExtractRightLegalList = this.ExtractRightLegalService.GetAll().Where(x => x.RightId.EgrnId == this.ExtractEgrn).ToList();


            if(this.ExtractRightIndList.Count>0) this.GetIndividualOwners();
            if(this.ExtractRightLegalList.Count>0) this.GetLegalOwners();


            var rr = new ResponceResult();
            var result = "";
            if (this.ClaimWorkAccountDetail.PersonalAccount.AreaShare == 1)
            {
                result = "Добавлено " + (this.NewOwnersList.Count +  this.NewOwnersLegalList.Count)  +" собственника(ов)";
            }
            else
            {
                result =
                    "Внимание! По данному лицевому счету доля собственности меньше единицы. Необходимо убрать лишнего собственника и печатать заявление по лицевому счету";
                rr.AreaWarning = true;
            }

            rr.Message = result;

            return new BaseDataResult(rr);
        }

        private void GetIndividualOwners()
        {
            foreach (var right in ExtractRight)
            {
                var individuals = this.ExtractRightIndList.Where(x => x.RightId == right);
                var share = right.Share;

                foreach (var ind in individuals)
                {
                    var newOwner = new LawsuitIndividualOwnerInfo();

                    var numerator = 1;
                    var denomerator = 1;

                    if (share != null && share.Contains("/"))
                    {
                        numerator = Convert.ToInt32(share.Split('/')[0]);
                        denomerator = Convert.ToInt32(share.Split('/')[1]);
                    }

                    if (individuals.Count() > 1)
                    {
                        newOwner.SharedOwnership = true;
                    }

                    newOwner.AreaShareNumerator = numerator;
                    newOwner.AreaShareDenominator = denomerator;
                    newOwner.Lawsuit = this.Lawsuit;
                    newOwner.PersonalAccount = this.ClaimWorkAccountDetail.PersonalAccount;
                    newOwner.StartPeriod = this.PeriodDomain.GetAll().FirstOrDefault(x =>
                        x.StartDate <= newOwner.PersonalAccount.OpenDate && x.EndDate.HasValue && x.EndDate.Value >= newOwner.PersonalAccount.OpenDate);
                    newOwner.EndPeriod = this.PeriodDomain.GetAll().Where(x => x.EndDate.HasValue).OrderByDescending(x => x.Id).FirstOrDefault();
                    newOwner.Surname = ind.Surname;
                    newOwner.FirstName = ind.FirstName;
                    newOwner.SecondName = ind.Patronymic;
                    newOwner.BirthPlace = ind.BirthPlace;
                    newOwner.LivePlace = null; //Поле нужно для ручного заполнения
                    newOwner.BirthDate = ind.BirthDate.ToString("dd.MM.yyyy");
                    newOwner.SNILS = ind.Snils;
                    newOwner.DocIndCode = ind.DocIndCode;
                    newOwner.DocIndName = ind.DocIndName;
                    newOwner.DocIndSerial = ind.DocIndSerial;
                    newOwner.DocIndNumber = ind.DocIndNumber;
                    newOwner.DocIndDate = ind.DocIndDate;
                    newOwner.DocIndIssue = ind.DocIndIssue;


                    var birthDate = new DateTime();
                    DateTime.TryParse(newOwner.BirthDate, out birthDate);

                    // Save today's date.
                    DateTime today = DateTime.Today;

                    // Calculate the age.
                    int age = today.Year - birthDate.Year;

                    // Go back to the year the person was born in case of a leap year
                    if (birthDate > today.AddYears(-age)) age--;
                    if (age < 18)
                    {
                        newOwner.Underage = true;
                    }

                    newOwner.Name = ind.Surname + " " + ind.FirstName + " " + ind.Patronymic;

                    var collisionList = NewOwnersList.FindAll(x => x.Name == newOwner.Name && x.BirthDate == newOwner.BirthDate);
                    if (collisionList.IsNotEmpty() && collisionList.Count == 1)
                    {
                        int oldDen = collisionList.First().AreaShareDenominator;
                        int oldNum = collisionList.First().AreaShareNumerator;

                        int addDen = newOwner.AreaShareDenominator;
                        int addNum = newOwner.AreaShareNumerator;

                        int newDen = oldDen * addDen;
                        int newNum = oldDen * addNum + oldNum * addDen;

                        int divisor = BigInteger.GreatestCommonDivisor(newDen, newNum).ToInt();
                        newOwner.AreaShareDenominator = newDen / divisor;
                        newOwner.AreaShareNumerator = newNum / divisor;

                        this.NewOwnersList.Remove(collisionList.First());
                    }

                    this.NewOwnersList.Add(newOwner);
                }
            }

            var index = 1;
            foreach (var newOwner in this.NewOwnersList)
            {
                //Проставляем номера заявлений и сохраняем
                newOwner.ClaimNumber = $"{this.Lawsuit.BidNumber}/{index}";
                index++;
                try
                {
                    this.LawsuitOwnerIndividualDomain.Save(newOwner);
                }
                catch (Exception e)
                {
                    var unused = e.Message;
                }
            }
        }

        private void GetLegalOwners()
        {
            foreach (var right in ExtractRight)
            {
                var legals = this.ExtractRightLegalList.Where(x => x.RightId == right);
                bool first = true;

                foreach (var legal in legals)
                {
                    var newOwner = new LawsuitLegalOwnerInfo();

                    string share = null;
                    if (first)
                    {
                        share = right.Share;
                        first = false;
                    }

                    var numerator = 1;
                    var denomerator = 1;

                    if (share != null && share.Contains("/"))
                    {
                        numerator = Convert.ToInt32(share.Split('/')[0]);
                        denomerator = Convert.ToInt32(share.Split('/')[1]);
                    }

                    if (legals.Count() > 1)
                    {
                        newOwner.SharedOwnership = true;
                    }

                    newOwner.AreaShareNumerator = numerator;
                    newOwner.AreaShareDenominator = denomerator;
                    newOwner.Lawsuit = this.Lawsuit;
                    newOwner.PersonalAccount = this.ClaimWorkAccountDetail.PersonalAccount;
                    newOwner.StartPeriod = this.PeriodDomain.GetAll().FirstOrDefault(x =>
                        x.StartDate <= newOwner.PersonalAccount.OpenDate && x.EndDate.HasValue && x.EndDate.Value >= newOwner.PersonalAccount.OpenDate);
                    newOwner.EndPeriod = this.PeriodDomain.GetAll().Where(x => x.EndDate.HasValue).OrderByDescending(x => x.Id).FirstOrDefault();
                    
                    newOwner.Name = legal.Name;
                    newOwner.ContragentName = legal.Name;
                    newOwner.Inn = legal.Inn.ToString();
                    newOwner.Kpp = string.Empty;
                    
                    var collisionList = this.NewOwnersLegalList.FindAll(x => x.Name == newOwner.Name);
                    if (collisionList.IsNotEmpty() && collisionList.Count == 1)
                    {
                        int oldDen = collisionList.First().AreaShareDenominator;
                        int oldNum = collisionList.First().AreaShareNumerator;

                        int addDen = newOwner.AreaShareDenominator;
                        int addNum = newOwner.AreaShareNumerator;

                        int newDen = oldDen * addDen;
                        int newNum = oldDen * addNum + oldNum * addDen;

                        int divisor = BigInteger.GreatestCommonDivisor(newDen, newNum).ToInt();
                        newOwner.AreaShareDenominator = newDen / divisor;
                        newOwner.AreaShareNumerator = newNum / divisor;

                        this.NewOwnersLegalList.Remove(collisionList.First());
                    }

                    this.NewOwnersLegalList.Add(newOwner);
                }
            }

            var index = 1;
            foreach (var newOwner in this.NewOwnersLegalList)
            {
                //Проставляем номера заявлений и сохраняем
                newOwner.ClaimNumber = $"{this.Lawsuit.BidNumber}/{index}";
                index++;
                try
                {
                    this.LawsuitOwnerLegalDomain.Save(newOwner);
                }
                catch (Exception e)
                {
                    var unused = e.Message;
                }
            }
        }
    }

    public class ResponceResult
    {
        public string Message { get; set; }

        public bool AreaWarning { get; set; }
    }
}