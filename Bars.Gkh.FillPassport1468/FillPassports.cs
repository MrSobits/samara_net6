namespace Bars.Gkh.FillPassport1468
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;

    public class FillPassports : BaseExecutionAction
    {
        public override string Description => @"Заполнение паспортов 1468 для МКД";

        public override string Name => "1468 - Заполнение паспортов";

        public override Func<IDataResult> Action => this.Fill;

        public IDomainService<HouseProviderPassport> PaspProviderService { get; set; }

        public IDomainService<MetaAttribute> DomainServiceMetaAttribute { get; set; }

        public IDomainService<PassportStruct> PassportStructService { get; set; }

        public IDomainService<HouseProviderPassportRow> RowsService { get; set; }

        public IDomainService<RealityObject> RoService { get; set; }

        public IDomainService<Part> PartService { get; set; }

        private List<HouseProviderPassportRow> RowsForSave { get; set; }

        public IDomainService<RealityObjectMissingCeo> MissSeService { get; set; }

        public IDomainService<RealityObjectStructuralElement> SeService { get; set; }

        private BaseDataResult Fill()
        {
            this.RowsForSave = new List<HouseProviderPassportRow>();

            var metaDict = new Dictionary<string, MetaAttribute>();

            var pasportStruct = this.PassportStructService.GetAll()
                .Where(x => x.PassportType == PassportType.Mkd)
                .Where(x => (x.ValidFromYear < 2014) || (x.ValidFromYear == 2014 && x.ValidFromMonth <= 11))
                .OrderByDescending(x => x.ValidFromYear)
                .ThenByDescending(x => x.ValidFromMonth)
                .FirstOrDefault();

            var parts = this.PartService.GetAll().Where(x => x.Struct == pasportStruct)
                .Select(x => x.Id).ToList();

            var metaList = this.DomainServiceMetaAttribute.GetAll()
                .Where(
                    x => parts.Contains(x.ParentPart.Id) &&
                        (x.Type == MetaAttributeType.Simple || x.Type == MetaAttributeType.GroupedWithValue))
                .ToList();

            metaList.ForEach(
                x =>
                {
                    if (!metaDict.ContainsKey(x.IntegrationCode))
                    {
                        metaDict.Add(x.IntegrationCode, x);
                    }
                });

            var values = this.RowsService.GetAll()
                .Where(
                    x => (x.ProviderPassport.RealityObject.TypeHouse == TypeHouse.ManyApartments ||
                        x.ProviderPassport.RealityObject.TypeHouse == TypeHouse.SocialBehavior) &&
                        x.ProviderPassport.PassportStruct.PassportType == PassportType.Mkd).ToList();

            var provPasList = this.PaspProviderService.GetAll()
                .Where(x => x.HouseType == HouseType.Mkd && x.ReportYear == 2014 && x.ReportMonth == 11)
                .ToList();

            var houseInfo = this.RoService.GetAll()
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments || x.TypeHouse == TypeHouse.SocialBehavior)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Address,
                        x.BuildYear,
                        TypeProject = x.TypeProject.Name,
                        x.CadastreNumber,
                        x.AreaMkd,
                        x.AreaNotLivingFunctional,
                        x.NumberApartments,
                        x.NumberLiving,
                        x.MaximumFloors,
                        x.Floors,
                        x.NumberEntrances,
                        x.HeatingSystem
                    }).ToList();

            var misCe = this.MissSeService.GetAll()
                .Where(
                    x =>
                        x.RealityObject.TypeHouse == TypeHouse.ManyApartments ||
                            x.RealityObject.TypeHouse == TypeHouse.SocialBehavior)
                .Select(
                    x => new ConstrEl
                    {
                        Id = x.RealityObject.Id,
                        Name = x.MissingCommonEstateObject.GroupType.Name
                    }).ToList();

            var existCe = this.SeService.GetAll()
                .Where(
                    x =>
                        x.RealityObject.TypeHouse == TypeHouse.ManyApartments ||
                            x.RealityObject.TypeHouse == TypeHouse.SocialBehavior)
                .Select(
                    x => new ConstrEl
                    {
                        Id = x.RealityObject.Id,
                        Name = x.StructuralElement.Group.Name
                    }).ToList();

            // заполняем инфо по дому
            houseInfo.ForEach(
                x =>
                {
                    var passport = provPasList.FirstOrDefault(p => p.RealityObject.Id == x.Id);
                    if (passport != null)
                    {
                        this.SetValue(values, x.Id, "C_1.1.", x.Id.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.2.", x.Address, metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.2.", x.BuildYear.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.1.", x.TypeProject, metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.3.2.", x.CadastreNumber, metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.15.", x.AreaMkd.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.19.6.", x.AreaNotLivingFunctional.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.18.1.", x.NumberApartments.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.9.", x.NumberLiving.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.7.", x.MaximumFloors.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.6.", x.Floors.ToStr(), metaDict, passport);
                        this.SetValue(values, x.Id, "C_1.5.4.", x.NumberEntrances.ToStr(), metaDict, passport);
                        if (x.HeatingSystem == HeatingSystem.Centralized)
                        {
                            this.SetValue(values, x.Id, "C_7.2.1.1.", "Да", metaDict, passport);
                        }
                        if (x.HeatingSystem == HeatingSystem.Individual)
                        {
                            this.SetValue(values, x.Id, "C_7.2.1.3.", "Да", metaDict, passport);
                        }

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.2.1.",
                            this.ConExp(misCe, Cond.NotIn, "Инженерные системы холодного водоснабжения".ToLowerInvariant().Replace(" ", ""), x.Id)
                                && this.ConExp(existCe, Cond.In, "Инженерные системы холодного водоснабжения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.2.2.",
                            this.ConExp(misCe, Cond.In, "Инженерные системы холодного водоснабжения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.3.1.",
                            this.ConExp(misCe, Cond.NotIn, "Инженерные системы водоотведения".ToLowerInvariant().Replace(" ", ""), x.Id)
                                && this.ConExp(existCe, Cond.In, "Инженерные системы водоотведения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.3.2.",
                            this.ConExp(misCe, Cond.In, "Инженерные системы водоотведения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.4.6.",
                            this.ConExp(misCe, Cond.In, "Инженерные системы горячего водоснабжения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.5.1.",
                            this.ConExp(misCe, Cond.NotIn, "Система электроснабжения".ToLowerInvariant().Replace(" ", ""), x.Id)
                                && this.ConExp(existCe, Cond.In, "Система электроснабжения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.5.2.",
                            this.ConExp(misCe, Cond.In, "Система электроснабжения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.6.3.",
                            this.ConExp(misCe, Cond.In, "Система газоснабжения".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);

                        this.SetValueForBool(
                            values,
                            x.Id,
                            "C_7.2.7.4.",
                            this.ConExp(misCe, Cond.In, "Система вентиляции".ToLowerInvariant().Replace(" ", ""), x.Id),
                            metaDict,
                            passport);
                    }
                });

            TransactionHelper.InsertInManyTransactions(this.Container, this.RowsForSave, useStatelessSession: true);

            //Обновляем процент заполнения
            provPasList.ForEach(this.UpdateFillPercent);

            return new BaseDataResult(true);
        }

        private bool ConExp(List<ConstrEl> constrEls, Cond cond, string value, long roId)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(value))
            {
                if (cond == Cond.In && constrEls.Any())
                {
                    result = constrEls.Any(x => x.Name == value && x.Id == roId);
                }

                if (cond == Cond.NotIn)
                {
                    result = !constrEls.Any(x => x.Name == value && x.Id == roId);
                }
            }
            return result;
        }

        private void SetValueForBool(
            List<HouseProviderPassportRow> values,
            long roId,
            string intKey,
            bool value,
            Dictionary<string, MetaAttribute> metaDict,
            HouseProviderPassport passport)
        {
            if (value)
            {
                var row = values.FirstOrDefault(x => x.ProviderPassport.RealityObject.Id == roId && x.MetaAttribute.IntegrationCode == intKey);

                if (row == null)
                {
                    row = new HouseProviderPassportRow
                    {
                        MetaAttribute = metaDict[intKey],
                        ProviderPassport = passport
                    };
                }

                row.Value = "Да";
                this.RowsForSave.Add(row);
            }
        }

        private void SetValue(
            List<HouseProviderPassportRow> values,
            long roId,
            string intKey,
            string value,
            Dictionary<string, MetaAttribute> metaDict,
            HouseProviderPassport passport)
        {
            var row = values.FirstOrDefault(x => x.ProviderPassport.RealityObject.Id == roId && x.MetaAttribute.IntegrationCode == intKey);

            if (row == null)
            {
                row = new HouseProviderPassportRow();
            }

            row.MetaAttribute = metaDict[intKey];
            row.ProviderPassport = passport;
            row.Value = value;

            this.RowsForSave.Add(row);
        }

        protected void UpdateFillPercent(HouseProviderPassport providerPasport)
        {
            var dsMetaAttr = this.Container.Resolve<IDomainService<MetaAttribute>>();

            var allCnt = dsMetaAttr.GetAll()
                .Where(x => x.ParentPart.Struct.Id == providerPasport.PassportStruct.Id)
                .Where(x => x.Type == MetaAttributeType.Simple || x.Type == MetaAttributeType.GroupedWithValue)
                .Select(x => x.Id)
                .Distinct()
                .Count();

            var fillCnt = this.RowsService.GetAll()
                .Where(x => x.ProviderPassport.Id == providerPasport.Id && x.Value != null && x.Value.Trim() != " ")
                .Select(x => x.MetaAttribute.Id)
                .Distinct()
                .Count();

            providerPasport.Percent = ((decimal) fillCnt * 100 / allCnt).RoundDecimal(20);

                // RoundDecimal(20) - Костыль для клиента Oracle во избежание ошибки переполнения OCI-22053, возникающей при точности более 26 знаков после запятой 

            this.PaspProviderService.Update(providerPasport);
        }
    }
}