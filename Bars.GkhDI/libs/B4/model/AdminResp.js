Ext.define('B4.model.AdminResp', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AdminResp'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfo', defaultValue: null },
        { name: 'SupervisoryOrg', defaultValue: null },
        { name: 'SupervisoryOrgName' },
        { name: 'AmountViolation', defaultValue: null },
        { name: 'SumPenalty', defaultValue: null },
        { name: 'DatePaymentPenalty', defaultValue: null },
        { name: 'DateImpositionPenalty', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'TypeViolation' },
        { name: 'TypePerson', defaultValue: 10 },
        { name: 'Fio' },
        { name: 'Position' },
        { name: 'DocumentName' },
        { name: 'DocumentNum' },
        { name: 'DateFrom' }
    ]
});