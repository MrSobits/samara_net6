Ext.define('B4.model.regoperator.PersAccMunicipality', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RegOpPersAccMunicipality'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RegOperator' },
        { name: 'Municipality' },
        { name: 'OwnerFio' },
        { name: 'PaidContributions' },
        { name: 'CreditContributions' },
        { name: 'CreditPenalty' },
        { name: 'CreditOrg' },
        { name: 'PaidPenalty' },
        { name: 'SubsidySumLocalBud' },
        { name: 'SubsidySumSubjBud' },
        { name: 'SubsidySumFedBud' },
        { name: 'SumAdopt' },
        { name: 'RepaySumAdopt' },
        { name: 'PersAccountNum' }
    ]
});