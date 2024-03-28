Ext.define('B4.view.report.PrescriptionViolationRemovalPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'prescriptionViolationRemovalPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.view.dict.municipality.Grid'
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
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    itemId: 'sfMunicipality',
                    textProperty: 'Name',
                    fieldLabel: 'Муниципальное образование',
                    store: 'B4.store.dict.Municipality',
                    listView: 'B4.view.dict.municipality.Grid',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateStart',
                    itemId: 'dfDateStart',
                    fieldLabel: 'Начало периода',
                    format: 'd.m.Y',
                    allowBlank: false,
                    value: new Date(new Date().getFullYear(), 0, 1)
                },
                {
                    xtype: 'datefield',
                    name: 'DateEnd',
                    itemId: 'dfDateEnd',
                    fieldLabel: 'Окончание периода',
                    format: 'd.m.Y',
                    allowBlank: false
                }]
        });

        me.callParent(arguments);
    }
});