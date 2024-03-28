Ext.define('B4.view.housingfundmonitoring.EditPanel', {

    extend: 'Ext.form.Panel',

    alias: 'widget.housingfundmonitoringeditpanel',

    closable: true,

    title: 'Мониторинг жилищного фонда',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Close',

        'B4.view.housingfundmonitoring.InfoGrid'
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
                            xtype: 'textfield',
                            name: 'Year',
                            width: 500,
                            labelWidth: 196,
                            fieldLabel: 'Отчетный период',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'housingfundmonitoringinfogrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});