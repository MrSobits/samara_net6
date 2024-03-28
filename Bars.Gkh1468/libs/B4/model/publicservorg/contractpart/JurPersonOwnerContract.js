Ext.define('B4.model.publicservorg.contractpart.JurPersonOwnerContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'JurPersonOwnerContract',
        writer: {
            type: 'b4writer',
            writeAllFields: true
        }
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PublicServiceOrgContract', defaultValue: null },
        { name: 'TypeContractPart', defaultValue: null },
        { name: 'TypeContactPerson', defaultValue: null },
        { name: 'TypeOwnerContract', defaultValue: null },
        { name: 'Contragent', defaultValue: null }
    ]
});