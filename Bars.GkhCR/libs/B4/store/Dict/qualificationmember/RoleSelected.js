Ext.define('B4.store.dict.qualificationmember.RoleSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.Role'],
    autoLoad: false,
    model: 'B4.model.Role',
    proxy: {
        type: 'b4proxy',
        controllerName: 'QualificationMember',
        listAction: 'ListRoles'
    }
});