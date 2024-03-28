Ext.define('B4.view.report.InformationOfManagOrgPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'informationOfManagOrgPanel',
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
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'OrgTypes',
                    itemId: 'tfOrgTypes',
                    fieldLabel: 'Тип организации',
                    emptyText: 'Все типы организаций'
                },
                {
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    value: new Date(),
                    allowBlank: false
                },
                {
                    xtype: 'b4combobox',
                    name: 'StatusManOrg',
                    itemId: 'cbStatusManOrg',
                    fieldLabel: 'Статус',
                    editable: false,
                    items: [[0, 'Действует'], [1, 'Не предоставляет услуги управления'], [2, "Банкрот"], [3, "Ликвидирован"]],
                    value: 0
                }

            ]
        });
        me.callParent(arguments);
    }
});