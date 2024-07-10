Ext.define('B4.controller.version.ProgramVersion', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.StateButton',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.view.version.ActualizePeriodWindow',
        'B4.view.version.ActualizeDelWindow',
        'B4.view.version.ActualizeProgramCrWindow',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    models: [
        'version.ProgramVersion'
    ],
    
    stores: ['dict.MunicipalityByOperator'],
    
    views: [
        'version.Grid',
        'version.Panel',
        'version.OrderWindow',
        'version.CopyWindow'
    ],

    mainView: 'version.Grid',
    mainViewSelector: 'programversiongrid',

    refs: [
        { ref: 'mainPanel', selector: 'programversiongrid' },
        { ref: 'editPanel', selector: 'programversionpanel' },
        { ref: 'versionRecordsGrid', selector: 'versionrecordsgrid' },
        { ref: 'versionLogsGrid', selector: 'actualizeloggridgrid' },
        { ref: 'copyWin', selector: 'versioncopywindow' },
        { ref: 'versionField', selector: 'programversionpanel b4selectfield[name="Version"]' }
    ],
    
    aspects: [
        {
            xtype: 'statebuttonaspect',
            name: 'stateButtonAspect',
            stateButtonSelector: 'programversionpanel #btnState',
            listeners: {
                transfersuccess: function (me, entityId, newState) {
                    me.setStateData(entityId, newState);
                }
            }
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'versionstatepermissionaspect',
            permissions: [
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.View",
                    applyTo: '[action=actualize]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeNewRecords",
                    applyTo: '[itemId=actualizeNewRecordsItem]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeLiftNewRecords",
                    applyTo: '[itemId=actualizeLiftNewRecordsItem]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeSum",
                    applyTo: '[action=ActualizeSum]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeYear",
                    applyTo: '[action=ActualizeYear]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeDel",
                    applyTo: '[action=ActualizeDeletedEntries]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeGroup",
                    applyTo: '[action=ActualizeGroup]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeOrder",
                    applyTo: '[action=ActualizeOrder]',
                    selector: 'versionrecordsgrid'
                },
                {
                    name: "Ovrhl.ProgramVersion.ActualizeProgram.Actions.ActualizeFromShortCr",
                    applyTo: '[action=ActualizeFromShortCr]',
                    selector: 'versionrecordsgrid'
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            gridSelector: 'versionrecordsgrid',
            buttonSelector: 'programversionpanel button[action=Export]',
            controllerName: 'VersionRecord',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'versionStateTransferAspect',
            gridSelector: 'programversiongrid',
            stateType: 'ovrhl_program_version',
            menuSelector: 'programversionstatecontextmenu'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.LongProgram.ProgramVersion.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'programversiongrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],
    
    init: function () {
        var me = this;
        
        me.control({
            'programversiongrid b4updatebutton': { 'click': { fn: me.updateGrid, scope: me } },
            'versionrecordsgrid b4updatebutton': { 'click': { fn: me.updateRecords, scope: me } },
            'programversiongrid': {
                rowaction: { fn: me.rowaction, scope: me },
                render: { fn: me.renderMainPanel, scope: me }
            },
            'versioncopywindow #copyProgramButton': { 'click': { fn: me.onCopyBtnClick, scope: me } },
            'versioncopywindow b4closebutton': { 'click': { fn: me.closeCopyWin, scope: me } },
            'versionorderwin b4closebutton': { 'click': { fn: me.closeOrderWin, scope: me } },
            'programversionpanel b4selectfield[name="Version"]': {
                'beforeload': { fn: me.onVersionStoreBeforeLoad, scope: me },
                'change': { fn: me.onVersionChange, scope: me }
            },
            'programversiongrid b4combobox[name="Municipality"]': {
                change: { fn: me.changeMunicipality, scope: me },
                render: { fn: me.renderMuField, scope: me }
            },
            'programversionpanel checkbox[name="IsMain"]': {
                'change': { fn: me.onIsMainCheckBoxChange, scope: me }
            },
            'programversionpanel checkbox[name="IsProgramPublished"]': {
                'change': { fn: me.onIsProgramPublishedCheckBoxChange, scope: me }
            },
            'versionrecordsgrid [action=actualize] menuitem': {
                click: {
                    fn: me.onActualizeMenuItemClick,
                    scope: me
                }
            },
            'versionactualizedelgrid b4updatebutton': {
                click: {
                    fn: me.onUpdateGrid,
                    scope: me
                }
            },
            'versionactualizedelgrid button[cmd=apply]': {
                click: {
                    fn: me.onActualizeDelApply,
                    scope: me
                }
            },
            'versionactualizeperiodwindow b4savebutton': {
                click: {
                        fn: me.onActualizeParamsApply,
                        scope: me
                }
            },
            'versionactualizeperiodwindow b4closebutton': {
                click: {
                    fn: function (btn) {
                        var win = btn.up('versionactualizeperiodwindow');
                        win.destroy();
                    },
                    scope: me
                }
            },
            'versionactualizeperiodwindow': {
                afterrender: {
                    fn: function (view) {
                        var fldYearEnd = view.down('numberfield[name=YearEnd]');
                        
                        // Для удаления и добавления должен быть только 1 параметр для остальных 2
                        if (view.action == 'ActualizeNewRecords' || view.action == 'ActualizeDeletedEntries'
                            || view.action == 'ActualizeYear' || view.action == 'ActualizeGroup' || view.action == 'ActualizeOrder') {
                            fldYearEnd.allowBlank = true;
                            fldYearEnd.hide();
                        } else {
                            fldYearEnd.allowBlank = false;
                            fldYearEnd.show();
                        }
                    },
                    scope: me
                }
            },
            'actualizeloggridgrid b4updatebutton': {
                click: {
                    fn: me.onLogsGridUpdate,
                    scope: me
                }
            },
            'versionactualizeprogramwindow b4savebutton': {
                click: {
                    fn: me.onActualizeProgramCrApply,
                    scope: me
                }
            },
            'versionactualizeprogramwindow b4closebutton': {
                click: {
                    fn: function (btn) {
                        var win = btn.up('versionactualizeprogramwindow');
                        win.destroy();
                    },
                    scope: me
                }
            },
            'versionactualizeprogramwindow b4selectfield[name=ProgramCr]': {
                'beforeload': { fn: me.onProgramCrBeforeLoad, scope: me }
            }
        });
        
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainPanel() || Ext.widget('programversiongrid');

        this.bindContext(view);
        this.application.deployView(view);
    },
    
    version: function (id) {
        var me = this,
            store,
            storeLogs,
            view;

        B4.Ajax.request(B4.Url.action('Get', 'ProgramVersion', {
            id: id
        })).next(function (response) {
            var resp = Ext.decode(response.responseText),
                data = resp.data,
                model = me.getModel('version.ProgramVersion'),
                params = {
                    versionId: id,
                    municipalityId: data.Municipality.Id
                };
            
            view = me.getEditPanel() || Ext.widget('programversionpanel', params);

            view.down('[name=Version]').setValue(data);
            view.down('[name=IsMain]').setValue(data.IsMain);
            view.down('[name=IsProgramPublished]').setValue(data.IsProgramPublished);

            me.bindContext(view);
            me.application.deployView(view);

            store = view.down('versionrecordsgrid').getStore();

            store.clearFilter(true);
            store.filter([
                { property: "version", value: params.versionId },
                { property: "municipalityId", value: params.municipalityId }
            ]);
            storeLogs = view.down('actualizeloggridgrid').getStore();

            storeLogs.clearFilter(true);
            storeLogs.filter([
                { property: "version", value: params.versionId },
                { property: "municipalityId", value: params.municipalityId }
            ]);
            
            me.getAspect('stateButtonAspect').setStateData(data.Id, data.State);

            me.getAspect('versionstatepermissionaspect').setPermissionsByRecord(new model({ Id: data.Id }));

        }).error(function () {
            B4.QuickMsg.msg('Ошибка!', 'При получении версии произошла ошибка', 'error');
        });
    },
    
    onLogsGridUpdate: function (btn) {
        btn.up('actualizeloggridgrid').getStore().load();
    },
    onLoadMunicipality: function (store, records) {
        var me = this,
            panel = me.getMainPanel(),
            muId = panel.down('b4combobox[name="Municipality"]').getValue(),
            countRecords = store.getCount();

        if (countRecords > 0 && !muId) {
            panel.down('b4combobox[name="Municipality"]').setValue(records[0].data);
        }
    },

    renderMainPanel: function (panel) {
        panel.getStore().on('beforeload', this.onBeforeLoadFirstStage, this);
    },

    renderMuField: function (field) {
        var me = this,
            store = field.getStore();

        store.on('load', me.onLoadMunicipality, me);
        store.load();
    },

    changeMunicipality: function () {
        var me = this,
            panel = me.getMainPanel();
        
        me.mask("Загрузка...", panel);
        panel.getStore().load({
            callback: function () {
                me.unmask();
            },
            failure: function () {
                me.unmask();
                B4.QuickMsg.msg('При получении списка версий произошла ошибка', message, 'error');
            }
        });
    },

    onBeforeLoadFirstStage: function (store, operation) {
        operation.params = operation.params || {};

        operation.params.municipalityId = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
    },
    
    updateGrid: function (btn) {
        btn.up('programversiongrid').getStore().load();
    },
    
    rowaction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'edit':
                this.showVersion(record);
                break;
            case 'delete':
                this.deleteRecord(record);
                break;
            case 'copy':
                this.copyRecord(record);
                break;
        }
    },
    
    copyRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null,
            model;

        me.versionId = id;

        model = me.getModel('B4.model.version.ProgramVersion');

        id ? model.load(id, {
            success: function (rec) {
                me.setCopyFormData(rec);
            },
            scope: me
        }) : me.setFormData(new model({ Id: 0 }));

        me.getCopyWindow().getForm().isValid();
    },

    setCopyFormData: function (rec) {
        var form = this.getCopyWindow();
        if (this.fireEvent('beforesetformdata', this, rec, form) !== false) {
            form.loadRecord(rec);
            form.getForm().updateRecord();
            form.getForm().isValid();
        }

        form.down('#tfName').setValue('Копия ' + rec.get('Name'));
        form.down('#dfVersionDate').setValue(new Date());

        form.show();
    },

    getCopyWindow: function () {
        var copyWindow = this.getCopyWin();
        if (copyWindow && copyWindow.isHidden() && copyWindow.rendered) {
            copyWindow = copyWindow.destroy();
        }
        if (!copyWindow) {
            copyWindow = this.getView('version.CopyWindow').create({ constrain: true, autoDestroy: true });
            if (B4.getBody().getActiveTab()) {
                B4.getBody().getActiveTab().add(copyWindow);
            } else {
                B4.getBody().add(copyWindow);
            }
        }
        return copyWindow;
    },

    closeCopyWin: function () {
        this.getCopyWin().destroy();
    },

    onCopyBtnClick: function () {
        var me = this,
            copyWin = me.getCopyWin();

        me.mask('Сохранение', me.getMainComponent());
        B4.Ajax.request(
        {
            method: 'POST',
            url: B4.Url.action('CopyProgram', 'ProgramVersion'),
            params: {
                versionId: me.versionId,
                Name: copyWin.down('#tfName').getValue(),
                VersionDate: copyWin.down('#dfVersionDate').getValue()
            },
            timeout: 9999999
        }).next(function () {
            me.unmask();
            copyWin.destroy();
            Ext.Msg.alert('Успех', 'Копирование успешно завершено');
            me.getMainPanel().getStore().load();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка', 'Не удалось скопировать версию: ' + e.message);
        });
    },

    deleteRecord: function (record) {
        var me = this,
            panel = me.getMainPanel();

        Ext.Msg.confirm('Удаление записи', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                var model = me.getModel('version.ProgramVersion');

                var rec = new model({ Id: record.getId() });
                me.mask('Удаление версии...', panel);

                rec.destroy()
                    .next(function () {
                        me.unmask();
                        panel.getStore().load();
                    }, me)
                    .error(function (e) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка удаления!', (e.responseData.message));
                    }, me);
            }
        }, me);
    },
    
    showVersion: function (rec) {
        Ext.History.add('show_version/' + rec.getId());
    },
    
    closeOrderWin: function (btn) {
        btn.up('versionorderwin').destroy();
        var form = this.getMainView();
        form.getStore().load();
    },
    
    updateRecords: function(btn) {
        btn.up('versionrecordsgrid').getStore().load();
    },
    
    onVersionStoreBeforeLoad: function (field, opts, store) {
        //var grid = this.getVersionRecordsGrid();
        //if (grid && grid.municipalityId) {
        //    Ext.apply(store.getProxy().extraParams, {
        //        municipalityId: grid.municipalityId
        //    });
        //}
    },
    
    onVersionChange: function (selectfield, newVal, oldVal) {
        var id = newVal && newVal.Id ? newVal.Id : 0,
            view = selectfield.up(),
            store;
        
        if (oldVal) {
            view.versionId = id;
            store = view.down('versionrecordsgrid').getStore();
            store.clearFilter(true);
            store.filter('version', id);
            view.down('checkbox[name="IsMain"]').setValue(newVal.IsMain);
        }
    },
    
    onIsMainCheckBoxChange: function (cbx, newValue, oldValue) {
        var me = this,
            view = cbx.up('programversionpanel'),
            selectfield = view.down('b4selectfield[name="Version"]'),
            value = selectfield.value;
         
        if (value.IsMain != undefined && value.IsMain != newValue) {
            value.IsMain = newValue;

            me.mask('Cохранение', view);
            B4.Ajax.request({
                url: B4.Url.action('Update', 'ProgramVersion'),
                params: {
                    id: value.Id,
                    records: Ext.encode([value])
                }
            }).next(function () {
                me.unmask();
                B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
                return true;
            }).error(function (result) {
                me.unmask();
                var message = "Ошибка при сохранении";
                if (!Ext.isEmpty(result) && !Ext.isEmpty(result.message)) {
                    message = result.message;
                }
                
                B4.QuickMsg.msg('Сохранение записи!', message, 'error');
                
                value.IsMain = oldValue;
                cbx.setValue(oldValue);
                return true;
            });
        }
    },

    onIsProgramPublishedCheckBoxChange: function (cbx, newValue, oldValue) {
        var me = this,
            view = cbx.up('programversionpanel'),
            selectfield = view.down('b4selectfield[name="Version"]'),
            value = selectfield.value;

        if (value.IsProgramPublished != newValue) {
            value.IsProgramPublished = newValue;

            me.mask('Cохранение', view);

            B4.Ajax.request({
                url: B4.Url.action('Update', 'ProgramVersion'),
                params: {
                    id: value.Id,
                    records: Ext.encode([value])
                }
            }).next(function () {
                me.unmask();
                B4.QuickMsg.msg('Сохранение записи!', 'Успешно сохранено', 'success');
                return true;
                }).error(function (result) {
                me.unmask();
                var message = "Ошибка при сохранении";
                if (!Ext.isEmpty(result) && !Ext.isEmpty(result.message)) {
                    message = result.message;
                }

                B4.QuickMsg.msg('Сохранение записи!', message, 'error');

                value.IsProgramPublished = oldValue;
                cbx.setValue(oldValue);
                return true;
            });
        }
    },

    onActualizeMenuItemClick: function (menuItem) {
        var me = this;

        if (menuItem.withOutWindow && menuItem.withOutWindow === true) {
            me.onActualizeActionWithOutWindow(menuItem);
            return;
        }

        if (menuItem.action == 'ActualizeFromShortCr') {
            // показываем окно выбора программы 
            var winProgramCr = Ext.widget('versionactualizeprogramwindow', {
                action: menuItem.action,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            me.setParamsAndExecuteFunc(menuItem, function (menuItem, params) {
                winProgramCr.params = params;
                winProgramCr.show();
            });
        }
        else {
            // показываем окно выбора параметров периода актуализации
            var winParams = Ext.widget('versionactualizeperiodwindow', {
                action: menuItem.action,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            });

            me.setParamsAndExecuteFunc(menuItem, function (menuItem, params) {
                winParams.params = params;
                winParams.show();
            });
        }
    },
    
    // Метод проверки любой формы на валидацию. Условие: форма должна быть extend: 'B4.form.Window'
    validationAllowBlankFields: function (wnd) {
        if (!wnd.getForm().isValid()) {
            //получаем все поля формы
            var fields = wnd.getForm().getFields();

            var invalidFields = '';

            //проверяем, если поле не валидно, то записиваем fieldLabel в строку инвалидных полей
            Ext.each(fields.items, function (field) {
                if (!field.isValid()) {
                    invalidFields += '<br>' + field.fieldLabel;
                }
            });

            //выводим сообщение
            Ext.Msg.alert('Ошибка заполнения полей!', 'Не заполнены обязательные поля: ' + invalidFields);
            return false;
        }

        return true;
    },

    onProgramCrBeforeLoad: function (fld, opts, store) {
        var win = fld.up('versionactualizeprogramwindow');
        
        if (win) {
            Ext.apply(store.getProxy().extraParams, {
                onlyDpkrCreation: true
            });
        }
    },

    // Действие срабатывает, если на форме выбора параметров Период актуализации С-По нажали Применить
    onActualizeProgramCrApply: function (btn) {
        var me = this,
            panel = me.getEditPanel(),
            win = btn.up('versionactualizeprogramwindow'),
            params = win.params;

        params.programCrId = win.down('b4selectfield[name=ProgramCr]').getValue();

        if (me.validationAllowBlankFields(win)) {
            // запускаем выполнение актуализации из КПКР
            me.actualize(me, panel, win.action, params, win);
        }
    },

    // Действие срабатывает если на форме выбора параметров Период актуализации С-По нажали Применить
    onActualizeParamsApply: function (btn) {
        var me = this,
            win = btn.up('versionactualizeperiodwindow'),
            params = win.params;

        params.yearStart = win.down('numberfield[name=YearStart]').getValue();
        params.yearEnd = win.down('numberfield[name=YearEnd]').getValue();

        if (me.validationAllowBlankFields(win)) {
            // запускаем проверку возможности выполнения данного действия
            me.validationAction(win.action, params, win);
        }
    },

    // Актуализация без окна с параметрами
    onActualizeActionWithOutWindow: function (menuItem) {
        var me = this;

        me.setParamsAndExecuteFunc(menuItem, function (menuItem, params) {
            me.validationAction(menuItem.action, params);
        });
    },

    // Установить дополнительные параметры, затем выполнить указанную функцию
    setParamsAndExecuteFunc: function (menuItem, afterSetParamsExecutingFunc) {
        var me = this,
            panel = me.getEditPanel(),
            params = {
                versionId: panel.versionId,
                municipalityId: panel.municipalityId
            },
            additionalParams = menuItem.additionalParams,
            additionalConfigParams = menuItem.additionalConfigParams;

        if (additionalParams && Array.isArray(additionalParams)) {
            additionalParams.forEach(function (additionalParam) {
                // Массив с параметрами в виде [ Ключ, Значение ]
                params[additionalParam[0]] = additionalParam[1];
            });
        }

        if (additionalConfigParams && Array.isArray(additionalConfigParams)) {
            additionalConfigParams.forEach(function (additionalConfigParam) {
                var configName = additionalConfigParam.configName;

                B4.Ajax.request({
                    url: B4.Url.action('ListItems', 'GkhConfig'),
                    params: {
                        parent: configName
                    }
                }).next(function (response) {
                    var responseData = Ext.JSON.decode(response.responseText).data;

                    additionalConfigParam.params.forEach(function (configParam) {
                        // Массив с параметрами в виде
                        // [ Ключ, НаименованиеПараметраКонфига, ДопФункцияДляПреобразованияЗначения ]
                        var additionalParamName = configParam[0],
                            configParamName = configParam[1],
                            configParamValueGettingFunc,
                            configParamValue = responseData.find(x => x.id === Ext.String.format('{0}.{1}', configName, configParamName)).value;

                        // Преобразовать значение параметра конфига, если указана доп.функция
                        if (configParam.length === 3) {
                            configParamValueGettingFunc = configParam[2];

                            if (configParamValueGettingFunc && Ext.isFunction(configParamValueGettingFunc)) {
                                configParamValue = configParamValueGettingFunc(configParamValue);
                            }
                        }

                        params[additionalParamName] = configParamValue;
                    });

                    afterSetParamsExecutingFunc(menuItem, params);
                });
            });
            return;
        }

        afterSetParamsExecutingFunc(menuItem, params);
    },

    // Метод проверки можно ли вообще выоплнять актуализацию
    validationAction: function (action, params, win) {
        var me = this,
            panel = me.getEditPanel();

        me.mask('Пожалуйста, подождите...', panel);

        if (win) {
            win.hide(); // чтобы в овремя выпонления окно было скрыто
        }

        B4.Ajax.request({
            url: B4.Url.action('GetWarningMessage', 'ProgramVersion'),
            params: params
        }).next(function (resp) {
            me.unmask();
            var response = Ext.decode(resp.responseText);
            if (response.data) {
                Ext.Msg.confirm('Внимание', response.data, function (result) {
                    if (result == 'yes') {
                        me.preActualize(me, panel, action, params, win);
                    }
                });
            } else {
                me.preActualize(me, panel, action, params, win);
            }

        }).error(function (e) {
            me.unmask();
            if (win) {
                win.destroy();
            }
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время актуализации произошла ошибка'));
        });
    },
    // Данный метод выполнетс яперед актуализацией потмоу что
    // некотоыре актуализации требуют показа последующих форм на которы хпользователь принимает какоето решение
    // напрмиер метод Удаления показывает форму на которой пользователь подтверждает что такие записи будут удалены
    preActualize: function (me, panel, action, params, win) {
        if (action == 'ActualizeDeletedEntries') {

            if (win) {
                win.destroy();
            }

            me.actualizeDelete(action, params);
            

        } else {
            // остальные методы отправляем на актуаизацию сразу
            me.actualize(me, panel, action, params, win);
        }
    },
    
    // Общий метод для всех актуализаций
    actualize: function (me, panel, action, params, win) {
        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action(action, 'ProgramVersion'),
            params: params,
            timeout: 9999999
        }).next(function () {
            me.unmask();
            if (win) {
                win.destroy();
            }
            panel.down('versionrecordsgrid').getStore().load();
            panel.down('actualizeloggridgrid').getStore().load();
            
            B4.QuickMsg.msg('Успешно', 'Программа успешно изменена!', 'success');
        }).error(function (e) {
            me.unmask();
            if (win) {
                win.destroy();
            }
            if (e.message == 'Нет записей для сохранения') {
                Ext.Msg.alert('Предупреждение', (e.message));
            } else {
                Ext.Msg.alert('Ошибка', (e.message || 'Во время выполнения актуализации произошла ошибка'));
            }
        });
    },
    
    // метод показа формы удаляемых строк 
    actualizeDelete: function (action, params) {
        var me = this,
            win = Ext.widget('versionactualizedelwindow', {
                action: action,
                params: params,
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy'
            }),
            store = win.down('grid').getStore();

        store.clearFilter(true);

        // Показвваем окно после закгрузки стора
        store.on('load', function (st, records) {
            if (records.length > 0) {
                win.show();
            } else {
                Ext.Msg.alert('Внимание!', 'Лишние записи не обнаружены');
            }
        },
        this,
        { single: true });
        
        store.filter([
                { property: "versionId", value: params.versionId },
                { property: "yearStart", value: params.yearStart }
        ]);
    },
    
    // Действие которое срабатывает если на форме удаляемых строк нажмут Применить
    onActualizeDelApply: function (btn) {
        var me = this,
            win = btn.up('versionactualizedelwindow');

        Ext.Msg.confirm('Внимание!', 'Данные записи будут удалены из версии программы, а также из корректировки и опубликованной программы', function (result) {
            if (result == 'yes') {
                var mainPanel = me.getEditPanel();

                me.actualize(me, mainPanel, win.action, win.params, win);
                
            } else {
                win.destroy();
            }
        });
    }
});