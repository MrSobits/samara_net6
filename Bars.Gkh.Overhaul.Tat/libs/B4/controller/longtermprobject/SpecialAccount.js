Ext.define('B4.controller.longtermprobject.SpecialAccount', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    models: ['account.Special'],
    stores: ['account.Special', 'account.operation.Special'],
    views: [
        'longtermprobject.specialaccount.Grid',
        'longtermprobject.specialaccount.EditWindow',
        'longtermprobject.specialaccount.OperationGrid',
        'longtermprobject.specialaccount.OperationEditWindow'
    ],

    mainView: 'longtermprobject.specialaccount.Grid',
    mainViewSelector: 'specialaccountgrid',

    refs: [
        { ref: 'editWindow', selector: 'specialaccounteditwin' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'specialAccountGridWindowAspect',
            gridSelector: 'specialaccountgrid',
            editFormSelector: 'specialaccounteditwin',
            storeName: 'account.Special',
            modelName: 'account.Special',
            editWindowView: 'longtermprobject.specialaccount.EditWindow',
            onSaveSuccess: function(asp, record) {
                asp.controller.setAccountId(record.getId());
            },
            listeners: {
                beforesave: function(asp, obj) {
                    obj.set('RealityObject', asp.controller.params.realityObjectId);
                    return true;
                },
                aftersetformdata: function(asp, record) {
                    asp.controller.setAccountId(record.getId());
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'specialAccountOperationGridWindowAspect',
            gridSelector: 'specaccountopergrid',
            editFormSelector: 'specaccountopereditwin',
            storeName: 'account.operation.Special',
            modelName: 'account.operation.Special',
            editWindowView: 'longtermprobject.specialaccount.OperationEditWindow',
            listeners: {
                beforesave: function(asp, obj) {
                    obj.set('Account', asp.controller.params.accountId);
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Create', applyTo: 'b4addbutton', selector: 'specialaccountgrid' },
                { name: 'Gkh.RealityObject.Register.Accounts.Special.Edit', applyTo: 'b4savebutton', selector: 'specialaccounteditwin' },
                {
                    name: 'Gkh.RealityObject.Register.Accounts.Special.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'specialaccountgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function() {
        this.getStore('account.Special').on('beforeload', this.onBeforeLoad, this);

        this.getStore('account.operation.Special').on('beforeload', this.onBeforeLoadOperations, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('account.Special').load();
    },

    onBeforeLoad: function(store, operation) {
        operation.params.roId = this.params.record.data.RealObjId;
    },

    onBeforeLoadOperations: function(store, operation) {
        operation.params.accountId = this.params.accountId;
    },

    setAccountId: function(id) {
        var grid = this.getEditWindow().down('specaccountopergrid'),
            store = grid.getStore();

        this.params.accountId = id;
        store.removeAll();
        grid.setDisabled(!id);

        if (id) {
            store.load();
        }
    }
});