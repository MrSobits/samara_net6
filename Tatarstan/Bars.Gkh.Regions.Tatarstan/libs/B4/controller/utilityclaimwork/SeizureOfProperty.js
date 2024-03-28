Ext.define('B4.controller.utilityclaimwork.SeizureOfProperty', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.ClaimWorkDocument',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'utilityclaimwork.SeizureOfProperty'
    ],

    views: [
        'utilityclaimwork.PropSeizureEditPanel'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'utilitypropseizureeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.SeizureOfProperty.Save', applyTo: 'b4savebutton', selector: 'utilitypropseizureeditpanel'
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'utilityPropSeizureCreateButtonAspect',
            buttonSelector: 'utilitypropseizureeditpanel acceptmenubutton',
            containerSelector: 'utilitypropseizureeditpanel'
        },
        {
            xtype: 'claimworkdocumentaspect',
            name: 'utilityPropSeizureEditPanelAspect',
            editPanelSelector: 'utilitypropseizureeditpanel',
            modelName: 'utilityclaimwork.SeizureOfProperty',
            docCreateAspectName: 'utilityPropSeizureCreateButtonAspect'
        }
    ],

    index: function (type, id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('utilitypropseizureeditpanel');

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}/{2}/propseizure', type, id, docId);
        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'docId', docId);
        me.application.deployView(view, 'claim_work');

        me.getAspect('utilityPropSeizureEditPanelAspect').setData(docId);
        me.getAspect('utilityPropSeizureCreateButtonAspect').setData(id, type);
    }
});