namespace Bars.Gkh.FormatDataExport.ExportableEntities.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контрагенты
    /// </summary>
    public class ContragentExportableEntity : BaseExportableEntity
    {
        /// <inheritdoc />
        public override string Code => "CONTRAGENT";

        /// <inheritdoc />
        protected override IList<ExportableRow> GetEntityData()
        {
            // как временное решение (не точно) для ссылающийся саму на себя секции, в данном случае ParentId
            var extProxyListCache = this.ProxySelectorFactory.GetSelector<ContragentProxy>()
                .ExtProxyListCache;

            return  this.ProxySelectorFactory.GetSelector<ContragentProxy>()
                .ExtProxyListCache
                .Select(x => new ExportableRow(x.Id,
                    new List<string>
                    {
                        x.Id.ToStr(),
                        x.Type.ToStr(),
                        x.FullName.Cut(255),
                        x.ShortName.Cut(200),
                        x.Name.Cut(255),
                        x.SurnameOfIndEnt.Cut(60),
                        x.NameOfIndEnt.Cut(255),
                        x.PatronymicOfIndEnt.Cut(255),
                        x.GenderOfIndEnt.ToStr(),
                        x.LegalFiasAddress.Cut(36),
                        x.LegalAddress.Cut(2000),
                        x.ActualFiasAddress.Cut(36),
                        x.ActualAddress.Cut(2000),
                        x.PostFiasAddress.Cut(36),
                        x.PostAddress.Cut(2000),
                        x.Ogrn.Cut(15),
                        x.Inn.Cut(12),
                        x.Kpp.Cut(9),
                        x.AccreditationNumber.Cut(11),
                        x.IncorporationCountry.ToStr(),
                        x.Registrator.Cut(50),
                        this.GetDate(x.RegistrationDate),
                        x.Okopf.Cut(5),
                        this.GetDate(x.TerminationDate),
                        x.IsActive ? this.Yes : this.No,
                        x.WebSite.Cut(200),
                        x.Position.Cut(200),
                        x.Phone.Cut(50),
                        x.Fax.Cut(30),
                        x.Email.Cut(100),
                        x.ParentId.ToStr(),
                        x.IsSmallBusiness.ToStr()
                    }))
                .ToList();
        }

        /// <inheritdoc />
        public override IList<string> GetHeader()
        {
            return new List<string>
            {
                "Уникальный код контрагента",
                "Тип Контрагента",
                "Полное наименование",
                "Краткое наименование",
                "Фирменное наименование ",
                "Фамилия ИП",
                "Имя ИП",
                "Отчество ИП",
                "Пол ИП",
                "Юридический адрес по ФИАС",
                "Юридический адрес",
                "Фактический адрес по ФИАС",
                "Фактический адрес",
                "Почтовый адрес по ФИАС",
                "Потовый адрес",
                "ОГРН (ОГРНИП)",
                "ИНН",
                "КПП",
                "Номер записи об аккредитации",
                "Страна регистрации",
                "Орган, принявший решение о регистрации",
                "Дата регистрации",
                "ОКОПФ",
                "Дата прекращения деятельности",
                "Статус активности",
                "Сайт",
                "Должность + ФИО руководителя организации в родительном падеже (в лице кого)",
                "Телефон",
                "Факс",
                "Электронная почта",
                "Код головной организации",
                "Контрагент является субъектом малого предпринимательства"
            };
        }

        /// <inheritdoc />
        protected override Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = (cell, row) =>
        {
            switch (cell.Key)
            {
                case 0:
                case 1:
                case 15:
                case 16:
                    return row.Cells[cell.Key].IsEmpty();
                case 2:
                    if (row.Cells[1] == "1" || row.Cells[1] == "2" || row.Cells[1] == "4")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 5:
                case 6:
                    if (row.Cells[1] == "3")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 9:
                case 18:
                case 19:
                    if (row.Cells[1] == "4")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 10:
                case 22:
                case 30:
                    if (row.Cells[1] == "2")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
                case 17:
                    if (row.Cells[1] == "2" || row.Cells[1] == "4")
                    {
                        return row.Cells[cell.Key].IsEmpty();
                    }
                    break;
            }
            return false;
        };

        /// <inheritdoc />
        /// Специально сделана обратная зависимость, для того, чтобы получить список банков в <see cref="ContragentRoleExportableEntity"/>
        /// через <see cref="ProxySelectorFactory.GetSelector{ContragentProxy}()"/>.ExtProxyListCache
        public override IList<string> GetInheritedEntityCodeList()
        {
            return this.GetEntityCodeList(typeof(ContragentRoleExportableEntity));
        }
    }
}