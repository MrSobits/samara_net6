Ext.define('B4.model.manorglicense.Request', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgLicenseRequest'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'ContragentMunicipality' },
        { name: 'DateRequest' },
        { name: 'RegisterNumber' },
        { name: 'RegisterNum' },
        { name: 'ConfirmationOfDuty' },
        { name: 'ReasonOffers' },
        { name: 'File' },
        { name: 'ReplyTo' },
        { name: 'RPGUNumber' },
        { name: 'ObjectCreateDate' },
        { name: 'DeclineReason' },
        { name: 'MessageId' },
        { name: 'ReasonRefusal' },
        { name: 'State' },
        { name: 'OfficialsCount', defaultValue: null }
    ]
});