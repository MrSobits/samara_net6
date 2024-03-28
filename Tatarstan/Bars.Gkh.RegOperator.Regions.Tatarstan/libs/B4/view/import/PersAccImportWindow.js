Ext.define('B4.view.import.PersAccImportWindow', {
    extend: 'B4.form.BaseFileImportWindow',
    alias: 'widget.persaccimportwin',

    requires: [
        'B4.form.FileField'
    ],
    
    getItems: () => [
        {
            xtype: 'b4filefield',
            name: 'FileImport',
            labelWidth: 100,
            labelAlign: 'right',
            fieldLabel: 'Файл',
            allowBlank: false,
            itemId: 'fileImport'
        },
        {
            xtype: 'checkbox',
            name: 'replaceData',
            hidden: true,
            checked: true,
            style: 'margin-left: 10px; margin-top: 20px; ',
            boxLabel: 'Заменить данные по лс',
            margin: '0 0 0 100'
        },
        {
            xtype: 'displayfield',
            itemId: 'log'
        }
    ]
});
