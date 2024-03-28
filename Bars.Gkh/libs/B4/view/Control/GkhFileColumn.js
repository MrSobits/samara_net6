Ext.define('B4.view.Control.GkhFileColumn', {
    extend: 'Ext.grid.column.Column',

    alias: 'widget.gkhfilecolumn',

    text: 'Файл',
    renderer: function (value) {
        return value ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + value.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
    }
});