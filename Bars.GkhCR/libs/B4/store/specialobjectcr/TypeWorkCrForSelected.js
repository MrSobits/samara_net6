Ext.define('B4.store.specialobjectcr.TypeWorkCrForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.specialobjectcr.TypeWorkCr'],
    model: 'B4.model.specialobjectcr.TypeWorkCr',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SpecialProtocolCrTypeWork',
        listAction: 'ListProtocolCrTypeWork'
    }
});