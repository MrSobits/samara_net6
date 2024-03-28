Ext.define('B4.controller.claimwork.PretensionBuildContract', {
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
        'claimwork.Pretension'
    ],

    stores: [
    ],

    views: [
        'claimwork.buildcontract.pretension.TabPanel',
        'claimwork.buildcontract.pretension.EditPanel'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'pretensionbctabpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.Debtor.Pretension.PaymentPlannedPeriodField_View',
                    applyTo: '#cwPaymentPlannedPeriod',
                    selector: 'pretensionbceditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.setVisible(allowed);
                            } else {
                                component.setVisible(allowed);
                            }
                        }
                    }
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'clwPretensionBcCreateButtonAspect',
            buttonSelector: 'pretensionbceditpanel acceptmenubutton',
            containerSelector: 'pretensionbceditpanel',
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
            name: 'pretensionClaimWorkBcEditPanelAspect',
            editPanelSelector: 'pretensionbceditpanel',
            modelName: 'claimwork.Pretension',
            docCreateAspectName: 'clwPretensionBcCreateButtonAspect',
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
            name: 'pretensionBcPrintAspect',
            buttonSelector: 'pretensionbceditpanel gkhbuttonprint',
            codeForm: 'Pretension',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                me.params.userParams = Ext.JSON.encode(param);
            }
        }
    ],

    init: function () {
        var me = this;
        me.callParent(arguments);
    },

    index: function (id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('pretensionbctabpanel');

        view.ctxKey = Ext.String.format('claimworkbc/BuildContractClaimWork/{0}/{1}/pretension', id, docId);
        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', 'BuildContractClaimWork');
        me.setContextValue(view, 'docId', docId);
        me.application.deployView(view, 'claim_work_bc');

        me.getAspect('pretensionClaimWorkBcEditPanelAspect').setData(docId);
        me.getAspect('clwPretensionBcCreateButtonAspect').setData(id, 'BuildContractClaimWork');
        me.getAspect('pretensionBcPrintAspect').loadReportStore();
    }
});