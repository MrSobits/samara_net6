Ext.define('B4.controller.cashpaymentcenter.Edit', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'CashPaymentCenter'
    ],

    views: [
        'cashpaymentcenter.EditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'cashpaymentcentereditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.CashPaymentCenter.Edit', applyTo: 'b4savebutton', selector: 'cashpaymentcentereditpanel' },
                {
                    name: 'Gkh.Orgs.CashPaymentCenter.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: 'cashpaymentcentereditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'editPanelAspect',
            editPanelSelector: 'cashpaymentcentereditpanel',
            modelName: 'CashPaymentCenter'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('cashpaymentcentereditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'cashPaymentCenterId', id);
        me.application.deployView(view, 'cashpayment_center');

        me.getAspect('editPanelAspect').setData(id);
    }
});