Ext.define('B4.ux.grid.column.Progress', {
    extend: 'Ext.grid.column.Column',
    alias: ['widget.progresscolumn'],
    alternateClassName: 'B4.grid.ProgressColumn',

    showValue: false,
    bgColor: '#99bce8',
    minValue: 0,
    maxValue: 100,
    constructor: function (config) {
        var me = this;

        Ext.applyIf(config, {
            align: 'center',
            tdCls: 'progress-column',
            renderer: function (value) {
                var result = 0,
                    render = '';
                if (typeof value === 'number') {
                    if (value < me.minValue) {
                        value = me.minValue;
                    }
                    if (value > me.maxValue) {
                        value = me.maxValue;
                    }
                    result = Math.round(value * 100) / 100;
                }
                return me.getProgressBlock(result) + me.getValueBlock(result);
            }
        });

        me.callParent([config]);
        me.filter = null;
    },

    getProgressBlock: function (value) {
        var me = this;

        return '<div style="background: ' + me.bgColor + ';width: ' + value + '%;" class="progress-column-bar"></div>';
    },

    getValueBlock: function (value) {
        var me = this;

        return me.showValue ? ('<div class="progress-column-value">' + value + ' %</div>') : '';
    }
});