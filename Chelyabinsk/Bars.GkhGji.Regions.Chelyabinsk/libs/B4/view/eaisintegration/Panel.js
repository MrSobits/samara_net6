Ext.define('B4.view.eaisintegration.Panel',{
    extend: 'Ext.panel.Panel',

    alias: 'widget.eaisintegrationpanel',

    requires: [
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.view.eaisintegration.Grid',
    ],

    title: 'Результаты обмена данными с ЕАИС',

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll: true,

    layout: 'fit',

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'eaisintegrationgrid'
                }
            ]
        });
        me.callParent(arguments);
    }
});