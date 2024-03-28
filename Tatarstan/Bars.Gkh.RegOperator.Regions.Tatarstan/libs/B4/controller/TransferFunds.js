//ToDo перекрыл контроллер кпоскольку для РТ добавился новый грид Перечислений по найму
Ext.define('B4.controller.TransferFunds', {
    /*
    * Контроллер раздела перечисления средств фонда
    */

    transferRFId: null,
    transferRFRecordId: null,
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditCtxWindow',
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
        'transferrf.RecObj',
        'transferfunds.Object',
        'transferfunds.Hire',
        'RealityObject'
    ],

    stores: [
        'TransferRf',
        'transferrf.Record',
        'transferrf.RecObj',
        'transferfunds.Object',
        'transferfunds.Hire'
    ],

    views: [
        'transferfunds.EditWindow',
        'transferfunds.RecordEditWindow',
        'transferfunds.Grid',
        'transferfunds.ObjectGrid',
        'transferfunds.HireGrid'
    ],

    aspects: [
        /*{
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRf.TransferRf.Create', applyTo: 'b4addbutton', selector: 'transferfundsgrid' },
                { name: 'GkhRf.TransferRf.Edit', applyTo: 'b4savebutton', selector: '#transferRfEditWindow' },
                {
                    name: 'GkhRf.TransferRf.Delete', applyTo: 'b4deletecolumn', selector: 'transferfundsgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRf.TransferRf.WithArchiveRecs', applyTo: 'checkbox[cmd=withArchiveRecs]', selector: 'transferfundsgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },*/
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
            stateButtonSelector: 'transferfundsrecordwindow #btnState',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var editWindowAspect = asp.controller.getAspect('transferRfRecordGridEditWindowAspect'),
                        model = this.controller.getModel('transferrf.Record');

                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    editWindowAspect.updateGrid();

                    model.load(entityId, {
                        success: function(rec) {
                            editWindowAspect.setFormData(rec);
                            this.controller.getAspect('transferRfPerm').setPermissionsByRecord(rec);
                        },
                        scope: this
                    });
                }
            }
        },
        {
            /*
            * Вешаем аспект смены статуса
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
            xtype: 'grideditctxwindowaspect',
            name: 'transferRfGridEditWindowAspect',
            gridSelector: 'transferfundsgrid',
            editFormSelector: 'transferfundswindow',
            storeName: 'TransferRf',
            modelName: 'TransferRf',
            editWindowView: 'transferfunds.EditWindow',
            otherActions: function (actions) {
                actions['transferfundswindow'] = { 'beforeshow': { fn: this.onBeforeShowEditWindow, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.setCurrentId(record);
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.setCurrentId(record);
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
            },
            setCurrentId: function (record) {
                var me = this,
                    editWindow = me.getForm(),
                    recordGrid = editWindow.down('transferfundsrecordgrid'),
                    recordStore = recordGrid.getStore(),
                    sfContractRf = editWindow.down('b4selectfield[name=ContractRf]'),
                    id;
                
                me.controller.transferRfId = id = record.getId();
                sfContractRf.setReadOnly(id > 0);
                recordGrid.setDisabled(id == 0);

                if (id > 0) {
                    me.controller.contractRfId = record.get('ContractRf').Id || record.get('ContractRf');
                    recordStore.load();
                } else {
                    recordStore.removeAll();
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела перечисления средств фонда
            */
            xtype: 'grideditctxwindowaspect',
            name: 'transferRfRecordGridEditWindowAspect',
            gridSelector: 'transferfundsrecordgrid',
            editFormSelector: 'transferfundsrecordwindow',
            modelName: 'transferrf.Record',
            editWindowView: 'transferfunds.RecordEditWindow',
            otherActions: function (actions) {
                actions['transferfundsrecordwindow'] = { 'resize': { fn: this.onMinimizeEditWindow, scope: this } };
                actions['transferfundsrecordwindow datefield[name=TransferDate]'] = { 'change': { fn: this.onChangeTransferDate, scope: this } };
            },
            onSaveSuccess: function (asp, record) {
                asp.setCurrentId(record);
            },
            listeners: {
                getdata: function (asp, record) {
                    if (asp.controller.transferRfId && !record.getId()) {
                        record.set('TransferRf', asp.controller.transferRfId);
                    }
                },
                aftersetformdata: function (asp, record) {
                    asp.setCurrentId(record);
                    asp.controller.getAspect('transferRfRecordStateButtonAspect').setStateData(record.getId(), record.get('State'));
                }
            },
            onMinimizeEditWindow: function () {
                var transferRfRecordEditWindow = Ext.ComponentQuery.query('transferfundsrecordwindow')[0];
                if (transferRfRecordEditWindow) {
                    transferRfRecordEditWindow.center();
                }
            },
            onChangeTransferDate: function (cmp, newValue) {
                if (newValue) {
                    newValue.setDate(1);
                    cmp.setValue(newValue);
                }
            },
            setCurrentId: function (record) {
                var me = this,
                    editWindow = me.getForm(),
                    objectGrid = editWindow.down('transferfundsobjectgrid'),
                    hireGrid = editWindow.down('transferfundshiregrid'),
                    objectStore = objectGrid.getStore(),
                    hireStore = hireGrid.getStore(),
                    dfTransferDate = editWindow.down('datefield[name=TransferDate]'),
                    id;

                me.controller.transferRfRecordId = id = record.getId();
                me.controller.dateTransfer = record.get('TransferDate');
                dfTransferDate.setReadOnly(id > 0);
                objectGrid.setDisabled(id == 0);
                hireGrid.setDisabled(id == 0);

                if (id > 0) {
                    objectStore.load();
                } else {
                    objectStore.removeAll();
                    hireStore.removeAll();
                }
            }
        },
        {
            /*
            * Аспект инлайн редактирования таблицы Перечислений по счету дома
            */
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'transferfundsobjectgrid',
            storeName: 'transferfunds.Object',
            modelName: 'transferfunds.Object',
            saveButtonSelector: 'transferfundsobjectgrid button[action=SaveObjects]'
        },
        {
            /*
            * Аспект инлайн редактирования таблицы Перечислений по найму
            */
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'transferfundshiregrid',
            storeName: 'transferfunds.Hire',
            modelName: 'transferfunds.Hire',
            saveButtonSelector: 'transferfundshiregrid button[action=SaveObjects]',
            listeners: {
                'beforesave': function(asp, store) {
                    var isValid = true;
                    var notValidNum = '';

                    var recs = store.getModifiedRecords();

                    Ext.each(recs, function(rec) {
                        if (rec.get('PaidTotal') < rec.get('BeforeTransfer') + rec.get('TransferredSum')) {
                            notValidNum = rec.get('AccountNum');
                            isValid = false;
                        }
                    });

                    if (!isValid) {
                        Ext.Msg.alert('Ошибка заполнения', Ext.String.format('Перечисленная сумма превышает сумму оплат по лицевому счету {0} в данном периоде', notValidNum));
                        return false;
                    }

                    return true;
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            gridSelector: 'transferfundsgrid',
            buttonSelector: 'transferfundsgrid button[action=Export]',
            controllerName: 'TransferRf',
            actionName: 'Export'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'transferfunds.Grid',
    mainViewSelector: 'transferfundsgrid',
    
    editWindowSelector: 'transferfundsrecordwindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'transferfundsgrid'
        },
        {
            ref: 'EditWindow',
            selector: 'transferfundswindow'
        },
        {
            ref: 'RecordGrid',
            selector: 'transferfundsrecordgrid'
        },
        {
            ref: 'RecordWindow',
            selector: 'transferfundsrecordwindow'
        },
        {
            ref: 'ObjectGrid',
            selector: 'transferfundsobjectgrid'
        },
        {
            ref: 'HireGrid',
            selector: 'transferfundshiregrid'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'transferfundsrecordgrid': {
                afterrender: function(grid) {
                    grid.getStore().on('beforeload', function(s, operation) {
                        operation.params.transferRfId = me.transferRfId;
                    });
                }
            },
            'transferfundsobjectgrid': {
                afterrender: function(grid) {
                    grid.getStore().on('beforeload', function(s, operation) {
                        operation.params.transferRfRecordId = me.transferRfRecordId;
                        operation.params.contractRfId = me.contractRfId;
                        operation.params.dateTransfer = me.dateTransfer;
                    });
                },
                rowaction: function (grid, action, record) {
                    switch (action.toLowerCase()) {
                        case 'account':
                            me.gotoAccount(record);
                            break;
                    }
                }
            },
            'transferfundshiregrid': {
                afterrender: function (grid) {
                    grid.getStore().on('beforeload', function (s, operation) {
                        operation.params.transferRfRecordId = me.transferRfRecordId;
                    });
                    if (me.transferRfRecordId > 0) {
                        grid.getStore().load();
                    }
                }
            },
            'transferfundsobjectgrid button[action=Calc]': { click: { fn: me.onClickCalc, scope: me } },
            'transferfundshiregrid button[action=Calc]': { click: { fn: me.onHireClickCalc, scope: me } },
            'transferfundsgrid': {
                'store.beforeload': function (s, operation) {
                    var view = me.getMainView();
                    operation.params.withArchiveRecs = view.down('checkbox[name=ShowArchive]').getValue();
                }
            }
        });

        this.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('transferfundsgrid');
            me.bindContext(view);
            me.application.deployView(view);
            view.getStore().load();
        
    },
    
    onClickCalc: function() {
        var me = this,
            store = me.getObjectGrid().getStore();

        me.mask('Расчет...', B4.getBody().getActiveTab());
        try {
            B4.Ajax.request({
                url: B4.Url.action('Calc', 'TransferObject'),
                timeout: 9999999,
                params: {
                    transferRecordId: me.transferRfRecordId
                }
            }).next(function () {
                me.unmask();
                store.load();
            }).error(function (result) {
                Ext.Msg.alert('Ошибка при выполнении расчёта!', result.message);
                me.unmask();
            });
        } catch (e) {
            me.unmask();
        }
    },
    
    onHireClickCalc: function () {
        var me = this,
            store = me.getHireGrid().getStore();

        me.mask('Расчет...', B4.getBody().getActiveTab());
        try {
            B4.Ajax.request({
                url: B4.Url.action('Calc', 'TransferHire'),
                timeout: 9999999,
                params: {
                    transferRecordId: me.transferRfRecordId
                }
            }).next(function () {
                me.unmask();
                store.load();
            }).error(function (result) {
                Ext.Msg.alert('Ошибка при выполнении расчёта!', result.message);
                me.unmask();
            });
        } catch (e) {
            me.unmask();
        }
    },
    
    gotoAccount: function(record) {
        var me = this,
            realObjId = record.get('RealityObject');

        if (realObjId) {
            me.application.redirectTo(Ext.String.format('realityobjectedit/{0}/', realObjId));
        }
    }
});