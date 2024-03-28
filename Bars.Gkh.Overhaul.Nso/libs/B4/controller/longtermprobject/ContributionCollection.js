Ext.define('B4.controller.longtermprobject.ContributionCollection', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    models: ['ContributionCollection'],
    stores: ['ContributionCollection'],

    views: [
        'longtermprobject.contributioncollection.Grid',
        'longtermprobject.contributioncollection.EditWindow'
    ],

    mainView: 'longtermprobject.contributioncollection.Grid',
    mainViewSelector: 'contributioncollectiongrid',

    refs: [
        { ref: 'editWindow', selector: 'contributioncollectioneditwin' }
    ],
    
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'contributionCollectionGridWindowAspect',
            gridSelector: 'contributioncollectiongrid',
            editFormSelector: 'contributioncollectioneditwin',
            storeName: 'ContributionCollection',
            modelName: 'ContributionCollection',
            editWindowView: 'longtermprobject.contributioncollection.EditWindow',
            listeners: {
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.data.LongTermPrObject = { Id: asp.controller.params.longTermObjId };
                    }
                    return true;
                }
            }
        }
    ],
    
    init: function () {
        this.getStore('ContributionCollection').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },
    
    onBeforeLoad: function (store, operation) {
        operation.params.longTermObjId = this.params.longTermObjId;
    },
    
    onLaunch: function () {
        this.getMainView().getStore().load();
    }
});