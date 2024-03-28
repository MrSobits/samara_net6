Ext.define('B4.ux.grid.column.YesNo', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.yesnocolumn'],
    alternateClassName: 'B4.grid.YesNoColumn',

    requires: [
        'B4.ux.grid.filter.YesNo'
    ],

    constructor: function(config) {
        var me = this,
            filter = config.filter === true
                ? {
                    xtype: 'b4dgridfilteryesno',
                    operator: 'eq'
                }
                : config.filter;

        Ext.apply(config, {
            renderer: function(value) {
                return value ? 'Да' : 'Нет';
            },
            filter: filter
        });

        me.callParent([config]);
    }
});