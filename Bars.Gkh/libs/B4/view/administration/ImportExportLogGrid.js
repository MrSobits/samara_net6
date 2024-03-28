Ext.define('B4.view.administration.ImportExportLogGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.importexportloggrid',
    itemId: 'importExportLogGrid',
    requires: [
        'B4.ux.button.Update'
    ],
    
    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            store: Ext.create('B4.store.administration.ImportExportLogStore'),
            columns: [
                {
                    header: 'Дата старта',
                    dataIndex: 'DateStart',
                    flex: 1
                },
                {
                    header: 'Тип операции',
                    dataIndex: 'Type',
                    flex: 1,
                    renderer: function(value) {
                        switch (value) {
                        case 0:
                            return 'Импорт';
                        case 1:
                            return 'Экспорт';
                        }
                    }
                },
                { header: 'Есть ошибки', dataIndex: 'HasErrors', flex: 1 },
                { header: 'Есть сообщения', dataIndex: 'HasMessages', flex: 1 },
                {
                    header: 'Файл лога',
                    dataIndex: 'FileInfo',
                    renderer: function (value) {
                        var fileId = value.Id;
                        if (fileId > 0) {
                            var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', fileId));
                            return '<a href="' + url + '" target="_blank" style="color: black">Скачать</a>';
                        }
                        return '';
                    },
                    flex: 1
                }
            ],
            viewConfig: {
                maskBody: true
            },
            dockedItems: [
                {
                    xtype: 'buttongroup',
                    dock: 'top',
                    items: [
                        {xtype: 'b4updatebutton'}
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});