Ext.define('B4.model.publicservorg.contractpart.FuelEnergyResourceContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FuelEnergyResourceContract',
        writer: {
            type: 'b4writer',
            writeAllFields: true
        }
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PublicServiceOrgContract', defaultValue: null },
        { name: 'TypeContractPart', defaultValue: null },
        { name: 'FuelEnergyResourceOrg', defaultValue: null }
    ]
});