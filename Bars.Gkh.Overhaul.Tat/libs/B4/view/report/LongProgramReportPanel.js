Ext.define('B4.view.report.LongProgramReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'longprogramreportpanel',
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
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    fieldLabel: 'Дата отчета',
                    name: 'DateReport',
                    value: new Date(),
                    itemId: 'dfDateReport'
                },
                {
                    xtype: 'b4combobox',
                    name: 'YearProgram',
                    itemId: 'cbYear',
                    fieldLabel: 'Год',
                    editable: false,
                    items: [[0, '-']]
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
            var cb = me.down('#cbYear');
            for (var y = yearStart; y < yearEnd; y++) {                
                cb.items.push([y, y]);
            }
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При получении параметров произошла ошибка!');
        });
        

        me.callParent(arguments);
    }
});