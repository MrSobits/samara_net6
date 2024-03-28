Ext.define('B4.controller.Builder', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.Builder'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['Builder'],
    stores: ['Builder'],
    views: [
        'builder.AddWindow',
        'builder.Grid'
    ],

    mainView: 'builder.Grid',
    mainViewSelector: 'builderGrid',

    refs: [{
        ref: 'mainView',
        selector: 'builderGrid'
    }],

    aspects: [
        {
            xtype: 'builderperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'builderGridWindowAspect',
            gridSelector: 'builderGrid',
            editFormSelector: '#builderAddWindow',
            storeName: 'Builder',
            modelName: 'Builder',
            editWindowView: 'builder.AddWindow',
            controllerEditName: 'B4.controller.builder.Navigation',
            deleteWithRelatedEntities: true,
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowNotValid'] = { 'change': { fn: this.cbShowNotValidChange, scope: this } };
                actions[this.editFormSelector + ' #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
            },
            onBeforeLoadContragent: function (store, operation) {
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.showAll = true;
            },
            cbShowNotValidChange: function () {
                this.controller.getStore(this.storeName).load();
            }
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'builderButtonExportAspect',
            gridSelector: 'builderGrid',
            buttonSelector: 'builderGrid #btnExport',
            controllerName: 'Builder',
            actionName: 'Export'
        }
    ],

    init: function () {
        this.getStore('Builder').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('builderGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('Builder').load();
    },

    onBeforeLoad: function (store, operation) {
        var mainView = this.getMainView();
        if (mainView) {
            operation.params.showNotValid = mainView.down('#cbShowNotValid').checked;
        }
    }
});