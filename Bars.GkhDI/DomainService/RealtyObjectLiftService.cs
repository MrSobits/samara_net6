namespace Bars.GkhDi.DomainService
{
    using B4;
    using Castle.Windsor;
    using Gkh.PassportProvider;
    using Gkh.Serialization;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Utils;
    using Gkh.Entities;
    using Services.DataContracts.GetManOrgRealtyObjectInfo;

    /// <summary>
    /// Сервис получение информации о лифтах Жилого дома 
    /// </summary>
    public class RealtyObjectLiftService : IRealtyObjectLiftService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получение информации о лифтах Жилого дома 
        /// </summary>
        /// <param name="realtyObjectId">Id жилого дома</param>
        /// <returns>Информация и лифтах жилого дома</returns>
        public HouseLift[] GetRealtyObjectLift(long realtyObjectId)
        {
            var realityObject= this.Container.ResolveDomain<RealityObject>().Get(realtyObjectId);

            var formId = "Form_4_2";
            var passport = this.Container.ResolveAll<IPassportProvider>().FirstOrDefault(x => x.Name == "Техпаспорт" && x.TypeDataSource == "xml");
            var componentCodes = passport.GetComponentCodes(formId).Where(x => x != "Form_4_2");
            var editors = (List<EditorTechPassport>)passport.GetEditors(formId);
            var tehPassportValues = this.Container.Resolve<IDomainService<TehPassportValue>>().GetAll()
                .Where(x => x.TehPassport.RealityObject.Id == realityObject.Id && componentCodes.Contains(x.FormCode)).ToArray();
            var ids = tehPassportValues.GroupBy(x => int.Parse(x.CellCode.Split(new char[] { ':' }).First()));

            var result =
                ids.Select(
                    id =>
                    new HouseLift
                    {
                        Id = id.Key,
                        PorchNumber = tehPassportValues.Where(y => y.CellCode == (id.Key + ":1")).Select(y => y.Value).FirstOrDefault(),
                        CommissioningYear = tehPassportValues.Where(y => y.CellCode == (id.Key + ":9")).Select(y => y.Value).FirstOrDefault(),
                        Type = tehPassportValues.Where(y => y.CellCode == (id.Key + ":19")).Select(
                                y =>
                                {
                                    var columnEditor = passport.GetEditorByFormAndComponentAndCode(formId, "Form_4_2_1", "19").ToString();
                                    var editorValue = editors.FirstOrDefault(x => x.Type == columnEditor).Values.FirstOrDefault(x => x.Code == y.Value);
                                    return editorValue != null ? editorValue.Name : "";
                                }).AggregateWithSeparator(x => x, ", ")
                    })
                    .ToArray();

            return result;
        }
    }
}