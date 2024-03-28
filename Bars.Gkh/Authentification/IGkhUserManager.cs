namespace Bars.Gkh.Authentification
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Entities;

    public interface IGkhUserManager : IUserManager
    {
        User SaveCustomUser(long id, string name, string login, string password, string email, IList<Role> listRoles);

        // Если необходимо получит ьсущность Оператора
        Operator GetActiveOperator();

        // Если необходимо получит ьактивного Юзера 
        // Напримр у Администратора нет оператора но часть проверок в системе идет по Роли
        User GetActiveUser();

        [Obsolete("Do not use. Use GetMunicipalityIds instead if you need ids only")]
        List<Municipality> GetActiveOperatorMunicipalities();

        bool HasOperatorInspector();

        bool HasOperatorContragent();

        bool HasOperatorMunicipalities();

        /// <summary>
        /// Идентификаторы контрагентов текущего оператора
        /// </summary>
        /// <returns></returns>
        List<long> GetContragentIds();

        /// <summary>
        /// Идентификаторы муниципальных образований текущего оператора
        /// </summary>
        /// <returns></returns>
        List<long> GetMunicipalityIds();

        /// <summary>
        /// Идентификаторы инспекторов текущего оператора
        /// </summary>
        /// <returns></returns>
        List<long> GetInspectorIds();

        /// <summary>
        /// Вовзращает список ролей, дома которой не фильтруются по организации 
        /// </summary>
        /// <returns>Список ролей</returns>
        IList<Role> GetNoServiceFilterRoles();

        /// <summary>
        /// Вовзращает список ролей, дома которой не фильтруются по организации 
        /// </summary>
        /// <param name="roleNames">Функция возвращающая список имен ролей, 
        ///     дома которой не фильтруются по организации</param>
        /// <returns></returns>
        IList<Role> GetNoServiceFilterRoles(Func<IList<string>> roleNames);

        /// <summary>
        /// Получение/создание базового оператора для входа через ЕСИА
        /// </summary>
        /// <returns></returns>
        Operator GetBaseOperatorForEsia();

        /// <summary>
        /// Получить список ролей текущего оператора
        /// </summary>
        IList<Role> GetActiveOperatorRoles();
    }
}