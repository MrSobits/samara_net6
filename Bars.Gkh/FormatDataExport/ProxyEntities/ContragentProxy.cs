namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxySelectors;

    /// <summary>
    /// Контрагент + Банки
    /// </summary>
    public class ContragentProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код контрагента
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Идентификатор контрагента в системе
        /// </summary>
        public long? ContragentId { get; set; }

        /// <summary>
        /// Идентификатор кредитной организации
        /// </summary>
        public long? BankId { get; set; }

        /// <summary>
        /// 2. Тип Контрагента
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 3. Полное наименование
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 4. Краткое наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 5. Фирменное наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 6. Фамилия ИП
        /// </summary>
        public string SurnameOfIndEnt => this.Type == 3 ? this.Contact?.Surname : null;

        /// <summary>
        /// 7. Имя ИП
        /// </summary>
        public string NameOfIndEnt => this.Type == 3 ? this.Contact?.Name : null;

        /// <summary>
        /// 8. Отчество ИП
        /// </summary>
        public string PatronymicOfIndEnt => this.Type == 3 ? this.Contact?.Patronymic : null;

        /// <summary>
        /// 9. Пол ИП
        /// </summary>
        public int? GenderOfIndEnt => this.GetGender();

        /// <summary>
        /// 10. Юридический адрес по ФИАС
        /// </summary>
        public string LegalFiasAddress { get; set; }

        /// <summary>
        /// 11. Юридический адрес
        /// </summary>
        public string LegalAddress { get; set; }

        /// <summary>
        /// 12. Фактический адрес по ФИАС
        /// </summary>
        public string ActualFiasAddress { get; set; }

        /// <summary>
        /// 13. Фактический адрес
        /// </summary>
        public string ActualAddress { get; set; }

        /// <summary>
        /// 14. Почтовый адрес по ФИАС
        /// </summary>
        public string PostFiasAddress { get; set; }

        /// <summary>
        /// 15. Потовый адрес
        /// </summary>
        public string PostAddress { get; set; }

        /// <summary>
        /// 16. ОГРН (ОГРНИП)
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// 17. ИНН
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// 18. КПП
        /// </summary>
        public string Kpp { get; set; }

        /// <summary>
        /// 19. Номер записи об аккредитации
        /// </summary>
        public string AccreditationNumber { get; set; }

        /// <summary>
        /// 20. Страна регистрации
        /// </summary>
        /// <remarks>
        /// По умолчанию: 643/Россия/Russian Federation/Европа
        /// </remarks>
        public int IncorporationCountry { get; set; } = 643;

        /// <summary>
        /// 21. Орган, принявший решение о регистрации
        /// </summary>
        public string Registrator { get; set; }

        /// <summary>
        /// 22. Дата регистрации
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// 23. ОКОПФ
        /// </summary>
        public string Okopf { get; set; }

        /// <summary>
        /// 24. Дата прекращения деятельности
        /// </summary>
        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// 25. Статус активности
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 26. Сайт
        /// </summary>
        public string WebSite { get; set; }

        /// <summary>
        /// 27. Должность + ФИО руководителя организации в родительном падеже (в лице кого)
        /// </summary>
        public string Position => this.GetPosition();

        /// <summary>
        /// 28. Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 29. Факс
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 30. Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 31. Код головной организации
        /// </summary>
        [ProxyId(typeof(ContragentProxy))]
        public long? ParentId { get; set; }

        /// <summary>
        /// 32. Контрагент является субъектом малого предпринимательства
        /// </summary>
        public int? IsSmallBusiness { get; set; }

        /// <summary>
        /// Является кредитной организацией
        /// </summary>
        public bool IsBankContragent { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public long? Oktmo { get; set; }

        /// <summary>
        /// Код роли контрагента
        /// </summary>
        public string MainRoleCode { get; set; }

        #region Банки
        /// <summary>
        /// 2. БИК банка
        /// </summary>
        public string Bik { get; set; }

        /// <summary>
        /// 3. Корреспондентский счет
        /// </summary>
        public string CorrAccount { get; set; }
        #endregion

        /// <summary>
        /// Контакт
        /// </summary>
        public ContragentContact Contact { private get; set; }

        private int? GetGender()
        {
            if (this.Contact != null && this.Type == 3) // ИП
            {
                if (this.Contact.Gender == Gender.Male)
                {
                    return 1;
                }
                if (this.Contact.Gender == Gender.Female)
                {
                    return 2;
                }
            }

            return null;
        }

        private string GetPosition()
        {
            if (this.Contact != null)
            {
                if (this.Contact.Position != null)
                {
                    return $"{this.Contact.Position.Name} {this.Contact.FullName}";
                }
                else
                {
                    return this.Contact.FullName;
                }
            }

            return null;
        }
    }
}