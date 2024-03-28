Ext.define('B4.controller.contragent.CasesEdit', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'Contragent'
    ],
    
    views: [
        'contragent.CasesEditPanel'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'contragentCasesEditPanel'
        }
    ],

    parentCtrlCls: 'B4.controller.contragent.Navi',

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Contragent.Register.CasesEdit.Edit', applyTo: 'b4savebutton', selector: 'contragentCasesEditPanel' }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'contragentCaseEditPanelAspect',
            editPanelSelector: 'contragentCasesEditPanel',
            modelName: 'Contragent'
        }
    ],
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentCasesEditPanel');

        me.bindContext(view);
        me.setContextValue(view, 'contragentId', id);
        me.application.deployView(view, 'contragent_info');

        this.getAspect('contragentCaseEditPanelAspect').setData(id); 
    }
});