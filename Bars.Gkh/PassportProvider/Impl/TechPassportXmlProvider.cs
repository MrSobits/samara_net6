namespace Bars.Gkh.PassportProvider
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    using B4.IoC;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.BasePassport;
    using Bars.Gkh.Serialization;

    using Castle.Windsor;

    /// <summary>
    /// Провайдер для работы с Техпаспортом жилого дома
    /// </summary>
    public class TechPassportXmlProvider : IPassportProvider
    {
        private readonly SerializeTechPassport structure;

#if DEBUG
        public TechPassportXmlProvider(IResourceManifestContainer resourceContainer)
        {
            using (var stream = resourceContainer.GetByVirtualPath("~/resources/TechPassportDecs.xml").CreateReadStream())
            {
                this.structure = SerializeTechPassport.Serializer.Deserialize(stream).To<SerializeTechPassport>();
            }
            this.Validate();
        }
#else
        public TechPassportXmlProvider()
        {
            using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(Properties.Resources.TechPassportDecs)))
            {
                this.structure = SerializeTechPassport.Serializer.Deserialize(stream).To<SerializeTechPassport>();
            }
        }
#endif

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }

        public IRepository<TehPassportValue> TehPassportValueRepository { get; set; }

        public IDomainService<TehPassport> TehPassportDomain { get; set; }

        /// <summary>
        /// Наименование техпаспорта
        /// </summary>
        public string Name => "Техпаспорт";

        /// <summary>
        /// Тип источника данных
        /// </summary>
        public string TypeDataSource => "xml";

        /// <summary>
        /// Вернуть меню
        /// </summary>
        /// <returns></returns>
        public IList<MenuItem> GetMenu()
        {
            return this.structure.GetMenu();
        }

        /// <summary>
        /// Поля, значения которых не надо удалять при сохранении их пустыми
        /// </summary>
        private readonly string[] emptyFields = { "Form_2#9:3", "Form_2#10:3"};
        
        /// <summary>
        /// Вернуть форму с данными
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <param name="data">Данные</param>
        /// <returns>Форма паспорта</returns>
        public object GetFormWithData(string formId, IDictionary<string, Dictionary<string, string>> data)
        {
            var formStruct = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            if (formStruct == null)
            {
                return new FormTechPassport();
            }

            var form = (FormTechPassport)formStruct.Clone();
            foreach (var comp in form.Components)
            {
                switch (comp.Type)
                {
                    case "Panel":
                    case "PropertyGrid":
                        TechPassportXmlProvider.FillPanelValues(data, comp);

                        break;
                    case "Grid":
                    case "InlineGrid":
                        TechPassportXmlProvider.FillGridValues(data, comp);
                        break;
                }
            }

            return form;
        }

        /// <summary>
        /// Обновить форму
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <param name="formId">Идентификатор формы</param>
        /// <param name="values">Значения паспорта</param>
        /// <param name="fromSync">Маркер источника вызова</param>
        /// <returns>Результат работы</returns>
        public bool UpdateForm(long realityObjectId, string formId, List<SerializePassportValue> values, bool fromSync = false)
        {
            if (realityObjectId == 0)
            {
                throw new ArgumentNullException("realityObjectId");
            }

            if (string.IsNullOrEmpty(formId))
            {
                throw new ArgumentNullException("formId");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            //год постройки
            var yearBuilding = values.Where(x => x.CellCode == "2:1" && x.ComponentCode == "Form_1").Select(x => x.Value).FirstOrDefault().ToInt();

            if (yearBuilding > 0)
            {
                //Год проведения реконструкции
                var yearReconstruction = values.Where(x => x.CellCode == "3:1" && x.ComponentCode == "Form_1").Select(x => x.Value).FirstOrDefault().ToInt();
                //Год проведения капитального ремонта
                var yearCr = values.Where(x => x.CellCode == "4:1" && x.ComponentCode == "Form_1").Select(x => x.Value).FirstOrDefault().ToInt();

                if (yearReconstruction > 0 && yearReconstruction < yearBuilding)
                {
                    throw new Exception("Год постройки не может превышать год проведения реконструкции");
                }

                if (yearCr > 0 && yearCr < yearBuilding)
                {
                    throw new Exception("Год постройки не может превышать год проведения капитального ремонта");
                }
            }
            
            // получаем техпаспорт по объекту недвижимости (!!! в дальнейшем надо будет расширить фильтрацию версией)
            var tehPassport = this.TehPassportDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == realityObjectId);
            if (tehPassport == null)
            {
                tehPassport = new TehPassport { RealityObject = this.RealityObjectDomain.Get(realityObjectId) };
                this.TehPassportDomain.Save(tehPassport);
            }

            // удаляем из данных не редактируемые ячейки
            values.RemoveAll(x => !this.IsEditableCell(x.ComponentCode, x.CellCode));

            foreach (var componentCode in this.GetComponentCodes(formId).Where(this.IsInlineGrid))
            {
                // получим все сохраненые идентификаторы значений техпаспорта с типом InlineGrid с возможность добавлять строки
                var inlineGridIds = this.TehPassportValueDomain.GetAll()
                    .Where(x => x.FormCode == componentCode && x.TehPassport.Id == tehPassport.Id)
                    .Select(x => (object)x.Id)
                    .ToList();

                //когда мы попадаем сюда из метода синхронизации "Общие сведение ЖД <==> Технический паспорт",
                //то получается мы затираем гриды, а новые значения не заполняем (потому что их нет), в результате ошибка.
                //но когда мы попадаем сюда непосредственно с формы редактирования паспорта, то информация по гридам у нас есть,
                //скорее всего поэтому ее сначала затирали, затем сохраняли заново, так как инфа по гридам летит каждый раз с формы паспорта
                if (!fromSync)
                {
                    foreach (var inlineGridId in inlineGridIds)
                    {
                        this.TehPassportValueDomain.Delete(inlineGridId);
                    }
                }
            }

            foreach (var value in values)
            {
                var tehPassportValue = this.TehPassportValueDomain.GetAll()
                    .FirstOrDefault(x => x.TehPassport == tehPassport && x.FormCode == value.ComponentCode && x.CellCode == value.CellCode);

                if (tehPassportValue == null)
                {
                    if (string.IsNullOrEmpty(value.Value))
                    {
                        continue;
                    }

                    tehPassportValue = new TehPassportValue
                    {
                        TehPassport = tehPassport,
                        FormCode = value.ComponentCode,
                        CellCode = value.CellCode,
                        Value = this.CorrectValue(value, fromSync)
                    };
                }
                else
                {
                    var correctValue = this.CorrectValue(value, fromSync);
                    var formCodeCellCode = value.ComponentCode + "#" + value.CellCode;
                    var notInclude = this.emptyFields.Contains(formCodeCellCode);

                    if (string.IsNullOrEmpty(value.Value) && !notInclude)
                    {
                        tehPassportValue.Value = correctValue;

                        // пришла ячейка с пустым значением, удаляем значение из базы
                        this.TehPassportValueDomain.Delete(tehPassportValue.Id);
                        continue;
                    }

                    if (correctValue == tehPassportValue.Value)
                    {
                        // значение не изменилось, пропускаем, чтобы каждый раз не менять ObjectEditDate
                        continue;
                    }
                    tehPassportValue.Value = correctValue;
                }

                this.TehPassportValueDomain.Save(tehPassportValue);
            }

            return true;
        }

        /// <summary>
        /// Обновить формы
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="fromSync">Маркер источника вызова</param>
        /// <returns>Результат работы</returns>
        public bool UpdateForms(Dictionary<long, Dictionary<string, List<SerializePassportValue>>> dict, bool fromSync = false)
        {
            var tpDcit = this.TehPassportDomain.GetAll()
                .WhereContains(x => x.RealityObject.Id, dict.Keys)
                .ToDictionary(x => x.RealityObject.Id);

            var formCodes = dict.Select(x => x.Value).SelectMany(x => x.Keys).ToHashSet();

            var inlineGridIdDict = this.TehPassportValueRepository.GetAll()
                .WhereContains(x => x.TehPassport.RealityObject.Id, dict.Keys)
                .WhereContains(x => x.FormCode, formCodes)
                .GroupBy(x => x.TehPassport.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var tpValuesForSave = new Dictionary<string, TehPassportValue>();
            var tpValuesForDel = new HashSet<long>();

            foreach (var kv in dict)
            {
                var roId = kv.Key;  
                var formIds = kv.Value.Select(x => x.Key).ToList();
                var tehPassport = tpDcit.Get(roId);

                if (tehPassport == null)
                {
                    tehPassport = new TehPassport { RealityObject = new RealityObject {Id = roId}};
                    this.TehPassportDomain.Save(tehPassport);
                }

                foreach (var formId in formIds)
                {
                    foreach (var componentCode in this.GetComponentCodes(formId).Where(this.IsInlineGrid))
                    {
                        var inlineGridIds = inlineGridIdDict.Get(tehPassport.Id)
                            .Where(x => x.FormCode == componentCode)
                            .Select(x => x.Id)
                            .ToList();

                        if (!fromSync)
                        {
                            tpValuesForDel.UnionWith(inlineGridIds);
                        }
                    }

                    var values = kv.Value.Get(formId);

                    foreach (var value in values.Where(x => this.IsEditableCell(x.ComponentCode, x.CellCode)))
                    {
                        var tehPassportValue = inlineGridIdDict.Get(tehPassport.Id)
                            ?.FirstOrDefault(x => x.FormCode == value.ComponentCode && x.CellCode == value.CellCode);

                        if (tehPassportValue == null)
                        {
                            if (string.IsNullOrEmpty(value.Value))
                            {
                                continue;
                            }

                            tehPassportValue = new TehPassportValue
                            {
                                TehPassport = tehPassport,
                                FormCode = value.ComponentCode,
                                CellCode = value.CellCode,
                                Value = this.CorrectValue(value, fromSync)
                            };
                        }
                        else
                        {
                            var correctValue = this.CorrectValue(value, fromSync);
                            if (string.IsNullOrEmpty(value.Value))
                            {

                                tehPassportValue.Value = correctValue;
                                
                                tpValuesForDel.Add(tehPassportValue.Id);
                                continue;
                            }

                            if (correctValue == tehPassportValue.Value)
                            {
                                continue;
                            }
                            tehPassportValue.Value = correctValue;
                        }

                        var key = $"{tehPassportValue.FormCode}##{tehPassportValue.CellCode}";
                        if (!tpValuesForSave.ContainsKey(key))
                        {
                            tpValuesForSave.Add(key, tehPassportValue);
                        }
                    }
                }
            }

            this.Container.InTransaction(() => tpValuesForDel.ForEach(x => this.TehPassportValueRepository.Delete(x)));
            TransactionHelper.InsertInManyTransactions(this.Container, tpValuesForSave.Values, 5000, true, true);

            return true;
        }

        /// <summary>
        /// Вернуть редактор
        /// </summary>
        /// <returns></returns>
        public object GetEditors()
        {
            return this.structure.Editors;
        }

        /// <summary>
        /// Получить редакторы
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <returns></returns>
        public object GetEditors(string formId)
        {
            var editors = new List<EditorTechPassport>();
            var form = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            if (form == null)
            {
                return null;
            }

            foreach (var comp in form.Components)
            {
                switch (comp.Type)
                {
                    case "Panel":
                        editors.AddRange(comp.Elements
                            .Select(el => new
                            {
                                Editor = el.Editor.ToString(),
                                el.EditorCode
                            })
                            .Select(x => this.structure.Editors.FirstOrDefault(y => x.EditorCode != 0 ? y.Code == x.EditorCode : y.Type == x.Editor))
                            .Where(x => x != null && !editors.Contains(x)));
                        break;
                    case "Grid":
                        editors.AddRange(comp.Columns
                            .Select(col => this.structure.Editors.FirstOrDefault(x => x.Code == (int)col.Editor))
                            .Where(x => x != null && !editors.Contains(x)));
                        break;
                    case "PropertyGrid":
                        editors.AddRange(comp.Elements
                            .Select(el => new
                            {
                                Editor = el.Editor.ToString(),
                                el.EditorCode
                            })
                            .Select(x => this.structure.Editors.FirstOrDefault(y => x.EditorCode != 0 ? y.Code == x.EditorCode : y.Type == x.Editor))
                            .Where(x => x != null && !editors.Contains(x)));
                        break;
                    case "InlineGrid":
                        editors.AddRange(comp.Columns.Select(col => this.structure.Editors.FirstOrDefault(x => x.Code == (int)col.Editor)).Where(x => x != null && !editors.Contains(x)));
                        break;

                    default:
                        return this.structure.Editors;
                }
            }

            return editors;
        }
        
        /// <summary>
        /// Вернуть редактор по форме и компоненту
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <param name="componentId">Идентификатор компонента</param>
        /// <param name="code">Код столбца</param>
        /// <returns>Редактор</returns>
        public TypeEditor GetEditorByFormAndComponentAndCode(string formId, string componentId, string code)
        {
            //TODO: safe
            return this.structure.Forms.FirstOrDefault(x => x.Id == formId)
                .Components.FirstOrDefault(x => x.Id == componentId)
                .Columns.FirstOrDefault(x => x.Code == code)
                .Editor;
        }

        /// <summary>
        /// Получить компонент по ИД формы и ИД компонента
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="componentId"></param>
        /// <returns>ComponentTechPassport либо NULL</returns>
        public object GetComponentBy(string formId, string componentId)
        {
            var form = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            return form == null ? null : form.Components.FirstOrDefault(x => x.Id == componentId);
        }

        /// <summary>
        /// Проверка ячейки на редактируемость
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Маркер редактируемости</returns>
        public bool IsEditableCell(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                return false;
            }

            switch (component.Type)
            {
                case "Grid":
                    var codeColumn = cellCode.Split(':')[1];
                    var col = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return col != null && col.Editable;
                case "Panel":
                    return true;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Вернуть наименование элемента
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Наименование</returns>
        public string GetLabelForFormElement(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            FormTechPassport frm;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    frm = form;
                    break;
                }
            }

            if (component == null)
            {
                return string.Empty;
            }

            switch (component.Type)
            {
                case "Grid":
                case "InlineGrid":
                    var codeColumn = cellCode.Split(':')[0];
                    var col =
                        component.Cells.Where(x => x.Code.Contains(codeColumn + ":"))
                            .OrderBy(x => x.Code)
                            .Select(x => x.Value)
                            .ToList();
                    if (col.Any())
                    {
                        var val = string.Join(",", col.ToArray());

                        return $"{component.Title} : {val}";
                    }

                    break;

                case "Panel":
                case "PropertyGrid":
                    var elem = component.Elements.FirstOrDefault(x => x.Code == cellCode);

                    if (elem != null)
                    {
                        return $"{component.Title} : {elem.Label}";
                    }

                    break;
            }

            return string.Empty;
        }

        /// <summary>
        /// Вернуть значения паспорта по дому
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        /// <returns></returns>
        public IList<SerializePassportValue> GetPassportValuesByRo(long realityObjectId)
        {
            if (realityObjectId == 0)
            {
                throw new ArgumentNullException("realityObjectId");
            }

            var values = new List<SerializePassportValue>();

            var serviceTehPassportDomain = this.Container.Resolve<IDomainService<TehPassport>>();
            var serviceTehPassportValueDomain = this.Container.ResolveDomain<TehPassportValue>();
            using (this.Container.Using(serviceTehPassportDomain, serviceTehPassportValueDomain))
            {
                // получаем техпаспорт по объекту недвижимости (!!! в дальнейшем надо будет расширить фильтрацию версией)
                var tehPassport = serviceTehPassportDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == realityObjectId);
                if (tehPassport == null)
                {
                    return values;
                }

                values = serviceTehPassportValueDomain.GetAll()
                    .Where(x => x.TehPassport.Id == tehPassport.Id)
                    .Select(x => new SerializePassportValue
                    {
                        CellCode = x.CellCode,
                        ComponentCode = x.FormCode,
                        Value = x.Value
                    }).ToList();

                var editorsDict = values
                    .Select(x => x.ComponentCode)
                    .Distinct()
                    .ToDictionary(x => x, y => ((List<EditorTechPassport>) this.GetEditors(y)));
                values.ForEach(x =>
                {
                    var editor = editorsDict[x.ComponentCode];
                    if (editor != null && editor.Any())
                    {
                        x.EditorValue =
                            editor
                                .SelectMany(y => y.Values)
                                .FirstOrDefault(y => y.Code == x.Value)
                                .Return(y => y.Name);
                    }
                });
            }

            return values;
        }


        private TypeEditor? GetTypeEditorByComponentAndCellCode(string componentCode, string cellCode, out int editorCode)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                editorCode = 0;
                return null;
            }

            TypeEditor editor = 0;
            editorCode = 0;

            switch (component.Type)
            {
                case "Grid":
                case "InlineGrid":
                    var codeColumn = cellCode.Split(':')[1];
                    var col = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    if (col != null)
                    {
                        editor = col.Editor;
                    }

                    break;

                case "Panel":
                case "PropertyGrid":
                    var elements = component.Elements;
                    var element = elements?.FirstOrDefault(x => x.Code == cellCode);
                    if (element != null)
                    {
                        editor = element.Editor;
                        editorCode = element.EditorCode;
                    }

                    break;
            }

            return editor;
        }

        /// <summary>
        /// Вернуть текст для ячейки
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <param name="value">Значение</param>
        /// <returns>Конвертированное поле</returns>
        public string GetTextForCellValue(string componentCode, string cellCode, string value)
        {
            var typeEditor = this.GetTypeEditorByComponentAndCellCode(componentCode, cellCode, out var editorCode);
            if (!typeEditor.HasValue)
            {
                return string.Empty;
            }

            return this.ValueAsText(typeEditor.Value, editorCode, value);
        }

        public string GetCellValue(string componentCode, string cellCode, string value)
        {
            var typeEditor = this.GetTypeEditorByComponentAndCellCode(componentCode, cellCode, out _);
            
            return typeEditor.HasValue 
                ? TechPassportXmlProvider.GetCellValue(typeEditor.Value, value)
                : null;
        }


        private string ValueAsText(TypeEditor typeEditor, int editorCode, string value)
        {
            var result = string.Empty;

            switch (typeEditor)
            {
                case 0:
                case TypeEditor.Text:
                case TypeEditor.Int:
                case TypeEditor.Decimal:
                case TypeEditor.Double:
                    result = value;
                    break;

                case TypeEditor.Bool:
                    if (value == "0")
                    {
                        result = "Нет";
                    }
                    else if (value == "1")
                    {
                        result = "Да";
                    }
                    break;

                case TypeEditor.Date:
                    {
                        DateTime date;
                        if (DateTime.TryParse(value, out date))
                        {
                            result = date.ToShortDateString();
                        }

                        break;
                    }
                case TypeEditor.Dict:
                {
                    long intVal;
                    var editor = this.structure.Editors.FirstOrDefault(x => x.Code == editorCode);
                    if (editor != null && long.TryParse(value, out intVal))
                    {
                        try
                        {
                            var repository = this.GetRepository(editor.EntityType);
                            if (repository != null)
                            {
                                using (this.Container.Using(repository))
                                {
                                    dynamic objValue = repository?.Get(intVal);
                                    result = objValue?.Name;
                                }
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    break;
                }
                case TypeEditor.MultiDict:
                {
                    var editor = this.structure.Editors.FirstOrDefault(x => x.Code == editorCode);
                    var ids = value.Trim('[', ']').ToLongArray();
                    if (editor != null && ids.Any())
                    {
                        try
                        {
                            var repository = this.GetRepository(editor.EntityType);
                            if (repository != null)
                            {
                                using (this.Container.Using(repository))
                                {
                                    result = this.GetDynamicNames(repository as dynamic, ids, editor.TextProperty);
                                }
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    }

                    break;
                }
                default:
                {
                    var editor = this.structure.Editors.FirstOrDefault(x => x.Type == typeEditor.ToString());

                    var text = editor?.Values.Where(x => x.Code == value).Select(x => x.Name).FirstOrDefault();

                    if (!string.IsNullOrEmpty(text))
                    {
                        result = text;
                    }

                    break;
                }
            }

            return result;
        }

        private static string GetCellValue(TypeEditor typeEditor, string value)
        {
            if (typeEditor != TypeEditor.MultiDict)
            {
                return value;
            }
                
            var ids = value?.TrimStart('[').TrimEnd(']').Split(",");
            return ids?.FirstOrDefault();

        }

        private IRepository GetRepository(string className)
        {
            var entityType = Type.GetType(className);
            if (entityType != null)
            {
                var reposType = typeof(IRepository<>).MakeGenericType(entityType);
                var repository = this.Container.Resolve(reposType);
                return repository as IRepository;
            }

            return null;
        }

        private string GetDynamicNames<T>(IRepository<T> repos, long[] ids, string propertyName)
            where T: IHaveId
        {
            var queryExpression = Expression.Parameter(typeof(T), "x");
            var selectorExpression = Expression.Lambda<Func<T, string>>(Expression.Property(queryExpression, propertyName),
                queryExpression);

            return repos?.GetAll()
                .Where(x => ids.Contains(x.Id))
                .Select(selectorExpression)
                .ToList()
                .AggregateWithSeparator(",");
        }

        /// <summary>
        /// Проверяем, является ли ячейка типа <see cref="bool"/>
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Маркер принадлежности</returns>
        public bool IsBoolean(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                return false;
            }

            var codeColumn = cellCode.Split(':')[1];
            switch (component.Type)
            {
                case "Grid":
                    var gridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return gridcol != null && gridcol.Editor == TypeEditor.Bool;
                case "InlineGrid":
                    var inlineGridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return inlineGridcol != null && inlineGridcol.Editor == TypeEditor.Bool;
                case "Panel":
                    var cell = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cell != null && cell.Editor == TypeEditor.Bool;
                case "PropertyGrid":
                    var cellPg = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cellPg != null && cellPg.Editor == TypeEditor.Bool;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Проверяем, является ли ячейка типа <see cref="DateTime"/>
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <param name="cellCode">Код ячейки</param>
        /// <returns>Маркер принадлежности</returns>
        public bool IsDate(string componentCode, string cellCode)
        {
            ComponentTechPassport component = null;
            foreach (var form in this.structure.Forms)
            {
                component = form.Components.FirstOrDefault(x => x.Id == componentCode);
                if (component != null)
                {
                    break;
                }
            }

            if (component == null)
            {
                return false;
            }

            var codeColumn = cellCode.Split(':')[1];
            switch (component.Type)
            {
                case "Grid":
                    var gridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return gridcol != null && gridcol.Editor == TypeEditor.Date;
                case "InlineGrid":
                    var inlineGridcol = component.Columns.FirstOrDefault(x => x.Code == codeColumn);
                    return inlineGridcol != null && inlineGridcol.Editor == TypeEditor.Date;
                case "Panel":
                    var cell = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cell != null && cell.Editor == TypeEditor.Date;
                case "PropertyGrid":
                    var cellPg = component.Elements.FirstOrDefault(x => x.Code == cellCode);
                    return cellPg != null && cellPg.Editor == TypeEditor.Date;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Проверяем, является ли компонент InlineGrid'ом
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <returns>Маркер принадлежности</returns>
        public bool IsInlineGrid(string componentCode)
        {
            return this.structure.Forms.Select(x => x.Components.FirstOrDefault(y => y.Id == componentCode))
                        .Where(x => x != null)
                        .Select(x => x.Type == "InlineGrid")
                        .FirstOrDefault();
        }

        /// <summary>
        /// Вернуть ячейки компонента
        /// </summary>
        /// <param name="componentCode">Код компонента</param>
        /// <returns>Словарь ячеек</returns>
        public Dictionary<string, string> GetCells(string componentCode)
        {
            var cells = new Dictionary<string, string>();

            var comp = this.structure.Forms.Select(form => form.Components.FirstOrDefault(x => x.Id == componentCode)).FirstOrDefault();

            if (comp == null)
            {
                return cells;
            }

            foreach (var cell in comp.Cells)
            {
                cells.Add(cell.Code, cell.Value);
            }

            return cells;
        }

        /// <summary>
        /// Вернуть коды компонентов формы
        /// </summary>
        /// <param name="formId">Идентификатор формы</param>
        /// <returns>Список компонентов</returns>
        public List<string> GetComponentCodes(string formId)
        {
            var form = this.structure.Forms.FirstOrDefault(x => x.Id == formId);
            return form == null ? new List<string>() : form.Components.Select(x => x.Id).ToList();
        }

        private static void FillGridValues(IDictionary<string, Dictionary<string, string>> data, ComponentTechPassport comp)
        {
            if (!data.ContainsKey(comp.Id)) return;

            foreach (var cell in data[comp.Id].Select(x => new CellTechPassport { Code = x.Key, Value = x.Value }))
            {
                if (comp.Cells.Any(x => x.Equals(cell)))
                {
                    // throw new Exception("Данная ячейка уже добавлена");
                    continue;
                }

                comp.Cells.Add(cell);
            }
        }

        private static void FillPanelValues(IDictionary<string, Dictionary<string, string>> data, ComponentTechPassport comp)
        {
            if (comp.Cells == null)
            {
                comp.Cells = new List<CellTechPassport>();
            }

            foreach (var code in comp.Elements
                .Select(x => x.Code)
                .Where(x => data.ContainsKey(comp.Id))
                .Where(code => data[comp.Id].ContainsKey(code)))
            {
                comp.Cells.Add(new CellTechPassport { Code = code, Value = data[comp.Id][code] });
            }
        }

        /// <summary>
        /// Валидация xml
        /// </summary>
        private void Validate()
        {
            // Проверка на уникальность идентификаторов форм
            var forms = new Dictionary<string, object>();
            foreach (var form in this.structure.Forms)
            {
                if (forms.ContainsKey(form.Id))
                {
                    var msg = $"Не корректный формат описания техпаспорта. Дублирование идентификатора {form.Id}";
                    throw new Exception(msg);
                }

                forms.Add(form.Id, form);
            }

            var components = new Dictionary<string, object>();
            foreach (var form in this.structure.Forms)
            {
                foreach (var comp in form.Components)
                {
                    if (components.ContainsKey(comp.Id))
                    {
                        var msg = $"Не корректный формат описания техпаспорта. Дублирование идентификатора {comp.Id}";
                        throw new Exception(msg);
                    }

                    components.Add(comp.Id, comp);
                }
            }
        }

        /// <summary>
        /// Корректирование приходящих данных
        /// </summary>
        /// <param name="pswValue"></param>
        /// <param name="fromSync"></param>
        /// <returns>Скорректированное значение</returns>
        private string CorrectValue(SerializePassportValue pswValue, bool fromSync = false)
        {
            if (string.IsNullOrEmpty(pswValue.Value))
            {
                return pswValue.Value;
            }
            var value = pswValue.Value;

            if (pswValue.ComponentCode == "Form_1_1" && pswValue.CellCode == "1:1" && fromSync)
            {
                switch (value)
                {
                    case "10":
                    case "20":
                        value = "3";
                        break;
                    case "30":
                        value = "2";
                        break;
                    case "40":
                        value = "4";
                        break;
                }
            }

            if (pswValue.ComponentCode == "Form_1_2_3" && pswValue.CellCode == "7:1" && fromSync)
            {
                value = ((TypeHouse) value.ToInt()).GetDisplayName();
            }

            if (this.IsBoolean(pswValue.ComponentCode, pswValue.CellCode))
            {
                value = value.Trim().ToLower();
                switch (value)
                {
                    case "true":
                        value = "1";
                        break;
                    case "false":
                        value = "0";
                        break;
                    default:
                        value = "0";
                        break;
                }

                return value;
            }

            if (this.IsDate(pswValue.ComponentCode, pswValue.CellCode))
            {
                value = value.Trim();
                var date = value.ToDateTime();
                return date != DateTime.MinValue ? date.ToString("s", DateTimeFormatInfo.InvariantInfo) : value;
            }

            return value;
        }
    }
}
