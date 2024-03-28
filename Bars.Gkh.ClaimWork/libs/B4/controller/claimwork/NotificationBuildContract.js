Ext.define('B4.controller.claimwork.NotificationBuildContract', {
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
            name: 'clwNotifBcCreateButtonAspect',
            buttonSelector: 'clwnotifeditpanel acceptmenubutton',
            containerSelector: 'clwnotifeditpanel',
            createDocument: function (params) {
                var me = this,
                    data,
                    container = me.componentQuery(me.containerSelector);

                me.controller.mask('Формирование документа', container);

                B4.Ajax.request({
                    url: B4.Url.action('CreateDocument', 'ClaimWorkDocument'),
                    method: 'POST',
                    timeout: 9999999,
                    params: params
                }).next(function (res) {
                    data = Ext.decode(res.responseText);

                    me.fireEvent('createsuccess', me);

                    Ext.History.add(Ext.String.format("claimworkbc/BuildContractClaimWork/{0}/{1}/{2}/aftercreatedoc", params.claimWorkId, data.Id, params.actionUrl));

                    me.controller.unmask();
                }).error(function (e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка формирования документа!', e.message || e);
                });
            },
        },
        {
            xtype: 'claimworkdocumentaspect',
            name: 'notifClaimWorkBcEditPanelAspect',
            editPanelSelector: 'clwnotifeditpanel',
            modelName: 'claimwork.Notification',
            docCreateAspectName: 'clwNotifBcCreateButtonAspect',
            recordDestroy: function (record, questionStr) {
                var me = this;
                Ext.Msg.confirm('Удаление записи!', questionStr, function (result) {
                    if (result === 'yes') {
                        me.mask('Удаление', B4.getBody());
                        record.destroy()
                            .next(function () {
                                var view = me.controller.getMainView(),
                                    claimworkId = me.controller.getContextValue(view, 'claimWorkId'),
                                    type = me.controller.getContextValue(view, 'type');
                                B4.QuickMsg.msg('Удаление', 'Документ удален успешно', 'success');
                                if (claimworkId && type) {
                                    Ext.History.add(Ext.String.format('claimworkbc/BuildContractClaimWork/{0}/deletedocument', claimworkId));
                                }
                                me.unmask();
                            }, me)
                            .error(function (result) {
                                me.unmask();
                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, me);
                    }
                }, me);
            }
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'clwNotifBcPrintAspect',
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

    index: function (id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('clwnotifeditpanel');

        view.ctxKey = Ext.String.format('claimworkbc/BuildContractClaimWork/{0}/{1}/notification', id, docId);
        me.bindContext(view);
        
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', 'BuildContractClaimWork');
        me.setContextValue(view, 'docId', docId);
        me.application.deployView(view, 'claim_work_bc');

        me.getAspect('notifClaimWorkBcEditPanelAspect').setData(docId);
        me.getAspect('clwNotifBcCreateButtonAspect').setData(id, 'BuildContractClaimWork');
        me.getAspect('clwNotifBcPrintAspect').loadReportStore();
    }
});