Ext.define('B4.model.dict.PaymentPenaltiesBasePersonalAccount', {
    extend: 'B4.base.Model',

    fields: [
        { name: 'Id' },
		{ name: 'Municipality' },
        { name: 'PersonalAccountNum' },
        { name: 'RoomAddress' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'PaymentPenaltiesBasePersonalAccount',
        timeout: 60000
    }
});