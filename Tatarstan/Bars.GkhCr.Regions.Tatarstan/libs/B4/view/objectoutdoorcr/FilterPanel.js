﻿Ext.define('B4.view.objectoutdoorcr.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.objectoutdoorcrfilterpanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    trackResetOnLoad: true,
    autoScroll: true,
    frame: true,
    border: false,
    requires: [
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
                    name: 'OutdoorProgram',
                    fieldLabel: 'Программа благоустройства',
                    width: 500
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальные районы',
                    width: 500
                }
            ]
        });

        me.callParent(arguments);
    }
});