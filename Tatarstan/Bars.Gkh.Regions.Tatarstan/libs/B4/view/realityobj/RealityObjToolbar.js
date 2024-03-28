Ext.define('B4.view.realityobj.RealityObjToolbar', {
    extend: 'Ext.toolbar.Toolbar',

    alias: 'widget.realityobjtoolbar',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.BackForward'
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
                {
                    xtype: 'button',
                    iconCls: 'icon-printer',
                    text: 'Экспорт ТехПаспорта',
                    itemId: 'btnExportTp'
                },
                {
                    xtype: 'button',
                    iconCls: 'icon-table-go',
                    text: 'Отправить ТехПаспорт в ГИС ЖКХ',
                    itemId: 'btnSendTp'
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
        },
        {
            xtype: 'backforwardbutton'
        }
    ]
});