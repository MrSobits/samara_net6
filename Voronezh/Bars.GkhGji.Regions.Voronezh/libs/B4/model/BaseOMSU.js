Ext.define('B4.model.BaseOMSU', {
    extend: 'B4.model.InspectionGji',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseOMSU'
    },
    fields: [
        { name: 'Municipality', defaultValue: null },
        { name: 'Plan', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'InspectionNumber' },
        { name: 'UriRegistrationNumber' },
        { name: 'UriRegistrationDate' },
        { name: 'DateStart' },
        { name: 'CountDays' },
        { name: 'CountHours' },
        { name: 'Reason' },
        { name: 'JurPersonInspectors' },
        { name: 'JurPersonZonalInspections' },
        { name: 'TypeBase', defaultValue: 31 },
        { name: 'TypeBaseOMSU', defaultValue: 10 },
        { name: 'TypeFact', defaultValue: 10 },
        { name: 'TypeForm', defaultValue: 10 },
        { name: 'DisposalNumber' },
        { name: 'InspectorNames' },
        { name: 'OmsuPerson' },
        { name: 'ZonalInspectionNames' },
        { name: 'DateStart' },
        { name: 'State', defaultValue: null },
        { name: 'AnotherReasons' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ControlType', defaultValue: 10 }
    ]
});