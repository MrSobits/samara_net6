namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;

    public class CorrectionMoSettlementArchangelskAction : BaseExecutionAction
    {
        public override string Description => "Изменение МО (поселения) домов после импорта ОКТМО (Архангельск)";

        public override string Name => "Изменение МО (поселения) домов (Архангельск)";

        public override Func<IDataResult> Action => this.CorrectionMoSettlement;

        public BaseDataResult CorrectionMoSettlement()
        {
            var municipalityRepository = this.Container.Resolve<IRepository<Municipality>>();
            var realtyObjRepository = this.Container.Resolve<IRepository<RealityObject>>();

            using (this.Container.Using(municipalityRepository, realtyObjRepository))
            {
                var muList = municipalityRepository.GetAll().ToList();

                // заменяем МО-поселение у некоторых домов
                var dict = new Dictionary<string, Municipality>();
                dict.Add("11608000#д. Кондратовская", muList.FirstOrDefault(x => x.Oktmo == "11608440" || x.Oktmo == "011608440"));
                dict.Add("11627000#д. Молодиловская", muList.FirstOrDefault(x => x.Oktmo == "11627157" || x.Oktmo == "011627157"));
                dict.Add("11644000#п. Шестиозерский", muList.FirstOrDefault(x => x.Oktmo == "11644431" || x.Oktmo == "011644431"));
                dict.Add("11644000#д. Андреевская", muList.FirstOrDefault(x => x.Oktmo == "11644428" || x.Oktmo == "011644428"));
                dict.Add("11644000#д. Иваново", muList.FirstOrDefault(x => x.Oktmo == "11644101" || x.Oktmo == "011644101"));
                dict.Add("11654000#д. Веригинская", muList.FirstOrDefault(x => x.Oktmo == "11654408" || x.Oktmo == "011654408"));
                dict.Add("11654000#д. Бережная", muList.FirstOrDefault(x => x.Oktmo == "11654408" || x.Oktmo == "011654408"));
                dict.Add("11605000#д. Семеновская", muList.FirstOrDefault(x => x.Oktmo == "11605456" || x.Oktmo == "011605456"));

                var realtyObjs = realtyObjRepository.GetAll()
                    .Where(x => x.Municipality != null)
                    .Where(x => x.FiasAddress != null)
                    .Where(x => dict.Keys.Contains(x.Municipality.Oktmo.ToString() + "#" + x.FiasAddress.PlaceName))
                    .ToList();

                realtyObjs.ForEach(
                    x =>
                    {
                        var newMoSettlement = dict[x.Municipality.Oktmo.ToString() + "#" + x.FiasAddress.PlaceName];
                        if (newMoSettlement != null)
                        {
                            x.MoSettlement = newMoSettlement;
                            realtyObjRepository.Update(x);
                        }
                    });

                // заменяем МО Тимошинское на Двинское
                var dvinskoe = muList.FirstOrDefault(x => x.Oktmo == "11608422" || x.Oktmo == "011608422");

                var realtyObjsTim = realtyObjRepository.GetAll()
                    .Where(x => x.MoSettlement.Oktmo == "11608452" || x.MoSettlement.Oktmo == "011608452")
                    .ToList();

                if (dvinskoe != null)
                {
                    realtyObjsTim.ForEach(
                        x =>
                        {
                            x.MoSettlement = dvinskoe;
                            realtyObjRepository.Update(x);
                        });
                }
            }

            return new BaseDataResult();
        }
    }

    internal struct LocalityMu
    {
        public int MuOktmo { get; set; }

        public string PlaceName { get; set; }
    }
}