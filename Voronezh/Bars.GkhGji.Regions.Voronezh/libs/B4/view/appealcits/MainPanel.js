Ext.define('B4.view.appealcits.MainPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    title: 'Реестр предостережений',
    alias: 'widget.appealcitsMainPanel',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.view.appealcits.AdmonitionGrid',
        'B4.view.appealcits.AdmonitionFilterPanel'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'appealcitsAdmonitionFilterPanel',
                    region: 'north',
                    split: false,
                    border: false,
                    padding: 2,
                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6'
                },
                {
                    xtype: 'admonitiongrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
