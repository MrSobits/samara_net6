Ext.define('B4.model.BasePlanAction', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePlanAction'
    },
    fields: [
        { name: 'Municipality', defaultValue: null },
        { name: 'TypeBase', defaultValue: 110 },
        { name: 'Plan', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'SubjectName' },
        { name: 'PersonAddress' },
        { name: 'DateStart' },
        { name: 'DateEnd' },
        { name: 'CountDays' },
        { name: 'Requirement' },
        { name: 'State', defaultValue: null },
        { name: 'MoSettlement' },
        { name: 'PlaceName' }
    ]
});