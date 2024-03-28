Ext.define('B4.model.regop.Fias', {
    extend: 'B4.model.Fias',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LocationCodeFias'
    }
});