Ext.define('B4.controller.RosRegExtractBig', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.form.ComboBox',
         'B4.aspects.GridEditWindow',
        'B4.aspects.FieldRequirementAspect'
    ],

    models: ['RosRegExtractBig',
             'RosRegExtractBigOwner'],
    stores: ['RosRegExtractBig',
             'RosRegExtractBigOwner'],
    views: [
        'rosregextract.Grid',
        'rosregextract.EditWindow',
        'rosregextract.OwnerGrid'],
    parentId: null,

    mixins: {
        context: 'B4.mixins.Context'
    },

    mainView: 'rosregextract.Grid',
    mainViewSelector: 'rosregextractgrid',
    editWindowSelector: 'rosregextractEditWindow',
    aspects: [

    {
        xtype: 'grideditwindowaspect',
        name: 'rosregextractGridWindowAspect',
        gridSelector: 'rosregextractgrid',
        editFormSelector: '#rosregextractEditWindow',
        storeName: 'RosRegExtractBig',
        modelName: 'RosRegExtractBig',
        editWindowView: 'rosregextract.EditWindow',
        otherActions: function (actions) {
            actions['#rosregextractEditWindow #sfPersAcc'] = { 'beforeload': { fn: this.onBeforeLoadPersAcc, scope: this } };
        },
        onBeforeLoadPersAcc: function (store, operation) {
            debugger;
            operation = operation || {};
            operation.params = operation.params || {};

            operation.params.parentId = parentId;
        },
        listeners: {
            aftersetformdata: function (asp, rec, form) {
                var me = this;
                //debugger;
                parentId = rec.getId();
                //   me.controller.getAspect('admonitionPrintAspect').loadReportStore();
                var grid = form.down('rosregextractownergrid'),
                store = grid.getStore();
                store.filter('parentId', rec.getId());
                /*
                var grid2 = form.down('rosregextractorggrid'),
                store2 = grid2.getStore();
                store2.filter('parentId', rec.getId());

                var grid3 = form.down('rosregextractgovgrid'),
                store3 = grid3.getStore();
                store3.filter('parentId', rec.getId());*/
            }
        }
  
    }
],


    refs: [
        {
            ref: 'mainView',
            selector: 'rosregextractgrid'
        }
    ],


    init: function () {
        var me = this;
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('rosregextractgrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});