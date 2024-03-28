Ext.define('B4.model.builder.Workforce', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderWorkforce'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Builder', defaultValue: null },
        { name: 'Specialty', defaultValue: null },
        { name: 'SpecialtyName' },
        { name: 'Institutions', defaultValue: null },
        { name: 'InstitutionsName' },
        { name: 'DocumentNum' },
        { name: 'DocumentDate' },
        { name: 'DocumentQualification' },
        { name: 'EmploymentDate' },
        { name: 'Fio' },
        { name: 'Position' },
        { name: 'File', defaultValue: null }
    ]
});