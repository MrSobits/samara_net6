Ext.define('B4.controller.claimwork.Notification', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.ClaimWorkDocument',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton',
        'B4.aspects.ClaimWorkButtonPrintAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.Notification'
    ],

    views: [
        'claimwork.notification.EditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'clwnotifeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [

            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'clwNotifCreateButtonAspect',
            buttonSelector: 'clwnotifeditpanel acceptmenubutton',
            containerSelector: 'clwnotifeditpanel'
        },
        {
            xtype: 'claimworkdocumentaspect',
            name: 'notifClaimWorkEditPanelAspect',
            editPanelSelector: 'clwnotifeditpanel',
            modelName: 'claimwork.Notification',
            docCreateAspectName: 'clwNotifCreateButtonAspect'
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'clwNotifPrintAspect',
            buttonSelector: 'clwnotifeditpanel gkhbuttonprint',
            codeForm: 'NotificationClw',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                me.params.userParams = Ext.JSON.encode(param);
            }
        }
    ],

    index: function (type, id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('clwnotifeditpanel');

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}/{2}/notification', type, id, docId);
        me.bindContext(view);
        
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'docId', docId);
        me.application.deployView(view, 'claim_work');

        me.getAspect('notifClaimWorkEditPanelAspect').setData(docId);
        me.getAspect('clwNotifCreateButtonAspect').setData(id, type);
        me.getAspect('clwNotifPrintAspect').loadReportStore();
    }
});