Ext.define('B4.model.realityobj.PublicServiceOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicServiceOrgContractRealObj',
        listAction: 'ListByRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject' },
        { name: 'PublicServiceOrg' },
        { name: 'PublicServiceOrgId' },
        { name: 'ContractId' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});