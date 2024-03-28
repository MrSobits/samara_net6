Ext.define('B4.model.realityobj.LiftSummary', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectLiftSum'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject', defaultValue: null },
        { name: 'MainCount' },
        { name: 'MainPassenger' },
        { name: 'MainCargo' },
        { name: 'MainMixed' },
        { name: 'Hinged' },
        { name: 'Lowerings' },
        { name: 'MjiCount' },
        { name: 'MjiPassenger' },
        { name: 'MjiCargo' },
        { name: 'MjiMixed' },
        { name: 'Risers' },
        { name: 'ShaftCount' },
        { name: 'BtiCount' },
        { name: 'BtiPassenger' },
        { name: 'BtiCargo' },
        { name: 'BtiMixed' }
    ]
});