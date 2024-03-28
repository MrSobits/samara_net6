Ext.define('B4.catalogs.PersonalAccountMultiSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор лицевого счета',
    store: 'B4.store.regop.personal_account.BasePersonalAccount',
    columns: [
        { text: 'Номер', dataIndex: 'PersonalAccountNum', flex: 1, filter: { xtype: 'textfield' } },
        { text: 'ФИО/Наименование абонента', flex: 1, dataIndex: 'AccountOwner', filter: { xtype: 'textfield' } }
    ],
    textProperty: 'AccountOwner',
    selectionMode: 'MULTI',
    editable: false,
    listeners: {
        beforeload: function (fld, operation, store, e) {
            var munFld = fld.ownerCt.down('[titleWindow="Выбор дома"]');
            if (munFld && operation) {
                var ids = munFld.getValue();
                if (ids)
                    operation.params.roIds = ids;
            }
        }
    }
});