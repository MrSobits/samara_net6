Ext.define('B4.catalogs.JurialContragentsMultiSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор юр. лица',
    store: 'B4.store.JurialContragents',
    columns: [{ text: 'Имя', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
    selectionMode: 'MULTI',
    editable: false,
    listeners: {
        beforeload: function (fld, operation, store, e) {
            var munFld = fld.ownerCt.down('[titleWindow="Выбор лицевого счета"]');
            if (munFld && operation) {
                var ids = munFld.getValue();
                if (ids)
                    operation.params.acIds = ids;
            }
        }
    },
});