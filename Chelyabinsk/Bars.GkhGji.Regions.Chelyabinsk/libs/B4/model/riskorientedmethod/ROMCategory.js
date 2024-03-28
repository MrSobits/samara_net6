Ext.define('B4.model.riskorientedmethod.ROMCategory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ROMCategory'
    },
    fields: [
        { name: 'Id' },
        { name: 'KindKND'},
        { name: 'YearEnums' },
        { name: 'SeverityGroup', defaultValue: 0},
        { name: 'ProbabilityGroup', defaultValue: 0},
        { name: 'Contragent' },
        { name: 'ContragentINN' },
        { name: 'Vp' },
        { name: 'Vn' },
        { name: 'Vpr' },
        { name: 'MonthCount' },
        { name: 'Ogrn' },
        { name: 'JuridicalAddress' },
        { name: 'MkdAreaTotal' },
        { name: 'Result' },
        { name: 'RiskCategory' },
        { name: 'CalcDate' },
        { name: 'Inspector' },
        { name: 'State' }
    ]
});