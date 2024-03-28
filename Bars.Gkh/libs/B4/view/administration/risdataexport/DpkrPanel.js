Ext.define('B4.view.administration.risdataexport.DpkrPanel',
{
    extend: 'Ext.panel.Panel',
    alias: 'widget.risdataexportdpkrpanel',

    requires: [
        'B4.view.administration.risdataexport.DpkrGrid',
        'B4.view.administration.risdataexport.DpkrDocumentGrid'
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
                            xtype: 'risdataexportdpkrgrid',
                            name: 'DpkrGrid'
                        },
                        {
                            xtype: 'risdataexportdpkrdocumentgrid',
                            name: 'DpkrDocumentGrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});