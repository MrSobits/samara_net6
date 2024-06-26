﻿Ext.define('B4.model.smev.ERKNM', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ERKNM'
    },
    fields: [
        { name: 'MessageId' },
        { name: 'RequestDate' },
        { name: 'CarryoutEvents' },
        { name: 'Answer' },
        { name: 'Goals' },
        { name: 'InspectionName' },
        { name: 'checkId' },
        { name: 'KindKND' },
        { name: 'ERPAddressType' },
        { name: 'ERPInspectionType' },
        { name: 'ERPNoticeType',defaultValue: 40 },
        { name: 'ERPObjectType' },
        { name: 'ERPReasonType' },
        { name: 'ERPRiskType' },
        { name: 'GisErpRequestType', defaultValue: 1 },
        { name: 'OKATO' },
        { name: 'ERPID' },
        { name: 'RequestState' },
        { name: 'SubjectAddress' },
        { name: 'Inspector' },
        { name: 'Disposal' },
        { name: 'ACT_DATE_CREATE' },
        { name: 'REPRESENTATIVE_FULL_NAME' },
        { name: 'REPRESENTATIVE_POSITION' },
        { name: 'START_DATE' },
        { name: 'DURATION_HOURS' },
        { name: 'HasViolations' },
        { name: 'NeedToUpdate' },
        { name: 'ProsecutorOffice' }
    ]
});