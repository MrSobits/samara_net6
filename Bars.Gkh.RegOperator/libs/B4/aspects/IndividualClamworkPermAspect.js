Ext.define('B4.aspects.IndividualClamworkPermAspect', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.individualclaimworkperm',

    permissions: [
        {
            name: 'Clw.ClaimWork.Individual.Update',
            applyTo: 'button[actionName=updState]',
            selector: 'individualclaimworkeditpanel'
        },
        {
            name: 'Clw.ClaimWork.Individual.Save',
            applyTo: 'b4savebutton',
            selector: 'individualclaimworkeditpanel'
        },
        {
            name: 'Clw.ClaimWork.Individual.CrDebt.CurrBaseTariffDebtSum',
            applyTo: '[name=CurrChargeBaseTariffDebt]',
            selector: 'individualclaimworkeditpanel',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.CrDebt.CurrDecisionTariffDebtSum',
            applyTo: '[name=CurrChargeDecisionTariffDebt]',
            selector: 'individualclaimworkeditpanel',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.CrDebt.CurrDebtSum',
            applyTo: '[name=CurrChargeDebt]',
            selector: 'individualclaimworkeditpanel',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.CrDebt.OrigBaseTariffDebtSum',
            applyTo: '[name=OrigChargeBaseTariffDebt]',
            selector: 'individualclaimworkeditpanel',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.CrDebt.OrigDecisionTariffDebtSum',
            applyTo: '[name=OrigChargeDecisionTariffDebt]',
            selector: 'individualclaimworkeditpanel',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.CrDebt.OrigDebtSum',
            applyTo: '[name=OrigChargeDebt]',
            selector: 'individualclaimworkeditpanel',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.Columns.BaseTariffDebtSum',
            applyTo: '[dataIndex=CurrChargeBaseTariffDebt]',
            selector: 'claimworkaccountdetailgrid',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.Columns.DecisionTariffDebtSum',
            applyTo: '[dataIndex=CurrChargeDecisionTariffDebt]',
            selector: 'claimworkaccountdetailgrid',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.Columns.DebtSum',
            applyTo: '[dataIndex=CurrChargeDebt]',
            selector: 'claimworkaccountdetailgrid',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.Columns.PrintAccountReport',
            applyTo: '[dataIndex=PrintAccountReport]',
            selector: 'claimworkaccountdetailgrid',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Individual.Columns.PrintAccountClaimworkReport',
            applyTo: '[dataIndex=PrintAccountClaimworkReport]',
            selector: 'claimworkaccountdetailgrid',
            applyBy: function(component, allowed) {
                if (component)
                    component.setVisible(allowed);
            }
        }
    ]
});