Ext.define('B4.model.realityobj.housingcommunalservice.Account', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'Apartment' },
        { name: 'PaymentCode' },
        { name: 'ResidentsCount' },
        { name: 'TemporaryGoneCount' },
        { name: 'ApartmentArea' },
        { name: 'LivingArea' },
        { name: 'RoomsCount' },
        { name: 'AccountState' },
        { name: 'HouseStatus' },
        { name: 'Living' },
        { name: 'Privatizied' },
        { name: 'Payment' },
        { name: 'Charged' },
        { name: 'Debt' }
    ]
});