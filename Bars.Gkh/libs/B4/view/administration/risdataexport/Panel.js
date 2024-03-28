Ext.define('B4.view.administration.risdataexport.Panel',
{
    extend: 'Ext.panel.Panel',
    alias: 'widget.risdataexportpanel',

    requires: [
        'B4.view.administration.risdataexport.ExportResultGrid',
        'B4.view.administration.risdataexport.ExportTaskGrid',
        'B4.view.administration.risdataexport.ExportInfoGrid'
    ],

    title: 'Экспорт данных системы в РИС ЖКХ',

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll: true,

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
                            xtype: 'risdataexporttaskgrid'
                        },
                        {
                            xtype: 'risdataexportresultgrid'
                        },
                        {
                            xtype: 'risdataexportinfogrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});