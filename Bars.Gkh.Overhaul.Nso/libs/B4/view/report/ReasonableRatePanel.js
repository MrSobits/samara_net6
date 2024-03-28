Ext.define('B4.view.report.ReasonableRatePanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reasonableratereportpanel',
    layout: {
        type: 'vbox'
    },
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
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'numberfield',
                    name: 'StartYear',
                    fieldLabel: 'Год начала периода',
                    allowBlank: false,
                    allowDecimals: false
                }
            ]
        });

        var yearStart = 0;
        var yearEnd = 0;
        
        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'Params')
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            yearStart = response.data.ProgrammPeriodStart;
            yearEnd = response.data.ProgrammPeriodEnd;
            var nf = me.down('[name=StartYear]');
            nf.setMinValue(yearStart);
            nf.setMaxValue(yearEnd);
            nf.validate();
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });
        

        me.callParent(arguments);
    }
});