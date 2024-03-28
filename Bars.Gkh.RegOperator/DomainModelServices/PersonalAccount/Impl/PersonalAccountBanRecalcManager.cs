namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using Entities;
    using Entities.PersonalAccount;
    using PersonalAccount;

    /// <summary>
    /// Менеджер запрета перерасчета
    /// </summary>
    public class PersonalAccountBanRecalcManager : IPersonalAccountBanRecalcManager
    {
        public IWindsorContainer Container { get; set; }

        private readonly List<PersonalAccountBanRecalc> banRecalcBuffer;
        private readonly List<EntityLogLight> entityLogLightBuffer;
        private string login;

        private string Login => this.login ?? (this.login = this.userManager.GetActiveUser()?.Login ?? "anonymous");

        private readonly IDomainService<PersonalAccountBanRecalc> banRecalcDomain;
        private readonly IDomainService<EntityLogLight> entityLogLightDomain;
        private readonly IGkhUserManager userManager;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="banRecalcDomain">Запрет перерасчета за период</param>
        /// <param name="userManager">Менеджер пользователей</param>
        /// <param name="entityLogLightDomain">Легковесная сущность для хранения изменения сущности</param>
        public PersonalAccountBanRecalcManager(IDomainService<PersonalAccountBanRecalc> banRecalcDomain, IGkhUserManager userManager, IDomainService<EntityLogLight> entityLogLightDomain)
        {
            this.banRecalcDomain = banRecalcDomain;
            this.entityLogLightDomain = entityLogLightDomain;
            this.userManager = userManager;

            this.banRecalcBuffer = new List<PersonalAccountBanRecalc>();
            this.entityLogLightBuffer = new List<EntityLogLight>();
        }

        /// <summary>
        /// Создание запрета перерасчета
        /// </summary>
        /// <param name="account"></param>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="type"></param>
        /// <param name="fileInfo"></param>
        /// <param name="reason"></param>
        public void CreateBanRecalc(BasePersonalAccount account, DateTime dateStart, DateTime dateEnd, BanRecalcType type, FileInfo fileInfo, string reason)
        {
            this.banRecalcBuffer.Add(new PersonalAccountBanRecalc
            {
                PersonalAccount = account,
                DateStart = dateStart,
                DateEnd = dateEnd,
                File = fileInfo,
                Reason = reason,
                Type = type
            }); 

            this.entityLogLightBuffer.Add(new EntityLogLight
            {
                ClassName = "BasePersonalAccount",
                EntityId = account.Id,
                ParameterName = "Запрет перерасчета",
                PropertyName = "Запрет перерасчета",
                DateActualChange = dateStart,
                DateEnd = dateEnd,
                DateApplied = DateTime.Now,
                Reason = reason,
                Document = fileInfo,
                PropertyValue = "",
                PropertyDescription = $"Установка {type.GetDisplayName()} с {dateStart:MM.yyyy} по {dateEnd:MM.yyyy}",
                User = this.Login
            });
        }

        /// <summary>
        /// Сохранение созданных изменений
        /// </summary>
        public void SaveBanRecalcs()
        {
            if (!this.banRecalcBuffer.Any())
            {
                return;
            }

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                this.banRecalcBuffer.ForEach(x => this.banRecalcDomain.Save(x));
                this.entityLogLightBuffer.ForEach(x => this.entityLogLightDomain.Save(x));

                this.banRecalcBuffer.Clear();
                this.entityLogLightBuffer.Clear();

                transaction.Commit();
            }
        }
    }
}