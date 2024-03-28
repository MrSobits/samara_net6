Ext.define('B4.catalogs.MunicipalityFormationMultiSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор муниципального образования',
    store: 'B4.store.dict.municipality.SettlementAndUrban',
    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
    selectionMode: 'MULTI',
    editable: false,
    listeners: {
        beforeload: function (fld, operation, store, e) {
            var munFld = fld.ownerCt.down('[titleWindow="Выбор муниципального района"]');
            if (munFld && operation) {
                var ids = munFld.getValue();
                if (ids)
                    operation.params.moIds = ids;
            }
        }
    },
});