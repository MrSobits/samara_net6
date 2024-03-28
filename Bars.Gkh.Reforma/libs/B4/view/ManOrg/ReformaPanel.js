Ext.define('B4.view.manorg.ReformaPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    alias: 'widget.manorgreformapanel',

    requires: ['B4.enums.ReformaRequestStatus'],

    title: 'Реформа ЖКХ',

    layout: {
        type: 'vbox',
        align: 'left'
    },

    defaults: {
        labelWidth: 150,
        labelAlign: 'right',
        width: 800,
        margin: '5 0'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            bodyPadding: 5,
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'RequestStatus',
                    items: B4.enums.ReformaRequestStatus.getItems(),
                    displayField: 'Display',
                    valueField: 'Value',
                    fieldLabel: 'Статус заявки',
                    editable: false,
                    readOnly: true
                },
                {
                    xtype: 'datefield',
                    name: 'RequestDate',
                    fieldLabel: 'Дата подачи запроса',
                    readOnly: true,
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'ProcessDate',
                    fieldLabel: 'Дата обработки заявки',
                    readOnly: true,
                    format: 'd.m.Y'
                }
            ]
        });

        me.callParent(arguments);
    }
});