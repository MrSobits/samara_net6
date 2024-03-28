Ext.define('B4.view.report.FillingTechPassportForManOrgRatingPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'fillingTechPassportForManOrgRatingPanel',
    layout: {
        type: 'vbox'
    },
    border: false,
    
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
                labelAlign: 'right',
                width: 600
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования'
                }
            ]
        });

        me.callParent(arguments);
    }
});