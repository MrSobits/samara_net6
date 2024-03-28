Ext.define('B4.controller.networkoperator.NetworkOperator', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['networkoperator.NetworkOperator'],
    stores: ['networkoperator.NetworkOperator'],
    views: [
        'networkoperator.Grid',
        'networkoperator.EditWindow'
    ],

    mainView: 'networkoperator.Grid',
    mainViewSelector: 'networkoperatorgrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'networkOperatorGridWindowAspect',
            gridSelector: 'networkoperatorgrid',
            editFormSelector: 'networkoperatoreditwindow',
            storeName: 'networkoperator.NetworkOperator',
            modelName: 'networkoperator.NetworkOperator',
            editWindowView: 'networkoperator.EditWindow'
        }
    ],
    refs: [
       { ref: 'mainView', selector: 'networkoperatorgrid' }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('networkoperatorgrid');

        me.bindContext(view);
        me.application.deployView(view);

        me.getStore('networkoperator.NetworkOperator').load();
    },

    init: function () {
        var me = this;
        me.callParent(arguments);
    }
});