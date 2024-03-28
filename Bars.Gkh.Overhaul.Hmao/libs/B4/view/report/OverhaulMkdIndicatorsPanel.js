Ext.define('B4.view.report.OverhaulMkdIndicatorsPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.overhaulmkdindicatorspanel',
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
                    emptyText: 'Все'
                },
                {
                    xtype: 'numberfield',
                    name: 'StartYear',
                    itemId: 'numStartYear',
                    fieldLabel: 'Год начала периода',
                    allowBlank: false,
                    allowDecimals: false
                },
                {
                    xtype: 'numberfield',
                    name: 'FinishYear',
                    itemId: 'numFinishYear',
                    fieldLabel: 'Год окончания периода',
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
            var nf2 = me.down('[name=FinishYear]');
            nf.setMinValue(yearStart);
            nf.setMaxValue(yearEnd);
            nf2.setMinValue(yearStart);
            nf2.setMaxValue(yearEnd);
            nf.validate();
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });


        me.callParent(arguments);
    }
});