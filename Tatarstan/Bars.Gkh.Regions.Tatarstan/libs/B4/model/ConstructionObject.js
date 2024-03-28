Ext.define('B4.model.ConstructionObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'constructionobject',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'Settlement', defaultValue: null },
        { name: 'FiasAddress' },
        { name: 'Address' },
        { name: 'Description' },
        { name: 'State' },
        { name: 'SumSmr' },
        { name: 'SumDevolopmentPsd' },
        { name: 'DateEndBuilder' },
        { name: 'DateStartWork' },
        { name: 'DateStopWork' },
        { name: 'DateResumeWork' },
        { name: 'ReasonStopWork' },
        { name: 'DateCommissioning' },
        { name: 'LimitOnHouse' },
        { name: 'TotalArea' },
        { name: 'NumberApartments' },
        { name: 'NumberEntrances' },
        { name: 'ResettleProgNumberApartments' },
        { name: 'NumberFloors' },
        { name: 'NumberLifts' },
        { name: 'RoofingMaterial', defaultValue: null },
        { name: 'TypeRoof', defaultValue: 10 },
        { name: 'WallMaterial', defaultValue: null },
        { name: 'MonitoringSmrId', defaultValue: null },
        { name: 'MonitoringSmrState', defaultValue: null },
        { name: 'ResettlementProgram', defaultValue: null }
    ]
});