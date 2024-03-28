Ext.define('B4.model.ExistingSolutionsModel', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'ManageStart' },
        { name: 'ManageEnd' },
        { name: 'ManageDecision' },
        { name: 'ManageUo' },
        { name: 'CrFundStart' },
        { name: 'CrFundEnd' },
        { name: 'CrFundDecision' },
        { name: 'OwnerStart' },
        { name: 'OwnerEnd' },
        { name: 'OwnerDecision' },
        { name: 'OwnerContragentType' },
        { name: 'OwnerContragentName' },
        { name: 'MinFundAmount' },
        { name: 'AuthorizedPerson' },
        { name: 'CreditOrg' },
        { name: 'AccountNumber' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectDecisions',
        readAction: 'GetExistingSolutions'
    }
});