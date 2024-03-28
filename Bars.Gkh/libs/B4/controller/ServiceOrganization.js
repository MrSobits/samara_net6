Ext.define('B4.controller.ServiceOrganization', {
    extend: 'B4.base.Controller',    
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.ServOrg'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        mixins: 'B4.mixins.Context'
    },
    
    models: ['ServiceOrganization'],
    stores: ['ServiceOrganization'],
    views: ['servorg.Grid', 'servorg.AddWindow'],

    mainView: 'servorg.Grid',
    mainViewSelector: 'servorgGrid',

    refs: [
    {
        ref: 'mainView',
        selector: 'servorgGrid'
    }],

    aspects: [
        {
            xtype: 'servorgperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'servorgGridWindowAspect',
            gridSelector: 'servorgGrid',
            editFormSelector: '#servorgAddWindow',
            storeName: 'ServiceOrganization',
            modelName: 'ServiceOrganization',
            editWindowView: 'servorg.AddWindow',
            controllerEditName: 'B4.controller.servorg.Navigation',
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
                        me.controller.application.redirectTo(Ext.String.format('serviceorganizationedit/{0}', id));
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
            name: 'serviceOrganizationButtonExportAspect',
            gridSelector: 'servorgGrid',
            buttonSelector: 'servorgGrid #btnExport',
            controllerName: 'ServiceOrganization',
            actionName: 'Export'
        }
    ],

    init: function () {
        this.getStore('ServiceOrganization').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('servorgGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('ServiceOrganization').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.showNotValid = this.showNotValid;
    }
});