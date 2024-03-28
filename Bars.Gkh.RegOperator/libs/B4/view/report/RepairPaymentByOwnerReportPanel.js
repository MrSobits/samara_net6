Ext.define('B4.view.report.RepairPaymentByOwnerReportPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.repairByOwnerReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.form.TreeSelectField',
        'B4.store.dict.MunicipalityTree'
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
                    xtype: 'datefield',
                    name: 'startDate',
                    itemId: 'dfStartDate',
                    fieldLabel: 'Дата с',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'endDate',
                    itemId: 'dfEndDate',
                    fieldLabel: 'Дата по',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'treeselectfield',
                    name: 'Municipality',
                    itemId: 'fiasMunicipalitiesTrigerField',
                    fieldLabel: 'Муниципальное образование',
                    titleWindow: 'Выбор муниципального образования',
                    store: 'B4.store.dict.MunicipalitySelectTree',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Address',
                    itemId: 'sfAddress',
                    store: 'B4.store.RealtyObjectByMo',
                    fieldLabel: 'Адрес',
                    editable: false,
                    disabled: true,
                    columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Fio',
                    itemId: 'sfFio',
                    store: 'B4.store.regoperator.AccountByRo',
                    fieldLabel: 'ФИО',
                    editable: false,
                    disabled: true,
                    textProperty: 'AccountOwnerName',
                    columns: [
                        { xtype: 'gridcolumn', header: 'Фио', dataIndex: 'AccountOwnerName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Number',
                    itemId: 'sfNumber',
                    store: 'B4.store.regoperator.PersonalNumByAcc',
                    fieldLabel: '№лс',
                    editable: false,
                    disabled: true,
                    textProperty: 'PersonalAccountNum',
                    columns: [
                        { xtype: 'gridcolumn', header: 'Номер', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});