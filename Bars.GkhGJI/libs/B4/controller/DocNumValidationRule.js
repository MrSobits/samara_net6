Ext.define('B4.controller.DocNumValidationRule', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'DocNumValidationRule'
    ],

    stores: [
        'DocNumValidationRule'
    ],

    views: [
        'docnumvalidationrule.Grid',
        'docnumvalidationrule.EditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'docnumvalidationrulegrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'docnumvalidationrulegridwindow',
            gridSelector: 'docnumvalidationrulegrid',
            editFormSelector: '#docnumvalidationruleeditwindow',
            storeName: 'DocNumValidationRule',
            modelName: 'DocNumValidationRule',
            editWindowView: 'docnumvalidationrule.EditWindow'
        }
    ],
    
    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('docnumvalidationrulegrid');
        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('DocNumValidationRule').load();
    }
});