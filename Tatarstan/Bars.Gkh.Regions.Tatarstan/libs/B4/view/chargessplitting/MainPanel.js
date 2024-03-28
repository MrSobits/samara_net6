Ext.define('B4.view.chargessplitting.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Расщепление платежей',
    alias: 'widget.chargessplittingmainpanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.view.chargessplitting.contrpersumm.Grid',
        'B4.view.chargessplitting.budgetorg.Grid',
        'B4.view.chargessplitting.fuelenergyresrc.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            xtype: 'contractperiodsummarygrid'
                        },
                        {
                            xtype: 'budgetorggrid'
                        },
                        {
                            xtype: 'fuelenergyresourcecontractgrid'
                        }
                    ]
                }
            ]
        }, me);

        me.callParent(arguments);
    }
});