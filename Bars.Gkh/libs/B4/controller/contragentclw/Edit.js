Ext.define('B4.controller.contragentclw.Edit', {
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
        'ContragentClw'
    ],

    views: [
        'contragentclw.EditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'contragentclweditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.ContragentClw.Edit', applyTo: 'b4savebutton', selector: 'contragentclweditpanel' },
                {
                    name: 'Gkh.Orgs.ContragentClw.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: 'contragentclweditpanel',
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
            editPanelSelector: 'contragentclweditpanel',
            modelName: 'ContragentClw'
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentclweditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'contragentClwId', id);
        me.application.deployView(view, 'contragent_clw');

        me.getAspect('editPanelAspect').setData(id);
    }
});