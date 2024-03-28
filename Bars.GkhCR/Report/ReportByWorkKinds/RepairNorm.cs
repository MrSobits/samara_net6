namespace Bars.GkhCr.Report.ReportByWorkKinds
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Enums;

    using System.Linq;

    public static class RepairNorm
    {
        private static readonly Func<TechnicalCharacteristics, decimal> HeatSystem = 
            x => x.TotalArea.HasValue ? x.TotalArea.Value * 0.6M : 0;

        private static readonly Func<TechnicalCharacteristics, decimal> RepairWaterSystem =
            x => x.TotalArea.HasValue ? x.TotalArea.Value * 0.15M : 0;

        private static readonly Func<TechnicalCharacteristics, decimal> RepairDrainageSystem =
            x => x.TotalArea.HasValue ? x.TotalArea.Value * 0.2M : 0;

        private static readonly Func<TechnicalCharacteristics, decimal> RepairEletricalSystem = RepairEletricalSystemRelease;

        private static decimal RepairEletricalSystemRelease(TechnicalCharacteristics x)
        {
            if (x.FlatsNum.HasValue && x.Storeys.HasValue && x.TotalArea.HasValue)
            {
                return ((x.FlatsNum.Value / (x.Storeys.Value * 4.0M) * 5M * x.Storeys.Value) + (x.TotalArea.Value * 2M / 13M)) * 1.2M;
            }

            return 0;
        }

        private static readonly Func<TechnicalCharacteristics, decimal> RepairBasements =
            x => x.Storeys.HasValue && x.Storeys.Value > 0 && x.TotalArea.HasValue ? x.TotalArea.Value / x.Storeys.Value : 0M;

        private static readonly Func<TechnicalCharacteristics, decimal> RepairPorch =
            x => x.TotalArea.HasValue ? x.TotalArea.Value * 0.3M : 0;

        private static readonly Func<TechnicalCharacteristics, decimal> RealtyObjectWorks =
            x => x.Storeys.HasValue && x.Storeys.Value > 0 && x.TotalArea.HasValue ? 
                    ((x.TotalArea.Value / (x.Storeys.Value * 13M) * 1.1M * 1.25M) + 13M) * 2M* 3M * x.Storeys.Value * 1.56M:
                    0M;

        private static readonly Func<TechnicalCharacteristics, decimal> RepairRoof =
            x =>
            {
                switch (x.TypeRoof)
                {
                    case TypeRoof.Plane:
                        return (x.Storeys.HasValue && x.Storeys.Value > 0 && x.TotalArea.HasValue ? x.TotalArea.Value / x.Storeys.Value : 0M) * 1.56M;
                    case TypeRoof.Pitched:
                        return (x.Storeys.HasValue && x.Storeys.Value > 0 && x.TotalArea.HasValue ? x.TotalArea.Value / x.Storeys.Value : 0M) * 2.002M;
                    default:
                        return 0;
                }
            };

        private static readonly Dictionary<TypeWorkCode, Func<TechnicalCharacteristics, decimal>> CalculationFormula
            = new Dictionary<TypeWorkCode, Func<TechnicalCharacteristics, decimal>>()
                  {
                      { TypeWorkCode.RepairHeatingSystem, HeatSystem },
                      { TypeWorkCode.RepairHotWaterSystem, RepairWaterSystem },
                      { TypeWorkCode.RepairColdWaterSystem, RepairWaterSystem },
                      { TypeWorkCode.RepairDrainageSystem, RepairDrainageSystem },
                      { TypeWorkCode.RepairEletricalSystem, RepairEletricalSystem },
                      { TypeWorkCode.RepairBasements, RepairBasements },
                      { TypeWorkCode.RepairRoof, RepairRoof },
                      { TypeWorkCode.RepairFacade, RealtyObjectWorks },
                      { TypeWorkCode.WinterizingFacade, RealtyObjectWorks },
                      { TypeWorkCode.StrengtheningFoundation, RealtyObjectWorks },
                      { TypeWorkCode.MountingFirePreventionSystem, RealtyObjectWorks },
                      { TypeWorkCode.YardLandscaping, RealtyObjectWorks },
                      { TypeWorkCode.RepairPorch, RepairPorch },
                  };

        private static Func<TechnicalCharacteristics, decimal> GetCalculationFunction(TypeWorkCode видРабот)
        {
            if (!CalculationFormula.ContainsKey(видРабот))
            {
                return x => 0;
            }

            return CalculationFormula[видРабот];
        }

        public static string GetInfo(string workCode, Dictionary<string, TypeWorkCrProxy> typeWorkCrProxyDict, TechnicalCharacteristics technicalCharacteristics, out bool error)
        {
            var typeWorkCode = TypeWorkCodeTranslator.GetWorkCode(workCode);
            var result = string.Empty;

            switch (typeWorkCode)
            {
                case TypeWorkCode.RepairHeatingSystem:
                case TypeWorkCode.RepairHotWaterSystem:
                case TypeWorkCode.RepairColdWaterSystem:
                case TypeWorkCode.RepairDrainageSystem:
                case TypeWorkCode.RepairEletricalSystem:
                case TypeWorkCode.RepairGasSystem:
                case TypeWorkCode.RepairBasements:
                case TypeWorkCode.RepairRoof:
                case TypeWorkCode.RepairFacade:
                case TypeWorkCode.WinterizingFacade:
                case TypeWorkCode.StrengtheningFoundation:
                case TypeWorkCode.MountingFirePreventionSystem:
                case TypeWorkCode.YardLandscaping:
                case TypeWorkCode.RepairPorch:
                case TypeWorkCode.RepairOfInjection:
                case TypeWorkCode.MountingDismantlingBalcony:
                    result = CheckVolume(workCode, typeWorkCrProxyDict, technicalCharacteristics, out error);
                    return result;

                case TypeWorkCode.RepairElevator:
                case TypeWorkCode.RepairElevatorShaft:
                    result = CheckNumberOfStoreys(workCode, typeWorkCrProxyDict, technicalCharacteristics, out error);
                    return result;

                case TypeWorkCode.MountingHeatingDevices:
                case TypeWorkCode.MountingHotWaterDevices:
                case TypeWorkCode.MountingColdWaterDevices:
                case TypeWorkCode.MountingElectricalDevices:
                case TypeWorkCode.MountingGasDevices:
                    result = CheckDeviceRepair(workCode, typeWorkCrProxyDict, out error);
                    return result;
            }

            error = false;
            return string.Empty;
        }

        public static string GetInfo(string workCode, Dictionary<string, TypeWorkCrProxy> typeWorkCrProxyDict, TechnicalCharacteristics technicalCharacteristics)
        {
            bool error;
            return GetInfo(workCode, typeWorkCrProxyDict, technicalCharacteristics, out error);
        }

        private static string CheckVolume(string workCode, IDictionary<string, TypeWorkCrProxy> typeWorkCrProxyDict, TechnicalCharacteristics technicalCharacteristics, out bool error)
        {
            decimal criterion = 0;
            var norm = GetCalculationFunction(TypeWorkCodeTranslator.GetWorkCode(workCode))(technicalCharacteristics);
            
            var typeWorkCrProxy = typeWorkCrProxyDict[workCode];

            if (norm != 0)
            {
                criterion = typeWorkCrProxy.Volume.HasValue ? Math.Round(typeWorkCrProxyDict[workCode].Volume.Value * 100 / norm, 2) : 0M;
                error = (criterion > 0 && criterion < 50) || criterion > 100;
            }
            else
            {
                error = false;
                return string.Empty;
            }

            return criterion.ToString();
        }

        private static string CheckNumberOfStoreys(string workCode, IDictionary<string, TypeWorkCrProxy> typeWorkCrProxyDict, TechnicalCharacteristics technicalCharacteristics, out bool error)
        {
            error = technicalCharacteristics.Storeys < 6 && typeWorkCrProxyDict[workCode].Sum > 0;

            return error ? "неверно" : string.Empty;
        }

        private static string CheckDeviceRepair(string workCode, IDictionary<string, TypeWorkCrProxy> typeWorkCrProxyDict, out bool error)
        {
            var secondWorkCode = string.Empty;
            switch (workCode)
            {
                case "7":
                    secondWorkCode = "1";
                    break;
                case "8":
                    secondWorkCode = "2";
                    break;
                case "9":
                    secondWorkCode = "3"; 
                    break;
                case "10":
                    secondWorkCode = "6"; 
                    break;
                case "11":
                    secondWorkCode = "5"; 
                    break;
            }

            if (string.IsNullOrEmpty(secondWorkCode) || !typeWorkCrProxyDict.ContainsKey(secondWorkCode))
            {
                error = false;
                return string.Empty;
            }

            error = typeWorkCrProxyDict[secondWorkCode].Sum > 0 && typeWorkCrProxyDict[workCode].Sum < 0;
            return error ? "без ремонта" : string.Empty;
        }
    }
}