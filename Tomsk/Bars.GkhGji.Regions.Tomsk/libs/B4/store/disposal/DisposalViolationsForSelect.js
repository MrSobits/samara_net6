Ext.define('B4.store.disposal.DisposalViolationsForSelect', {
    extend: 'B4.base.Store',
    storeId: 'disposalViolationsForSelect',
    fields: ['Id', 'Name', 'MunicipalityName', 'Address', 'DatePlanRemoval', 'Description'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalViolations',
        listAction: 'GetListViolations'
    }
});