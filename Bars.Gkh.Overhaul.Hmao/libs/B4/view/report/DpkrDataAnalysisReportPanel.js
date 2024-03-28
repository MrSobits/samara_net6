Ext.define('B4.view.report.DpkrDataAnalysisReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.dpkrdataanalysisreportpanel',
    layout: {
        type: 'vbox'
    },
    border: false,

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
                    fieldLabel: 'Муниципальный район',
                    emptyText: 'Все МР'
                },
                {
                    xtype: 'b4combobox',
                    name: 'Version',
                    items: [
                        [0, 'Основная версия'],
                        [1, 'Опубликованная версия']
                    ],
                    fieldLabel: 'Сборка по',
                    editable: false,
                    value: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});