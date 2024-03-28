Ext.define('B4.model.dict.CriteriaForActualizeVersion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CriteriaForActualizeVersion'
    },
    fields: [
        { name: 'Id' },
        { name: 'CriteriaType' },
        { name: 'ValueFrom' },
        { name: 'ValueTo' },
        { name: 'Points' },
        { name: 'Weight' }
    ]
});