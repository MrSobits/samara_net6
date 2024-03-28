Ext.define('B4.model.DocumentPhysInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'DocumentPhysInfo'
    },
    fields: [
        { name: 'Id', defaultValue: null },
        { name: 'PhysJob', defaultValue: null },
        { name: 'PhysPosition', defaultValue: null },
        { name: 'PhysBirthdayAndPlace', defaultValue: null },
        { name: 'PhysIdentityDoc', defaultValue: null },
        { name: 'TypeGender', defaultValue: 0 }
    ]
});