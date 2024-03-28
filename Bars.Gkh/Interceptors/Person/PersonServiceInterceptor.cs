namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    ﻿using B4.Modules.States;
    using B4.Utils;
    using Entities;
    using Utils;

    public class PersonServiceInterceptor : EmptyDomainInterceptor<Person>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Person> service, Person entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Check(service, entity, false);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Person> service, Person entity)
        {
            return Check(service, entity, true);
        }

        private IDataResult Check(IDomainService<Person> service, Person entity, bool update)
        {
            try
            {
                if (string.IsNullOrEmpty(entity.Name))
                {
                    throw new ValidationException("Небходимо заполнить Имя должностного лица");
                }

                if (string.IsNullOrEmpty(entity.Surname))
                {
                    throw new ValidationException("Небходимо заполнить Фамилию должностного лица");
                }

                if (string.IsNullOrEmpty(entity.Patronymic))
                {
                    throw new ValidationException("Небходимо заполнить Отчество должностного лица");
                }

                var fullName = string.Format("{0} {1} {2}", entity.Surname.Trim(), entity.Name.Trim(), entity.Patronymic.Trim());

                entity.FullName = fullName;

                if (!string.IsNullOrEmpty(entity.Inn))
                {
                    var persons = service.GetAll().Where(x => x.Inn == entity.Inn).Where(x => x.Id != entity.Id);
                    if (persons.Any())
                    {
                        throw new ValidationException("Должностное лицо с таким ИНН уже существует");
                    }

                    if (!Utils.VerifyInn(entity.Inn, false))
                    {
                        throw new ValidationException("Указаный ИНН не корректен");
                    }
                }

                if (update)
                {
                    var placeWorkDomain = Container.ResolveDomain<PersonPlaceWork>();
                    using (Container.Using(placeWorkDomain))
                    {
                        var actualWorkPlace =
                            placeWorkDomain.GetAll()
                                .Where(x => x.StartDate.HasValue && x.StartDate <= DateTime.Today)
                                .Where(x => !x.EndDate.HasValue || x.EndDate >= DateTime.Today)
                                .FirstOrDefault(x => x.Person.Id == entity.Id);

                        var fieldRequirementDomain = Container.ResolveDomain<FieldRequirement>();
                        var existPerms = fieldRequirementDomain.GetAll()
                            .Where(x => x.RequirementId == "GkhGji.PersonRegisterGji.Field.Contragent_Rqrd")
                            .Select(x => x.RequirementId)
                            .Distinct()
                            .ToList();

                        if (actualWorkPlace == null && existPerms.Count() > 0)
                        {
                            throw new ValidationException("Не найдено актуальное место работы должностного лица");
                        }
                    }
                }
            }
            catch (ValidationException ex)
            {
                return Failure(ex, null);
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Person> service, Person entity)
        {
            var persDisqServ = Container.ResolveDomain<PersonDisqualificationInfo>();
            var persQualServ = Container.ResolveDomain<PersonQualificationCertificate>();
            var persPlaceServ = Container.ResolveDomain<PersonPlaceWork>();
			var manOrgRequestPersonServ = Container.ResolveDomain<ManOrgRequestPerson>();

            try
            {
                if (persDisqServ.GetAll().Any(x => x.Person.Id == entity.Id))
                {
                    return Failure("Данное должностное лицо имеет связи в таблице Сведения о дисквалификации");
                }

                if (persQualServ.GetAll().Any(x => x.Person.Id == entity.Id))
                {
                    return Failure("Данное должностное лицо имеет связи в таблице Квалификационные аттестаты");
                }

                persPlaceServ.GetAll().Where(x => x.Person.Id == entity.Id).Select(x => x.Id).ForEach(x => persPlaceServ.Delete(x));
				manOrgRequestPersonServ.GetAll().Where(x => x.Person.Id == entity.Id).Select(x => x.Id).ForEach(x => manOrgRequestPersonServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(persDisqServ);
                Container.Release(persQualServ);
                Container.Release(persPlaceServ);
				Container.Release(manOrgRequestPersonServ);
            }
        }
    }
}