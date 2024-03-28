Ext.define('B4.controller.dict.WorkPrice', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.view.dict.workprice.EditWindow',
        'B4.view.dict.workprice.Grid',
        'B4.view.dict.workprice.CopyWorkPriceWindow',
        'B4.store.dict.WorkPrice',
        'B4.model.dict.WorkPrice',

        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.GridEditWindow',
        'B4.form.SelectField',

        'B4.aspects.permission.GkhGridPermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect',

        'B4.view.dict.workprice.MassAdditionWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'dict.WorkPrice',
        'dict.municipality.ListByParamAndOperator',
        'dict.MunicipalityForSelected',
        'dict.JobTreeStore'
    ],

    models: [
        'dict.WorkPrice'
    ],

    views: [
        'dict.workprice.EditWindow',
        'dict.workprice.Grid',
        'SelectWindow.MultiSelectWindow'
    ],
    
    mainView: 'dict.workprice.Grid',
    mainViewSelector: 'workpricepanel',
    aspects: [
        {
            xtype: 'grideditctxwindowaspect',
            name: 'workPriceGridAspect',
            gridSelector: 'workpricepanel',
            editFormSelector: 'workpricewindow',
            storeName: 'dict.WorkPrice',
            modelName: 'dict.WorkPrice',
            editWindowView: 'dict.workprice.EditWindow',
            onSaveSuccess: function () {
                this.controller.reloadYears();
                this.closeWindowHandler(this.getForm());
            },
            listeners: {
                beforesave: function (asp, record) {
                    if (Ext.isEmpty(record.get('NormativeCost')) && Ext.isEmpty(record.get('SquareMeterCost'))) {
                        Ext.Msg.alert('Ошибка!', 'Проверьте поля "Стоимость на единицу объема КЭ, руб." и "Стоимость на единицу площади МКД, руб." - одно из них должно быть заполнено');
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            xtype: 'gkhgridpermissionaspect',
            gridSelector: 'workpricepanel',
            permissionPrefix: 'Gkh.Dictionaries.WorkPrice'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Dictionaries.WorkPrice.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'workpricewindow'
                },
                {
                    name: 'Gkh.Dictionaries.WorkPrice.Field.CapitalGroup',
                    applyTo: '[name=CapitalGroup]',
                    selector: 'workpricewindow',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.Dictionaries.WorkPrice.Column.CapitalGroup',
                    applyTo: '[dataIndex=CapitalGroup]',
                    selector: 'workpricepanel',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'workpricepanel',
            buttonSelector: 'workpricepanel #btnExport',
            controllerName: 'WorkPrice',
            actionName: 'Export'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'copyToMunicipalityMultiSelectWindowAspect',
            fieldSelector: 'copyworkpricewindow #copyToMunicipalitiesTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#copyToMunicipalitySelectWindow',
            storeSelect: 'dict.municipality.ListByParamAndOperator',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальных образований для отбора',
            titleGridSelected: 'Выбранные муниципальные образования'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'workPriceMunicipalityMultiSelectWindowAspect',
            fieldSelector: 'copyworkpricewindow #municipalityWorkPricesTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workPriceMunicipalitySelectWindow',
            storeSelect: 'dict.WorkPriceByMunicipalityForSelect',
            storeSelected: 'dict.WorkPriceByMunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Job', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Единица измерения', xtype: 'gridcolumn', dataIndex: 'UnitMeasure', width: 110, filter: { xtype: 'textfield' } },
                { header: 'Год', xtype: 'gridcolumn', dataIndex: 'Year', width: 50,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                { header: 'Группа капитальности', xtype: 'gridcolumn', dataIndex: 'CapitalGroup', width: 125, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Job', flex: 1, filter: { xtype: 'textfield' } }
            ],
            textProperty: 'Job',
            titleSelectWindow: 'Выбор расценок',
            titleGridSelect: 'Расценки для отбора',
            titleGridSelected: 'Выбранные расценки',
            onBeforeLoad: function (store, operation) {
                operation.params['copyFromMunicipalityId'] = this.controller.copyFromMunicipalityId;
            },
            onSelectedBeforeLoad: function (store, operation) {
                operation.params['copyToMunicipalityId'] = this.controller.copyToMunicipalityId;
            },
            updateSelectedGrid: function () {
                // отменяем обновление грида выбранных элементов
            },
            updateSelectGrid: function () {
                var grid = this.getSelectGrid(),
                    capGroupColumn = this.componentQuery('workpricepanel gridcolumn[dataIndex=CapitalGroup]');

                if (grid) {
                    if (capGroupColumn) {
                        var selectGridCapGroupColumn = grid.down('gridcolumn[dataIndex=CapitalGroup]');

                        if (selectGridCapGroupColumn) {
                            selectGridCapGroupColumn.setVisible(!capGroupColumn.hidden);
                        }
                    }

                    grid.getStore().load();
                }
            }
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'workpricepanel' },
        { ref: 'massAdditionForm', selector: 'massaddwindow form' }
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);
        me.control({
            'workpricepanel b4combobox': {
                change: {
                    fn: me.toolbarFilter,
                    scope: me
                }
            },
            'workpricepanel button[cmd="massAddition"]': {
                click: { fn: me.doMassAddition, scope: me }
            },
            'massaddwindow b4selectfield[name="Municipality"]': {
                change: function (btn) {
                    var win = btn.up('massaddwindow');
                    var b4Combo = win.down('b4combobox[name="Year"]');
                    b4Combo.getStore().load();
                }
            },
            'massaddwindow b4closebutton': {
                click: function (btn) {
                    btn.up('massaddwindow').close();
                    me.getMainPanel().getStore().load();
                    me.reloadYears();
                }
            },
            'massaddwindow b4savebutton': {
                click: me.saveMassAddition
            },
            'workpricepanel button[action="export"]': {
                click: { fn: me.exportGrid, scope: me }
            },
            'workpricepanel button menuitem[action="copyworkprice"]': {
                click: { fn: me.doCopyWorkPrice, scope: me }
            },
            'copyworkpricewindow b4closebutton': {
                click: function (btn) {
                    btn.up('copyworkpricewindow').close();
                    me.getMainPanel().getStore().load();
                }
            },
            'copyworkpricewindow b4selectfield[name="CopyFrom"]': {
                change: { fn: me.onChangeCopyFromMunicpality, scope: me }
            },
            'copyworkpricewindow b4selectfield[name="CopyTo"]': {
                change: { fn: me.onChangeCopyToMunicpality, scope: me }
            },
            'copyworkpricewindow b4savebutton': {
                click: { fn: me.onSaveCopiedWork, scope: me }
            }
        });
    },

    index: function () {
        var view = this.getMainPanel() || Ext.widget('workpricepanel');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
        this.reloadYears();
    },

    reloadYears: function () {
       
    },

    toolbarFilter: function (cb, newVal) {
        var store = this.getMainPanel().getStore();
        store.filter(cb.displayField.toLowerCase(), newVal);
    },

    doMassAddition: function () {
        var me = this;
        if (!me.massAddWindow) {
            me.massAddWindow = Ext.create('B4.view.dict.workprice.MassAdditionWindow', {
                modal: true,
                closeAction: 'hide'
            });

            var b4Combo = me.massAddWindow.down('b4combobox[name="Year"]');
            var sfMunicipality = me.massAddWindow.down('b4selectfield[name = "Municipality"]');

            if (b4Combo && b4Combo.getStore() != undefined) {
                b4Combo.getStore().on('beforeload', function (store, operation) {
                    operation.params = operation.params || {};

                    if (sfMunicipality) {
                        operation.params.Municipality = sfMunicipality.getValue();
                    }
                });
            }
        }
        me.massAddWindow.on('show', me.onMassAddWindowShow, me);
        me.massAddWindow.show();
    },

    onMassAddWindowShow: function () {
    },

    saveMassAddition: function (btn) {
        var form = btn.up('massaddwindow').down('form');
        var values = form.getValues();
        form.disable();

        if (values.Municipality === undefined
            || values.InflCoef === undefined || values.InflCoef === ""
            || values.Year === undefined || values.Year === 0
            || values.From === undefined || values.From === ""
            || values.To === undefined || values.To === "") {
            B4.QuickMsg.msg('Внимание!', 'Заполните все параметры.', 'warning');
            form.enable();
            return;
        }

        if (+values.Year >= +values.From) {
            B4.QuickMsg.msg('Сохранение записи.', 'Начало периода, в который должны быть добавлены расценки, должен быть больше года выбора.', 'warning');
            form.enable();
            return;
        }

        if (+values.From > +values.To) {
            B4.QuickMsg.msg('Внимание!', 'Год начала периода должен быть больше года окончания периода.', 'warning');
            form.enable();
            return;
        }

        B4.Ajax.request({
            url: B4.Url.action('MassAddition', 'WorkPrice'),
            params: values,
            timeout: 5 * 60 * 1000 // 5 минут
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText);
            if (json.success) {
                B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
                form.down('b4combobox').getStore().load();
            } else {
                var message = 'Произошла непредвиденная ошибка';

                if (Ext.isString(json.message) && json.message.length > 0) {
                    message = json.message;
                }

                B4.QuickMsg.msg('Сохранение записи!', message, 'error');
            }
            form.enable();
        }).error(function () {
            B4.QuickMsg.msg('Сохранение записи!', 'Произошла ошибка', 'error');
            form.enable();
        });
    },


    doCopyWorkPrice: function () {
        var me = this;
        if (!me.copyWorkPriceWindow) {
            me.copyWorkPriceWindow = Ext.create('B4.view.dict.workprice.CopyWorkPriceWindow', {
                modal: true,
                closeAction: 'hide'
            });
            
            me.copyWorkPriceWindow.on('show', me.onCopyWorkPriceWindowShow, me);
        }
        
        me.copyWorkPriceWindow.show();
    },

    onCopyWorkPriceWindowShow: function () {
    },

    onChangeCopyFromMunicpality: function (field, newVal) {
        var me = this,
            form = field.up('copyworkpricewindow'),
            workPriceField = form.down('gkhtriggerfield[name="municipalityWorkPrices"]');

        if (newVal) {
            me.copyFromMunicipalityId = newVal.Id;
            workPriceField.enable();
        } else {
            workPriceField.disable();
        }
        
        workPriceField.setValue(null);
        workPriceField.updateDisplayedText(null);
        workPriceField.isValid();
    },

    onChangeCopyToMunicpality: function (field, newVal) {
        var me = this,
            workPriceField = field.up('copyworkpricewindow').down('gkhtriggerfield[name="municipalityWorkPrices"]');

        if (newVal) {
            me.copyToMunicipalityId = newVal.Id;
            workPriceField.enable();
            return;
        }

        workPriceField.disable();
    },

    onSaveCopiedWork: function (btn) {
        var me = this,
            copyWorkWin = btn.up('copyworkpricewindow'),
            recordIds = copyWorkWin.down('gkhtriggerfield[name="municipalityWorkPrices"]').value,
            copyFromId = copyWorkWin.down('b4selectfield[name="CopyFrom"]').getValue(),
            munIds = copyWorkWin.down('gkhtriggerfield[name="CopyToMunicipalities"]').value;

        if (me.formValidation(copyWorkWin)) {
            me.mask('Копирование', me.getMainComponent());
            B4.Ajax.request({
                method: 'POST',
                timeout: 9999999,
                url: B4.Url.action('AddWorkPricesByMunicipality', 'WorkPrice'),
                params: {
                    copyFromId: copyFromId,
                    objectIds: Ext.encode(recordIds ? recordIds.toString().split(',') : ''),
                    copyToMunicipalityIds: Ext.encode(munIds.toString().split(','))
                }
            }).next(function() {
                Ext.Msg.alert('Копирвоание!', 'Расценки по работам успешно скопированы');
                me.unmask();
                return true;
            }).error(function(e) {
                Ext.Msg.alert("Ошибка", (e.message || e));
                me.unmask();
            });
        }

    },
    
    formValidation : function(form)
    {
        if (form.getForm().isValid()) {
            return true;
        } 
        
        var fields = form.getForm().getFields();

        var invalidFields = '';

        Ext.each(fields.items, function (field) {
            if (!field.isValid()) {
                invalidFields += '<br>' + field.fieldLabel;
            }
        });

        Ext.Msg.alert('Ошибка заполнения формы!', 'Не заполнены обязательные поля: ' + invalidFields);
        return false;

    }
});