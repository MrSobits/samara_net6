Ext.define('B4.controller.SupplyResourceOrg', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.SupplyResourceOrg'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['SupplyResourceOrg'],
    stores: ['SupplyResourceOrg'],
    views: ['supplyresourceorg.Grid', 'supplyresourceorg.AddWindow'],

    mainView: 'supplyresourceorg.Grid',
    mainViewSelector: 'supplyResOrgGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'supplyResOrgGrid'
        }
    ],

    aspects: [
        {
            xtype: 'supplyresorgperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'supplyResOrgGridWindowAspect',
            gridSelector: 'supplyResOrgGrid',
            editFormSelector: 'supplyresorgaddwindow',
            storeName: 'SupplyResourceOrg',
            modelName: 'SupplyResourceOrg',
            editWindowView: 'supplyresourceorg.AddWindow',
            controllerEditName: 'B4.controller.supplyresourceorg.Navigation',
            deleteWithRelatedEntities: true,
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowNotValid'] = { 'change': { fn: this.cbShowNotValidChange, scope: this } };
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
            },
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.showAll = true;
                operation.params.operatorHasContragent = true;
            },
            cbShowNotValidChange: function (cb, newValue) {
                this.controller.showNotValid = newValue;
                this.controller.getStore(this.storeName).load();
            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.data.Id : null,
                    model;

                model = me.controller.getModel(me.modelName);

                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('supplyresorgedit/{0}', id));
                    }
                    else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                }
                else {
                    me.setFormData(new model({ Id: 0 }));
                }
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'supplyResourceOrgButtonExportAspect',
            gridSelector: 'supplyResOrgGrid',
            buttonSelector: 'supplyResOrgGrid #btnExport',
            controllerName: 'SupplyResourceOrg',
            actionName: 'Export'
        }
    ],

    init: function () {
        this.getStore('SupplyResourceOrg').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('supplyResOrgGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('SupplyResourceOrg').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.showNotValid = this.showNotValid;
    }
});