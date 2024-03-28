Ext.define('B4.model.menu.ManagingOrgRealityObjDataMenu', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisclosureInfoRealityObj'
    },
    fields: [
        { name: 'AddressName' },
        { name: 'AreaLiving' },
        { name: 'DateLastOverhaul' },
        { name: 'HouseAccounting' },
        { name: 'percent'}
    ]
});