Ext.define('B4.model.PersonalAccount', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisPersonalAccount'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Uic' },
        { name: 'AccountId' },
        { name: 'RealityObjectId' },
        { name: 'HouseId' },
        { name: 'PSS' },
        { name: 'PaymentCode' },
        { name: 'ApartmentNumber' },
        { name: 'Prefix' },
        { name: 'RoomNumber' },
        { name: 'IsOpen' }
    ]
});