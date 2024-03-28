Ext.define('B4.store.dict.RealObjForJurInst', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.JurInstitutionRealObj'],
    autoLoad: false,
    model: 'B4.model.dict.JurInstitutionRealObj',
    proxy: {
        type: 'b4proxy',
        controllerName: 'JurInstitution',
        listAction: 'ListRealObj'
    }
});