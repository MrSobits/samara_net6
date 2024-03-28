Ext.define('B4.model.BusinessActivity', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BusinessActivity'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentName' },
        { name: 'TypeKindActivity' },
        { name: 'IncomingNotificationNum' },
        { name: 'DateBegin', defaultValue: null },
        { name: 'DateRegistration', defaultValue: null },
        { name: 'DateNotification', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'IsNotBuisnes', defaultValue: false },
        { name: 'AcceptedOrganization' },
        { name: 'RegNum' },
        { name: 'IsOriginal', defaultValue: false },
        { name: 'HasFile', defaultValue: false },
        { name: 'State' },
        { name: 'ContragentInn' },
        { name: 'ContragentOgrn' },
        { name: 'ContragentMailingAddress' },
        { name: 'OrgFormName' },
        { name: 'MunicipalityName' },
        { name: 'ServiceCount' },
        { name: 'Registered' },
        { name: 'RegNumDateYear', defaultValue: null }
    ]
});