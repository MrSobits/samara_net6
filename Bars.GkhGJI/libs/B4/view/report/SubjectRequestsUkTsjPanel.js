Ext.define("B4.view.report.SubjectRequestsUkTsjPanel", {
    extend: "Ext.form.Panel",
    title: '',
    itemId: 'subjectRequestsUkTsjPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
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
                    fieldLabel: 'Муниципальные образования'
                },
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    itemId: 'dfDateStart',
                    disabled: false,
                    fieldLabel: 'Начало периода'
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    itemId: 'dfDateEnd',
                    disabled: false,
                    fieldLabel: 'Окончание периода'
                }
            ]
        });

        me.callParent(arguments);
    }
});