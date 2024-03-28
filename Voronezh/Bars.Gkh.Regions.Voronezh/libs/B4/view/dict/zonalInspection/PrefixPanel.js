Ext.define('B4.view.dict.zonalinspection.PrefixPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.store.dict.ZonalInspectionPrefix'
    ],

    title: 'Префиксные номера отдела',
    itemId: 'zonalInspectionPrefixPanel',
    alias: 'widget.zonalinspectionprefixpanel',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me,
        {
            layout: 'form',
            split: false,
            border: false,
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'container',
                    padding: '5px 10px',
                    defaults: {
                        xtype: 'textfield',
                        labelWidth: 160,
                        labelAlign: 'right',
                        maxLength: 50
                    },
                    items: [
                        {
                            name: 'ActCheckPrefix',
                            fieldLabel: 'Акта проверки'
                        },
                        {
                            name: 'ProtocolPrefix',
                            fieldLabel: 'Протокол'
                        },
                        {
                            name: 'PrescriptionPrefix',
                            fieldLabel: 'Предписание'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});