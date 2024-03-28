Ext.define('B4.view.objectcr.MonitoringSmrEditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 500,
    bodyPadding: 5,
    trackResetOnLoad: true,
    autoScroll: true,
    
    title: 'Мониторинг СМР',
    alias: 'widget.monitoringsmrpanel',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});