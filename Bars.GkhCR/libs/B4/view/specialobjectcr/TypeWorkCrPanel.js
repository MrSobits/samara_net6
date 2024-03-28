Ext.define('B4.view.specialobjectcr.TypeWorkCrPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,

    alias: 'widget.typeworkspecialcrpanel',
    
    title: 'Виды работ',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.specialobjectcr.TypeWorkCrGrid',
        'B4.view.specialobjectcr.TypeWorkCrHistoryGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    enableTabScroll: true,
                    flex: 1,
                    items: [
                        { xtype: 'typeworkspecialcrgrid' },
                        { xtype: 'typeworkspecialcrhistorygrid' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});