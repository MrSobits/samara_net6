/*
 * Был обернут дополнительным контейнером для избежание проблем с модальными окнами
 */
Ext.define('B4.view.administration.operator.MainPanel', {
    extend: 'Ext.panel.Panel',
    
    requires: [
        'B4.view.administration.operator.Grid'
    ],

    title: 'Операторы',
    alias: 'widget.administrationOperatorPanel',
    layout: 'fit',
    closable: true,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'admoperatorgrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});
