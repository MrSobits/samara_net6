Ext.define('B4.store.QualificationRegister', {
    extend: 'B4.base.Store',
    requires: ['B4.model.QualificationRegister'],
    autoLoad: false,
    storeId: 'qualificationRegisterStore',
    model: 'B4.model.QualificationRegister',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Qualification',
        listAction: 'ListView'
    }
});