Ext.define('B4.model.chargessplitting.fuelenergyresrc.FuelEnergyOrgContractDetail', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FuelEnergyOrgContractDetail',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'PeriodSummId' },
        { name: 'Municipality' },
        { name: 'PublicServiceOrg' },
        { name: 'Service' },
        { name: 'Charged' },
        { name: 'Paid' },
        { name: 'Debt' },
        { name: 'GasEnergyPercents' },
        { name: 'ElectricityEnergyPercents' },
        { name: 'PlanPayGas' },
        { name: 'PlanPayElectricity' }
    ]
});