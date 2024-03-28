Ext.define('B4.view.manorglicense.RequestListPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.manorglicenserequestlistpanel',
    closable: true,
    minWidth: 700,
    layout: {
        type: 'hbox',
        align: 'stretch'
    },
    title: 'Реестр заявлений на выдачу лицензии',
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.view.manorglicense.RequestListGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'manorglicenserequestlistgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});