Ext.define('B4.store.regop.ClosedChargePeriod', {
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
        var sort = operation.sorters[0];
        if (sort && sort.property === 'Name') {
            store.nameDir = sort.direction = store.nameDir === 'ASC' ? 'DESC' : 'ASC';
            sort.property = 'StartDate';
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