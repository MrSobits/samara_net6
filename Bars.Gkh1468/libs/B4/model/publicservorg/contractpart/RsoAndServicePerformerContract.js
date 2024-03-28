Ext.define('B4.model.publicservorg.contractpart.RsoAndServicePerformerContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RsoAndServicePerformerContract',
        writer: {
            type: 'b4writer',
            writeAllFields: true
        }
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PublicServiceOrgContract', defaultValue: null },
        { name: 'TypeContractPart', defaultValue: null },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'CommercialMeteringResourceType', defaultValue: null }
    ]
});