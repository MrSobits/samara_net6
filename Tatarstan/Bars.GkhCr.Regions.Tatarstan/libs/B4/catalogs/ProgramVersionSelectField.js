Ext.define('B4.catalogs.ProgramVersionSelectField', {
    extend: 'B4.form.SelectField',
    titleWindow: 'Выбор версии ДПКР',
    requires: [
        'B4.store.dict.ProgramVersionSelected',
        'B4.form.ComboBox'
    ],
    store: 'B4.store.dict.ProgramVersionSelected',
    columns: [
        { 
            text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
            filter: {
                xtype: 'b4combobox',
                operand: CondExpr.operands.eq,
                storeAutoLoad: true,
                hideLabel: true,
                editable: false,
                valueField: 'Name',
                emptyItem: { Name: '-' },
                url: '/Municipality/ListWithoutPaging'
            }
        },
        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' }},
        { text: 'Дата', dataIndex: 'VersionDate', flex: 1, filter: { xtype: 'datefield' }}
    ],
    editable: false,
    listeners: {
        beforeload: function (field, operation) {
            var dpkrSelectField = field.ownerCt.down('[titleWindow="Выбор версии ДПКР"]');
            
            // Если для отчета было выбрано 2 справочника ProgramVersionSelectField
            // Если это главный главный справочник, то не выполняем доп. действия
            if (dpkrSelectField.name === field.name) return;
            
            // Если пользователь выбирает второстепнный справочник без выбора знаения в основном, то выводим ошибку
            if (dpkrSelectField.name !== field.name && !dpkrSelectField.value){
                Ext.Msg.alert('Ошибка', 'Сначала нужно выбрать значение в первом параметре');
                field.onSelectWindowClose();
                return;
            }
            
            //Если в главном справочнике было выбрано значение, то версии для второстепенного справочника
            //будут подгружены предварительно в отфильтрованом по МО виде
            if (dpkrSelectField && dpkrSelectField.value && operation) {
                var municipalityId = dpkrSelectField.value.MunicipalityId;
                if (municipalityId)
                    operation.params.municipalityId = municipalityId;
            }
        }
    },
});