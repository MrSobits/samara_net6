/**
*     CopyColumn
*
*     @example
*     
*     {
*       xtype: 'b4copycolumn',
*       scope: this
*     }
*
*/
Ext.define('B4.ux.grid.column.Copy', {
    extend: 'Ext.grid.column.Action',
    alias: ['widget.b4copycolumn'],
    alternateClassName: 'B4.grid.CopyColumn',

    hideable: false,

    constructor: function (config) {
        config.width = 20;

        this.callParent([config]);
    },

    tooltip: "Копировать",

    icon: B4.Url.content('content/img/icons/page_copy.png'),

    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
        var scope = this.origScope;

        // Если scope не задан в конфиге, то берем грид которому принадлежит наша колонка
        if (!scope) {
            scope = this.up('grid');
        }

        scope.fireEvent('rowaction', scope, 'copy', rec);
    }
});