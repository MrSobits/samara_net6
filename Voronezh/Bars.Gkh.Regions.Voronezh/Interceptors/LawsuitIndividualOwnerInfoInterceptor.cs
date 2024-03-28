namespace Bars.Gkh.Regions.Voronezh.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Voronezh.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Utils;

    public class LawsuitIndividualOwnerInfoInterceptor : LawsuitOwnerInfoInterceptor<LawsuitIndividualOwnerInfo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<LawsuitIndividualOwnerInfo> service, LawsuitIndividualOwnerInfo entity)
        {
            this.UpdateFields(entity);
            if (CheckNotUnique(service, entity))
            {
                return this.Failure("Собственник с данным фио, датой рождения и лс уже существует");
            }
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<LawsuitIndividualOwnerInfo> service, LawsuitIndividualOwnerInfo entity)
        {
            this.UpdateFields(entity);

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<LawsuitIndividualOwnerInfo> service, LawsuitIndividualOwnerInfo entity)
        {
            var sspDomain = Container.ResolveDomain<LawSuitDebtWorkSSP>();

            using (Container.Using(sspDomain))
            {
                var ssp = sspDomain.GetAll()
                .Where(x => x.LawsuitOwnerInfo == entity)
                .FirstOrDefault();

                if (ssp != null)
                {
                    sspDomain.Delete(ssp.Id);

                    Container.Release(sspDomain);
                }
            }

            if (entity.Underage)
            {
                var repDomain = Container.ResolveDomain<LawsuitOwnerRepresentative>();

                using (Container.Using(repDomain))
                {
                    var rep = repDomain.GetAll()
                    .Where(x => x.Rloi == entity)
                    .FirstOrDefault();

                    if (rep != null)
                        repDomain.Delete(rep.Id);

                    Container.Release(repDomain);
                } 
            }

            return base.BeforeDeleteAction(service, entity);
        }

        private bool CheckNotUnique(IDomainService<LawsuitIndividualOwnerInfo> service, LawsuitIndividualOwnerInfo entity)
        {
            var isNotUnique = service.GetAll()
                .WhereIf(entity.Id > 0, x => x.Id != entity.Id)
                .Where(x => x.Lawsuit.Id == entity.Lawsuit.Id)
                .Where(x => x.PersonalAccount.Id == entity.PersonalAccount.Id)
                .Any(x => x.Name == entity.Name && x.BirthDate == entity.BirthDate);
            return isNotUnique;
        }

        private void UpdateFields(LawsuitIndividualOwnerInfo entity)
        {
            entity.OwnerType = PersonalAccountOwnerType.Individual;
            entity.Name = new[]
                {
                    entity.Surname,
                    !string.IsNullOrEmpty(entity.FirstName)
                        ? entity.FirstName.ToString()
                        : string.Empty,
                    !string.IsNullOrEmpty(entity.SecondName)
                        ? entity.SecondName.ToString()
                        : string.Empty
                }
                .AggregateWithSeparator(" ");
        }
    }
}