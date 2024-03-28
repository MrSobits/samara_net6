Ext.define('B4.aspects.permission.Lawsuit', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.lawsuitpermission',

    applyBy: function (component, allowed) {
        component.setVisible(allowed);
    },

    permissions: [
        {
            name: 'Clw.DocumentRegister.LawsuitOwnerInfo_View',
            applyTo: 'lawsuitownerinfogrid',
            selector: 'clwlawsuitmaininfopanel'
        },
        {
            name: 'Clw.ClaimWork.Debtor.Lawsuit.Field.DateDirectionForSsp_View',
            applyTo: 'field[name=DateDirectionForSsp]',
            selector: 'clwlawsuitcollectionpanel'
        },
        {
            name: 'Clw.ClaimWork.Debtor.CourtOrderApplication.BaseTariffDebtSum', applyTo: '[name=DebtBaseTariffSum]', selector: 'clwlawsuitcourtclaiminfopanel',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.CourtOrderApplication.DecisionTariffDebtSum', applyTo: '[name=DebtDecisionTariffSum]', selector: 'clwlawsuitcourtclaiminfopanel',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.CourtOrderApplication.DebtSum', applyTo: '[name=DebtSum]', selector: 'clwlawsuitcourtclaiminfopanel',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.ClaimStatement.BaseTariffDebtSum', applyTo: '[name=DebtBaseTariffSum]', selector: 'clwlawsuitmaininfopanel',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.ClaimStatement.DecisionTariffDebtSum', applyTo: '[name=DebtDecisionTariffSum]', selector: 'clwlawsuitmaininfopanel',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.ClaimStatement.DebtSum', applyTo: '[name=DebtSum]', selector: 'clwlawsuitmaininfopanel',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        // Сведения о собственниках
        {
            name: 'Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DebtCalculate', applyTo: 'button[action=DebtCalculate]', selector: 'lawsuitownerinfogrid',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.LawsuitOwnerInfo.CalcPeriod', applyTo: '[dataIndex=CalcPeriod]', selector: 'lawsuitownerinfogrid',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.LawsuitOwnerInfo.CalcPeriod', applyTo: 'container[name=CalcPeriod]', selector: 'lawsuitownerinfowindow',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.LawsuitOwnerInfo.BaseTariffDebtSum', applyTo: '[dataIndex=DebtBaseTariffSum]', selector: 'lawsuitownerinfogrid',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.LawsuitOwnerInfo.DecisionTariffDebtSum', applyTo: '[dataIndex=DebtDecisionTariffSum]', selector: 'lawsuitownerinfogrid',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.LawsuitOwnerInfo.PenaltyDebt', applyTo: '[dataIndex=PenaltyDebt]', selector: 'lawsuitownerinfogrid',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.CourtOrderApplication.Print',
            applyTo: 'gkhbuttonprint',
            selector: 'clwlawsuitcourtclaiminfopanel',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'Clw.ClaimWork.Debtor.CourtOrderApplication.Delete', applyTo: 'button[action=delete]', selector: 'clwlawsuitcourtclaiminfopanel',
            applyBy: function (component, allowed) {
                if (component) component.setVisible(allowed);
            }
        }
    ]
});