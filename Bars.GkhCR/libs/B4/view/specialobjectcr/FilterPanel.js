Ext.define('B4.view.specialobjectcr.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.specialobjectcrfilterpnl',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    border: false,
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'tfProgramCr',
                    fieldLabel: 'Программа КР',
                    width: 500
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'tfMunicipality',
                    fieldLabel: 'Муниципальные районы',
                    width: 500
                }
            ]
        });

        me.callParent(arguments);
    }
});