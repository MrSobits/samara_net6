Ext.define('B4.controller.contragent.Bank', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'contragent.Bank'
    ],

    stores: [
        'contragent.Bank'
    ],

    views: [
        'contragent.BankGrid',
        'contragent.BankEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentBankGrid'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Create', applyTo: 'b4addbutton', selector: 'contragentBankGrid' },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Edit', applyTo: 'b4savebutton', selector: '#contragentBankEditWindow' },
                { name: 'Gkh.Orgs.Contragent.Register.Bank.Delete', applyTo: 'b4deletecolumn', selector: 'contragentBankGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'contragentBankGridWindowAspect',
            gridSelector: 'contragentBankGrid',
            editFormSelector: '#contragentBankEditWindow',
            storeName: 'contragent.Bank',
            modelName: 'contragent.Bank',
            editWindowView: 'contragent.BankEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.Contragent = asp.controller.getContextValue(asp.controller.getMainComponent(), 'contragentId');
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('contragent.Bank').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentBankGrid');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        this.getStore('contragent.Bank').load();
    },
    
    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.contragentId = me.getContextValue(me.getMainComponent(), 'contragentId');
    }
});