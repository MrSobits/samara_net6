namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.Gkh.Authentification;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    using NHibernate.Util;

    public class RealityObjectOutdoorProgramDomainService : FileStorageDomainService<RealityObjectOutdoorProgram>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var result = base.Save(baseParams);
            this.UpdateChangeJournal(result);
            return result;
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var result = base.Update(baseParams);
            this.UpdateChangeJournal(result);
            return result;
        }

        /// <summary>
        /// Обновление журнала программы.
        /// </summary>
        private void UpdateChangeJournal(IDataResult result)
        {
            // в случае успешного сохранения, добавляем запись в журнал
            if (!result.Success || !(result.Data is IEnumerable<RealityObjectOutdoorProgram> data) || !data.Any())
            {
                return;
            }

            var userManager = this.Container.Resolve<IGkhUserManager>();
            var changeJournalDomain = this.Container.Resolve<IDomainService<RealityObjectOutdoorProgramChangeJournal>>();

            using (this.Container.Using(userManager, changeJournalDomain))
            {
                var user = userManager.GetActiveUser();
                this.InTransaction(() =>
                {
                    foreach (var program in data)
                    {
                        changeJournalDomain.Save(new RealityObjectOutdoorProgramChangeJournal
                        {
                            RealityObjectOutdoorProgram = program,
                            ChangeDate = DateTime.Now,
                            UserName = user == null ? "Администратор" : user.Name
                        });
                    }
                });
            }
        }
    }
}
