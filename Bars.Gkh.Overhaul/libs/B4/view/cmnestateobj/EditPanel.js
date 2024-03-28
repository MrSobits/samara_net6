Ext.define('B4.view.cmnestateobj.EditPanel', {
    extend: 'Ext.tab.Panel',

    alias: 'widget.cmnestateobjeditpanel',
    
    closable: true,
    
    objectId: null,
    
    requires: [
        'B4.view.cmnestateobj.MainInfoPanel',
        'B4.view.cmnestateobj.GroupPanel'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'cmnestateobjmaininfo'
                },
                {
                    xtype: 'grouppanel',
                    disabled: true
                }
            ]
        });

        me.callParent(arguments);
    }
});