Ext.define('B4.model.controlorg.TatarstanZonalInspection', {
    extend: 'B4.model.dict.ZonalInspection',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TatarstanZonalInspection'
    },
    fields: [
        { name: 'ControlOrganization'}
    ]
});