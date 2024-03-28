Ext.define('B4.ux.grid.plugin.MemoryHeaderFilters', {
    extend: 'B4.ux.grid.plugin.HeaderFilters',
    alias: 'plugin.b4gridmemoryheaderfilters',
    ptype: 'b4gridmemoryheaderfilters',

    getFilters: function () {
        return this.parseFilters();
    },

    parseFilters: function () {
        var fields = this.fields,
            filters = [],
            field, value;
        
        if (!fields)
            return filters;
        for (var fn in fields) {
            
            field = fields[fn];
            
            if (!field.isDisabled() && field.isValid()) {
                
                value = field.getSubmitValue();
                
                if (!Ext.isEmpty(value)) {

                    filters.push({
                        value: field.getValue(),
                        operand: field.operand || CondExpr.operands.contains,
                        name: field.filterName
                    });
                }
            }
        }
        return filters;
    },

    applyFilters: function () {
        var me = this,
            filter = me.parseFilters(),
            curFilters,
            store = me.grid.getStore(),
            storeFilters;
        
        if (me.grid.fireEvent('beforeheaderfiltersapply', me.grid, filter, store) !== false) {

            me.grid.fireEvent('headerfiltersapply', me.grid, filter, store);
            curFilters = me.getFilters();

            store.clearFilter(true);

            storeFilters = me.convertToStoreFilters(curFilters);

            store.filter(storeFilters);

            store.complexFilter = curFilters;

            me.grid.fireEvent('headerfilterchange', me.grid, curFilters, me.lastApplyFilters, store);
            me.lastApplyFilters = curFilters;
        }
    },
    
    convertToStoreFilters: function (curFilters) {
        var storeFilters = [];

        Ext.each(curFilters, function (f) {
            var obj = {};

            switch (f.operand.toLowerCase()) {
                case "eq":
                    obj.filterFn = function (rec) {
                        return rec.get(f.name) == f.value;
                    };
                    break;
                case "icontains":
                    obj.filterFn = function (rec) {
                        try {
                            var v = rec.get(f.name);

                            if (Ext.isEmpty(v)) {
                                return false;
                            }

                            v = v.toLowerCase();

                            return v.indexOf(f.value.toLowerCase()) >= 0;
                        } catch(e) {
                            return true;
                        } 
                    };
                    break;
            }

            storeFilters.push(obj);
        });

        return storeFilters;
    }
});