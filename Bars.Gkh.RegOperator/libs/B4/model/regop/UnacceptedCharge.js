Ext.define('B4.model.regop.UnacceptedCharge', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
        { name: 'AccountNum' },
        { name: 'Penalty' },
        { name: 'Recalc' },
        { name: 'RecalcByDecision' },
        { name: 'RecalcPenalty' },
        { name: 'ChargeTariff' },
        { name: 'Charge' },
        { name: 'ChargeDt' },
        { name: 'Description' },
        { name: 'AccountId' },
        { name: 'ContragentAccountNumber' },
        { name: 'AccountState' },
        { name: 'CrFundFormationDecisionType' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'UnacceptedCharge',
        timeout: 60000 * 5
    }
});