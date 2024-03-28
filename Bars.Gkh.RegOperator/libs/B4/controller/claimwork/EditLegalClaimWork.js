Ext.define('B4.controller.claimwork.EditLegalClaimWork', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ClaimWorkDocCreateButton',
        'B4.aspects.DebtorClaimWorkEditAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'claimwork.LegalClaimWork'
    ],

    views: [
        'claimwork.LegalEditPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'legalclaimworkeditpanel'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.ClaimWork.Legal.Update', applyTo: 'button[actionName=updState]', selector: 'legalclaimworkeditpanel'
                },
                {
                    name: 'Clw.ClaimWork.Legal.Save', applyTo: 'b4savebutton', selector: 'legalclaimworkeditpanel'
                },
                {
                    name: 'Clw.ClaimWork.Legal.CrDebt.CurrBaseTariffDebtSum', applyTo: '[name=CurrChargeBaseTariffDebt]', selector: 'legalclaimworkeditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.CrDebt.CurrDecisionTariffDebtSum', applyTo: '[name=CurrChargeDecisionTariffDebt]', selector: 'legalclaimworkeditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.CrDebt.CurrCurrDebtSum', applyTo: '[name=CurrChargeDebt]', selector: 'legalclaimworkeditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.CrDebt.OrigBaseTariffDebtSum', applyTo: '[name=OrigChargeBaseTariffDebt]', selector: 'legalclaimworkeditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.CrDebt.OrigDecisionTariffDebtSum', applyTo: '[name=OrigChargeDecisionTariffDebt]', selector: 'legalclaimworkeditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.CrDebt.OrigDebtSum', applyTo: '[name=OrigChargeDebt]', selector: 'legalclaimworkeditpanel',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.Columns.BaseTariffDebtSum', applyTo: '[dataIndex=CurrChargeBaseTariffDebt]', selector: 'claimworkaccountdetailgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.Columns.DecisionTariffDebtSum', applyTo: '[dataIndex=CurrChargeDecisionTariffDebt]', selector: 'claimworkaccountdetailgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'Clw.ClaimWork.Legal.Columns.DebtSum', applyTo: '[dataIndex=CurrChargeDebt]', selector: 'claimworkaccountdetailgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            xtype: 'claimworkdoccreatebuttonaspect',
            name: 'legalClaimWorkCreateButtonAspect',
            buttonSelector: 'legalclaimworkeditpanel acceptmenubutton',
            containerSelector: 'legalclaimworkeditpanel'
        },
        {
            xtype: 'debtorclaimworkeditaspect',
            name: 'legalClaimWorkEditPanelAspect',
            editPanelSelector: 'legalclaimworkeditpanel',
            modelName: 'claimwork.LegalClaimWork',
            listeners: {
                savesuccess: function() {
                    this.controller.getAspect('legalClaimWorkCreateButtonAspect').reloadMenu();
                },
                afterSetPanelData: function (aspect, rec, panel) {
                    panel.setDisabled(false);
                    aspect.controller.setContextValue(panel, 'moId', rec.get('MunicipalityId'));
                }
            }
        }
    ],

    index: function (type, id) {
        var me = this,
            view = me.getMainView() || Ext.widget('legalclaimworkeditpanel'),
            jurisdictionAddressStore = view.down('b4selectfield[name=JurisdictionAddress]').getStore();

        jurisdictionAddressStore.on('beforeload', me.onBeforeLoadAddresses, me);

        me.bindContext(view);
        
        me.setContextValue(view, 'claimWorkId', id);
        me.setContextValue(view, 'type', type);
        me.application.deployView(view, 'claim_work');

        view.down('claimworkaccountdetailgrid toolbar').setVisible(false);

        me.getAspect('legalClaimWorkCreateButtonAspect').setData(id, type);
        me.getAspect('legalClaimWorkEditPanelAspect').setData(id, type);
    },

    onBeforeLoadAddresses: function (store, operation) {
        var me = this,
            view = me.getMainView();
        Ext.apply(operation.params, {moId: me.getContextValue(view, 'moId')});
    }
});