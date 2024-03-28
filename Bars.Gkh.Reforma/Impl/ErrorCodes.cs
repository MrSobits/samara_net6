namespace Bars.Gkh.Reforma.Impl
{
    /// <summary>
    ///     Коды ошибок Реформы
    /// </summary>
    public static class ErrorCodes
    {
        #region Constants

        /// <summary>
        ///     (1002) Логин не найден или логин не соответствует паролю.
        /// </summary>
        public const string AuthenticationFailed = "1002";

        /// <summary>
        ///     (1022) Файл с таким расширение не допустим к загрузке
        /// </summary>
        public const string ExtensionIsNotValid = "1022";

        /// <summary>
        ///     (1020) Внешняя система не подписана на раскрытие данных по  управляющей организации
        /// </summary>
        public const string ExternalSystemHasNoRequest = "1020";

        /// <summary>
        ///     (1010) Поле «НАЗВАНИЕ ПОЛЯ» заполнено не верно
        /// </summary>
        public const string FieldIsNotValid = "1010";

        /// <summary>
        ///     (1023) Размер загружаемого файла превышает 15MB
        /// </summary>
        public const string FileSizeLimitExceeded = "1023";

        /// <summary>
        ///     (403) Нет прав на выполнение запроса
        /// </summary>
        public const string Forbidden = "403";

        /// <summary>
        ///     (1033) Дом с указанным адресом не найден
        /// </summary>
        public const string HouseAddressWasntFound = "1033";

        /// <summary>
        ///     (1004) Дом с указанным идентификатором не найден
        /// </summary>
        public const string HouseIdentifierWasntFound = "1004";

        /// <summary>
        ///     (1008) Дом не в управлении
        /// </summary>
        public const string HouseIsManagedByAnyOrganization = "1008";

        /// <summary>
        ///     (1011) Дом, находится в управлении другой организации «НАЗВАНИЕ УО», по которой нет подписки
        /// </summary>
        public const string HouseIsNotInCompany = "1011";

        /// <summary>
        ///     (1012) Дом уже под управлением
        /// </summary>
        public const string HouseUnderTheManagement = "1012";

        /// <summary>
        ///     (1036) Управление домом уже прекращено
        /// </summary>
        public const string HouseUnlinkedAlready = "1036";

        /// <summary>
        ///     (1005) Управляющая организация с  указанным ИНН не найдена
        /// </summary>
        public const string InnWasntFound = "1005";

        /// <summary>
        ///     (500) Внутренняя ошибка сервера. Сервер неспособен выполнить запрос. Попробуйте обратиться позже.
        /// </summary>
        public const string InternalServerError = "500";

        /// <summary>
        ///     (1009) Обязательное поле «НАЗВАНИЕ ПОЛЯ» не заполнено
        /// </summary>
        public const string MandatoryFieldIsNull = "1009";

        /// <summary>
        ///     (1014) Населенный пункт с указанным идентификатором не найден
        /// </summary>
        public const string MissingCity = "1014";

        /// <summary>
        ///     (1027) Анкета управляющей организации за указанный отчетный период не найдена
        /// </summary>
        public const string MissingCompanyProfileInThisReportingPeriod = "1027";

        /// <summary>
        ///     (1035) Файл с указанным идентификатором не найден
        /// </summary>
        public const string MissingFile = "1035";

        /// <summary>
        ///     (1007) Отчетный период с указанным идентификатором не найден
        /// </summary>
        public const string MissingReportingPeriod = "1007";

        /// <summary>
        ///     (1015) Улица с указанным идентификатором не найдена
        /// </summary>
        public const string MissingStreet = "1015";

        /// <summary>
        ///     (1016) Взаимодействие с системой "Реформа ЖКХ" запрещено
        /// </summary>
        public const string NoInteraction = "1016";

        /// <summary>
        ///     (1021) У внешней системы нет разрешения на раскрытие данных от  управляющей организации
        /// </summary>
        public const string NoPermissions = "1021";

        /// <summary>
        ///     (404) Указанный метод не найден
        /// </summary>
        public const string NotFound = "404";

        /// <summary>
        ///     (1028) Указан раздел "ИДЕНТИФИКАТОР РАЗДЕЛА", который не относится к анкете управляющей организации
        /// </summary>
        public const string PartIsNotInCompanyProfile = "1028";

        /// <summary>
        ///     (1026) Указан раздел "ИДЕНТИФИКАТОР РАЗДЕЛА", который не относится к анкете дома
        /// </summary>
        public const string PartIsNotInHouseProfile = "1026";

        /// <summary>
        ///     (1029) Запрос не создан. Запрос на подписку  уже был подан
        /// </summary>
        public const string RequestHaveBeenAlreadySubmitted = "1029";

        /// <summary>
        ///     (1032) Управляющая организация с указанным ИНН уже зарегистрирована в системе
        /// </summary>
        public const string TheCompanyWithInnAlreadyExist = "1032";

        /// <summary>
        ///     (401) Неавторизованный запрос
        /// </summary>
        public const string Unauthorized = "401";

        /// <summary>
        ///     (1006) Пользователь не соответствует внешней системе
        /// </summary>
        public const string UserDoesntConformToExternalSystem = "1006";

        /// <summary>
        ///     (1003) Пользователь заблокирован
        /// </summary>
        public const string UserIsBlocked = "1003";

        /// <summary>
        ///     (1013) Не заполнена дата начала управления домом. Прежде чем прекратить управление домом, Вам необходимо заполнить
        ///     дату начала управления с помощью метода SetHouseProfile
        /// </summary>
        public const string MissingManagementDate = "1013";

        #endregion
    }
}