Ext.define('B4.model.riskorientedmethod.ROMCategory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ROMCategory'
    },
    fields: [
        { name: 'KindKND'},
        { name: 'YearEnums' },
        { name: 'Contragent' },
        { name: 'ContragentINN' },
        { name: 'Vp' },
        { name: 'Vn' },
        { name: 'Vpr' },
        { name: 'MonthCount' },
        { name: 'MkdAreaTotal' },
        { name: 'Result' },
        { name: 'RiskCategory' },
        { name: 'CalcDate' },
        { name: 'Inspector' },
        { name: 'State' }
    ]
});