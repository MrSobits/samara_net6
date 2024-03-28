Ext.define('B4.view.report.PublishedProgramReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportPublishedProgramReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: ['B4.view.Control.GkhTriggerField'],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipality',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'numberfield',
                    name: 'StartYear',
                    fieldLabel: 'Год начала периода',
                    allowBlank: false,
                    allowDecimals: false
                },
                {
                    xtype: 'numberfield',
                    name: 'EndYear',
                    fieldLabel: 'Год окончания периода',
                    allowBlank: false,
                    allowDecimals: false
                }
            ]
        });

        me.callParent(arguments);

        var startYear = 0;
        var endYear = 0;

        Ext.iterate(Gkh.config.Overhaul, function (k, v) { startYear = v["ProgrammPeriodStart"] });
        Ext.iterate(Gkh.config.Overhaul, function (k, v) { endYear = v["ProgrammPeriodEnd"] });

        var start = me.down('[name=StartYear]');
        start.setMinValue(startYear);
        start.setMaxValue(endYear);
        start.validate();
        var end = me.down('[name=EndYear]');
        end.setMinValue(startYear);
        end.setMaxValue(endYear);
        end.validate();
    }
});