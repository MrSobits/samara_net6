Ext.define('B4.model.otherservice.OtherService', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'OtherService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'Name', defaultValue: null },
        { name: 'Code', defaultValue: null },
        { name: 'UnitMeasure', defaultValue: null },
        { name: 'UnitMeasureName', defaultValue: null },
        { name: 'Tariff', defaultValue: null },
        { name: 'Provider', defaultValue: null },
        { name: 'TemplateId', defaultValue: null },
        { name: 'TemplateName', defaultValue: null }
    ]
});