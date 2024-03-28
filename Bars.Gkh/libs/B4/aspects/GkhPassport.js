Ext.define('B4.aspects.GkhPassport', {
    extend: 'B4.base.Aspect',

    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.enums.TypeEditor',
        'B4.base.Model'
    ],

    alias: 'widget.gkhpassportaspect',

    selectedAllValue: [{ Id: -1, Name: 'Выбраны все' }],
    errorStyle: { background: '#F7CFCE' },
    defaultStyle: { background: '#FFF' },

    init: function () {
        this.callParent(arguments);
        //иницилизируем коллекцию экшенов, что бы не дублировать подписки в дальнейшем проверяем есть ли
        //подписка в коллекции или нет
        this.actionsCollection = new Ext.util.MixedCollection();
    },

    //метод создающий метаструктуру. Точка входа. data - сериализованый json
    createMetastruct: function (data) {
        var asp = this,
            mainComponent = asp.controller.getMainComponent(),
            components = data.form.Components,
            actions = {};
        //инициализируем массив селекторы компонентов. При создании добавляем в массив. 
        //При сохранение сохраняем все формы которые в массиве селекторов
        asp.arraySelectors = [];
        asp.editors = data.editors;

        ////подписываемся на сохранение
        var mainPanelSaveSelector = '#' + asp.controller.mainPanelSelector + ' b4savebutton';
        if (!asp.actionsCollection.containsKey(mainPanelSaveSelector)) {
            actions[mainPanelSaveSelector] = { 'click': { fn: asp.save, scope: asp } };
            asp.actionsCollection.add(mainPanelSaveSelector, mainPanelSaveSelector);
        }
        asp.controller.control(actions);
        //Смотрим какие компоненты пришли. По типу строим их и наполняем
        //Panel - form, Grid - grid(inline), InlineGrid - grid(inline с кнопкой добавить), PropertyGrid - propertygrid
        mainComponent.removeAll();

        Ext.Array.each(components, function (value) {
            var component;
            switch (value.Type) {
                case 'Panel':
                    component = asp.createFormComponent(value);
                    break;
                case 'Grid':
                    component = asp.createGridComponent(value, false);
                    break;
                case 'InlineGrid':
                    component = asp.createGridComponent(value, true);
                    break;
                case 'PropertyGrid':
                    component = asp.createPropertyGridComponent(value);
                    break;
            }
            if (!Ext.isEmpty(component)) {
                mainComponent.add(component);
                mainComponent.doLayout();
            }
        });
    },

    //метод создания грида. withAddButton  - с кнопкой добавить
    createGridComponent: function (component, withAddButton) {
        var me = this;
        //парсим json
        component = this.convertGridComponent(component);

        var gridSelector = 'grid' + component.Id;
        this.arraySelectors.push({ type: 'grid', selector: '#' + gridSelector });

        //формируем конфиг
        var config = {
            title: component.Title,
            scroll: 'horizontal',
            withAddButton: withAddButton,
            itemId: gridSelector,
            store: component.store,
            columns: component.gridColumns,
            sortableColumns: false,
            padding: 5,
            flex: component.Flex,
            height: component.Height,
            anchor: component.Anchor,
            width: component.Width ? component.Width : '100%',
            plugins: [Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                pluginId: 'cellEditing'
            })],
            listeners: {
                render: function (grid) {
                    if (!component.NoSort) {
                        grid.getStore().sort(grid.columns[0].dataIndex, 'ASC');
                    }
                }
            },
            viewConfig: {
                markDirty: false,
                getRowClass: function (record, rowId, rowParams, store) {
                    var row = component.Rows.filter(function(e) {
                            return e.Code == record.data.Id
                        })[0],
                        requirementOn = [],
                        rowNum = 0,
                        cellNum = 0,
                        rec = {},
                        value = '';

                    if (Ext.isEmpty(row) || Ext.isEmpty(row.RequirementOn)) {
                        return '';
                    }
                    requirementOn = row.RequirementOn.split(':');
                    rowNum = requirementOn[0];
                    cellNum = +requirementOn[1];
                    rec = store.getRange().filter(function (s) { return s.data.Id == rowNum; })[0];

                    if (!Ext.isEmpty(rec) && Ext.isEmpty(rec.data[cellNum])) {
                        return 'back-pink';
                    }

                    return '';
                }
            }
        };

        //добавляем кнопки добавить и удалить. Подписываемся на событие добавления
        if (withAddButton) {
            if (!component.Height)
                config.height = 500;
            
            config.columns.push(
                {
                    xtype: 'b4deletecolumn',
                    icon: B4.Url.content('content/img/icons/delete.png'),
                    width: 20,
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var grid = me.componentQuery('#' + gridSelector);
                        var store = grid.getStore();
                        store.remove(rec);
                    }
                });

            config.dockedItems = [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }
                            ]
                        }
                    ]
                }
            ];

            var actions = {};
            var addSelector = '#' + gridSelector + ' b4addbutton';
            if (!this.actionsCollection.containsKey(addSelector)) {
                actions[addSelector] = { 'click': { fn: this.addRow, scope: this, selector: '#' + gridSelector } };
                this.actionsCollection.add(addSelector, addSelector);
            }
            this.controller.control(actions);
        }

        var componentInst = Ext.create(component.type, config);
        return componentInst;
    },

    //парсим json для grid
    convertGridComponent: function (component) {
        var asp = this,
            storeFields = [],
            recFields = [];

        component.type = 'Ext.grid.Panel';
        //формируем метаструктуру полей
        component.gridColumns = [];
        Ext.Array.each(component.Columns, function (value) {
            var field = {},
                minValue = value.MinValue != 0 ? value.MinValue : null,
                maxValue = value.MaxValue != 0 ? value.MaxValue : null;
            if (value.IsId) {
                field.name = 'Id';
                recFields.push({ name: 'Id' });
            }
            else {
                field.name = value.Code;
                recFields.push({ name: value.Code });

                var renderer = asp.createRenderer(value.Editor);
                var requrementOn = asp.getRequirementOn(value.RequirementOn);
                //накапливаем колонки грида
                var col = {
                    xtype: 'gridcolumn',
                    dataIndex: field.name,
                    tooltip: value.Title,
                    text: value.Title,
                    editor: value.Editable ? asp.createField(value.Editor, value.EditorCode, maxValue, minValue) : null,
                    typeEditor: value.Editor,
                    hidden: value.Hidden,
                    isRequirement: value.IsRequirement,
                    dependsOn: value.DependsOn,
                    requirementOn: requrementOn,
                    renderer: function (val, meta, record, row, col) {
                        var res = val,
                            column = this.columns[col],
                            requirement = Ext.each(column.requirementOn,
                                function (req) {
                                    var cellValue = record.get(req.Cell.split(':')[0]);

                                    if (!Ext.isEmpty(cellValue) && req.isEqual(cellValue)) {
                                        return false;
                                    }
                                }) !== true;

                        if (renderer)
                            res = renderer(val);
                        if (res)
                            meta.tdAttr = 'data-qtip="' + res + '"';

                        if (Ext.isEmpty(val) && (column.isRequirement || requirement)) {
                            meta.style = 'background-color: #F7CFCE;';
                        }
                        return res;
                    }
                };

                var widthCol = +value.Width;
                if (widthCol > 0) {
                    col.width = widthCol;

                    col.header = Ext.apply(col.header, {
                        style: {
                            whiteSpace: 'normal !important',
                            lineHeight: '15px'
                        }
                    });

                } else {
                    col.flex = 1;
                }



                component.gridColumns.push(col);
            }

            //накапливаем поля стора
            storeFields.push(field);
        });

        //формируем строки и заполняем их.
        var storeData = [];
        var idCollection = new Ext.util.MixedCollection();
        var functionGetBool = this.getBool;

        var modelName = 'B4.model.' + component.Id;

        Ext.define(modelName, {
            extend: 'B4.base.Model',
            idProperty: 'Id',
            fields: recFields
        });

        Ext.Array.each(component.Cells, function (cell) {
            var rec,
                celRowCollumnPair = cell.Code.split(':'), // 0 - row , 1 - code
                row = celRowCollumnPair[0],
                code = celRowCollumnPair[1],
                columns = this.Columns.filter(function (el) { return el.Code == code; }) || [],
                columnMeta = columns[0],
                specModel;

            if (columnMeta) {
                var editor = columnMeta.Editor;
                //найти editor по коду
                Ext.Array.each(this.Columns, function (column) {
                    if (column.Code == code) {
                        editor = column.Editor;
                        return false;
                    }
                });
                var value = cell.Value;
                if (editor == B4.enums.TypeEditor.Bool) {
                    // меняем приходящий с сервера текст на булево значение
                    value = functionGetBool(cell.Value);
                }

                if (Ext.isEmpty(idCollection.getByKey(row))) {
                    rec = Ext.create(modelName);
                    rec.setId(row);
                    rec.set(code, value);
                    idCollection.add(row, rec);
                }
                else {
                    rec = idCollection.getByKey(row);
                    rec.set(code, value);
                }
                
                if (editor === B4.enums.TypeEditor.RealityObjectStructuralElementLift && !Ext.isEmpty(value)) {
                    specModel = asp.controller.getModel('B4.model.realityobj.StructuralElement');
                    if (specModel) {
                        specModel.load(parseInt(value), {
                            callback: function(record, op) {
                                if (record) {
                                    rec.set(code, record.data);
                                }
                            }
                        });
                    }                    
                }
            }
        }, component);
        idCollection.each(function (rec) { storeData.push(rec); });

        //создаем компоненту стор
        component.store = Ext.create('Ext.data.ArrayStore', {
            autoDestroy: true,
            storeId: 'store' + component.Id,
            fields: storeFields,
            data: storeData
        });

        return component;
    },
    // меняет текст на булево значение
    getBool: function (val) {
        if (Ext.isEmpty(val)) {
            return null;
        }

        if (Ext.isBoolean(val)) {
            return val;
        }

        if (Ext.isNumber(+val)) {
            //возможно имеет смысл проверить на val<2
            return Boolean(+val);
        }

        if (val === '1' || val.toLowerCase() === 'true') {
            return true;
        }
        else if (val === '0' || val.toLowerCase() === 'false') {
            return false;
        }

        return null;
    },
    //метод создания propertygrid
    createPropertyGridComponent: function(component) {
        var asp = this,
            allRequirements = {},
            config = {},
            propertyGridSelector = '';
        component = this.convertPropertyGridComponent(component);
        propertyGridSelector = 'propertyGrid' + component.Id;

        asp.arraySelectors.push({ type: 'propertyGrid', selector: '#' + propertyGridSelector });

        config =
        {
            title: component.Title,
            itemId: propertyGridSelector,
            source: component.source,
            customEditors: component.customEditors,
            isRequirements: component.IsRequirement,
            requirementsOn: component.requirementOn,
            customRenderers: component.customRenderers,
            propertyNames: component.propertyNames,
            nameColumnWidth: '45%',
            padding: 5,
            width: component.Width ? component.Width : '100%',
            height: component.Height,
            flex: component.Flex,
            sortableColumns: false,
            viewConfig: {
                markDirty: false,
                getRowClass: function(record, rowIndex, rowParams, store) {
                    var value = record.get('value'),
                        formPanel = this.up('panel[type=tpform]'),
                        requirementOn = component.requirementOn[record.get('name')],
                        requirement = false,
                        editor = component.customEditors[record.get('name')],
                        validationWithEmptyValue;

                    requirement = Ext.each(requirementOn,
                            function (req) {
                                var propertyGrid = formPanel.down('#propertyGrid' + req.Form),
                                    form = formPanel.down('#form' + req.Form),
                                    cellValue = component.source[req.Cell];
                                if (!Ext.isEmpty(propertyGrid)) {
                                    cellValue = propertyGrid.getSource()[req.Cell];
                                }
                                if (!Ext.isEmpty(form)) {
                                    cellValue = form.down('[name=' + req.Cell + ']').getValue();
                                }
                                if (!Ext.isEmpty(cellValue) && req.isEqual(cellValue)) {
                                    return false;
                                }
                        }) !== true;
                        
                    if(component.IsRequirement[record.get('name')] || requirement) {
                        validationWithEmptyValue = false;
                    }
                    else{
                        validationWithEmptyValue = true;
                    }
                    
                    if(editor.field){
                        editor.field.validationWithEmptyValue = validationWithEmptyValue;
                    }
                    else{
                        editor.validationWithEmptyValue = validationWithEmptyValue;
                    }

                    if (component.typeEditor[record.get('name')] == B4.enums.TypeEditor.Display && component.IsRequirement[record.get('name')]) {
                        return 'back-strawyellow';
                    }

                    if (Ext.isEmpty(value) && (component.IsRequirement[record.get('name')] || requirement)) {
                        return 'back-pink';
                    }

                    return '';
                }
            }
        };
        var componentInst = Ext.create(component.Type, config);

        componentInst.on('beforeedit', function(editor, e, eOpts) {
            var record = e.record || {},
                dependsOn = component.dependsOn[record.internalId],
                allowEdit = true;
            if (component.typeEditor[record.get('name')] === B4.enums.TypeEditor.Display) {
                return false;
            }

            allowEdit = Ext.each(dependsOn, function(code) {
                if (!component.source[code]) {
                    return false;
                }
            });

            if (allowEdit !== true) {
                return false;
            }
        });

        for (var cellCode in component.source) {
            var typeEditor = component.typeEditor[cellCode],
                value = component.source[cellCode];

            if (!Ext.isEmpty(value) && typeEditor === B4.enums.TypeEditor.Dict) {
                var editor = component.customEditors[cellCode];
                editor.getStore()
                    .model.load(parseInt(value), {
                        scope: editor,
                        callback: function(rec, op) {
                            this.setValue(rec.data);
                        }
                    });
            }

            if (!Ext.isEmpty(value) && typeEditor === B4.enums.TypeEditor.MultiDict) {
                var editor = component.customEditors[cellCode];
                editor.cellCode = cellCode;
                editor.getStore()
                    .load({
                        scope: editor,
                        callback: function(rec, op) {
                            var stringVal = component.source[this.cellCode],
                                val = stringVal.length > 0 ? Ext.decode(stringVal) : [],
                                allValue = +val[0],
                                records = [];
                            if (allValue === -1) {
                                this.setValue(asp.selectedAllValue);
                                return;
                            }
                            for (var i = 0; i < val.length; i++) {
                                val[i] = +val[i]
                            };
                            Ext.each(rec, function(r) {
                                if (val.indexOf(r.internalId) != -1) {
                                    records.push(r.data)
                                }
                            });
                            this.setValue(records);
                        }
                    });
            }
        }

        return componentInst;
    },

    convertPropertyGridComponent: function (component) {
        var asp = this,
            functionGetBool = asp.getBool;

        //создание конфига
        component.Type = 'Ext.grid.property.Grid';
        component.source = {};
        component.customEditors = {};
        component.customRenderers = {};
        component.propertyNames = {};
        component.typeEditor = {};
        component.IsRequirement = {};
        component.dependsOn = {};
        component.requirementOn = {};

        //для каждого элемента создаем render, editor, имя
        Ext.Array.each(component.Elements, function (value) {
            var editor = value.MaxValue != 0
                ? asp.getEditorConfig(value.Editor, value.EditorCode, value.ValidationTemplate, value.ValidationErrorMessage, value.MaxValue, value.MinValue, value.MaxLength)
                    : asp.getEditorConfig(value.Editor, value.EditorCode, value.ValidationTemplate, value.ValidationErrorMessage),
                dependsOn = value.DependsOn || '',
                requirementOn = value.RequirementOn || '',
                code = value.Code;

            if (value.Editor == B4.enums.TypeEditor.Dict ||
                value.Editor == B4.enums.TypeEditor.MultiDict) {

                editor.on('change', function(sfl) {
                    var grid = asp.componentQuery('#propertyGrid' + component.Id);
                    grid.setProperty(code, sfl.value || '');
                });
            }

            component.customEditors[code] = editor;
            component.customRenderers[code] = asp.createRenderer(value.Editor, editor, value.EditorCode);
            component.propertyNames[code] = value.Label;
            component.source[code] = '';
            component.typeEditor[code] = value.Editor;
            component.IsRequirement[code] = value.IsRequirement;
            component.dependsOn[code] = dependsOn.length > 0 ? dependsOn.split(',') : [];
            component.requirementOn[code] = asp.getRequirementOn(requirementOn);
        });
        Ext.Array.each(component.Cells, function (cell) {
            if (!Ext.isEmpty(cell.Value)) {
                var cellValue = cell.Value;

                if (component.typeEditor[cell.Code] === B4.enums.TypeEditor.Bool) {
                    // меняем приходящий с сервера текс на булево значение
                    cellValue = functionGetBool(cellValue);
                }
                if(component.typeEditor[cell.Code] === B4.enums.TypeEditor.Date){
                    // проставляем верный формат даты
                    cellValue = Ext.Date.format(new Date(cellValue), component.customEditors[cell.Code].format);
                }
                component.source[cell.Code] = cellValue;
                component.customEditors[cell.Code].setRawValue(cellValue);
            }
        });

        return component;
    },

    //метод создания form
    createFormComponent: function (component) {
        component = this.convertFormComponent(component);

        var formSelector = 'form' + component.Id;
        this.arraySelectors.push({ type: 'form', selector: '#' + formSelector });

        var config =
        {
            xtype: component.Type,
            itemId: formSelector,
            items: component.items,
            bodyPadding: 5,
            closeAction: 'hide',
            trackResetOnLoad: true,
            title: component.Title,
            padding: 5,
            width: component.Width ? component.Width : '100%',
            height: component.Height,
            flex: component.Flex
        };

        if (!Ext.isEmpty(component.Layout)) {
            config.layout = component.Layout;
        }
        if (!Ext.isEmpty(component.Defaults)) {
            config.defaults = component.Defaults;
        }

        //Пробуем получить через selector(если повторный клик), иначе создаем
        var componentInst = Ext.create(component.Type, config);
        return componentInst;
    },

    convertFormComponent: function (component) {
        var type;
        var asp = this;
        var functionGetBool = this.getBool;

        //создание конфига
        type = 'Ext.form.Panel';
        component.Layout = {
            type: 'hbox',
            pack: 'start'
        };
        component.Defaults = {
            anchor: '100%',
            labelWidth: component.LabelWidth || 200,
            labelAlign: 'right'
        };

        //создание контейнеров для hbox
        var containers = [];
        Ext.Array.each(component.Elements, function (value) {
            if (value.ColumnIndex in containers) {

            }
            else {
                containers[value.ColumnIndex] = {
                    xtype: 'container',
                    layout: 'anchor',
                    flex: 1,
                    defaults: component.Defaults,
                    items: []
                };
            }
        });

        //создание items
        component.items = [];
        Ext.Array.each(component.Elements, function (value) {
            var item = asp.createField(value.Editor, value.EditorCode),
                editor = value.Editor;

            item.name = value.Code;
            item.anchor = '100%',
            item.fieldLabel = value.Label;
            item.IsRequirement = value.IsRequirement;
            item.RequirementOn = value.RequirementOn;
            item.listeners = item.listeners || {};
            Ext.applyIf(item.listeners, {
                beforerender: function (f) {
                    var formPanel = this.up('panel[type=tpform]'),
                        nv = f.getValue(),
                        requirement = asp.getRequirementOnValue(formPanel, nv, f.RequirementOn);

                    if ((Ext.isEmpty(nv) || nv === '0') && (f.IsRequirement || requirement)) {
                        f.setFieldStyle(asp.errorStyle);
                    } else {
                        f.setFieldStyle(asp.defaultStyle);
                    }
                },
                change: function (f, nv, ov) {
                    var formPanel = this.up('panel[type=tpform]'),
                        requirement = asp.getRequirementOnValue(formPanel, nv, f.RequirementOn);

                    if ((Ext.isEmpty(nv) || nv === '0') && (f.IsRequirement || requirement)) {
                        f.setFieldStyle(asp.errorStyle);
                    } else {
                        f.setFieldStyle(asp.defaultStyle);
                    }
                }
            });

            Ext.Array.each(component.Cells, function(cell) {
                if (cell.Code == value.Code) {
                    var cellValue = cell.Value;
                    if (editor == B4.enums.TypeEditor.Bool) {
                        // меняем приходящий с сервера текст на булевское значение
                        cellValue = functionGetBool(cellValue);
                    } else if (editor === B4.enums.TypeEditor.Dict ||
                        editor === B4.enums.TypeEditor.MultiDict) {
                        var editorField = item;
                        editorField.getStore()
                            .model.load(cellValue, {
                                callback: function(rec) {
                                    editorField.setValue(rec.data);
                                }
                            });
                    }

                    item.value = cellValue;
                }
            });
            containers[value.ColumnIndex].items.push(item);
        });

        //кладем в панель контейнеры
        Ext.Array.each(containers, function (value) {
            component.items.push(value);
        });

        component.Type = type;

        return component;
    },

    //создание поля по типу. Тип приходит в json-e
    createField: function (editorType, editorCode, maxValue, minValue) {
        var me = this,
            field;

        switch (editorType) {
            case B4.enums.TypeEditor.Text:
                field = {
                    xtype: 'textfield',
                    maxLength: 250
                };
                break;
            case B4.enums.TypeEditor.Date:
                field = {
                    xtype: 'datefield',
                    format: 'd.m.Y'
                };
                break;
            case B4.enums.TypeEditor.Int:
                field = {
                    xtype: 'gkhintfield'
                };
                
                if (!Ext.isEmpty(maxValue)) {
                    field.maxValue = maxValue;
                }

                if (!Ext.isEmpty(minValue)) {
                    field.minValue = minValue;
                }

                break;
            case B4.enums.TypeEditor.Double:
            case B4.enums.TypeEditor.Decimal:
                {
                    field = {
                        xtype: 'gkhdecimalfield'
                    };
                    
                    if (!Ext.isEmpty(maxValue)) {
                        field.maxValue = maxValue;
                    }
                    
                    if (!Ext.isEmpty(minValue)) {
                        field.minValue = minValue;
                    }
                    break;
                }
            case B4.enums.TypeEditor.Bool:
                field = {
                    xtype: 'checkboxfield'
                };
                break;

            case B4.enums.TypeEditor.Dict:
                field = me.createSelectField(editorCode);
                break;
            case B4.enums.TypeEditor.MultiDict:
                field = me.createSelectField(editorCode, 'MULTI');
                break;
            case B4.enums.TypeEditor.Enum:
            {
                var items = [];
                Ext.Array.each(this.editors, function (editor) {
                    if (editor.Code == editorCode) {
                        Ext.Array.each(editor.Values, function (value) {
                            items.push([value.Code, value.Name]);
                        });
                        if (!items.some(function (v) { return v[0] == '0'; })) {
                            items.unshift(['', 'Не задано']);
                        }
                    }
                });

                field = {
                    xtype: 'b4combobox',
                    editable: false,
                    items: items
                };
                break;
            }
            case B4.enums.TypeEditor.TypeLiftShaft:
            case B4.enums.TypeEditor.TypeBasement:
            case B4.enums.TypeEditor.TransferResource:
            case B4.enums.TypeEditor.ChooseEnergy:
            case B4.enums.TypeEditor.TypeHeating:
            case B4.enums.TypeEditor.TypeHotWater:
            case B4.enums.TypeEditor.TypeColdWater:
            case B4.enums.TypeEditor.TypeSewage:
            case B4.enums.TypeEditor.TypePower:
            case B4.enums.TypeEditor.TypeGas:
            case B4.enums.TypeEditor.TypeVentilation:
            case B4.enums.TypeEditor.TypeDrainage:
            case B4.enums.TypeEditor.ConstructionChute:
            case B4.enums.TypeEditor.TypeRoof:
            case B4.enums.TypeEditor.TypeLift:
            case B4.enums.TypeEditor.TypeCommResourse:
            case B4.enums.TypeEditor.ExistMeterDevice:
            case B4.enums.TypeEditor.InterfaceType:
            case B4.enums.TypeEditor.UnutOfMeasure:
            case B4.enums.TypeEditor.FirefightingType:
                {
                    var items = [];
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == editorType) {
                            Ext.Array.each(editor.Values, function (value) {
                                items.push([value.Code, value.Name]);
                            });
                            if (!items.some(function (v) { return v[0] == '0'; }))
                                items.unshift(['0', 'Не задано']);
                        }
                    });

                    field = {
                        xtype: 'b4combobox',
                        editable: false,
                        items: items
                    };
                    break;
                }
            case B4.enums.TypeEditor.RealityObjectStructuralElementLift:
                {
                    try {
                        field = Ext.create('B4.grid.RealObjStructuralElementSelectiFieldEditor', {
                            editable: false,
                            isGetOnlyIdProperty: false,
                            windowCfg: { width: 900 },
                            listeners: {
                                beforeload: function(sf, operation, store) {
                                    operation.params.objectId = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                                    operation.params.showLiftTp = true;
                                }
                            }
                        });
                    } catch (exception) {
                        field = {
                            xtype: 'textfield'
                        };
                    }
                }
                break;
            default:
                field = {
                    xtype: 'textfield'
                };
        }

        return field;
    },

    //создание editor-ов по типу. Тип приходит в json-e
    getEditorConfig: function (editorType, editorCode, validationTemplate, validationErrorMessage, maxValue, minValue, maxLength) {
        var me = this,
            customEditors,
            config = {};
        
        switch (editorType) {
            case B4.enums.TypeEditor.Display:
                customEditors = Ext.create('Ext.form.field.Text', { readOnly: true });
                break;
            case B4.enums.TypeEditor.Text:
                customEditors = Ext.create('Ext.form.field.Text');
                break;
            case B4.enums.TypeEditor.Date:
                customEditors = Ext.create('Ext.form.field.Date', {
                    maskRe: /[0-9\.]/,
                    format: 'd.m.Y'
                });
                break;
            case B4.enums.TypeEditor.Int:
                if (!Ext.isEmpty(maxValue)) {
                    config.maxValue = maxValue;
                }
                
                if (!Ext.isEmpty(minValue)) {
                    config.minValue = minValue;
                }

                if (!Ext.isEmpty(maxLength)) {
                    config.maxLength = maxLength;
                }
                
                customEditors = Ext.create('B4.view.Control.GkhIntField', config);
                break;
            case B4.enums.TypeEditor.Double:
            case B4.enums.TypeEditor.Decimal:
                if (!Ext.isEmpty(maxValue)) {
                    config.maxValue = maxValue;
                }

                if (!Ext.isEmpty(minValue)) {
                    config.minValue = minValue;
                }
                customEditors = Ext.create('B4.view.Control.GkhDecimalField', config);
                break;
            case B4.enums.TypeEditor.Bool:
                customEditors = Ext.create('Ext.form.field.Checkbox');
                break;
            case B4.enums.TypeEditor.Dict:
                customEditors = me.createSelectField(editorCode);
                break;
            case B4.enums.TypeEditor.MultiDict:
                customEditors = me.createSelectField(editorCode, 'MULTI');
                break;
            case B4.enums.TypeEditor.Enum:
            {
                var items = [];
                Ext.Array.each(this.editors, function (editor) {
                    if (editor.Code == editorCode) {
                        Ext.Array.each(editor.Values, function (value) {
                            items.push([value.Code, value.Name]);
                        });
                        if (!items.some(function(v) { return v[0] == '0'; })) {
                            items.unshift(['', 'Не задано']);
                        }
                    }
                });

                customEditors = Ext.create('B4.form.ComboBox', { items: items, editable: false });
                break;
            }
            case B4.enums.TypeEditor.TypeLiftShaft:
            case B4.enums.TypeEditor.TypeBasement:
            case B4.enums.TypeEditor.TransferResource:
            case B4.enums.TypeEditor.ChooseEnergy:
            case B4.enums.TypeEditor.TypeHeating:
            case B4.enums.TypeEditor.TypeHotWater:
            case B4.enums.TypeEditor.TypeColdWater:
            case B4.enums.TypeEditor.TypeSewage:
            case B4.enums.TypeEditor.TypePower:
            case B4.enums.TypeEditor.TypeGas:
            case B4.enums.TypeEditor.TypeVentilation:
            case B4.enums.TypeEditor.TypeDrainage:
            case B4.enums.TypeEditor.ConstructionChute:
            case B4.enums.TypeEditor.TypeRoof:
            case B4.enums.TypeEditor.TypeLift:
            case B4.enums.TypeEditor.TypeCommResourse:
            case B4.enums.TypeEditor.ExistMeterDevice:
            case B4.enums.TypeEditor.InterfaceType:
            case B4.enums.TypeEditor.UnutOfMeasure:
            case B4.enums.TypeEditor.FirefightingType:
            case B4.enums.TypeEditor.YesNoNotSet:
                {
                    var items = [];
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == editorType) {
                            Ext.Array.each(editor.Values, function (value) {
                                items.push([value.Code, value.Name]);
                            });
                            if (!items.some(function(v) { return v[0] == '0'; }))
                                items.unshift(['0', 'Не задано']);
                        }
                    });

                    customEditors = Ext.create('B4.form.ComboBox', { items: items, editable: false });
                    break;
                }
            case B4.enums.TypeEditor.RealityObjectStructuralElementLift:
                {
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code == editorType) {
                            config.isGetOnlyIdProperty = false;
                            config.editable = false;
                            config.windowCfg = {
                                width: 900
                            }
                        }
                    });
                    customEditors = Ext.create('B4.form.RealObjStructuralElementSelectiField', config);
                }
                break;
            default:
                customEditors = Ext.create('Ext.form.field.Text');
        }
        
        if(validationTemplate != null){
            customEditors.validator = function(value) {
                var template = validationTemplate,
                    regExp,
                    errorMessage = validationErrorMessage || '';
                
                // Если поле не обязательное - добавляем возможность оставить поле пустым
                if(this.validationWithEmptyValue) {
                    template += "|^$";
                    errorMessage += '<br>Значение может быть пустым';
                }
                regExp = new RegExp(template);

                if (!regExp.test(value)) {
                    return '<b>Некорректный формат поля!</b><br>' + errorMessage;
                }

                return true;
            }
       }
        
       return customEditors;
    },

    //создание render-ов по типу. Тип приходит в json-e
    createRenderer: function (editorType, editor, editorCode) {
        var render;
        switch (editorType) {
            case B4.enums.TypeEditor.Date:
                render = function(val) {
                    var result = '',
                        day,
                        month,
                        year;
                    if (!Ext.isEmpty(val)) {
                        if (Ext.isString(val) && val.length == 10) {
                            val = new Date(
                                parseInt(val.substring(6, 10)),
                                parseInt(val.substring(3, 5)) - 1,
                                parseInt(val.substring(0, 2)));
                        }
                        val = Ext.isDate(val) ? val : new Date(val);
                        day = ('0' + val.getDate()).slice(-2);
                        month = ('0' + (1 + val.getMonth())).slice(-2);
                        year = val.getFullYear();
                        result = day + '.' + month + '.' + year;
                    }
                    return result;
                };
                break;
            case B4.enums.TypeEditor.Double:
            case B4.enums.TypeEditor.Decimal:
                render = function(val) {
                    if (!Ext.isEmpty(val)) {
                        val = '' + val;
                        if (val.indexOf('.') != -1) {
                            val = val.replace('.', ',');
                        }
                        return val;
                    }
                    return '';
                };
                break;
            case B4.enums.TypeEditor.Bool:
                render = function(val) {
                    if (Ext.isEmpty(val)) {
                        return 'Не задано';
                    }
                    if (Ext.isBoolean(val)) {
                        if (val == true) {
                            return 'Да';
                        } else {
                            return 'Нет';
                        }
                    }
                    if (Ext.isNumber(val)) {
                        var bval = Boolean(+val);

                        switch (bval) {
                            case true:
                                return 'Да';
                            case false:
                                return 'Нет';
                        }
                    }
                    if (val === '1' || val === 'True' || val === 'true') {
                        return 'Да';
                    }
                    if (val === '0' || val === 'False' || val === 'false') {
                        return 'Нет';
                    }
                    return 'Не задано';
                };
                break;
            case B4.enums.TypeEditor.Dict:
                render = function (val) {
                    if (Ext.isEmpty(val))
                        return;
                    return val[editor.textProperty];
                };
                break;
            case B4.enums.TypeEditor.MultiDict:
                render = function (values) {
                    if (Ext.isEmpty(values)) {
                        return;
                    }

                    var names = [];
                    Ext.Array.each(values, function (value) {
                        names.push(value.Name);
                    });

                    return names.join();
                };
                break;
            case B4.enums.TypeEditor.Enum:
                {
                    var values = [];
                    Ext.Array.each(this.editors, function (editor) {
                        if (editor.Code === editorCode) {
                            values = editor.Values;
                        }
                    });

                    render = function (val) {
                        var result = 'Не задано';
                        Ext.Array.each(values, function (editorVal) {
                            if (editorVal.Code === val) {
                                result = editorVal.Name;
                            }
                        });
                        return result;
                    };
                }
                break;
            case B4.enums.TypeEditor.TypeLiftShaft:
            case B4.enums.TypeEditor.TypeBasement:
            case B4.enums.TypeEditor.TransferResource:
            case B4.enums.TypeEditor.ChooseEnergy:
            case B4.enums.TypeEditor.TypeHeating:
            case B4.enums.TypeEditor.TypeHotWater:
            case B4.enums.TypeEditor.TypeColdWater:
            case B4.enums.TypeEditor.TypeSewage:
            case B4.enums.TypeEditor.TypePower:
            case B4.enums.TypeEditor.TypeGas:
            case B4.enums.TypeEditor.TypeVentilation:
            case B4.enums.TypeEditor.TypeDrainage:
            case B4.enums.TypeEditor.ConstructionChute:
            case B4.enums.TypeEditor.TypeRoof:
            case B4.enums.TypeEditor.TypeLift:
            case B4.enums.TypeEditor.TypeCommResourse:
            case B4.enums.TypeEditor.ExistMeterDevice:
            case B4.enums.TypeEditor.InterfaceType:
            case B4.enums.TypeEditor.UnutOfMeasure:
            case B4.enums.TypeEditor.FirefightingType:
            case B4.enums.TypeEditor.YesNoNotSet:
                {
                    var values = [];
                    Ext.Array.each(this.editors, function(editor) {
                        if (editor.Code === editorType) {
                            values = editor.Values;
                        }
                    });

                    render = function(val) {
                        var result = val;
                        Ext.Array.each(values, function(editorVal) {
                            if (editorVal.Code === val) {
                                result = editorVal.Name;
                            }
                        });
                        return result;
                    };
                }
                break;
            case B4.enums.TypeEditor.RealityObjectStructuralElementLift:
                {
                    render = function(val) {
                        if (Ext.isObject(val)) {
                            val = val.Name || val.ElementName;
                        }

                        return val;
                    }
                }
                break;
        }

        return render;
    },

    //метод добавления record-a в грид с кнопкой
    addRow: function (btn, event, eOpts) {
        var grid = this.componentQuery(eOpts.selector),
            store, record;
        if (!Ext.isEmpty(grid)) {
            store = grid.getStore();
            record = Ext.create('B4.base.Model', { fields: store.fields });
            record.setId(this.getNumberRow(store));
            Ext.Array.each(grid.columns, function (column) {
                record.set(column.dataIndex, null);
            });
            store.insert(0, record);
        }
    },

    //метод вычисления id записи
    getNumberRow: function (store) {
        var num = 0;
        store.each(function (item) {
            if (item.getId() > num) {
                num = item.getId();
            }
        });
        return Number(num) + 1;
    },

    //Сохранение данных
    save: function () {
        var records = [],
            asp = this,
            controller = asp.controller,
            view = controller.getMainComponent(),
            invalidFields;

        Ext.Array.each(this.arraySelectors, function (val) {
            var component = asp.controller.getMainComponent().down(val.selector);
            if (val.type === 'grid') {
                asp.gridSave(records, component);
            }
            if (val.type === 'form') {
                asp.formSave(records, component);
            }
            if (val.type === 'propertyGrid') {
                invalidFields = asp.validatePropertyGrid(component);
                if(invalidFields != ''){
                    return false;
                }
                
                asp.propertyGridSave(records, component);
            }
        });
        
        if(invalidFields && invalidFields != ''){
            Ext.Msg.alert('Ошибка валидации!', 'Некорректное заполнение полей:<br>' + invalidFields, asp);
            return;
        }
        
        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

        //аякс строк на сервер
        B4.Ajax.request({
            timeout: 999999,
            url: B4.Url.action('UpdateForm', 'TechPassport'),
            params: {
                values: Ext.encode(records),
                sectionId: controller.getContextValue(view, 'sectionId'),
                realityObjectId: controller.getContextValue(view, 'realityObjectId')
            }
        }).next(function () {
            B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
            Ext.each(view.getRefItems(), function (item) {
                var itemId = item.itemId || '';
                if (itemId.startsWith('propertyGrid') || itemId.startsWith('grid')) {
                    item.getView().refresh();
                }
            });
            asp.controller.unmask();
            return true;
        }).error(function (result) {
            asp.controller.unmask();
            Ext.Msg.alert('Ошибка', result.message);
        });
        
    },

    //сохранение грида
    gridSave: function (records, grid) {
        var formCode = grid.itemId.replace('grid', ''),
            recId = 0;

        Ext.Array.each(grid.getStore().getModifiedRecords(), function(rec) {
            Ext.Array.each(grid.columns, function(column) {
                if (column.dataIndex != 'Id') {
                    var row = grid.withAddButton ? recId : rec.getId(),
                        value = rec.get(column.dataIndex);
                    
                    if (Ext.isObject(value)) {
                        value = value.Id;
                    }

                    records.push({ ComponentCode: formCode, CellCode: row + ':' + column.dataIndex, Value: value });
                }
            });
            recId++;
        });
    },

    //сохранение формы
    formSave: function (records, form) {
        var formCode = form.itemId.replace('form', '');

        var fieldCollection = form.getForm().getFields().items;
        Ext.Array.each(fieldCollection, function(field) {
            if (field.name != 'Id') {
                var value = field.getValue();
                if (Ext.isObject(value) || Ext.isArray(value)) {
                    value = field.getSubmitValue();
                }

                records.push({ ComponentCode: formCode, CellCode: field.name, Value: value });
            }
        });
    },

    //сохранение проперти грида
    propertyGridSave: function (records, propertyGrid) {
        var propertyGridCode = propertyGrid.itemId.replace('propertyGrid', ''),
            source = propertyGrid.getSource(),
            editors = propertyGrid.customEditors;
        
        Ext.iterate(source, function (key, value, myself) {
            //дергаем обратно сохраняемые значения вместо объектов
            if (Ext.isObject(value) || Ext.isArray(value)) {
                if (editors[key].field) {
                    source[key] = editors[key].field.getSubmitValue();
                }
                else {
                    source[key] = editors[key].getSubmitValue();
                }
            }
            records.push({ ComponentCode: propertyGridCode, CellCode: key, Value: Ext.isEmpty(source[key]) ? null : source[key] });
        });
    },

    createSelectField: function (code, selectionMode) {
        var asp = this,
            config = {},
            selectField = {};
        Ext.each(this.editors, function (editor) {
            if (editor.Code == code) {
                config.store = asp.getStore(editor);
                config.listView = editor.View;
                config.textProperty = editor.TextProperty;
                config.columns = [];
                config.isGetOnlyIdProperty = false;
                config.editable = false;
                config.selectionMode = selectionMode || 'SINGLE';

                if (config.selectionMode == 'MULTI') {
                    config.getSubmitValue = function () {
                        var me = this;

                        if (!Ext.isEmpty(me.idProperty)) {
                            if (Ext.isEmpty(me.value))
                                return null;

                            if (Ext.isString(me.value))
                                return me.value;

                            if (Ext.isObject(me.value))
                                return me.value[me.idProperty];

                            if (Ext.isArray(me.value))
                                return Ext.encode(Ext.Array.map(me.value, function (data) {
                                    return data[me.idProperty];
                                }));
                        }

                        return me.callParent(arguments);
                    }
                    config.onSelectAll = function() {
                        var me = this;

                        me.setValue(asp.selectedAllValue);
                        me.selectWindow.hide();
                    };
                }
                config.listeners = {
                    change: function(f, record, id) {
                        if (Ext.isEmpty(record) && f.IsRequirement) {
                            f.setFieldStyle(asp.errorStyle);
                        } else {
                            f.setFieldStyle(asp.defaultStyle);
                        }
                    }
                }

                Ext.Array.each(editor.Columns, function (column) {
                    config.columns.push({ text: column.Text, dataIndex: column.DataIndex, flex: 1 });
                });
                return false;
            }
        });
        return Ext.create('B4.form.SelectField', config);
    },

    getRequirementOn: function (requirementOn) {
        var result = Ext.isString(requirementOn) && requirementOn.length > 0
            ? Ext.Array.map(requirementOn.split(','),
                function(arr) {
                    var formCellValue = arr.length > 0 ? arr.match(/(?:(\S+)\|)?(\d+\:\d+)(?:\=(\S+))?(?:\!\=(\S+))?/) : []; //Form_1|20:1=Да или 20:1=Да или 20:1
                    return { Form: formCellValue[1], Cell: formCellValue[2], Value: formCellValue[3], NotValue: formCellValue[4], isEqual:
                        function (v) {
                            if (Ext.isDefined(this.Value))
                                return v == this.Value;
                            if (Ext.isDefined(this.NotValue))
                                return v != this.NotValue;
                            return true;
                        }
                    };
                })
            : [];

        return result;
    },

    getRequirementOnValue: function (formPanel, newValue, requirementOn) {
        return Ext.each(requirementOn, function(req) {
            var propertyGrid = formPanel.down('#propertyGrid' + req.Form),
                form = formPanel.down('#form' + req.Form),
                propertyGrid = formPanel.down('#grid' + req.Form),
                formValue = newValue;
            if (!Ext.isEmpty(propertyGrid)) {
                formValue = propertyGrid.getSource()[req.Cell];
            }
            if (!Ext.isEmpty(form)) {
                formValue = form.down('[name=' + req.Cell + ']').getValue();
            }
            if (!Ext.isEmpty(formValue) && req.isEqual(formValue)) {
                return false;
            }
        }) !== true;
    },

    getStore: function (editor) {
        if (!Ext.isEmpty(editor.ControllerName)) {
            return Ext.create('B4.store.dict.BaseDict', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: editor.ControllerName
                }
            });
        }

        return Ext.getStore(editor.Store);
    },
    
    validatePropertyGrid: function(propertyGrid){
        var me = this,
            editors = propertyGrid.customEditors,
            names = propertyGrid.propertyNames,
            isRequirements = propertyGrid.isRequirements,
            requirementsOn = propertyGrid.requirementsOn,
            source = propertyGrid.source,
            invalidFields = '',
            gridView = propertyGrid;

        Ext.iterate(editors, function (key, editor, myself) {
            var isValid = editor.field != undefined 
                        ? editor.field.isValid()
                        : editor.isValid(),
                isRequired = isRequirements[key] || false,
                reqiredOn = !Ext.isEmpty(requirementsOn[key]) 
                    ? me.getRequirementOnValue(gridView, null, requirementsOn[key])
                    : false;
            
            if((isRequired || reqiredOn || !Ext.isEmpty(source[key])) && !isValid) {
                invalidFields += '<b>' + names[key] + '</b><br>';
            }
        });
                    
        return invalidFields;
    }                
    
});