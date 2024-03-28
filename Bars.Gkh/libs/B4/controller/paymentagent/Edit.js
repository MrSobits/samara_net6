Ext.define('B4.controller.paymentagent.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['PaymentAgent'],

    views: [
        'paymentagent.EditPanel'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.PaymentAgent.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: '#paymentAgentEditPanel',
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
            name: 'paymentAgentEditPanelAspect',
            editPanelSelector: '#paymentAgentEditPanel',
            modelName: 'PaymentAgent' 
        }
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'paymentagent.EditPanel',
    mainViewSelector: '#paymentAgentEditPanel',

    onLaunch: function () {
        if (this.params) {
            this.getAspect('paymentAgentEditPanelAspect').setData(this.params.get('Id'));
        }
    }
});