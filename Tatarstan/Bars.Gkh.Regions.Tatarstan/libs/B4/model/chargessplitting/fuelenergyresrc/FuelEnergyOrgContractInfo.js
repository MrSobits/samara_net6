Ext.define('B4.model.chargessplitting.fuelenergyresrc.FuelEnergyOrgContractInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FuelEnergyOrgContractInfo',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'FuelEnergyOrg' },
        { name: 'CommunalResource' },
        { name: 'SaldoIn' },
        { name: 'SaldoOut' },
        { name: 'DebtIn' },
        { name: 'Charged' },
        { name: 'Paid' },
        { name: 'DebtOut' },
        { name: 'PlanPaid' },
        { name: 'PaidDelta' }
    ]
});