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
        'dict.JobTreeStore',
        'dict.municipality.MoArea',
        'dict.municipality.Settlement'
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
    isSettlementLevel: false,
    isMunicipalityLevel: false,

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
            otherActions: function (actions) {
                actions['workpricewindow [name=Municipality]'] = {
                    'change': { fn: this.onChangeMu, scope: this },
                    'trigger2Click': { fn: this.onChangeMu, scope: this }
                };
            },
            listeners: {
                aftersetformdata: function (asp) {
                    var muField = asp.getForm().down('[name=Municipality]');

                    if (!asp.controller.isMunicipalityLevel) {
                        muField.allowBlank = true;
                        muField.hide();
                    } else {
                        muField.allowBlank = false;
                        muField.show();
                        muField.validate();
                    }

                    var setlField = asp.getForm().down('[name=Settlement]');
                    if (!asp.controller.isSettlementLevel) {
                        setlField.allowBlank = true;
                        setlField.hide();
                    } else {
                        setlField.allowBlank = false;
                        setlField.show();
                        setlField.validate();
                    }
                },

                beforesave: function (asp, record) {
                    var settl = record.get('Settlement');

                    if (Ext.isEmpty(record.get('NormativeCost')) && Ext.isEmpty(record.get('SquareMeterCost'))) {
                        Ext.Msg.alert('Ошибка!', 'Проверьте поля "Стоимость на единицу объема КЭ, руб." и "Стоимость на единицу площади МКД, руб." - одно из них должно быть заполнено');
                        return false;
                    }

                    if (settl && asp.controller.isSettlementLevel) {
                        record.set('Municipality', settl);
                    }

                    return true;
                }
            },
            onChangeMu: function (fld, newValue) {
                if (!this.controller.isMunicipalityLevel || !this.controller.isSettlementLevel) {
                    return;
                }

                var me = this,
                    win = me.getForm(),
                    slfd = win.down('[name=Settlement]');

                if (newValue && !newValue.HasChildren) {
                    slfd.allowBlank = true;
                }
                else {
                    slfd.allowBlank = false;
                }

                slfd.setValue(null);
                slfd.validate();
                slfd.setDisabled(!newValue);
                slfd.getStore().filter('muId', newValue ? newValue.Id : 0);
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
            actionName: 'ExportHmao'
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
                {
                    text: 'Муниципальный район', dataIndex: 'ParentMo', flex: 2,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    text: 'Муниципальное образование', dataIndex: 'Name', flex: 3,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListSettlementWithoutPaging'
                    }
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальных образований для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            listeners: {
                getdata: function (asp, record) {
                    var field = Ext.ComponentQuery.query(this.fieldSelector)[0];
                    asp.controller.onChangeCopyToMunicpality(field, record.keys);
                }
            }
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
                { header: 'Тип дома', xtype: 'gridcolumn', dataIndex: 'RealEstateType', width: 125, filter: { xtype: 'textfield' } },

                { header: 'Единица измерения', xtype: 'gridcolumn', dataIndex: 'UnitMeasure', width: 110, filter: { xtype: 'textfield' } },
                {
                    header: 'Год', xtype: 'gridcolumn', dataIndex: 'Year', width: 50,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                { header: 'Группа капитальности', xtype: 'gridcolumn', dataIndex: 'CapitalGroup', width: 125, filter: { xtype: 'textfield' } },
                { header: 'Тип дома', xtype: 'gridcolumn', dataIndex: 'RealEstateType', width: 125, filter: { xtype: 'textfield' } }
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
                change: { fn: me.onChangeMuMassWin, scope: me },
                trigger2Click: { fn: me.onChangeMuMassWin, scope: me }
            },
            'massaddwindow b4selectfield[name="Settlement"]': {
                change: { fn: me.onChangeSettMassWin, scope: me },
                trigger2Click: { fn: me.onChangeSettMassWin, scope: me }
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
                change: { fn: me.onChangeCopyFromMunicpality, scope: me },
                gridcreated: { fn: me.hideSettlColumn, scope: me }
            },
            'copyworkpricewindow b4savebutton': {
                click: { fn: me.onSaveCopiedWork, scope: me }
            }
        });
    },

    index: function () {
        var me = this,
            view = me.getMainPanel() || Ext.widget('workpricepanel');

        me.bindContext(view);
        me.application.deployView(view);

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            var json = Ext.JSON.decode(response.responseText),
                muCol = view.down('[dataIndex=ParentMo]'),
                settlCol = view.down('[dataIndex=Municipality]');

            switch (json.WorkPriceMoLevel) {
                case 10:
                    me.isSettlementLevel = true;
                    me.isMunicipalityLevel = false;
                    settlCol.show();
                    break;
                case 20:
                    me.isSettlementLevel = true;
                    me.isMunicipalityLevel = true;
                    settlCol.show();
                    break;
                case 0:
                default:
                    me.isSettlementLevel = false;
                    me.isMunicipalityLevel = true;
                    settlCol.hide();
                    break;
            }
        }).error(function () {
        });

        view.getStore().load();
        this.reloadYears();
    },

    reloadYears: function () {

    },

    toolbarFilter: function (cb, newVal) {
        var store = this.getMainPanel().getStore();
        store.filter(cb.displayField.toLowerCase(), newVal);
    },
    onChangeMuMassWin: function (fld, newValue) {
        var me = this,
            win = fld.up('massaddwindow'),
            b4Combo = win.down('b4combobox[name="Year"]'),
            settfld = win.down('[name=Settlement]');

        if (me.isSettlementLevel) {

            if (me.isMunicipalityLevel && newValue && !newValue.HasChildren) {
                settfld.allowBlank = true;
            }
            else {
                settfld.allowBlank = false;
            }

            settfld.setDisabled(!newValue);
            settfld.setValue(null);
            settfld.validate();
            settfld.getStore().filter('muId', newValue ? newValue.Id : 0);
        } else {
            b4Combo.getStore().load();
        }
    },

    onChangeSettMassWin: function (fld) {
        var win = fld.up('massaddwindow'),
            b4Combo = win.down('b4combobox[name="Year"]');

        b4Combo.getStore().load();
    },

    hideSettlColumn: function (fld, grid) {
        var column = grid.down('[dataIndex=Name]');

        if (this.isSettlementLevel) {
            column.show();
        } else {
            column.hide();
        }
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
            var sfSettlement = me.massAddWindow.down('b4selectfield[name = "Settlement"]');

            if (b4Combo && b4Combo.getStore() != undefined) {
                b4Combo.getStore().on('beforeload', function (store, operation) {
                    operation.params = operation.params || {};

                    if (sfMunicipality) {
                        operation.params.Municipality = me.isSettlementLevel ? sfSettlement.getValue() : sfMunicipality.getValue();
                    }
                });
            }
        }

        var setlField = me.massAddWindow.down('[name=Settlement]');
        if (!me.isSettlementLevel) {
            setlField.hide();
        } else {
            setlField.show();
        }

        var muField = me.massAddWindow.down('[name=Municipality]');
        if (!me.isMunicipalityLevel) {
            muField.hide();
        } else {
            muField.show();
        }

        me.massAddWindow.on('show', me.onMassAddWindowShow, me);
        me.massAddWindow.show();
    },

    onMassAddWindowShow: function () {
    },

    saveMassAddition: function (btn) {
        var me = this,
            form = btn.up('massaddwindow').down('form'),
            values = form.getValues(),
            slfd = form.down('[name=Settlement]');

        form.disable();

        if ((me.isMunicipalityLevel && values.Municipality === undefined)
            || values.InflCoef === undefined || values.InflCoef === ''
            || values.Year === undefined || values.Year === 0
            || values.From === undefined || values.From === ''
            || values.To === undefined || values.To === ''
            || (!slfd.allowBlank && me.isSettlementLevel && !me.isMunicipalityLevel && values.Settlement === undefined)) {
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

        if (me.isSettlementLevel && values.Settlement) {
            values.Municipality = values.Settlement;
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

        workPriceField.setValue('');
        workPriceField.updateDisplayedText(null);
        workPriceField.isValid();
    },

    onChangeCopyToMunicpality: function (field, newVal) {
        var me = this;

        if (newVal && newVal.length > 0) {
            me.copyToMunicipalityIds = newVal.join();
            return;
        }
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
            }).next(function () {
                Ext.Msg.alert('Копирование!', 'Расценки по работам успешно скопированы');
                me.unmask();
                return true;
            }).error(function (e) {
                Ext.Msg.alert('Ошибка', (e.message || e));
                me.unmask();
            });
        }

    },

    formValidation: function (form) {
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