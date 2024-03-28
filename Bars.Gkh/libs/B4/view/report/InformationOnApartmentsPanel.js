Ext.define('B4.view.report.InformationOnApartmentsPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'informationOnApartmentsPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.ResettlementProgram',
        'B4.view.dict.resettlementprogram.Grid',
        'B4.form.ComboBox'
    ],

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
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ResettlementProgram',
                    itemId: 'sfResettlementProgram',
                    textProperty: 'Name',
                    fieldLabel: 'Программа переселения',
                    store: 'B4.store.dict.ResettlementProgram',
                    editable: false,
                    allowBlank: false,
                    columns: [
                       {
                           text: 'Наименование',
                           dataIndex: 'Name', flex: 1,
                           filter: { xtype: 'textfield' }
                       }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});