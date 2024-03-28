Ext.define('B4.view.report.HouseInformationCeReport', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportHouseInformationCePanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.store.CommonEstateObject',
        'B4.form.SelectField',
        'B4.form.EnumCombo'
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
                    emptyText: 'Все'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'HouseTypes',
                    itemId: 'tfHouseType',
                    fieldLabel: 'Тип дома',
                    emptyText: 'Все'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Ceos',
                    itemId: 'tfCeo',
                    fieldLabel: 'ООИ',
                    allowBlank: false
                },
                {
                    xtype: 'b4combobox',
                    fieldLabel: 'Список домов',
                    itemId: 'housesList',
                    items: [[1, 'Реестр жилых домов'], [2, 'Опубликованная программа']],
                    editable: false,
                    operand: CondExpr.operands.eq,
                    value: 1
                },
                {
                    xtype: 'checkbox',
                    itemId: 'cbMain',
                    name: 'Main',
                    fieldLabel: 'Основная версия программы',
                    width: 500
                },
            ]
        });

        me.callParent(arguments);
    }
});