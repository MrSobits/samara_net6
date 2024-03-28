Ext.define('B4.model.publicservorg.contractpart.OfferContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseContractPart'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PublicServiceOrgContract', defaultValue: null },
        { name: 'TypeContractPart', defaultValue: null }
    ]
});