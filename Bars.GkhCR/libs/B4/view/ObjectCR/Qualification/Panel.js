Ext.define('B4.view.objectcr.qualification.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Квалификационный отбор',
    alias: 'widget.qualificationpanel',
    layout: {
        type: 'border'
    },
    features: [{
        ftype: 'summary'
    }],
    requires: [
        'B4.view.objectcr.TypeWorkCrGrid',
        'B4.view.objectcr.qualification.Grid',
        'B4.view.objectcr.qualification.TypeWorkCrGrid',
        'B4.ux.grid.feature.GroupingSummaryTotal'
    ],
    initComponent: function () {
        var me = this;
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'qualtypeworkgrid',
                    region: 'north',
                    height: 350,
                    closable: false
                },
                {
                    xtype: 'qualgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
