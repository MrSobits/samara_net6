using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.Gkh.Enums;

namespace Bars.Gkh.Interceptors
{

    using B4;

    using Entities;


    public class PersonDisqualificationInfoInterceptor : EmptyDomainInterceptor<PersonDisqualificationInfo>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PersonDisqualificationInfo> service, PersonDisqualificationInfo entity)
        {
            var result = CheckLicenseCancelation(entity);
            if (!result.Success)
            {
                return result;
            }

            return Check(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PersonDisqualificationInfo> service, PersonDisqualificationInfo entity)
        {
            var result = CheckLicenseCancelation(entity);
            if (!result.Success)
            {
                return result;
            }

            return Check(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PersonDisqualificationInfo> service, PersonDisqualificationInfo entity)
        {
            return CheckLicenseCancelation(entity, true);
        }

        private IDataResult CheckLicenseCancelation(PersonDisqualificationInfo entity, bool beforeDelete = false )
        {
            var placeWorkDomain = Container.Resolve<IDomainService<PersonPlaceWork>>();
            var qualDomain = Container.Resolve<IRepository<PersonQualificationCertificate>>();
            var licenseDomain = Container.Resolve<IDomainService<ManOrgLicense>>();

            try
            {

                if (!entity.DisqDate.HasValue)
                {
                    return Failure("Дата выдачи неуказана");
                }

                switch (entity.TypeDisqualification)
                {
                    case TypePersonDisqualification.CancelationLicenze:
                    {
                        // поулчаем актуалильно место работы
                        var placeQuery = placeWorkDomain.GetAll()
                            .Where(x => x.Person.Id == entity.Person.Id);

                        var licenseList = licenseDomain.GetAll()
                            .Where(x => placeQuery.Where(y => y.Contragent.Id == x.Contragent.Id)
                                                    .Any(y => (y.StartDate >= x.DateIssued && (!x.DateTermination.HasValue || (x.DateTermination.HasValue && x.DateTermination >= y.StartDate))) // Если начало работы попадает в период лицензии
                                                                || (y.StartDate < x.DateIssued && !y.EndDate.HasValue) // если работа началась раньше чем период начала лицензии и незакончилась
                                                                || (y.EndDate.HasValue && y.EndDate >= x.DateIssued && (!x.DateTermination.HasValue || (x.DateTermination.HasValue && x.DateTermination >= y.EndDate))) // Если работа закончилась в периоде лицензии
                                                        )
                                                    )
                            .Select(x => new
                            {
                                x.LicNum,
                                x.TypeTermination,
                                x.State.FinalState
                            });

                        if (beforeDelete)
                        {
                            // Если мы удаляем дисквалификацию с Типом ДЛ = Аннулирование лицензии и Лицензия Аннулирвоана то нельзя удалять
                            if (licenseList.Any(x => x.FinalState))
                            {
                                return
                                    Failure(
                                        "Сведения о дисквалификации нельзя удалить, т.к. лицензия должностного лица аннулирована");
                            }
                        }
                        else
                        {
                            // Если мы обновляем дисквалификацию с Типом ДЛ = Аннулирование лицензии и Лицензия не аннулирована, то нельзя обновлять
                            if (!licenseList.Any())
                            {
                                return Failure("По данному должностному лицу не найдена лицензия");
                            }

                            if (licenseList.Any(x => !x.FinalState))
                            {
                                return
                                    Failure(
                                        "Для дисквалификации должностного лица по данному основанию необходимо аннулировать лицензию");
                            }
                        }
                    }
                    break;
                    
                    case TypePersonDisqualification.DisqualificationSanction:
                    {
                        if (beforeDelete)
                        {
                            
                        }
                        else
                        {
                            // получаем тольк оте квал аттестаты у которых дата выдачи меньше даты дисквалификации И Тип оснвоания аннулирваония не задан
                            var quals =
                                qualDomain.GetAll()
                                    .Where(
                                        x =>
                                            x.Person.Id == entity.Person.Id && x.IssuedDate < entity.DisqDate &&
                                            x.TypeCancelation == TypeCancelationQualCertificate.NotSet)
                                    .ToList();

                            foreach (var item in quals)
                            {
                                item.TypeCancelation = TypeCancelationQualCertificate.Disqualification;
                                item.CancelationDate = entity.DisqDate;
                                item.EndDate = entity.DisqDate;
                                item.HasCancelled = true;

                                qualDomain.Update(item);
                            }
                        }
                    }
                    break;
                }

            }
            catch (ValidationException ex)
            {
                return Failure(ex, null);
            }
            finally
            {
                Container.Release(placeWorkDomain);
                Container.Release(licenseDomain);
                Container.Release(qualDomain);
            }


            return Success();
        }

        private IDataResult Check(PersonDisqualificationInfo entity)
        {
            var disqDomain = Container.Resolve<IDomainService<PersonDisqualificationInfo>>();

            try
            {
                var dateStart = entity.DisqDate.Value;
                var dateEnd = DateTime.MaxValue;

                if (entity.EndDisqDate.HasValue)
                {
                    dateEnd = entity.EndDisqDate.Value;
                }

                var recs = disqDomain.GetAll()
                    .Where(x => x.Person.Id == entity.Person.Id)
                    .Where(x => x.Id != entity.Id)
                    .Where(x => x.DisqDate.HasValue)
                    .Select(x => new
                    {
                        x.Id,
                        DisqDate = x.DisqDate.Value,
                        EndDate = x.EndDisqDate.HasValue ? x.EndDisqDate.Value : DateTime.MaxValue

                    })
                    .Where(
                        x =>
                            (x.DisqDate <= dateStart && x.EndDate >= dateStart) ||
                            (x.DisqDate <= dateEnd && x.EndDate >= dateEnd) ||
                            (dateStart <= x.DisqDate && x.DisqDate >= dateEnd) ||
                            (dateStart <= x.EndDate && x.EndDate >= dateEnd));

                if (recs.Any())
                {
                    return Failure("Период дисквалификации пересекается с предыдущим");
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
    }
}