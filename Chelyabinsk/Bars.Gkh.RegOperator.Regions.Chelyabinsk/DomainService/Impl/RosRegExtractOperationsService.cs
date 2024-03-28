// ReSharper disable once CheckNamespace
namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Domain
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

        /// <inheritdoc />
        /// расчет начала задолженности 
        public IDataResult GetOwners(BaseParams baseParams)
        {
            var docId = baseParams.Params.GetAs<long>("docId");

            if (docId == 0)
            {
                return BaseDataResult.Error("Не найдена информация о документе");
            }

            var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>();
            var lawsuidOwnerDomain = this.Container.Resolve<IDomainService<LawsuitIndividualOwnerInfo>>();
            Lawsuit lawsuid = this.Container.Resolve<IDomainService<Lawsuit>>().Get(docId);
            DocumentClw documentClw = this.Container.Resolve<IDomainService<DocumentClw>>().Get(docId);
            ClaimWorkAccountDetail claimWorkAccountDetail = this.Container.Resolve<IDomainService<ClaimWorkAccountDetail>>()
                .GetAll().FirstOrDefault(x => x.ClaimWork == documentClw.ClaimWork);

            var newOwnersList = new List<LawsuitIndividualOwnerInfo>();

            var extractEgrn = this.Container.ResolveDomain<ExtractEgrn>().GetAll()
                .OrderByDescending(x=> x.Id)
                .FirstOrDefault(x => x.RoomId == claimWorkAccountDetail.PersonalAccount.Room);
            if (extractEgrn == null)
            {
                return BaseDataResult.Error("Не найдена выписка, связанная с комнатой");
            }
            var extractRight = this.Container.ResolveDomain<ExtractEgrnRight>().GetAll().Where(x=>x.EgrnId==extractEgrn);
            var extractRightInd = this.Container.ResolveDomain<ExtractEgrnRightInd>().GetAll();

            foreach (var right in extractRight)
            {
                var individuals = extractRightInd.Where(x => x.RightId == right);
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
                    newOwner.Lawsuit = lawsuid;
                    newOwner.PersonalAccount = claimWorkAccountDetail.PersonalAccount;
                    newOwner.StartPeriod = periodDomain.GetAll().FirstOrDefault(x =>
                        x.StartDate <= newOwner.PersonalAccount.OpenDate && x.EndDate.HasValue && x.EndDate.Value >= newOwner.PersonalAccount.OpenDate);
                    newOwner.EndPeriod = periodDomain.GetAll().Where(x => x.EndDate.HasValue).OrderByDescending(x => x.Id).FirstOrDefault();
                    newOwner.Surname = ind.Surname;
                    newOwner.FirstName = ind.FirstName;
                    newOwner.SecondName = ind.Patronymic;
                    newOwner.BirthPlace = ind.BirthPlace;
                    newOwner.LivePlace = null; //Поле нужно для ручного заполнения
                    newOwner.BirthDate = ind.BirthDate.ToString("dd.MM.yyyy");
                    newOwner.SNILS = ind.Snils;


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

                    var collisionList = newOwnersList.FindAll(x => x.Name == newOwner.Name && x.BirthDate == newOwner.BirthDate);
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

                        newOwnersList.Remove(collisionList.First());
                    }

                    newOwnersList.Add(newOwner);
                }
            }

            var index = 1;
            foreach (LawsuitIndividualOwnerInfo newOwner in newOwnersList)
            {
                //Проставляем номера заявлений и сохраняем
                newOwner.ClaimNumber = $"{lawsuid.BidNumber}/{index}";
                index++;
                try
                {
                    lawsuidOwnerDomain.Save(newOwner);
                }
                catch (Exception e)
                {
                    string unused = e.Message;
                }
            }


            var rr = new ResponceResult();
            var result = "";
            if (claimWorkAccountDetail.PersonalAccount.AreaShare == 1)
            {
                result = "Добавлено " + newOwnersList.Count + " собственника(ов)";
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
    }

    public class ResponceResult
    {
        public string Message { get; set; }

        public bool AreaWarning { get; set; }
    }
}