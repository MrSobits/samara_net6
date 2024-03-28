Ext.define('B4.ux.grid.column.Decimal', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.decimalcolumn',

    decimalPrecision: 2,

    defaultRenderer: function (value) {
        var me = this;
        return Ext.util.Format.currency(value, null, me.decimalPrecision);
    }
});