/*
Базовый стор для справочника
необходимо задать proxy при создании стора:
    Ext.create('B4.store.dict.BaseDict', {
        proxy: {
            type: 'b4proxy',
            controllerName: 'controllerName'
        }
    });
 */
Ext.define('B4.store.dict.BaseDict', {
    extend: 'B4.base.Store',
    requires: 'B4.model.dict.BaseDict',
    model: 'B4.model.dict.BaseDict',
    autoLoad: false,
});