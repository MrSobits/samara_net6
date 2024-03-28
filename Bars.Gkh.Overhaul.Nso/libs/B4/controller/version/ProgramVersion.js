Ext.define('B4.controller.version.ProgramVersion', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.view.version.Grid',
        'B4.view.version.Panel',
        'B4.view.version.OrderWindow',
        'B4.view.version.ActualizePeriodWindow',
        'B4.view.version.ActualizeDelWindow',
        'B4.view.version.CopyWindow',
        'B4.form.SelectField',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
   
    models: [
        'version.ProgramVersion',
        'version.VersionRecord'
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'programversiongrid' },
        { ref: 'versionRecordsGrid', selector: 'programversionpanel' },
        { ref: 'versionLogsGrid', selector: 'actualizeloggridgrid' },
        { ref: 'versionField', selector: 'programversionpanel b4selectfield[name="Version"]' },
        { ref: 'copyWin', selector: 'versioncopywindow' }
    ],
    
    views: [
        'version.RecordEditWindow'
    ],

    mainView: 'version.Grid',
    mainViewSelector: 'programversiongrid',

    aspects: [
            {
                xtype: 'gkhpermissionaspect',
                permissions: [
                    {
                        name: "Ovrhl.ProgramVersions.Actualize.View",
                        applyTo: '[action=actualize]',
                        selector: 'versionrecordsgrid',
                        applyBy: function (component, allowed) {
                            if (component) {
                                if (allowed) component.show();
                                else component.hide();
                            }
                            }
                    },
                    {
                        name: "Ovrhl.ProgramVersions.Actualize.Actions.ActualizeNewRecords",
                        applyTo: '[action=ActualizeNewRecords]',
                        selector: 'versionrecordsgrid'
                    },
                    {
                        name: "Ovrhl.ProgramVersions.Actualize.Actions.ActualizeSum",
                        applyTo: '[action=ActualizeSum]',
                        selector: 'versionrecordsgrid'
                    },
                    {
                        name: "Ovrhl.ProgramVersions.Actualize.Actions.ActualizeYear",
                        applyTo: '[action=ActualizeYear]',
                        selector: 'versionrecordsgrid'
                    },
                    {
                        name: "Ovrhl.ProgramVersions.Actualize.Actions.ActualizeDel",
                        applyTo: '[action=ActualizeDeletedEntries]',
                        selector: 'versionrecordsgrid'
                    },
                    {
                        name: "Ovrhl.ProgramVersions.Actualize.Actions.ActualizeOrder",
                        applyTo: '[action=ActualizeOrder]',
                        selector: 'versionrecordsgrid'
                    }
                ]
            },
            {
                xtype: 'grideditwindowaspect',
                name: 'versionRecordGridWindowAspect',
                gridSelector: 'versionrecordsgrid',
                editFormSelector: 'versionrecordeditwin',
                modelName: 'version.VersionRecord',
                editWindowView: 'version.RecordEditWindow',
                onSaveSuccess: function (asp, rec) {
                    asp.controller.mask('Пожалуйста, подождите...', asp.getForm());
                    B4.Ajax.request({
                        url: B4.Url.action('ChangeRecordsIndex', 'ProgramVersion'),
                        timeout: 9999999,
                        params: { recordId: rec.getId() }
                    }).next(function () {
                        asp.controller.unmask();
                        asp.getGrid().getStore().load();
                        B4.QuickMsg.msg('Успешно', 'Данные успешно изменены!', 'success');
                    }).error(function (e) {
                        asp.controller.unmask();
                        Ext.Msg.alert('Ошибка!', (e.message || 'Во время изменения произошла ошибка'));
                    });
                },
                listeners: {
                    beforesave: function (asp, rec) {
                        rec.set('IsChangedYear', true);
                        return true;
                    },
                    aftersetformdata: function (asp, record, form) {
                        var yearField = form.down('[name=Year]');
                        
                        if (yearField && record) {
                            yearField.setMinValue(record.get('PeriodStart'));
                            yearField.setMaxValue(record.get('PeriodEnd'));
                        }
                    }
                }
            }
    ],

    init: function () {
        var me = this;
        
        this.control({
            'programversiongrid b4updatebutton': { 'click': { fn: me.updateGrid, scope: me } },
            'versionrecordsgrid b4updatebutton': { 'click': { fn: me.updateRecords, scope: me } },
            'versionrecordsgrid button[cmd="order"]': { click: { fn: me.showOrder, scope: me } },
            'programversiongrid': { rowaction: { fn: me.rowaction, scope: me } },
            'versionorderwin b4closebutton': { 'click': { fn: me.closeOrderWin, scope: me } },
            'programversionpanel b4selectfield[name="Version"]': {
                'beforeload': { fn: me.onVersionStoreBeforeLoad, scope: me },
                'change'         : { fn: me.onVersionChange, scope: me }
            },
            'programversionpanel checkbox[name="IsMain"]': {
                'change': { fn: me.onIsMainCheckBoxChange, scope: me }
            },
            'versionrecordsgrid [action=actualize] menuitem': {
                click: {
                    fn: me.onActualizeMenuItemClick,
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
                            || view.action == 'ActualizeYear' || view.action == 'ActualizeOrder') {
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
            'versioncopywindow [action=copy]': { 'click': { fn: me.onCopyBtnClick, scope: me } },
            'versioncopywindow b4closebutton': { 'click': { fn: me.closeCopyWin, scope: me } }
        });
        


        this.callParent(arguments);
    },

    index: function() {
        var view = this.getMainPanel() || Ext.widget('programversiongrid');

        this.bindContext(view);
        this.application.deployView(view);

        view.getStore().load();
    },
    
    version: function (id) {
        var me = this,
            store,
            storeLogs,
            view;

        B4.Ajax.request({
            url: B4.Url.action('Get', 'ProgramVersion'),
            params: {
                id: id
            }
        }).next(function (response) {
            var resp = Ext.decode(response.responseText),
                data = resp.data;

            view = me.getVersionRecordsGrid() || Ext.widget('programversionpanel', {
                versionId: id
            });

            view.down('[name=Version]').setValue(data);
            view.down('[name=IsMain]').setValue(data.IsMain);

            me.bindContext(view);
            me.application.deployView(view);

            store = view.down('versionrecordsgrid').getStore();

            store.clearFilter(true);
            store.filter('version', id);
            
            storeLogs = view.down('actualizeloggridgrid').getStore();

            storeLogs.clearFilter(true);
            storeLogs.filter('version', id);
            
        }).error(function () {
            B4.QuickMsg.msg('Ошибка!', 'При получении версии произошла ошибка', 'error');
        });
    },
    
    updateGrid: function(btn) {
        btn.up('programversiongrid').getStore().load();
    },
    
    rowaction: function (grid, action, record) {
        var me = this;
        switch (action.toLowerCase()) {
            case 'edit':
                me.showVersion(record);
                break;
            case 'delete':
                me.deleteRecord(record);
                break;
            case 'copy':
                me.copyRecord(record);
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
    
    deleteRecord: function (record) {
        var me = this,
            panel = this.getMainPanel();

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                var model = this.getModel('version.ProgramVersion');

                var rec = new model({ Id: record.getId() });
                me.mask('Удаление версии...', panel);
                
                rec.destroy()
                    .next(function (sResult) {
                        me.unmask();
                        panel.getStore().load();
                    }, this)
                    .error(function (e) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка удаления!', (e.responseData.message));
                    }, this);
            }
        }, me);
    },
    
    showVersion: function(rec) {
        Ext.History.add('show_version/' + rec.getId());
    },
    
    showOrder: function (btn) {
        var store,
            win;

        win = Ext.widget('versionorderwin');

        store = win.down('grid').getStore();

        store.clearFilter(true);
        store.filter('version', this.getVersionRecordsGrid().versionId);

        win.show();
    },
    
    closeOrderWin: function(btn) {
        btn.up('versionorderwin').close();
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
            view = cbx.up(),
           selectfield = view.down('b4selectfield[name="Version"]'),
           value = selectfield.value;
         
        if (value.IsMain != undefined && value.IsMain != newValue) {
            value.IsMain = newValue;

            me.mask('Cохранение', view);
            B4.Ajax.request({
                url: B4.Url.action('Update', 'ProgramVersion'),
                params: {
                    id : value.Id,
                    records:  Ext.encode([value]) 
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
    
    onLogsGridUpdate: function (btn) {
        btn.up('actualizeloggridgrid').getStore().load();
    },
    
    onActualizeMenuItemClick: function (menuitem) {
        // показываем окно выбора параметров периода актуализации
        var winParams = Ext.widget('versionactualizeperiodwindow', {
            action: menuitem.action,
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy'
        });

        winParams.show();

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

    // Действие срабатывает если на форме выбора параметров Период актуализации С-По нажали Применить
    onActualizeParamsApply: function (btn) {
        var me = this,
            panel = me.getVersionRecordsGrid(),
            win = btn.up('versionactualizeperiodwindow'),
            params = {
                versionId: panel.versionId,
                yearStart: win.down('numberfield[name=YearStart]').getValue(),
                yearEnd: win.down('numberfield[name=YearEnd]').getValue()
            };

        if (me.validationAllowBlankFields(win)) {
            // запускаем проверку возможности выполнения данного действия
            me.validationAction(win.action, params, win);
        }
    },

    // Метод проверки можно ли вообще выоплнять актуализацию
    validationAction: function (action, params, win) {
        var me = this,
            panel = me.getVersionRecordsGrid();

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
                Ext.Msg.confirm('Внимание!', response.data, function (result) {
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
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время выполнения актуализации произошла ошибка'));
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
                var mainPanel = me.getVersionRecordsGrid();

                me.actualize(me, mainPanel, win.action, win.params, win);

            } else {
                win.destroy();
            }
        });
    },

    setCopyFormData: function (rec) {
        var me = this,
            form = this.getCopyWindow();
        if (me.fireEvent('beforesetformdata', me, rec, form) !== false) {
            form.loadRecord(rec);
            form.getForm().updateRecord();
            form.getForm().isValid();
        }

        form.down('[name=NewName]').setValue('Копия ' + rec.get('Name'));
        form.down('[name=NewVersionDate]').setValue(new Date());

        form.show();
    },

    getCopyWindow: function () {
        var me = this,
            copyWindow = me.getCopyWin();
        if (copyWindow && copyWindow.isHidden() && copyWindow.rendered) {
            copyWindow = copyWindow.destroy();
        }
        if (!copyWindow) {
            copyWindow = me.getView('version.CopyWindow').create({ constrain: true, autoDestroy: true });
            if (B4.getBody().getActiveTab()) {
                B4.getBody().getActiveTab().add(copyWindow);
            } else {
                B4.getBody().add(copyWindow);
            }
        }
        return copyWindow;
    },

    closeCopyWin: function (btn) {
        btn.up('versioncopywindow').close();
    },

    onCopyBtnClick: function (btn) {
        var me = this,
            copyWin = btn.up('versioncopywindow');

        me.mask('Сохранение', me.getMainComponent());
        B4.Ajax.request(
        {
            method: 'POST',
            url: B4.Url.action('CopyProgram', 'ProgramVersion'),
            params: {
                versionId: me.versionId,
                Name: copyWin.down('[name=NewName]').getValue(),
                VersionDate: copyWin.down('[name=NewVersionDate]').getValue()
            },
            timeout: 9999999
        }).next(function () {
            me.unmask();
            copyWin.destroy();
            Ext.Msg.alert('Успех!', 'Копирование успешно завершено');
            me.getMainPanel().getStore().load();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Не удалось скопировать версию: ' + e.message);
        });
    }
});