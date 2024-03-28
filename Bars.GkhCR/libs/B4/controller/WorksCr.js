Ext.define('B4.controller.WorksCr', {
    /**
    * Контроллер реестра Актов выполненных работ
    */
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.controller.workscr.Navi',
        'B4.form.SelectField',
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['objectcr.TypeWorkCr'],

    stores: [
        'WorksCr',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'dict.ProgramCrObj',
        'dict.ProgramCrForSelected',
        'dict.WorkSelect',
        'dict.WorkSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'workscr.FilterPanel',
        'workscr.AddWindow',
        'workscr.Panel',
        'workscr.Grid',
        'objectcr.ImportWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'workscr.Panel',
    mainViewSelector: 'workcrpanel',

    refs: [
        {
            ref: 'Grid',
            selector: 'workscrgrid'
        },
        {
            ref: 'FilterPanel',
            selector: 'workscrfilterpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhCr.TypeWorkCr.Create', applyTo: 'b4addbutton', selector: 'workscrgrid' },
                {
                    name: 'GkhCr.TypeWorkCr.Import', applyTo: 'gkhbuttonimport', selector: 'workscrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'GkhCr.WorkCr.Delete', applyTo: 'b4deletecolumn', selector: 'workscrgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhbuttonimportaspect',
            name: 'worksCrImportAspect',
            buttonSelector: 'workscrgrid #btnImport',
            codeImport: 'ObjectCr',
            windowImportView: 'objectcr.ImportWindow',
            windowImportSelector: '#importObjectCrWindow',
            listeners: {
                aftercreatewindow: function (window, importId) {
                    var label = window.down('#containerInfo'),
                        chk1 = window.down('[name=HasLinkOverhaul]'),
                        chk2 = window.down('[name=ReplaceObjectCr]'),
                        chk3 = window.down('[name=ReplaceTypeWork]');

                    label.setVisible(importId == 'FinanceSourceObjectCrImport');
                   
                    if (importId == 'ProgramCrImport') {
                        chk1.show();
                        chk2.show();
                        chk3.show();
                    } else {
                        chk1.hide();
                        chk2.hide();
                        chk3.hide();
                    }
                }
            }
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'objectCrGridWindowAspect',
            gridSelector: 'workscrgrid',
            editFormSelector: '#workscraddwin',
            storeName: 'WorksCr',
            modelName: 'objectcr.TypeWorkCr',
            editWindowView: 'workscr.AddWindow',
            controllerEditName: 'B4.controller.workscr.Navi',
            editRecord: function (record) {
                var me = this,
                    id = record ? record.get('Id') : null,
                    objectId = record ? record.get('ObjectCr') : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('workscredit/{0}/{1}', id, objectId));
                    } else {
                        model.load(id, {
                            success: function(rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            saveRecord: function (rec) {
                var me = this,
                    frm = me.getForm(),
                    model = me.getModel(rec);

                me.mask('Сохранение', frm);

                B4.Ajax.request({
                    url: B4.Url.action('CreateTypeWork', 'TypeWorkCr'),
                    params: {
                        roId: frm.down('b4selectfield[name=RealityObject]').getValue(),
                        programId: frm.down('b4selectfield[name=ProgramCr]').getValue(),
                        workId: frm.down('b4selectfield[name=Work]').getValue()
                    }
                }).next(function (response) {
                    var json = Ext.JSON.decode(response.responseText);
                    me.unmask();
                    me.updateGrid();

                    model.load(json.Id, {
                        success: function (newRec) {
                            me.setFormData(newRec);
                            me.fireEvent('savesuccess', me, newRec);
                        }
                    });

                }).error(function (e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка сохранения!', e.message);
                });
            }
        },
        {
            /**
             * фильтр по муниципальным образованиям
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'municipalitymultiselectwindow',
            fieldSelector: 'workscrfilterpanel [name=Municipality]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workscrmunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', dataIndex: 'Name', flex: 1,
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
                { header: 'Федеральный номер', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getFilterPanel(),
                            field = filterPanel.down('[name=Municipality]');

                        field.setValue(asp.parseRecord(records, asp.valueProperty));
                        field.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одно или несколько муниципальных образования');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
             * фильтр по программам
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'workcrprogramcrmultiselect',
            fieldSelector: 'workscrfilterpanel [name=ProgramCr]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workscrprogramCrSelectWindow',
            storeSelect: 'dict.ProgramCrObj',
            storeSelected: 'dict.ProgramCrForSelected',
            columnsGridSelect: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                {
                    header: 'Период', dataIndex: 'PeriodName', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Period/List'
                    }
                }
            ],
            columnsGridSelected: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Период', dataIndex: 'PeriodName', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getFilterPanel(),
                            field = filterPanel.down('[name=ProgramCr]');

                        field.setValue(asp.parseRecord(records, asp.valueProperty));
                        field.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать одну или несколько програм КР');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            /*
             * фильтр по видам работ
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'workscrworkmultiselect',
            fieldSelector: 'workscrfilterpanel [name=Work]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#workscrworkSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            columnsGridSelect: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var filterPanel = asp.controller.getFilterPanel(),
                            field = filterPanel.down('[name=Work]');

                        field.setValue(asp.parseRecord(records, asp.valueProperty));
                        field.updateDisplayedText(asp.parseRecord(records, asp.textProperty));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать один или несколько видом работ');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'workscrButtonExportAspect',
            gridSelector: 'workscrgrid',
            buttonSelector: 'workscrgrid #btnExport',
            controllerName: 'TypeWorkCr',
            actionName: 'Export'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'workscrStateTransferAspect',
            gridSelector: 'workscrgrid',
            menuSelector: 'workscrGridStateMenu',
            stateType: 'cr_type_work'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView();
        
        if (!view) {
            view = Ext.widget('workcrpanel');
            
            me.bindContext(view);
            me.application.deployView(view);

            me.getAspect('worksCrImportAspect').loadImportStore();
            me.getGrid().getStore().on('beforeload', me.onStoreBeforeLoad, me);
        }
        
        me.getGrid().getStore().load();
    },
    
    onStoreBeforeLoad: function(store, operation) {
        var filterPanel = this.getFilterPanel();
        
        operation.params['mu'] = filterPanel.down('[name=Municipality]').getValue();
        operation.params['program'] = filterPanel.down('[name=ProgramCr]').getValue();
        operation.params['work'] = filterPanel.down('[name=Work]').getValue();
    }
});