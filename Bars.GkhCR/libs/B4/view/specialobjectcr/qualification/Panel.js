Ext.define('B4.view.specialobjectcr.qualification.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Квалификационный отбор',
    alias: 'widget.specialobjectcrqualificationpanel',
    layout: {
        type: 'border'
    },
    features: [{
        ftype: 'summary'
    }],
    requires: [
        //'B4.view.specialobjectcr.TypeWorkCrGrid',
        'B4.view.specialobjectcr.qualification.Grid',
        'B4.view.specialobjectcr.qualification.TypeWorkCrGrid',
        'B4.ux.grid.feature.GroupingSummaryTotal'
    ],
    initComponent: function () {
        var me = this;
        
        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'specialobjectcrqualtypeworkgrid',
                    region: 'north',
                    height: 350,
                    closable: false
                },
                {
                    xtype: 'specialobjectcrqualgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
