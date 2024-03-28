Ext.define('B4.controller.claimwork.Pretension', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.ClaimWorkDocument',
        'B4.aspects.DocumentClwAccountDetailAspect',
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
        'claimwork.pretension.DebtPayment'
    ],

    views: [
        'claimwork.pretension.TabPanel',
        'claimwork.pretension.EditPanel',
        'claimwork.pretension.DebtPaymentGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'pretensiontabpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.Debtor.Pretension.DebtPayment', applyTo: 'pretensiondebtpaymentgrid', selector: 'pretensiontabpanel',
                    applyBy: function (component, allowed) {
                        var me = this,
                            view = me.controller.getMainView(),
                            type = me.controller.getContextValue(view, 'type');
                        if (allowed && (type === 'LegalClaimWork' || type === 'IndividualClaimWork')) {
                            component.tab.show();
                        } else {
                            component.tab.hide();
                        }
                    }
                },
                {
                    name: 'Clw.ClaimWork.Debtor.Pretension.PaymentPlannedPeriodField_View',
                    applyTo: '#cwPaymentPlannedPeriod',
                    selector: 'pretensioneditpanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.setVisible(allowed);
                            } else {
                                component.setVisible(allowed);
                            }
                        }
                    }
                },
                {
                    name: 'Clw.ClaimWork.Debtor.Pretension.BaseTariffDebtSum', applyTo: '[name=BaseTariffSum]', selector: 'pretensioneditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Debtor.Pretension.DecisionTariffDebtSum', applyTo: '[name=DecisionTariffSum]', selector: 'pretensioneditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Debtor.Pretension.DebtSum', applyTo: '[name=Sum]', selector: 'pretensioneditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'clwPretensionCreateButtonAspect',
            buttonSelector: 'pretensioneditpanel acceptmenubutton',
            containerSelector: 'pretensioneditpanel'
        },
        {
            xtype: 'claimworkdocumentaspect',
            name: 'pretensionClaimWorkEditPanelAspect',
            editPanelSelector: 'pretensioneditpanel',
            modelName: 'claimwork.Pretension',
            docCreateAspectName: 'clwPretensionCreateButtonAspect'
        },
        {
            xtype: 'claimworkbuttonprintaspect',
            name: 'pretensionPrintAspect',
            buttonSelector: 'pretensioneditpanel gkhbuttonprint',
            codeForm: 'Pretension',
            getUserParams: function () {
                var me = this,
                    view = me.controller.getMainView(),
                    param = { DocumentId: me.controller.getContextValue(view, 'docId') };

                me.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'documentclwaccountdetailaspect',
            name: 'documentClwAccountDetail',
            panelSelector: 'pretensiontabpanel',
            getDocumentId: function() {
                var me = this.controller,
                    view = me.getMainView();

                return me.getContextValue(view, 'docId');
            }
        }
    ],

    init: function () {
        
        var me = this,
            actions = [];
        //me.getStore('claimwork.pretension.DebtPayment').on('beforeload', me.onDebtPaymentGridBeforeLoad, me);
        actions['pretensioneditpanel button[action=GenNumPretension]'] = { click: me.genNumPretension, scope: me }

        me.control({
            'pretensiontabpanel': { 'tabchange': { fn: me.changeTab, scope: me } },
            'pretensiondebtpaymentgrid': { 'render': function (grid) { grid.getStore().on('beforeload', me.onDebtPaymentGridBeforeLoad, me); } },
            'pretensiondebtpaymentgrid b4updatebutton': { click: me.updateDebtPaymentGrid }
        });
        me.control(actions);
        me.callParent(arguments);
        
    },

    index: function (type, id, docId) {
        var me = this,
            view = me.getMainView() || Ext.widget('pretensiontabpanel');

        view.ctxKey = Ext.String.format('claimwork/{0}/{1}/{2}/pretension', type, id, docId);
        me.bindContext(view);

        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.setContextValue(view, 'docId', docId);
        me.application.deployView(view, 'claim_work');

        me.getStore('claimwork.pretension.DebtPayment').load();

        me.getAspect('pretensionClaimWorkEditPanelAspect').setData(docId);
        me.getAspect('clwPretensionCreateButtonAspect').setData(id, type);
        me.getAspect('pretensionPrintAspect').loadReportStore();
    },

    onDebtPaymentGridBeforeLoad: function (store, operation) {
        var me = this,
            view = me.getMainView();
        operation.params.pretensionId = me.getContextValue(view, 'docId');
    },

    changeTab: function (tabPanel, newTab, oldTab) {
        if (newTab.xtype === 'pretensiondebtpaymentgrid') {
            newTab.getStore().load();
        }
    },

    updateDebtPaymentGrid: function (btn) {
        btn.up('pretensiondebtpaymentgrid').getStore().load();
    },
    genNumPretension: function (btn) {
        debugger;
       
        var me = this,
            view = me.getMainView(),
            docId = me.getContextValue(view, 'docId');
        var panel = btn.up('pretensioneditpanel');
        me.mask('Генерация номера...', Ext.getBody());

        B4.Ajax.request({
            url: B4.Url.action('GenNumberForPretention', 'GenNumber'),
            method: 'POST',
            timeout: 100 * 60 * 60 * 3,
            params: {
                docId: docId
            }
        })
            .next(function (response) {
                var obj = Ext.JSON.decode(response.responseText);
                var bidnumbfield = panel.down('#numberPretension');
                bidnumbfield.setValue(obj.resNum);
                me.unmask();
            })
            .error(function (error) {
                me.unmask();
                Ext.Msg.alert('Ошибка', 'Ошибка генерации');
            });
    }
});