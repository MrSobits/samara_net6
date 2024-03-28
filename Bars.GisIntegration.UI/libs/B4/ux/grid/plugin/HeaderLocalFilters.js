Ext.define('B4.ux.grid.plugin.HeaderLocalFilters', {
    extend: 'B4.ux.grid.plugin.HeaderFilters',

    alias: 'plugin.b4gridheaderlocalfilters',
    ptype: 'b4gridheaderlocalfilters',

    renderFilters: function() {
        this.callParent(arguments);

        var filterFields = this.fields;

        Ext.iterate(filterFields, function (k, v) {

            var filterFn = v.filterFn;

            if (!filterFn) {
                filterFn = this.defaultFilterFn[v.xtype];
            }

            if (!filterFn) {
                filterFn = function(value, filterValue) {
                    return value === filterValue;
                };
            }

            v.filterFn = function (item) {
                var value = item.get(v.filterName),
                filterValue = v.getValue();
                return filterFn.call(v, value, filterValue);
            };
        }, this);
    },

    applyFilters: function () {
        var store = this.grid.getStore(),
            filterFields = this.fields,
            filters = [];

        Ext.iterate(filterFields, function (k, v) {
            var filterValue = v.getValue();

            if (!Ext.isEmpty(filterValue)) {
                filters.push(
                    v.filterFn);
            }
        }, this);

        if (filters.length) {
            store.clearFilter(true);
            store.filter(filters);
        } else {
            store.clearFilter();
        }
    },

    defaultFilterFn: {
        b4enumcombo: function (value, filterValue) {
            return value === filterValue;
        },
        datefield: function (value, filterValue) {
            return value && Ext.util.Format.date(value, this.format) == Ext.util.Format.date(filterValue, this.format);
        },
        textfield: function (value, filterValue) {
            return value && value.toLowerCase().indexOf(filterValue.toLowerCase()) > -1;
        },
        numberfield: function (value, filterValue) {
            return value === filterValue;
        }
    }
});
