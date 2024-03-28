Ext.define('B4.store.regop.ChargePeriod', {
    extend: 'B4.base.Store',
    model: 'B4.model.regop.ChargePeriod',
    requires: ['B4.model.regop.ChargePeriod'],
    autoLoad: false,
    sorters: [
        {
            property: 'StartDate',
            direction: 'DESC'
        }
    ],
    manualSorter: function (store, operation) {
        var sort;
        if (operation.sorters[0]) {
            sort = operation.sorters[0];
            if (sort && sort.property === 'Name') {
                store.nameDir = sort.direction = store.nameDir === 'ASC' ? 'DESC' : 'ASC';
                sort.property = 'StartDate';
            }
        }
        else {
            store.nameDir = 'DESC';
            operation.sorters.push({ direction: 'DESC', property: 'StartDate', root: "data" })
        }
    },
    sortOnLoad: true,

    listeners: {
        beforeload: function (store, operation) {
            if (this.manualSorter) {
                this.manualSorter(store, operation);
            }
        }
    }
});