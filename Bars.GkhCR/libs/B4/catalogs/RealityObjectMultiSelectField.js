Ext.define('B4.catalogs.RealityObjectMultiSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор дома',
    store: 'B4.store.view.ViewRealityObject',
    columns: [{ text: 'Aдрес', dataIndex: 'FullAddress', flex: 1, filter: { xtype: 'textfield' } }],
    textProperty: 'FullAddress',
    selectionMode: 'MULTI',
    editable: false,
    listeners: {
        beforeload: function (fld, operation, store, e) {
            var munFld = fld.ownerCt.down('[titleWindow="Выбор населенного пункта"]');
            if (munFld && operation) {
                var ids = munFld.getValue();
                if (ids)
                    operation.params.moIds = ids;
            }
        }
    },
});