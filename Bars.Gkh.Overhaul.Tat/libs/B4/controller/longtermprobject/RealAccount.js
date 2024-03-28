Ext.define('B4.controller.longtermprobject.RealAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.enums.AccountType'
    ],

    models: ['account.Real'],
    stores: ['account.Real', 'account.operation.Real'],
    views: [
        'longtermprobject.realaccount.Grid',
        'longtermprobject.realaccount.EditWindow',
        'longtermprobject.realaccount.OperationGrid',
        'longtermprobject.realaccount.OperationEditWindow'
    ],

    mainView: 'longtermprobject.realaccount.Grid',
    mainViewSelector: 'realaccountgrid',

    refs: [
        { ref: 'editWindow', selector: 'realaccounteditwin' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'realAccountGridWindowAspect',
            gridSelector: 'realaccountgrid',
            editFormSelector: 'realaccounteditwin',
            storeName: 'account.Real',
            modelName: 'account.Real',
            editWindowView: 'longtermprobject.realaccount.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setAccountId(record.getId());
            },
            listeners: {
                beforesave: function (asp, obj) {
                    obj.set('RealityObject', asp.controller.params.realityObjectId);
                    obj.set('AccountType', B4.enums.AccountType.getMeta('Real').Value);
                    return true;
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.setAccountId(record.getId());
                }
            },

            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realAccountOperationGridWindowAspect',
            gridSelector: 'realaccountopergrid',
            editFormSelector: 'realaccountopereditwin',
            storeName: 'account.operation.Real',
            modelName: 'account.operation.Real',
            editWindowView: 'longtermprobject.realaccount.OperationEditWindow',
            listeners: {
                beforesave: function (asp, obj) {
                    obj.set('Account', asp.controller.params.realAccountId);
                    return true;
                }
            },

            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            }
        }
    ],

    init: function() {
        this.getStore('account.Real').on('beforeload', this.onBeforeLoad, this);
        this.getStore('account.operation.Real').on('beforeload', this.onBeforeLoadOperations, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('account.Real').load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.roId = this.params.realityObjectId;
    },
    
    onBeforeLoadOperations: function(store, operation) {
        operation.params.realAccountId = this.params.realAccountId;
    },

    setAccountId: function (id) {
        var grid = this.getEditWindow().down('realaccountopergrid'),
            store = grid.getStore();

        this.params.realAccountId = id;
        store.removeAll();
        grid.setDisabled(!id);

        if (id) {
            store.load();
        }
    }
});