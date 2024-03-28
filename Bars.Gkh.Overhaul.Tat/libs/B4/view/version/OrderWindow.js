Ext.define('B4.view.version.OrderWindow', {
    extend: 'Ext.window.Window',
    requires: [
        'B4.view.version.VersionParamsGrid'
    ],
    alias: 'widget.versionorderwin',
    title: 'Очередность',
    modal: true,
    width: 500,
    height: 300,
    layout: {
        align: 'stretch',
        type: 'fit'
    },
    closable: true,
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            flex: 1,
            items: [
                {
                    xtype: 'versionparamsgrid',
                    margins: -1
                }
            ]
        });

        me.callParent(arguments);
    }
});