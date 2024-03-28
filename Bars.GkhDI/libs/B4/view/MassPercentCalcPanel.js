Ext.define('B4.view.MassPercentCalcPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    title: 'Массовый расчет процентов',
    itemId: 'massPercentCalcPanel',
    requires: [
            'B4.view.Control.GkhTriggerField',
            'B4.form.ComboBox',
            'B4.form.SelectField',
            'B4.store.dict.PeriodDi',
            'B4.view.dict.perioddi.Grid'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                width: 500,
                labelAlign: 'right'
            },
            items: [
               {
                   xtype: 'b4selectfield',
                   editable: false,
                   name: 'PeriodDi',
                   margin: '5px 0 0 0',
                   itemId: 'sfPeriodDi',
                   fieldLabel: 'Период',
                  

                   store: 'B4.store.dict.PeriodDi',
                   flex: 1
               },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    margin: '5px 0 0 0',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования'
                },
               {
                   xtype: 'button',
                   margin: '5px 0 0 200px',
                   itemId: 'btnMassPercentCalc',
                   width: 300,
                   text: 'Пересчитать проценты',
                   flex: 1
               }
            ]
        });

        me.callParent(arguments);
    }
});
