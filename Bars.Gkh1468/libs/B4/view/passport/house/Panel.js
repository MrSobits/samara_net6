Ext.define('B4.view.passport.house.Panel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.housepanel',
    closable: true,

    title: 'Паспорт',

    objectId: null,

    requires: [
        'B4.view.passport.house.info.Grid',
        'B4.view.passport.house.info.PassportPanel'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'infohousepassportgrid'
                },
                {
                    xtype: 'infohousepassportpanel'
                }
            ]
        });

        me.callParent(arguments);
    }
});