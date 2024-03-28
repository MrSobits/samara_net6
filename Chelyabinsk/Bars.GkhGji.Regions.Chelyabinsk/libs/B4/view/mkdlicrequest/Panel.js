Ext.define('B4.view.mkdlicrequest.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Внесение изменений в реестр лицензий',
    alias: 'widget.mkdLicRequestPanel',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'border'
    },
    requires: [
        'B4.view.mkdlicrequest.FilterPanel',
        'B4.view.mkdlicrequest.Grid'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'mkdLicRequestFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    frame: true,
                    padding: 2,
                    bodyStyle: 'text-align:center; font: 14px tahoma,arial,helvetica,sans-serif;'
                },
                {
                    xtype: 'mkdLicRequestGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
