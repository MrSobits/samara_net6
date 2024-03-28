Ext.define('B4.model.riskorientedmethod.EffectiveKNDIndex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EffectiveKNDIndex'
    },
    fields: [
        { name: 'KindKND'},
        { name: 'YearEnums' },
        { name: 'CurrentIndex' },
        { name: 'Code' },
        { name: 'Name' },
        { name: 'TargetIndex' }
    ]
});