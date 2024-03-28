Ext.define('B4.view.import.chesimport.AddressComparingPanel', {
    extend: 'Ext.panel.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.view.import.chesimport.AddressToMatchGrid',
        'B4.view.import.chesimport.RealityObjectGrid'
        
    ],

    title: 'Адреса',
    alias: 'widget.chesimportaddresscomparingpanel',

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
                    xtype: 'chesaddresstomatchgrid',
                    flex: 0.6
                },
                {
                    xtype: 'chesimportrealityobjectgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});