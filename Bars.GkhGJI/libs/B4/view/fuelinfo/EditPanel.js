Ext.define('B4.view.fuelinfo.EditPanel', {

    extend: 'Ext.form.Panel',

    alias: 'widget.fuelinfoeditpanel',

    closable: true,

    title: 'Сведения о наличии и расходе топлива',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.enums.Month',

        'B4.view.fuelinfo.FuelAmountInfoGrid',
        'B4.view.fuelinfo.FuelExtractionDistanceInfoGrid',
        'B4.view.fuelinfo.FuelContractObligationInfoGrid',
        'B4.view.fuelinfo.FuelEnergyDebtInfoGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    bodyStyle: Gkh.bodyStyle,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    layout: 'vbox',
                    name: 'periodInfo',
                    width: 500,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 150,
                        anchor: '100%',
                        readOnly: true
                    },
                    bodyPadding: 5,
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Municipality',
                            width: 500,
                            labelWidth: 196,
                            fieldLabel: 'Муниципальное образование',
                            editable: false,
                            readOnly: true,
                            valueToRaw: function (value) {
                                return value && Ext.isString(value) ? value : value && value.Name ? value.Name : '';
                            }
                        },
                        {
                            xtype: 'container',
                            width: 500,
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'label',
                                    text: 'Отчетный период:',
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Month',
                                    labelWidth: 50,
                                    editable: false,
                                    readOnly: true,
                                    flex: 1,
                                    valueToRaw: function (val) {
                                        return B4.enums.Month.displayRenderer(val);
                                    }
                                },
                                {
                                    xtype: 'textfield',
                                    margin: '0 0 0 10',
                                    name: 'Year',
                                    labelWidth: 50,
                                    editable: false,
                                    readOnly: true,
                                    flex: 1
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    name: 'Details',
                    flex: 1,
                    items: [
                        {
                            xtype: 'fuelamountinfogrid',
                            title: 'Раздел 1. Поставка, расход и остатки топлива'
                        },
                        {
                            xtype: 'fuelextractiondistanceinfogrid',
                            title: 'Раздел 2. Расстояние от места добычи топлива до потребителя'
                        },
                        {
                            xtype: 'fuelcontractobligationinfogrid',
                            title: 'Раздел 3. Выполнение договорных обязательств по поставкам топлива'
                        },
                        {
                            xtype: 'fuelenergydebtinfogrid',
                            title: 'Раздел 4. Задолженность за ранее потребленные топливно-энергетические ресурсы (ТЭР) по состоянию на конец отчетного периода'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});