Ext.define('B4.view.program.EditOrderWindow', {
    extend: 'Ext.window.Window',
    requires: [
        'B4.view.program.CurrentPriorityGrid'
    ],
    alias: 'widget.editorderwin',
    title: 'Очередность',
    modal: true,
    closeAction: 'destroy',
    width: 500,
    height: 300,
    layout: {
        align: 'stretch',
        type: 'fit'
    },
    closable: false,
    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            flex: 1,
            items: [
                {
                    xtype: 'currprioritygrid',
                    margins: -1
                }
            ]
        });

        me.callParent(arguments);
    }
});