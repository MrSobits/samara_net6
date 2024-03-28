Ext.define('B4.view.terminatecontract.GridPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.terminatecontractgridpanel',
    closable: false,
    itemId: 'terminateContractGridPanel',
    layout: 'border',
    
    requires: [
        'B4.view.terminatecontract.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    padding: 2,
                    style: 'font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 3px;',
                    html: '<span class="im-info"></span>  На данной странице представлены данные только для чтения. Для редактирования данных необходимо перейти по кнопке "Управление домами".'
                },
                {
                    xtype: 'terminatecontractgrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});
