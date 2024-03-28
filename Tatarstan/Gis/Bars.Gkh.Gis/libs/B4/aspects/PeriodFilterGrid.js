/**
 * Аспект реализующий логику взаимодействия грида и фильтра по периоду на родительском контейнере.
 */
Ext.define('B4.aspects.PeriodFilterGrid', {
    extend: 'B4.base.Aspect',
    requires: ['B4.mixins.MaskBody'],
    alias: 'widget.period_filter_grid',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    /**
     * @cfg {String} gridSelector
     * Селектор по которому аспект находит грид
     */
    gridSelector: null,

    /**
     * @cfg {String} editFormSelector
     * Селектор для фильтра год
     * значение по умолчанию 'house_info_panel combobox[name=month]'
     */
    yearFilterSelector: 'house_info_panel combobox[name=year]',

    /**
     * @cfg {String} editFormSelector
     * Селектор для фильтра месяц
     * значение по умолчанию 'house_info_panel combobox[name=month]'
     */
    monthFilterSelector: 'house_info_panel combobox[name=month]',

    /**
     * @method init
     */
    init: function (controller) {
        var actions = {};
        this.callParent(arguments);

        actions[this.gridSelector + ' b4updatebutton'] = {
            'click': {
                fn: this.refresh,
                scope: this
            }
        };

        actions[this.yearFilterSelector] = {
            'change': {
                fn: this.refresh,
                scope: this
            }
        };

        actions[this.monthFilterSelector] = {
            'change': {
                fn: this.refresh,
                scope: this
            }
        };

        controller.control(actions);
    },

    refresh: function () {
        var grid = this.componentQuery(this.gridSelector),
            comboMonth = this.componentQuery(this.monthFilterSelector),
            comboYear = this.componentQuery(this.yearFilterSelector);

        if (!grid || !comboMonth.getValue() || !comboYear.getValue()) {
            return;
        }

        var proxy = grid.getStore().getProxy();
        proxy.setExtraParam('month', comboMonth.getValue());
        proxy.setExtraParam('year', comboYear.getValue());

        grid.getStore().load();
    }
});