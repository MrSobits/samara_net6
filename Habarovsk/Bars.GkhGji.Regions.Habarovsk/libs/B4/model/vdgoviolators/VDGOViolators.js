Ext.define('B4.model.vdgoviolators.VDGOViolators', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VDGOViolators'
    },
    fields: [
        { name: 'Id' },
        { name: 'Contragent' },
        { name: 'Address' },
        { name: 'MinOrgContragent' },
        { name: 'NotificationNumber' },
        { name: 'NotificationDate' },
        { name: 'Description'},
        { name: 'Email'},
        { name: 'FIO'},
        { name: 'PhoneNumber'},
        { name: 'DateExecution'},
        { name: 'MarkOfExecution' },
        { name: 'File' },
        { name: 'NotficationFile' },
        { name: 'MarkOfMessage' }
    ]
});