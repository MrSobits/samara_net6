Ext.define('B4.store.dict.YearCorrection', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.YearCorrection'],
    autoLoad: false,
    model: 'B4.model.dict.YearCorrection',
    sorters: [{
        property: 'Year',
        direction: 'ASC'
    }],
});