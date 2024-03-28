Ext.define('B4.view.objectoutdoorcr.typeworkrealityobjectoutdoor.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,

    alias: 'widget.typeworkrealityobjectoutdoorpanel',

    title: 'Виды работ',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.view.objectoutdoorcr.typeworkrealityobjectoutdoor.Grid',
        'B4.view.objectoutdoorcr.typeworkrealityobjectoutdoor.HistoryGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    name: 'typeworkrealityobjectoutdoortabpanel',
                    enableTabScroll: true,
                    flex: 1,
                    items: [
                        { xtype: 'typeworkrealityobjectoutdoorgrid' },
                        { xtype: 'typeworkrealityobjectoutdoorhistorygrid' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});