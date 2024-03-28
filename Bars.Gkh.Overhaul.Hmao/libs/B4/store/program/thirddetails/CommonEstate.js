Ext.define('B4.store.program.thirddetails.CommonEstate', {
    extend: 'Ext.data.TreeStore',

    requires: ['B4.model.program.thirddetails.CommonEstate'],

    alias: 'thirdstepdetailsce',
    model: 'B4.model.program.thirddetails.CommonEstate',
    defaultRootProperty: 'Children'
});