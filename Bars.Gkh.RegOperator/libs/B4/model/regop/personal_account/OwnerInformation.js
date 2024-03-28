Ext.define('B4.model.regop.personal_account.OwnerInformation', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PersonalAccountOwnerInformation'
    },

    fields: [
        { name: 'Id' },
        { name: 'DocumentNumber' },
        { name: 'AreaShare' },
        { name: 'StartDate' },
        { name: 'EndDate' },
        { name: 'BasePersonalAccount' },
        { name: 'Owner' },
        { name: 'OwnerName' },
        { name: 'File' }
    ]
});