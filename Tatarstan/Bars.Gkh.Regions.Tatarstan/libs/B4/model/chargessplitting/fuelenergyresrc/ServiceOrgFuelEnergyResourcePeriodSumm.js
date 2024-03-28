Ext.define('B4.model.chargessplitting.fuelenergyresrc.ServiceOrgFuelEnergyResourcePeriodSumm', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ServiceOrgFuelEnergyResourcePeriodSumm',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'PublicServiceOrg' },
        { name: 'Period' },
        { name: 'Charged' },
        { name: 'Paid' },
        { name: 'Debt' },
        { name: 'PlanPayGas' },
        { name: 'PlanPayElectricity' }
    ]
});