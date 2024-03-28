Ext.define('B4.store.service.HousingCostItem', {
    extend: 'B4.base.Store',
    requires: ['B4.model.service.CostItem'],
    storeId: 'housingCostItemStore',
    model: 'B4.model.service.CostItem'
});