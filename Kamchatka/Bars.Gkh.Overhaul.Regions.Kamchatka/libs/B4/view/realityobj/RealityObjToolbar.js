Ext.define('B4.view.realityobj.RealityObjToolbar', {
    extend: 'Ext.toolbar.Toolbar',

    alias: 'widget.realityobjtoolbar',

    requires: [
        'B4.ux.button.Save'
    ],

    dock: 'top',
    items: [
        {
            xtype: 'buttongroup',
            columns: 4,
            items: [
                {
                    xtype: 'b4savebutton'
                },
                {
                    xtype: 'button',
                    iconCls: 'icon-map-go',
                    text: 'Карта',
                    itemId: 'btnMap'
                },
                //{
                //    xtype: 'button',
                //    iconCls: 'icon-printer',
                //    text: 'Экспорт ТехПаспорта',
                //    itemId: 'btnExportTp'
                //},
                {
                    xtype: 'button',
                    iconCls: 'icon-printer',
                    text: 'Экспорт технического паспорта',
                    itemId: 'btnExportCh'
                }
            ]
        },
        {
            xtype: 'tbfill'
        },
        {
            xtype: 'buttongroup',
            itemId: 'statusButtonGroup',
            items: [
                {
                    xtype: 'button',
                    iconCls: 'icon-accept',
                    itemId: 'btnState',
                    text: 'Статус',
                    menu: []
                }
            ]
        }
    ]
});