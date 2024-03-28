namespace Bars.Gkh1468.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    public class SetParentValueForPassportRows : BaseExecutionAction
    {

        public ISessionProvider SessionProvider { get; set; }

        public override string Description => "Сохранение иерархии значений атрибутов старых паспортов";

        public override string Name => "1468 - миграция старых значений паспортов";

        public override Func<IDataResult> Action => this.Restore;

        private MetaAttribute GetParent(MetaAttribute node)
        {
            if (node.Parent == null)
            {
                return null;
            }

            if (node.Parent.Type == MetaAttributeType.GroupedComplex)
            {
                return node.Parent;
            }

            return GetParent(node.Parent);
        }

        public BaseDataResult Restore()
        {
            RestoreProviderPassportRow<HouseProviderPassportRow, HouseProviderPassport>();
            RestoreProviderPassportRow<OkiProviderPassportRow, OkiProviderPassport>();

            return new BaseDataResult();
        }

        private void RestoreProviderPassportRow<TRow, TProvider>() where TRow : BaseProviderPassportRow<TProvider>, new() where TProvider : BaseProviderPassport
        {
            var domain = Container.ResolveDomain<TRow>();
            try
            {
                var rows = domain.GetAll().Where(x => x.GroupKey > 0).ToList();

                var metaGroup = rows.GroupBy(x => GetParent(x.MetaAttribute));

                var multyMetaRows =
                    domain.GetAll()
                        .Where(x => x.MetaAttribute.Type == MetaAttributeType.GroupedComplex)
                        .GroupBy(x => new { meta = x.MetaAttribute.Id, passport = x.Passport.Id, provider = x.ProviderPassport.Id, value = x.Value })
                        .ToDictionary(x => x.Key, x => x.First().Id);

                var valuesForSave = new List<TRow>(5000);

                using (var session = SessionProvider.OpenStatelessSession())
                {
                    foreach (var keyPair in metaGroup)
                    {
                        if (keyPair.Key != null && keyPair.Key.Type == MetaAttributeType.GroupedComplex)
                        {
                            var meta = keyPair.Key;

                            var providerGroup = keyPair.GroupBy(x => x.ProviderPassport);

                            foreach (var providerRows in providerGroup)
                            {
                                var provider = providerRows.Key;

                                var passportGroup = providerRows.GroupBy(x => x.Passport);

                                foreach (var passports in passportGroup)
                                {
                                    var passport = passports.Key;
                                    var groupKeyRowsGroup = passports.GroupBy(x => x.GroupKey);

                                    foreach (var row in groupKeyRowsGroup)
                                    {
                                        var parentValue = row.Key.ToStr();
                                        var parentId = multyMetaRows.Get(new { meta = meta.Id, passport = passport.Id, provider = provider.Id, value = parentValue });

                                        if (parentId == 0)
                                        {
                                            parentId =
                                                (long)
                                                session.Insert(
                                                    new TRow { MetaAttribute = meta, Passport = passport, ProviderPassport = provider, Value = parentValue, ObjectCreateDate = DateTime.Now });
                                        }

                                        foreach (var x in row)
                                        {
                                            if (x.ParentValue == parentId)
                                            {
                                                continue;
                                            }

                                            x.ParentValue = parentId;
                                            valuesForSave.Add(x);

                                            if (valuesForSave.Count < 5000)
                                            {
                                                continue;
                                            }

                                            TransactionHelper.InsertInManyTransactions(Container, valuesForSave, useStatelessSession: true);
                                            valuesForSave.Clear();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    TransactionHelper.InsertInManyTransactions(Container, valuesForSave, useStatelessSession: true);
                }
            }
            finally
            {
                Container.Release(domain);
            }
        }
    }
}