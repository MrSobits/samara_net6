Ext.define('B4.view.passport.oki.Panel', {
    extend: 'Ext.tab.Panel',
    alias: 'widget.okipanel',
    closable: true,

    title: 'Паспорт',

    objectId: null,

    requires: [
        'B4.view.passport.oki.info.Grid',
        'B4.view.passport.oki.info.PassportPanel'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'infookipassportgrid'
                },
                {
                    xtype: 'infookipassportpanel'
                }
            ]
        });

        me.callParent(arguments);
    }
});