/**
*     MagicColumn используется как доп колонка для любых целей
*
*     @example
*     
*     {
*       xtype: 'gkhrfmagiccolumn',
*       scope: this
*     }
*
*/
Ext.define('B4.view.Control.GkhCustomColumn', {
    extend: 'Ext.grid.column.Action',
    alias: ['widget.gkhcustomcolumn'],
    actionName: 'custom',
    constructor: function (config) {
        config.width = 20;

        this.callParent([config]);
    },

    /**
    * @cfg {String} iconCls
    * A CSS class to apply to the icon image. To determine the class dynamically, configure the Column with
    * a `{@link #getClass}` function.
    */
    icon: B4.Url.content('content/img/icons/arrow_undo.png'),

    /**
    * @cfg {Function} handler
    * A function called when the icon is clicked.
    * @cfg {Ext.view.Table} handler.view The owning TableView.
    * @cfg {Number} handler.rowIndex The row index clicked on.
    * @cfg {Number} handler.colIndex The column index clicked on.
    * @cfg {Object} handler.item The clicked item (or this Column if multiple {@link #cfg-items} were not configured).
    * @cfg {Event} handler.e The click event.
    */
    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
        var me = this,
            scope = me.origScope;

        scope.fireEvent('rowaction', scope, this.actionName, rec);
    }
});