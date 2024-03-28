Ext.define('B4.store.administration.massdelete.RealityObject', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: [
        { name: 'Id' },
        { name: 'Address' },
        { name: 'Municipality' }
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectStructuralElement',
        listAction: 'ListRoForMassDelete'
    }
});