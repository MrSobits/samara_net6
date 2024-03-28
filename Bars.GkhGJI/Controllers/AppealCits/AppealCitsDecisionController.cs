namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class AppealCitsDecisionController : B4.Alt.DataController<AppealCitsDecision>
    {

        public IBlobPropertyService<AppealCitsDecision, AppealCitsDecisionLongText> LongTextService { get; set; }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }


        //public IDomainService<AppealCitsDecision> DomainService { get; set; }

        ///// <summary>
        ///// Вернуть описание
        ///// </summary>
        ///// <param name="baseParams">Базовые параметры запроса</param>
        ///// <returns>Результат запроса</returns>
        //public virtual ActionResult GetDescription(BaseParams baseParams)
        //{
        //    var result = GetPropValue(baseParams);
        //    return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        //}

        ///// <summary>
        ///// Сохранить описание
        ///// </summary>
        ///// <param name="baseParams">Базовые параметры запроса</param>
        ///// <returns>Результат операции</returns>
        //public virtual ActionResult SaveDescription(BaseParams baseParams)
        //{
        //    var result = SavePropValue(baseParams);

        //    return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        //}

        //public IDataResult SavePropValue(BaseParams baseParams)
        //{
        //    var parentId = baseParams.Params.GetAs("id", 0L, true);
        //    if (parentId == 0)
        //    {
        //        return new BaseDataResult(false, "Не передан идентификатор");
        //    }

        //    var field = baseParams.Params.GetAs<string>("field");
        //    if (string.IsNullOrEmpty(field))
        //    {
        //        return new BaseDataResult(false, "Не передано имя поля");
        //    }

        //    var entity = DomainService.Get(parentId);

        //    var property = typeof(AppealCitsDecision).GetProperty(field);
        //    if (property == null || property.PropertyType != typeof(byte[]))
        //    {
        //        return new BaseDataResult(false, "Передано неверное имя поля");
        //    }

        //    var value = baseParams.Params.GetAs("value", string.Empty);
        //    property.SetValue(entity, Encoding.UTF8.GetBytes(value), null);

        //    if (entity.Id > 0)
        //    {
        //        this.DomainService.Update(entity);
        //    }

        //    var result = new Dictionary<string, string> { { "Id", parentId.ToString() }, { field, value } };
        //    var previewLength = baseParams.Params.GetAs("previewLength", 0);
        //    if (previewLength > 0)
        //    {
        //        var preview = value.Length > previewLength ? value.Substring(0, previewLength) : value;
        //        result.Add("preview", preview);

        //        //var autoSavePreview = baseParams.Params.GetAs<bool>("autoSavePreview");
        //        //if (autoSavePreview)
        //        //{
        //        //    this.SavePreview(baseParams, parentId, preview);
        //        //}
        //    }

        //    return new BaseDataResult(result);
        //}

        //private IDataResult GetPropValue(BaseParams baseParams)
        //{
        //    var parentId = baseParams.Params.GetAs("id", 0L, true);
        //    if (parentId == 0)
        //    {
        //        return new BaseDataResult(false, "Не передан идентификатор");
        //    }

        //    var field = baseParams.Params.GetAs<string>("field");
        //    if (string.IsNullOrEmpty(field))
        //    {
        //        return new BaseDataResult(false, "Не передано имя поля");
        //    }

        //    var property = typeof(AppealCitsDecision).GetProperty(field);
        //    if (property == null || property.PropertyType != typeof(byte[]))
        //    {
        //        return new BaseDataResult(false, "Передано неверное имя поля, либо поле имеет неверный тип");
        //    }
        //    var entity = DomainService.Get(parentId);
        //    var value = Encoding.UTF8.GetString(property.GetValue(entity, null) as byte[] ?? new byte[0]);
        //    int previewLength;
        //    if (baseParams.Params.GetAs<bool>("previewOnly") && (previewLength = baseParams.Params.GetAs("previewLength", 0)) > 0)
        //    {
        //        var preview = value.Length > previewLength ? value.Substring(0, previewLength) : value;
        //        return new BaseDataResult(new Dictionary<string, string> { { "Id", parentId.ToString() }, { "preview", preview } });
        //    }

        //    return new BaseDataResult(new Dictionary<string, string> { { "Id", parentId.ToString() }, { field, value } });
        //}
    }
}