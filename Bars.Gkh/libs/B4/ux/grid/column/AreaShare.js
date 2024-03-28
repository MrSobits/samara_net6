Ext.define('B4.ux.grid.column.AreaShare', {
    extend: 'B4.ux.grid.column.Decimal',
    alias: 'widget.areasharecolumn',

    metaStyle: null,

    initComponent: function () {
        var me = this;
        Ext.apply(me,
            {
                decimalPrecision: Gkh.config.RegOperator.GeneralConfig.AreaShareConfig.DecimalsAreaShare,
                defaultRenderer: function (value, meta) {
                    if (me.metaStyle !== null) {
                        meta.style = me.metaStyle;
                    }
                    return Ext.util.Format.currency(value, null, me.decimalPrecision);
                }
            });

        me.callParent(arguments);
    }
});