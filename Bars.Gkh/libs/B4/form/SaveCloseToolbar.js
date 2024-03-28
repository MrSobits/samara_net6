Ext.define('B4.form.SaveCloseToolbar', {
    extend: 'Ext.toolbar.Toolbar',
    
    alias: 'widget.b4saveclosetoolbar',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    dock: 'top',
    items: [
        {
            xtype: 'buttongroup',
            items: [
                {
                    xtype: 'b4savebutton'
                }
            ]
        },
        {
            xtype: 'tbfill'
        },
        {
            xtype: 'buttongroup',
            items: [
                {
                    xtype: 'b4closebutton'
                }
            ]
        }
    ]
});