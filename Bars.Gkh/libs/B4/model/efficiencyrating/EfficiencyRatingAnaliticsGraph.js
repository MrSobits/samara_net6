Ext.define('B4.model.efficiencyrating.EfficiencyRatingAnaliticsGraph', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EfficiencyRatingAnaliticsGraph',
        updateAction: 'SaveOrUpdateGraph',
        createAction: 'SaveOrUpdateGraph',
        writer: {
            type: 'b4writer',
            writeAllFields: true
        }
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'AnaliticsLevel' },
        { name: 'Category' },
        { name: 'ViewParam' },
        { name: 'FactorCode' },
        { name: 'DiagramType' },
        { name: 'ManagingOrganizations' },
        { name: 'Periods' },
        { name: 'Municipalities' },
        { name: 'DiagramType' },
        { name: 'Data' }
    ]
});