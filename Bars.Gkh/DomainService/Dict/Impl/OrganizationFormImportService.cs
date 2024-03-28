using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.GkhExcel;
using Castle.Windsor;
namespace Bars.Gkh.DomainService.Dict.Impl
{
    using Bars.B4.IoC;

    public class OrganizationFormImportService : IOrganizationFormImportService
    {
        public IWindsorContainer Container { get; set; }

        public B4.IDataResult Import(B4.FileData fileData)
        {
            #region Проверка fileData
            if (fileData == null)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "FileData is null"
                };
            }
            if (!fileData.Extention.Equals("xls", System.StringComparison.CurrentCultureIgnoreCase))
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Недопустимое расширение файла"
                };
            }
            #endregion

            var orgFormService = this.Container.Resolve<IDomainService<OrganizationForm>>();
            var newOrgForms = new List<OrgForm>();

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var memoryStreamFile = new MemoryStream(fileData.Data))
                {
                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);
                    rows.RemoveAt(0); // удаляем заголовки
                    foreach (var row in rows)
                    {
                        newOrgForms.Add(new OrgForm
                            {
                                OldCode = row[0].Value,
                                NewCode = row[1].Value,
                                Name = row[2].Value
                            }
                        );
                    }
                }
            }

            #region Update orgForms in transaction
            var transaction = this.Container.Resolve<IDataTransaction>();
            using (this.Container.Using(transaction))
            {
                try
                {
                    foreach (var of in newOrgForms)
                    {
                        this.UpdateOrgForm(of, orgFormService);
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
            #endregion

            return new BaseDataResult
            {
                Success = true
            };
        }

        private void UpdateOrgForm(OrgForm of, IDomainService<OrganizationForm> service)
        {
            var orgForm = service.GetAll().FirstOrDefault(x => x.Code.Equals(of.OldCode));
            if (orgForm != null)
            {
                orgForm.Code = of.NewCode;
                orgForm.Name = of.Name;
                service.Update(orgForm);
            }
            else
            {
                orgForm = new OrganizationForm()
                {
                    Name = of.Name,
                    Code = of.NewCode
                };
                service.Save(orgForm);
            }


        }

        private class OrgForm
        {
            public string NewCode { get; set; }
            public string Name { get; set; }
            public string OldCode { get; set; }
        }
    }
}
