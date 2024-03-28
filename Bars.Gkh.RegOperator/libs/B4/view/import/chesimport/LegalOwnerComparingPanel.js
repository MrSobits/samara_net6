Ext.define('B4.view.import.chesimport.LegalOwnerComparingPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.view.import.chesimport.LegalOwnerToMatchGrid',
        'B4.view.import.chesimport.LegalAccountOwnerGrid'
    ],

    title: 'Юридические лица',
    alias: 'widget.chesimportlegalownercomparingpanel',
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
                    xtype: 'chesimportlegalownertomatchgrid',
                    name: 'MatchGrid',
                    flex: .6
                },
                {
                    xtype: 'chesimportlegalaccountownergrid',
                    name: 'ComparingGrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});