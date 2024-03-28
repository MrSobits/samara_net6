Ext.define('B4.model.publicservorg.ContractTempGraph', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicServiceOrgTemperatureInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OutdoorAirTemp', defaultValue: null },
        { name: 'CoolantTempSupplyPipeline', defaultValue: null },
        { name: 'CoolantTempReturnPipeline', defaultValue: null },
        { name: 'Contract', defaultValue: null }
    ]
});