Ext.define('B4.view.administration.risdataexport.DpkrWorksPanel',
{
    extend: 'Ext.panel.Panel',
    alias: 'widget.risdataexportdpkrworkspanel',

    requires: [
        'B4.view.administration.risdataexport.DpkrWorksGrid'
    ],

    bodyStyle: Gkh.bodyStyle,
    
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function () {
        var me = this;

        Ext.apply(me,
        {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            xtype: 'risdataexportdpkrworksgrid',
                            name: 'DpkrWorksGrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});