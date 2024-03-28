Ext.define('B4.store.EntityHistoryInfo', {
    extend: 'B4.base.Store',
    autoLoad: false,
    sortOnLoad: true,

    groupType: null,
    parentName: null,
    entityName: null,

    proxy: {
        type: 'b4proxy',
        controllerName: 'EntityHistoryInfo',
        listAction: 'list',
        createAction: '',
        getAction: '',
        updateAction: '',
        deleteAction: ''
    },
    fields: [
        { name: 'Id' },
        { name: 'EditDate' },
        { name: 'ActionKind' },
        { name: 'IpAddress' },
        { name: 'EntityId' },
        { name: 'EntityName' },
        { name: 'ParentEntityId' },
        { name: 'ParentEntityName' },
        { name: 'Login' },
        { name: 'Username' }
    ],
    sorters: [
        {
            property: 'Id',
            direction: 'DESC'
        }
    ],
    listeners: {
        beforeload: function(store, options) {
            var me = this;
            options.params = options.params || {}
            Ext.apply(options.params, {
                groupType: me.groupType,
                parentName: me.parentName,
                entityName: me.entityName
            });
        }
    },
    setGroup: function(groupType) {
        var me = this;
        me.groupType = groupType;
    },
    setNames: function (entityName, parentName) {
        var me = this;
        me.entityName = entityName;
        me.parentName = parentName;
    }
});