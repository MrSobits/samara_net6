Ext.define('B4.view.report.ProgramCrByDpkrForm2Panel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportprogcrbydpkrform2panel',
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
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все'
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

        var startYear = 0;
        var endYear = 0;

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'Params')
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            startYear = response.data.ProgrammPeriodStart;
            endYear = response.data.ProgrammPeriodEnd;
            var start = me.down('[name=StartYear]');
            start.setMinValue(startYear);
            start.setValue(startYear);
            start.setMaxValue(endYear);
            start.validate();
            var end = me.down('[name=EndYear]');
            end.setMinValue(startYear);
            end.setValue(endYear);
            end.setMaxValue(endYear);
            end.validate();
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });

        me.callParent(arguments);
    }
});