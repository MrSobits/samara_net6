Ext.define('B4.model.violationgroup.ViolationGroup', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentViolGroup'
    },
    fields: [
        { name: 'Id', useNull: true }, 
        { name: 'Document' },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'PointCodes' },
        { name: 'PointIds' },
        { name: 'Points' },
        { name: 'Description' },
        { name: 'Action' },
        { name: 'DatePlanRemoval' },
        { name: 'DateFactRemoval' },
        { name: 'NormDocNums' }
    ]
});