Ext.define('B4.controller.utilityclaimwork.DepartureRestriction', {
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
        'utilityclaimwork.DepartureRestriction'
    ],

    views: [
        'utilityclaimwork.DepartRestrictEditPanel'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'utilitydepartrestricteditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.UtilityDebtor.DepartureRestriction.Save', applyTo: 'b4savebutton', selector: 'utilitydepartrestricteditpanel'
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'utilityDepartRestrictCreateButtonAspect',
            buttonSelector: 'utilitydepartrestricteditpanel acceptmenubutton',
            containerSelector: 'utilitydepartrestricteditpanel'
        },
        {
            xtype: 'claimworkdocumentaspect',
            name: 'utilityDepartRestrictEditPanelAspect',
            editPanelSelector: 'utilitydepartrestricteditpanel',
            modelName: 'utilityclaimwork.DepartureRestriction',
            docCreateAspectName: 'utilityDepartRestrictCreateButtonAspect'
        }
    ],

    index: function (type, id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('utilitydepartrestricteditpanel');

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}/{2}/departrestrict', type, id, docId);
        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'docId', docId);
        me.application.deployView(view, 'claim_work');

        me.getAspect('utilityDepartRestrictEditPanelAspect').setData(docId);
        me.getAspect('utilityDepartRestrictCreateButtonAspect').setData(id, type);
    }
});