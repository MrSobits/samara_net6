Ext.define('B4.controller.PublicServOrg', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.PublicServicesOrg',
        'B4.controller.publicservorg.Navigation'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    models: ['PublicServiceOrg'],
    stores: ['PublicServiceOrg'],
    views: ['publicservorg.Grid', 'publicservorg.AddWindow'],

    mainView: 'publicservorg.Grid',
    mainViewSelector: 'publicservorgGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'publicservorgGrid'
        }
    ],

    aspects: [
        {
            xtype: 'publicserorgperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'publicservorgGridWindowAspect',
            gridSelector: 'publicservorgGrid',
            editFormSelector: '#publicservorgAddWindow',
            storeName: 'PublicServiceOrg',
            modelName: 'PublicServiceOrg',
            editWindowView: 'publicservorg.AddWindow',
            controllerEditName: 'B4.controller.publicservorg.Navigation',
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
                        me.controller.application.redirectTo(Ext.String.format('publicservorgedit/{0}', id));
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
            name: 'publicServiceOrgtButtonExportAspect',
            gridSelector: 'publicservorgGrid',
            buttonSelector: 'publicservorgGrid #btnExport',
            controllerName: 'PublicServiceOrg',
            actionName: 'Export'
        }
    ],

    init: function () {
        this.getStore('PublicServiceOrg').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('publicservorgGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('PublicServiceOrg').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.operatorHasContragent = true;
        operation.params.showNotValid = this.showNotValid;
    }
});