Ext.define('B4.controller.longtermprobject.OverhaulFund', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    models: ['realityobj.housingcommunalservice.AccountCharge'],
    stores: ['longtermprobject.AccountCharge'],

    views: [
        'longtermprobject.overhaulfund.Grid'
    ],

    mainView: 'longtermprobject.overhaulfund.Grid',
    mainViewSelector: 'overhaulfundgrid',
    
    init: function () {
        this.getStore('longtermprobject.AccountCharge').on('beforeload', this.onBeforeLoad, this);

        this.control({
            'overhaulfundgrid b4updatebutton': { click: { fn: this.updateGrid, scope: this } }
        });

        this.callParent(arguments);
    },
    
    updateGrid: function () {
        this.getMainView().getStore().load();
    },
    
    onBeforeLoad: function (store, operation) {
        operation.params.longTermObjId = this.params.longTermObjId;
    },
    
    onLaunch: function () {
        this.getMainView().getStore().load();
    }
});