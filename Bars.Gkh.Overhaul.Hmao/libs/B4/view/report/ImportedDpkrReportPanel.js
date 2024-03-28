Ext.define('B4.view.report.ImportedDpkrReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'importedDpkrReportPanel',
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
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    defaults: {
                        labelWidth: 200,
                        width: 600,
                        labelAlign: 'right'
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
                            xtype: 'b4combobox',
                            name: 'DataSource',
                            fieldLabel: 'Собирать по',
                            items: [
                                //значения енума Reports.ImportedDpkrReport.DataSource
                                [0, 'Основная версия программы'],
                                [1, 'Долгосрочная программа']
                            ],
                            editable: false,
                            value: 0
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});