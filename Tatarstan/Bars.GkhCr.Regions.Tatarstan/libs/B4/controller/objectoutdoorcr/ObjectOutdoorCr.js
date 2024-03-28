Ext.define('B4.controller.objectoutdoorcr.ObjectOutdoorCr', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ObjectOutdoorCr'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'objectoutdoorcr.ObjectOutdoorCr',
        'dict.RealityObjectOutdoorProgram',

    ],

    stores: [
        'objectoutdoorcr.ObjectOutdoorCr',
        'dict.RealityObjectOutdoorProgramSelected'
    ],

    views: [
        'objectoutdoorcr.Grid',
        'objectoutdoorcr.Panel',
        'objectoutdoorcr.AddWindow',
        'objectoutdoorcr.DeletedObjectOutdoorCrGrid',
        'objectoutdoorcr.DeletedObjectOutdoorCrEditWindow',
        'SelectWindow.MultiSelectWindow',
    ],

    mainView: 'objectoutdoorcr.Panel',
    mainViewSelector: 'objectoutdoorcrpanel',

    refs: [
        {
            ref: 'ObjectOutdoorCrFilterPanel',
            selector: 'objectoutdoorcrfilterpanel'
        }
    ],

    aspects: [
        {
            xtype: 'objectoutdoorcrperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'objectOutdoorCrGridWindowAspect',
            gridSelector: 'objectoutdoorcrgrid',
            editFormSelector: 'objectoutdoorcraddwindow',
            modelName: 'objectoutdoorcr.ObjectOutdoorCr',
            editWindowView: 'objectoutdoorcr.AddWindow',

            listeners: {
                beforerowaction: function(asp, grid, action, record) {
                    if (action.toLowerCase() === 'doubleclick') {
                        return false;
                    }
                }
            },

            editRecord: function(record) {
                var me = this,
                    model = me.controller.getModel(me.modelName),
                    id = record ? record.get('Id') : null;

                if (id) {
                    me.controller.application.redirectTo(Ext.String.format('objectoutdoorcredit/{0}', id));
                    return;
                }
                me.setFormData(new model({ Id: 0 }));
            },


        },
        {
            /*
             * аспект взаимодействия таблицы удаленных объектов и формы редактирования
             */
            xtype: 'grideditwindowaspect',
            name: 'deletedObjectOutdoorCrGridWindowAspect',
            gridSelector: 'deletedobjectoutdoorcrgrid',
            editFormSelector: 'deletedobjectoutdoorcreditwindow',
            modelName: 'objectoutdoorcr.ObjectOutdoorCr',
            editWindowView: 'objectoutdoorcr.DeletedObjectOutdoorCrEditWindow',
            listeners: {
                aftersetformdata: function (asp, rec, form) {
                    var grid = form.down('typeworkrealityobjectoutdoorhistorygrid'),
                        store = grid.getStore(),
                        buttonGroup = grid.down('buttongroup');
                    store.on('beforeload', this.onBeforeLoad, this, rec.getId());
                    buttonGroup.hide();
                    store.load();
                }
            },

            onBeforeLoad: function (store, operation, recordId) {
                operation.params.objectOutdoorCrId = recordId;
            },
        },
        {
            /*
            * аспект взаимодействия триггер-поля фильтрации программ и таблицы объектов
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'objectOutdoorCrProgramTriggerFieldMultiSelectWindowAspect',
            fieldSelector: 'objectoutdoorcrfilterpanel [name=OutdoorProgram]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#objectOutdoorCrSelectWindow',
            storeSelect: 'dict.RealityObjectOutdoorProgram',
            storeSelected: 'dict.RealityObjectOutdoorProgramSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false },
                { header: 'Период', xtype: 'gridcolumn', dataIndex: 'PeriodName', flex: 1, sortable: false }
            ],

            listeners: {
                getdata: function (asp, records) {
                    return asp.controller.checkTriggerFieldValue(asp, records, true);
                }
            },

            onBeforeLoad: function(store, operation) {
                operation.params.needOnlyWithFullVisibility = true;
            }
        },
        {
            /*
             * аспект взаимодействия триггер-поля фильтрации мун. районов и таблицы объектов
             */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'objectOutdoorCrMunTriggerFieldMultiSelectWindowAspect',
            fieldSelector: 'objectoutdoorcrfilterpanel [name=Municipality]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#municipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'Okato', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    return asp.controller.checkTriggerFieldValue(asp, records, false);
                }
            },
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'objectOutdoorCrStateTransferAspect',
            gridSelector: 'objectoutdoorcrpanel objectoutdoorcrgrid',
            menuSelector: 'objectOutdoorCrGridStateMenu',
            stateType: 'object_outdoor_cr'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'objectOutdoorCrButtonExportAspect',
            gridSelector: 'objectoutdoorcrgrid',
            buttonSelector: 'objectoutdoorcrgrid [action="exporttoexcel"]',
            controllerName: 'ObjectOutdoorCr',
            actionName: 'ExportToExcel'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'objectoutdoorcrpanel [name=objectOutdoorCrTabPanel]': { 'tabchange': { fn: me.changeTab, scope: me } },
            'deletedobjectoutdoorcrgrid [action=recover]': { 'click': { fn: me.recover, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);

        view.down('objectoutdoorcrgrid').getStore().on('beforeload', me.onBeforeLoad, me, false);
        view.down('deletedobjectoutdoorcrgrid').getStore().on('beforeload', me.onBeforeLoad, me, true);
        view.down('objectoutdoorcrgrid').getStore().load();
    },

    changeTab: function (tabPanel, newTab, oldTab) {
        newTab.getStore().load();
    },

    onBeforeLoad: function (store, operation, needDeleted) {
        operation.params.needDeleted = needDeleted;
        var filterPanel = this.getObjectOutdoorCrFilterPanel();
        if (filterPanel) {
            operation.params.municipalityIds = filterPanel.down('[name=Municipality]').getValue();
            operation.params.programIds = filterPanel.down('[name=OutdoorProgram]').getValue();
        }
    },

    checkTriggerFieldValue: function (asp, records, isProgram) {
        if (records.length === 0) {
            Ext.Msg.alert('Ошибка!', isProgram
                ? 'Необходимо выбрать одну или несколько программ благоустройства'
                : 'Необходимо выбрать один или несколько муниципальных районов');
            return false;
        }
    },

    recover: function (btn) {
        var me = this,
            grid = btn.up('deletedobjectoutdoorcrgrid'),
            selectedIds = Ext.Array.map(grid.getSelectionModel().getSelection(), function (el) { return el.get('Id'); });

        if (selectedIds.length === 0) {
            Ext.Msg.alert('Сообщение', 'Выберите объекты для восстановления!');
            return false;
        }

        var message = selectedIds.length === 1
            ? 'Восстановить выбранный объект?'
            : 'Восстановить выбранные объекты?'

        Ext.Msg.confirm('Восстановление объекта', message, function (result) {
            if (result === 'yes') {
                me.mask('Восстановление', B4.getBody().getActiveTab());
                B4.Ajax.request({
                    url: B4.Url.action('Recover', 'ObjectOutdoorCr'),
                    timeout: 9999999,
                    params: {
                        selectedIds: selectedIds
                    }
                }).next(function () {
                    B4.QuickMsg.msg('Восстановление объектов', 'Объекты успешно восстановлены', 'success');
                    grid.getStore().load();
                    me.unmask();
                }).error(function (error) {
                    B4.QuickMsg.msg('Восстановление объектов', error.message || error, 'error');
                    me.unmask();
                });
            }
        }, me);
    }
});