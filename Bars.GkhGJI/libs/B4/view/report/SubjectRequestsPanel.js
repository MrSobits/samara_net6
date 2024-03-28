Ext.define("B4.view.report.SubjectRequestsPanel", {
    extend: "Ext.form.Panel",
    title: '',
    itemId: 'subjectRequestsPanel',
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
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    itemId: 'dfDateStart',
                    disabled: false,
                    allowBlank: false,
                    fieldLabel: 'Начало периода'
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    itemId: 'dfDateEnd',
                    disabled: false,
                    allowBlank: false,
                    fieldLabel: 'Окончание периода'
                }
            ]
        });

        me.callParent(arguments);
    }
});