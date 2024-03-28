Ext.define('B4.store.dict.ProgramCrAon', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ProgramCr'],
    autoLoad: false,
    storeId: 'programCrForSelect',
    model: 'B4.model.dict.ProgramCr',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramCr',
        listAction: 'GetAonProgramsList'
    }
});