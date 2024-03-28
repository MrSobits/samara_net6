Ext.define('B4.store.specialobjectcr.PerformedWorkActForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.specialobjectcr.PerformedWorkAct'],
    autoLoad: false,
    model: 'B4.model.specialobjectcr.PerformedWorkAct',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialPerformedWorkAct',
        listAction: 'ListByActiveNewOpenPrograms'
    }
});