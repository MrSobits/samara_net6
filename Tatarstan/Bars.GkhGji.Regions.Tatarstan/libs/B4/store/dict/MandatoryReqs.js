Ext.define('B4.store.dict.MandatoryReqs', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.MandatoryReqs'],
    autoLoad: false,
    storeId: 'mandatoryReqsStore',
    model: 'B4.model.dict.MandatoryReqs'
});