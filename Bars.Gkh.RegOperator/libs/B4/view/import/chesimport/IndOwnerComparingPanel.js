Ext.define('B4.view.import.chesimport.IndOwnerComparingPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.import.chesimport.IndOwnerToMatchGrid',
        'B4.view.import.chesimport.IndAccountOwnerGrid'
    ],

    title: 'Физические лица',
    alias: 'widget.chesimportindownercomparingpanel',
    panelSelector: 'comparingPanel',

    bodyStyle: Gkh.bodyStyle,
    layout: { type: 'hbox', align: 'stretch' },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                margin: 1
            },
            items: [
                {
                    xtype: 'chesimportindownertomatchgrid',
                    name: 'MatchGrid',
                    flex: .6
                },
                {
                    xtype: 'chesimportindaccountownergrid',
                    name: 'ComparingGrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});