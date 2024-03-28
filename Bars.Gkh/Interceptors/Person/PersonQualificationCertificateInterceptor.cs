using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Bars.B4.DataAccess;
using Bars.Gkh.Enums;

namespace Bars.Gkh.Interceptors
{

    using B4;
    using Bars.B4.Modules.States;
    using Entities;


    public class PersonQualificationCertificateInterceptor : EmptyDomainInterceptor<PersonQualificationCertificate>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PersonQualificationCertificate> service, PersonQualificationCertificate entity)
        {
            var result = CheckCancelation(entity);
            var stateProvider = Container.Resolve<IStateProvider>();
            entity.TypeCancelation = TypeCancelationQualCertificate.NotSet;
            stateProvider.SetDefaultState(entity);
            if (!result.Success)
            {
                return result;
            }

            return Check(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PersonQualificationCertificate> service, PersonQualificationCertificate entity)
        {
            if (entity.State == null)
            {
                var stateProvider = Container.Resolve<IStateProvider>();
                stateProvider.SetDefaultState(entity);
            }
            if (!entity.HasCancelled)
            {
                entity.CancelFile = null;
                entity.CancelNumber = null;
                entity.CancelProtocolDate = null;
                entity.CancelationDate = null;
                entity.TypeCancelation = TypeCancelationQualCertificate.NotSet;
            }

            var result = CheckCancelation(entity);
            if (!result.Success)
            {
                return result;
            }

            return Check(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PersonQualificationCertificate> service, PersonQualificationCertificate entity)
        {
            return CheckCancelation(entity, true);
        }

        private IDataResult CheckCancelation(PersonQualificationCertificate entity, bool beforeDelete = false)
        {
            var disqDomain = Container.Resolve<IRepository<PersonDisqualificationInfo>>();

            try
            {
                if (entity.TypeCancelation == TypeCancelationQualCertificate.Disqualification)
                {
                    var disquals = disqDomain.GetAll().Where(x => x.Person.Id == entity.Person.Id)
                        .Select(x => new
                        {
                            x.DisqDate,
                            x.EndDisqDate,
                            x.TypeDisqualification
                        });


                    if (beforeDelete)
                    {
                        if (disquals.Any(x => entity.IssuedDate < x.DisqDate))
                        {
                            return Failure(
                                        "Сведения об аннулировании квалификационного аттестата нельзя удалить, т.к. должностное лицо дисквалифицировано");
                        }
                    }
                    else
                    {
                        if (!disquals.Any())
                        {
                            return Failure(
                                        "Для аннулирования квалификационного аттестата по данному основанию необходимо заполнить сведения о дисквалификации должностного лица");
                        }
                    }
                }
            }
            catch (ValidationException ex)
            {
                return Failure(ex, null);
            }
            finally
            {
                Container.Release(disqDomain);
            }


            return Success();
        }

        private IDataResult Check(PersonQualificationCertificate entity)
        {
            try
            {
                if (!entity.IssuedDate.HasValue)
                {
                    return Success();
                }
                var dateStart = entity.IssuedDate.Value;
                var dateEnd = dateStart.AddYears(5);

                if (entity.CancelationDate.HasValue)
                {
                    dateEnd = entity.CancelationDate.Value;
                }

                // если аннулирование отменено то дату анулирвоания убираем и возвращаем ту дату которая була изначально
                if (entity.HasRenewed)
                {
                    dateEnd = dateStart.AddYears(5);
                }

                var recs = Container.Resolve<IDomainService<PersonQualificationCertificate>>().GetAll()
                    .Where(x => x.Person.Id == entity.Person.Id)
                    .Where(x => x.Id != entity.Id)
                    .Where(x => x.IssuedDate.HasValue && x.EndDate.HasValue)
                    .Where(
                        x =>
                            (x.IssuedDate.Value <= dateStart && x.EndDate.Value >= dateStart) ||
                             (x.IssuedDate.Value <= dateEnd && x.EndDate.Value >= dateEnd) ||
                             (dateStart <= x.IssuedDate.Value && x.IssuedDate.Value >= dateEnd) ||
                             (dateStart <= x.EndDate.Value && x.EndDate.Value >= dateEnd));

                if (recs.Any())
                {
                  //  убираем проверку так как может быть много КА у одного человека
                   // throw new ValidationException("Период действия пересекается с периодом действия предыдущего квалификационного аттестата");
                }

                entity.EndDate = dateEnd;
            }
            catch (ValidationException ex)
            {
                return Failure(ex, null);
            }

            return Success();
        }
    }
}