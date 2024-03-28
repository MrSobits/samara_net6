Ext.define('B4.store.dict.ProgramCrObj', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ProgramCr'],
    autoLoad: false,
    storeId: 'programCr',
    model: 'B4.model.dict.ProgramCr',
    proxy: {
        autoLoad: false,
        type: 'ajax',
        url: B4.Url.action('List', 'ProgramCr', {
            forObjCr: true
        }),
        reader: {
            type: 'json',
            root: 'data',
            idProperty: 'Id',
            totalProperty: 'totalCount',
            successProperty: 'success',
            messageProperty: 'message'
        }
    }
});