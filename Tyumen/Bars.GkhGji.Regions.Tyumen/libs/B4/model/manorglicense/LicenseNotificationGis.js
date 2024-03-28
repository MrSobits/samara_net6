Ext.define('B4.model.manorglicense.LicenseNotificationGis', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LicenseNotification'
     },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ManagingOrgRealityObject' },
        { name: 'LocalGovernment' },
        { name: 'NoticeOMSSendDate' },
        { name: 'RegistredNumber' },
        { name: 'NoticeResivedDate' },
        { name: 'LicenseNotificationNumber' },
        { name: 'OMSNoticeResult' },
        { name: 'OMSNoticeResultNumber' },
        { name: 'OMSNoticeResultDate' },
        { name: 'Comment' },
        { name: 'Contragent' },
        { name: 'MoDateStart' },
        { name: 'RealityObject' },
        { name: 'Municipality' }
    ]
});