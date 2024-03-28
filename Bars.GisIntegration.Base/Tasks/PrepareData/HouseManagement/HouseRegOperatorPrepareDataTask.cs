namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using Bars.B4.Utils;

    /// <summary>
    /// Задача подготовки данных по домам для регионального оператора (используется методы экспорта домов для РСО)
    /// </summary>
    public class HouseRegOperatorPrepareDataTask : HouseRSOPrepareDataTask
    {
        /// <summary>
        /// Переопределить параметры сбора данных
        /// </summary>
        /// <param name="parameters">Параметры сбора</param>
        protected override void OverrideExtractingParametes(DynamicDictionary parameters)
        {
            parameters.Add("regOperatorId", this.Contragent.GkhId);
        }
    }
}
