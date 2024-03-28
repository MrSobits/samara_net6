Ext.define('B4.store.dict.InspectorForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.Inspector'],
    autoLoad: false,
    storeId: 'inspectorForSelectedStore',
    model: 'B4.model.dict.Inspector',
    listeners: {
        //инспекторы должны быть отсортированы в порядке добавления 
        beforeload: function (store) {
            store.sorters.add(new Ext.util.Sorter({ property: 'Order', direction: 'ASC' }));
        }
    }
});