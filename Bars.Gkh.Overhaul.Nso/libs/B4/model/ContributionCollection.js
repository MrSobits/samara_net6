Ext.define('B4.model.ContributionCollection', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContributionCollection'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LongTermPrObject' },
        { name: 'Date' },
        { name: 'PersonalAccount' },
        { name: 'MinContributions' },
        { name: 'AreaOwnerAccount' },
        { name: 'SumMinContributions' },
        { name: 'SetContributions' },
        { name: 'SumSetContributions' },
        { name: 'DifferenceSumContributions' }
    ]
});