Ext.define('B4.controller.supplyresourceorg.Edit', {
    extend: 'B4.base.Controller',
    views: ['supplyresourceorg.EditPanel'],
    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['SupplyResourceOrg'],

    mainView: 'supplyresourceorg.EditPanel',
    mainViewSelector: 'supplyresorgeditpanel',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.SupplyResource.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: 'supplyresorgeditpanel',
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
            name: 'supplyResOrgEditPanelAspect',
            editPanelSelector: 'supplyresorgeditpanel',
            modelName: 'SupplyResourceOrg'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('supplyresorgeditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'supplyresorgId', id);
        me.application.deployView(view, 'supplyres_org');

        me.getAspect('supplyResOrgEditPanelAspect').setData(id);
    }
});