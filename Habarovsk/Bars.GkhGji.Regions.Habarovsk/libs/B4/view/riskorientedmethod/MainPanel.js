Ext.define('B4.view.riskorientedmethod.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр категорий риска',
    alias: 'widget.riskorientedmethodMainPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.riskorientedmethod.ROMCategoryGrid',
        'B4.view.riskorientedmethod.ROMCategoryFilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'romcategoryfilterpanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6'
                },
                {
                    xtype: 'romcategorygrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
