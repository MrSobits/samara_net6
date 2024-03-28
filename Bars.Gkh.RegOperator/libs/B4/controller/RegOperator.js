Ext.define('B4.controller.RegOperator', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.controller.regoperator.Navigation',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['RegOperator'],
    stores: ['RegOperator'],
    views: [
        'regoperator.Grid',
        'regoperator.AddWindow'
    ],

    mainView: 'regoperator.Grid',
    mainViewSelector: 'regoperatorgrid',

    refs: [
        {
            ref: 'AddWindow',
            selector: 'regoperatoraddwindow'
        },
        {
            ref: 'mainView',
            selector: 'regoperatorgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRegOp.FormationRegionalFund.RegOperator.Create', applyTo: 'b4addbutton', selector: 'regoperatorgrid' },
                {
                    name: 'GkhRegOp.FormationRegionalFund.RegOperator.Delete', applyTo: 'b4deletecolumn', selector: 'regoperatorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgrideditformaspect',
            gridSelector: 'regoperatorgrid',
            editFormSelector: 'regoperatoraddwindow',
            storeName: 'RegOperator',
            modelName: 'RegOperator',
            editWindowView: 'regoperator.AddWindow',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.regoperator.Navigation'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'regoperatorgrid',
            buttonSelector: 'regoperatorgrid #btnExport',
            controllerName: 'RegOperator',
            actionName: 'Export'
        }
    ],
    index: function () {
        var view = this.getMainView() || Ext.widget('regoperatorgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('RegOperator').load();
    }
});