Ext.define('B4.view.report.HouseInformationCeFiasPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    alias: 'widget.reportHouseInformationCeFiasPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
    requires: [
        'B4.view.Control.GkhTriggerField'
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
                    xtype: 'b4combobox',
                    fieldLabel: 'Список домов',
                    itemId: 'housesList',
                    items: [[1, 'Реестр жилых домов'], [2, 'Опубликованная программа']],
                    editable: false,
                    operand: CondExpr.operands.eq,
                    value: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});