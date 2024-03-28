Ext.define('B4.ux.grid.column.UtcDate', {
    extend: 'Ext.grid.column.Date',
    alias: ['widget.utcdatecolumn'],
        
    defaultRenderer: function (value) {
        var me = this,
            date;
        if (Ext.isWebKit) {
            //Если у даты не указана временная зона
            if (value.search && value.search(/\+\d\d:\d\d$/i) == -1) {
                date = new Date(value + '.000Z');
            }
        }
        else {
            date = new Date(value);
        }
        return Ext.util.Format.date(date, me.format);
    }
});