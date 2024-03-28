Ext.define('B4.view.overhaulpropose.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.overhaulproposefilterpanel',
    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'overhaulProposalFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update',
        'B4.view.Control.GkhTriggerField'
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
                    itemId: 'tfProgramCr',
                    fieldLabel: 'Программа КР',
                    width: 500
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'tfMunicipality',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные районы',
                    width: 500
                }
            ]
        });

        me.callParent(arguments);
    }

});