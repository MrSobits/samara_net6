/*этот стор используется для получения домов,
 *которые не добавлены во вкладку "Дома, включенные в договор"
 *в других договорах
 */
Ext.define('B4.store.contractrf.ActualRealObjForSelect', {
    extend: 'B4.base.Store',
    requires: ['B4.model.RealityObject'],
    autoLoad: false,
    storeId: 'actualRealityObject',
    model: 'B4.model.RealityObject',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ContractRf',
        listAction: 'ActualRealityObjectList'
    }
});