Ext.define('B4.controller.deliveryagent.Edit', {
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
        'DeliveryAgent'
    ],

    views: [
        'deliveryagent.EditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'deliveryagenteditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.DeliveryAgent.Edit', applyTo: 'b4savebutton', selector: 'deliveryagenteditpanel' },
                {
                    name: 'Gkh.Orgs.DeliveryAgent.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: 'deliveryagenteditpanel',
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
            editPanelSelector: 'deliveryagenteditpanel',
            modelName: 'DeliveryAgent'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('deliveryagenteditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'delAgentId', id);
        me.application.deployView(view, 'delivery_agent');

        me.getAspect('editPanelAspect').setData(id);
    }
});