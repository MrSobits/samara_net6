Ext.define('B4.model.InterdepartmentalRequestsDTO', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'DepartmentlRequestsDTO',
        listAction: 'GetListInterdepartmentalRequests'
    },
    fields: [
        { name: 'Id' },
        { name: 'Date'},
        { name: 'Number'},
        { name: 'Department'},
        { name: 'Answer' },
        { name: 'NameOfInterdepartmentalDepartment' },        
        { name: 'Inspector' },
        { name: 'RequestState' },
        { name: 'FrontControllerName' },
        { name: 'FrontModelName' }
    ]
});