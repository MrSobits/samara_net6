Ext.define('B4.view.workscr.FilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.workscrfilterpanel',

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
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    width: 500
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    width: 500
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Work',
                    fieldLabel: 'Вид работы',
                    width: 500
                }
            ]
        });

        me.callParent(arguments);
    }
});