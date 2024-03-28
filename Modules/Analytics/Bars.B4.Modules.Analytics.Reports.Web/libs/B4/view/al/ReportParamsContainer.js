Ext.define('B4.view.al.ReportParamsContainer', {
    extend: 'Ext.container.Container',
    requires: ['B4.helpers.al.ReportParamFieldBuilder'],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function() {
        var me = this;
        me.callParent(arguments);
    }
});