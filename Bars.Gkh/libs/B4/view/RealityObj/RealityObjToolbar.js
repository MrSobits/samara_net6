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
            columns: 2,
            items: [
                {
                    xtype: 'b4savebutton'
                },
                {
                    xtype: 'button',
                    iconCls: 'icon-map-go',
                    text: 'Карта',
                    itemId: 'btnMap'
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