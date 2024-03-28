Ext.define('B4.catalogs.LocalitySelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор населенного пункта',
    store: 'B4.store.Locality',
    selectionMode: 'MULTI',
    columns: [
        {
            text: 'Муниципальный район',
            dataIndex: 'Municipality',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            text: 'Муниципальный образование',
            dataIndex: 'Settlement',
            flex: 1,
            filter: { xtype: 'textfield' }
        },
        {
            text: 'Населенный пункт',
            dataIndex: 'Name',
            flex: 1,
            filter: { xtype: 'textfield' }
        }
    ],
    listeners: {
        beforeload: function (fld, operation, store, e) {
            var munFld = fld.ownerCt.down('[titleWindow="Выбор муниципального образования"]');
            if (munFld && operation) {
                var ids = munFld.getValue();
                if (ids)
                    operation.params.moIds = ids;
            }
        }
    },
    editable: false
});