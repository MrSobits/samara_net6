Ext.define('B4.view.repairobject.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.repairobjectfilterpnl',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'repairObjectFilterPanel',
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
                    name: 'tfRepairProgram',
                    itemId: 'tfRepairProgram',
                    fieldLabel: 'Программа',
                    width: 500
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'tfMunicipality',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальное образование',
                    width: 500
                }
            ]
        });

        me.callParent(arguments);
    }
});