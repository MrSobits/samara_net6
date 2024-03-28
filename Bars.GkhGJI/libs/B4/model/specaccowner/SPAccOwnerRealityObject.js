Ext.define('B4.model.specaccowner.SPAccOwnerRealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SPAccOwnerRealityObject'
    },
    fields: [
        { name: 'Id' },
        { name: 'CreditOrg' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'RealityObject' },
        { name: 'SpacAccNumber' },
        { name: 'Municipality' },
        { name: 'SpecialAccountOwner' }
    ]
});