Ext.define('B4.view.roletypehousepermission.Panel', {
    extend: 'Ext.panel.Panel',
    requires: ['B4.view.roletypehousepermission.Grid'],
    alias: 'widget.roletypehousepermissionpanel',
    
    title: 'Настройка добавления домов',
    closable: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    margins: { left: 6, top: 8, right: 0, bottom: 6 },
                    items: [
                        {
                            xtype: 'combobox',
                            itemId: 'cbRole',
                            emptyText: 'Выберите роль',
                            editable: false,
                            displayField: 'Name',
                            width: 350,
                            labelWidth: 35,
                            fieldLabel: 'Роль',
                            queryMode: 'local',
                            store: Ext.create('B4.store.Role'),
                            valueField: 'Id',
                            margin: '0 50 0 0'
                        }
                    ]
                },
                {
                    xtype: 'roletypehousepermissionGrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});