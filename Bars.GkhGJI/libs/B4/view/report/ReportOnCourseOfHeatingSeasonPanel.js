Ext.define('B4.view.report.ReportOnCourseOfHeatingSeasonPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'ReportOnCourseOfHeatingSeasonPanel',
    layout: { type: 'vbox' },

    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox'
    ],
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            
            items: [
                {
                    xtype: 'datefield',
                    name: 'dateStart',
                    itemId: 'dfdateStart',
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    allowBlank : false
                },
                
                {
                    xtype: 'datefield',
                    name: 'dateEnd',
                    itemId: 'dfdateEnd',
                    fieldLabel: 'Окончание период',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipalities',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                    
                }
            ]
        });

        me.callParent(arguments);
    }
});