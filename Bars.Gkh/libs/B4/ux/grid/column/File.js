Ext.define('B4.ux.grid.column.File', {
    extend: 'Ext.grid.column.Action',
    alias: ['widget.filecolumn'],
    
    text: 'Файл',
    tooltip: 'Скачать',
    width: 50,
    align: 'center',
    icon: B4.Url.content('content/img/icons/disk.png'),

    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
        var me = this,
            grid = me.up('grid'),
            file = rec.get(me.dataIndex),
            id = 0;

        if (Ext.isNumber(file)) {
            id = file;
        } else if (Ext.isObject(file)) {
            id = file.Id || 0;
        }

        if (id > 0) {
            window.open(B4.Url.action('/FileUpload/Download?id=' + id));
        } else {
            Ext.Msg.alert('Ошибка', 'Отсутствует файл для загрузки');
        }
    }
});