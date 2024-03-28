Ext.define('B4.view.roletypehousepermission.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'Ext.ux.CheckColumn'
    ],

    alias: 'widget.roletypehousepermissionGrid',
    store: 'RoleTypeHousePermission',
    border: true,
    viewConfig: {
        autoFill: true
    },
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Тип дома',
                    width: 300,
                    sortable: false
                },
                {
                    xtype: 'checkcolumn',
                    text: 'Доступен для создания',
                    dataIndex: 'Allowed',
                    width: 150,
                    sortable: false
                }
            ],
            tbar: [
                {
                    iconCls: 'icon-disk',
                    text: 'Сохранить',
                    itemId: 'btnSavePermissions'
                },
                {
                    xtype: 'tbseparator'
                },
                {
                    iconCls: 'icon-accept',
                    text: 'Отметить все',
                    itemId: 'btnMarkAll'
                },
                {
                    iconCls: 'icon-decline',
                    text: 'Снять все отметки',
                    itemId: 'btnUnmarkAll'
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});