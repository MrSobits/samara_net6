namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class SectionImport8 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #8";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var listResult = new List<ContragentContact>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section8.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;

            var contragentService = this.Container.Resolve<IDomainService<Contragent>>();
            var positionService = this.Container.Resolve<IDomainService<Position>>();
            var contragentContactService = this.Container.Resolve<IDomainService<ContragentContact>>();

            using (this.Container.Using(contragentService, positionService, contragentContactService))
            {
                var contragent = contragentService.GetAll().FirstOrDefault(x => x.Inn == inn);
                if (contragent == null)
                {
                    logImport.Error(Name, string.Format("Не найден контрагент по данному ИНН = {0}", inn));
                    return;
                }

                var contragentContactList = contragentContactService.GetAll().Where(x => x.Contragent.Id == contragent.Id).ToList();

                foreach (var section8Record in sectionsData.Section8)
                {
                    var positionCode = string.Empty;
                    switch (section8Record.Type)
                    {
                        case "1":
                            positionCode = "6";
                            break;
                        case "2":
                            positionCode = "5";
                            break;
                    }

                    var position = positionService.FirstOrDefault(x => x.Code == positionCode);
                    if (position == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить должность с кодом {0}", positionCode));
                    }

                    var fullName = string.Format(
                        "{0} {1} {2}",
                        section8Record.Surname,
                        section8Record.Name,
                        section8Record.Patronomyc);

                    var contragentContact = contragentContactList.FirstOrDefault(x => x.FullName == fullName);

                    if (contragentContact == null)
                    {
                        logImport.Info(this.Name, string.Format("Добавлена запись контактных данных  {0}", fullName));

                        contragentContact = new ContragentContact
                        {
                            Id = 0,
                            Contragent = contragent
                        };
                    }

                    contragentContact.Surname = section8Record.Surname;
                    contragentContact.Name = section8Record.Name;
                    contragentContact.Patronymic = section8Record.Patronomyc;
                    contragentContact.Position = position;
                    contragentContact.Phone = section8Record.Phone;
                    contragentContact.Email = section8Record.Email;

                    if (contragentContact.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    listResult.Add(contragentContact);
                }

                this.InTransaction(listResult, contragentContactService);
                listResult.Clear();
            }
        }

        #region Транзакция

        private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        private void InTransaction(IEnumerable<PersistentObject> list, IDomainService repos)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        #endregion
    }
}
