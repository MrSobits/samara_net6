Ext.define('B4.store.dict.ConditionHouseForSelect', {
    extend: 'Ext.data.Store',
    requires: ['B4.model.dict.HouseType'],
    model: 'B4.model.dict.HouseType',
    data: B4.enums.ConditionHouse.getItemsMeta()
});