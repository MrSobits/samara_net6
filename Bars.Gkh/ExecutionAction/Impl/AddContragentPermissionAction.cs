namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Core.Internal;

    public class AddContragentPermissionAction : BaseExecutionAction
    {
        public override string Description => @"Добавить права на обязательность полям контрагента ";

        public override string Name => "Добавить права на обязательность полям контрагента ";

        public override Func<IDataResult> Action => this.AddContragentPermission;

        public BaseDataResult AddContragentPermission()
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var fieldRequirementDomain = this.Container.ResolveDomain<FieldRequirement>();

                var requirements = new List<string>
                {
                    "Gkh.Orgs.Contragent.Field.Inn_Rqrd",
                    "Gkh.Orgs.Contragent.Field.Kpp_Rqrd",
                    "Gkh.Orgs.Contragent.Field.FiasJuridicalAddress_Rqrd",
                    "Gkh.Orgs.Contragent.Field.FiasFactAddress_Rqrd",
                    "Gkh.Orgs.Contragent.Field.Ogrn_Rqrd",
                    "Gkh.Orgs.Contragent.Field.Oktmo_Rqrd"
                };

                try
                {
                    var existPerms =
                        fieldRequirementDomain.GetAll()
                            .Where(x => requirements.Contains(x.RequirementId))
                            .Select(x => x.RequirementId)
                            .Distinct()
                            .ToList();

                    fieldRequirementDomain.GetAll().AsEnumerable().ForEach(
                        x =>
                        {
                            foreach (var requirement in requirements.Where(y => !existPerms.Contains(y)))
                            {
                                fieldRequirementDomain.Save(
                                    new FieldRequirement()
                                    {
                                        RequirementId = requirement
                                    });
                            }
                        });

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(fieldRequirementDomain);
                }
            }

            return new BaseDataResult();
        }
    }
}