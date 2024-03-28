Ext.define('B4.model.manorg.ManagingOrgRegistry', {
    extend: 'B4.base.Model',
    fields: ['Id', 'InfoDate', 'InfoType', 'RegNumber', 'EgrulDate', 'Doc', 'ManagingOrganization'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingOrgRegistry'
    }
});