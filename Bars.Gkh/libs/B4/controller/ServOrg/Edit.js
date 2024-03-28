Ext.define('B4.controller.servorg.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['ServiceOrganization'],
    views: ['servorg.EditPanel'],

    mainView: 'servorg.EditPanel',
    mainViewSelector: 'servorgeditpanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.Serv.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: 'servorgeditpanel',
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
            name: 'servorgEditPanelAspect',
            editPanelSelector: 'servorgeditpanel',
            modelName: 'ServiceOrganization'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('servorgeditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'servorgId', id);
        me.application.deployView(view, 'serv_org');

        me.getAspect('servorgEditPanelAspect').setData(id);
    }
});