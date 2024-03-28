Ext.define('B4.view.import.BaseWindow', {
    extend: 'Ext.window.Window',
    modal: true,
    requires: [
        'B4.form.FileField'
    ],

    alias: 'widget.baseimportwin',

    title: 'Импорт',

    items: {
        xtype: 'form',
        bodyStyle: Gkh.bodyStyle,
        border: 0,
        margin: '5 5 5 5',
        layout: 'hbox',
        items: [
            {
                xtype: 'b4filefield',
                name: 'file',
                fieldLabel: 'Путь к файлу',
                flex: 1,
                required: true
            },
            {
                xtype: 'button',
                actionName: 'submit',
                text: 'Импорт',
                style: {
                    marginLeft: 5
                }
            }
        ]
    }
});