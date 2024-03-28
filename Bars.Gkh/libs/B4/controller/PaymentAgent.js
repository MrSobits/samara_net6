Ext.define('B4.controller.PaymentAgent', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    controllers: ['B4.controller.paymentagent.Navigation'],
    models: ['PaymentAgent'],
    stores: ['PaymentAgent'],
    views: [
        'paymentagent.AddWindow',
        'paymentagent.Grid'
    ],

    aspects: [
        {
            xtype: 'gkhgrideditformaspect',
            name: 'paymentAgentGridWindowAspect',
            gridSelector: 'paymentAgentGrid',
            editFormSelector: '#paymentAgentAddWindow',
            storeName: 'PaymentAgent',
            modelName: 'PaymentAgent',
            editWindowView: 'paymentagent.AddWindow',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.paymentagent.Navigation'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'paymentAgentButtonExportAspect',
            gridSelector: 'paymentAgentGrid',
            buttonSelector: 'paymentAgentGrid #btnExport',
            controllerName: 'PaymentAgent',
            actionName: 'Export'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.PaymentAgent.Create', applyTo: 'b4addbutton', selector: 'paymentAgentGrid' },
                { name: 'Gkh.Orgs.PaymentAgent.Edit', applyTo: 'b4savebutton', selector: '#paymentAgentEditPanel' },
                {
                    name: 'Gkh.Orgs.PaymentAgent.Delete', applyTo: 'b4deletecolumn', selector: 'paymentAgentGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    mainView: 'paymentagent.Grid',
    mainViewSelector: 'paymentAgentGrid',

    refs: [
    {
        ref: 'mainView',
        selector: 'paymentAgentGrid'
    }],

    index: function () {
        var view = this.getMainView() || Ext.widget('paymentAgentGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('PaymentAgent').load();
    }
});