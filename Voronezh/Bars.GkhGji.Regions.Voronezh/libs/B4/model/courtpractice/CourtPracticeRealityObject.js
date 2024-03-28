Ext.define('B4.model.courtpractice.CourtPracticeRealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPracticeRealityObject'
    },
    fields: [
        { name: 'Id' },
        { name: 'CourtPractice'},
        { name: 'RealityObject'},
        { name: 'Municipality'}
       
    ]
});