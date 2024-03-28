Ext.define('B4.controller.TransferRf', {
    /*
    * Контроллер раздела перечисления средств фонда
    */

    transferRFId: null,
    transferRFRecordId: null,
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.StateButton',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.TransferRf',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],

    models: [
        'TransferRf',
        'transferrf.Record',
        'transferrf.RecObj'
    ],

    stores: [
        'TransferRf',
        'transferrf.Record',
        'transferrf.RecObj'
    ],

    views: [
        'transferrf.EditWindow',
        'transferrf.RecordEditWindow',
        'transferrf.Grid',
        'transferrf.RecObjGrid'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRf.TransferRf.Create', applyTo: 'b4addbutton', selector: 'transferRfGrid' },
                { name: 'GkhRf.TransferRf.Edit', applyTo: 'b4savebutton', selector: '#transferRfEditWindow' },
                {
                    name: 'GkhRf.TransferRf.Delete', applyTo: 'b4deletecolumn', selector: 'transferRfGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferRf.WithArchiveRecs', applyTo: 'checkbox[cmd=withArchiveRecs]', selector: 'transferRfGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'transferrfperm',
            editFormAspectName: 'transferRfRecordGridEditWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            name: 'transferRfPerm'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'transferRfRecordStateButtonAspect',
            stateButtonSelector: '#transferRfRecordEditWindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('transferRfRecordGridEditWindowAspect');
                    editWindowAspect.updateGrid();
                    var model = this.controller.getModel('transferrf.Record');
                    entityId ? model.load(entityId, {
                        success: function (rec) {
                            editWindowAspect.setFormData(rec);
                            this.controller.getAspect('transferRfPerm').setPermissionsByRecord(rec);
                        },
                        scope: this
                    }) : this.controller.getAspect('transferRfPerm').setPermissionsByRecord(new model({ Id: 0 }));
                }
            },
            updateGrid: function () {
                this.controller.getStore(this.storeName).load();
            }
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'transferRfRecordStateTransferAspect',
            gridSelector: '#transferRfRecordGrid',
            stateType: 'rf_transfer_record',
            menuSelector: 'transferRfRecordGridStateMenu'
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела перечисления средств фонда
            */
            xtype: 'grideditwindowaspect',
            name: 'transferRfGridEditWindowAspect',
            gridSelector: 'transferRfGrid',
            editFormSelector: '#transferRfEditWindow',
            storeName: 'TransferRf',
            modelName: 'TransferRf',
            editWindowView: 'transferrf.EditWindow',
            otherActions: function (actions) {
                actions['#transferRfEditWindow'] = { 'beforeshow': { fn: this.onBeforeShowEditWindow, scope: this } };
                actions[this.gridSelector + ' checkbox[cmd=withArchiveRecs]'] = { 'change': { fn: this.onShowWithArchiveRecs, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record, 'Rec');
            },
            onShowWithArchiveRecs: function (fld, newValue) {
                var transferRfGrid = this.getGrid();
                if (transferRfGrid) {
                    var store = transferRfGrid.getStore();
                    store.filter('withArchiveRecs', newValue);
                }
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record, 'Rec');
                }
            },
            onBeforeShowEditWindow: function () {
                this.controller
                    .getStore('transferrf.Record')
                    .sorters
                    .add(
                        Ext.create(Ext.util.Sorter, {
                            property: 'TransferDate',
                            direction: 'DESC'
                        })
                    );
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела перечисления средств фонда
            */
            xtype: 'grideditwindowaspect',
            name: 'transferRfRecordGridEditWindowAspect',
            gridSelector: '#transferRfRecordGrid',
            editFormSelector: '#transferRfRecordEditWindow',
            storeName: 'transferrf.Record',
            modelName: 'transferrf.Record',
            editWindowView: 'transferrf.RecordEditWindow',
            otherActions: function (actions) {
                actions['#transferRfRecordEditWindow'] = { 'resize': { fn: this.onMinimizeEditWindow, scope: this } };
                actions['#transferRfRecordEditWindow #transferDate'] = { 'change': { fn: this.onChangeTransferDate, scope: this } };                
            },
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record, 'RecObj');
            },            
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.transferRfId && !record.data.Id) {
                        record.data.TransferRf = this.controller.transferRfId;
                    }
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record, 'RecObj');
                    this.controller.getAspect('transferRfRecordStateButtonAspect').setStateData(record.get('Id'), record.get('State'));
                }
            },
            onMinimizeEditWindow: function () {
                var transferRfRecordEditWindow = Ext.ComponentQuery.query('#transferRfRecordEditWindow')[0];
                if (transferRfRecordEditWindow) {
                    transferRfRecordEditWindow.center();
                }
            },
            onChangeTransferDate: function (record) {
                var date = record.getValue();
                if (date) {
                    date.setDate(1);
                    var form = this.getForm();
                    form.down("#transferDate").setValue(date);
                }
            }
        },
        {
            /*
            * Аспект инлайн редактирования таблицы
            */
            xtype: 'gkhinlinegridaspect',
            name: 'transferRfRecObjGkhInlineGridAspect',
            gridSelector: '#transferRfRecObjGrid',
            storeName: 'transferrf.RecObj',
            modelName: 'transferrf.RecObj',
            saveButtonSelector: '#transferRfRecObjGrid #transferRfRecObjSaveButton'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'TransferRfButtonExportAspect',
            gridSelector: 'transferRfGrid',
            buttonSelector: 'transferRfGrid #btnTransferRfExport',
            controllerName: 'TransferRf',
            actionName: 'Export'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'transferrf.Grid',
    mainViewSelector: 'transferRfGrid',
    
    editWindowSelector: '#transferRfRecordEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'transferRfGrid'
        }
    ],

    init: function () {
        this.getStore('transferrf.Record').on('beforeload', this.onBeforeLoad, this, 'Rec');
        this.getStore('transferrf.RecObj').on('beforeload', this.onBeforeLoad, this, 'RecObj');
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('transferRfGrid');

        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('TransferRf').load();

        var withArchiveRecs = view.down('checkbox[cmd=withArchiveRecs]').getValue();
        var transferRfStore = view.getStore();        
        transferRfStore.filter('withArchiveRecs', withArchiveRecs);
    },

    onBeforeLoad: function (store, operation, type) {
        if (type == 'Rec') {
            if (this.transferRfId) {
                operation.params.transferRfId = this.transferRfId;
            }
        }
        else if (type == 'RecObj') {
            operation.params.transferRfRecordId = this.transferRfRecordId;
            operation.params.contractRfId = this.contractRfId;
            operation.params.dateTransfer = this.dateTransfer;
        }
    },

    setCurrentId: function (record, type) {
        var editWindow, store, gridSelector, id;
        if (type == 'Rec') {
            this.transferRfId = id = record.getId();
            editWindow = Ext.ComponentQuery.query('#transferRfEditWindow')[0];
            store = this.getStore('transferrf.Record');
            gridSelector = '#transferRfRecordGrid';
            if (id > 0) {
                editWindow.down('#ContractRf').setReadOnly(true);
                this.contractRfId = record.get('ContractRf').Id || record.get('ContractRf');
            }
            else {
                editWindow.down('#ContractRf').setReadOnly(false);
            }
        }
        else if (type == 'RecObj') {
            this.transferRfRecordId = id = record.getId();
            this.dateTransfer = record.get('TransferDate');
            editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
            store = this.getStore('transferrf.RecObj');
            gridSelector = '#transferRfRecObjGrid';
            if (id > 0) {
                editWindow.down('#transferDate').setReadOnly(true);
            }
            else {
                editWindow.down('#transferDate').setReadOnly(false);
            }
        }
        if (id > 0) {
            editWindow.down(gridSelector).setDisabled(false);
        } else {
            editWindow.down(gridSelector).setDisabled(true);
        }
        store.load();
    }
});