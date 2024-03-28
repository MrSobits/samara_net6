Ext.define('B4.model.publicservorg.RealObjPublicServiceOrgService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublicServiceOrgContractService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'StartDate', defaultValue: null },
        { name: 'HeatingSystemType', defaultValue: null },
        { name: 'SchemeConnectionType', defaultValue: null },
        { name: 'CommunalResource', defaultValue: null },
        { name: 'EndDate', defaultValue: null },
        { name: 'Service', defaultValue: null },
        { name: 'ResOrgContract', defaultValue: null },
        { name: 'PlanVolume', defaultValue: null },
        { name: 'ServicePeriod', defaultValue: null },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'TermBillingPaymentNoLaterThan', defaultValue: null },
        { name: 'TermPaymentNoLaterThan', defaultValue: null },
        { name: 'DeadlineInformationOfDebt', defaultValue: null },
        { name: 'DayStart', defaultValue: null },
        { name: 'StartDeviceMetteringIndication', defaultValue: 0 },
        { name: 'DayEnd', defaultValue: null },
        { name: 'EndDeviceMetteringIndication', defaultValue: 0 }
    ]
});