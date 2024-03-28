Ext.define('B4.controller.report.Form1Controll', {
    extend: 'B4.controller.BaseReportController',

    mainView: 'B4.view.report.Form1ControllPanel',
    mainViewSelector: '#form1ControllPanel',

    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.form.ComboBox'
    ],

    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected',
        'B4.ux.button.Update',
        'dict.InspectorForSelect',
        'dict.InspectorForSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow'
    ],

    MunicipalityTriggerFieldSelector: '#form1ControllPanel #tfMunicipality',
    DateStartFieldSelector: '#form1ControllPanel #dfDateStart',
    DateEndFieldSelector: '#form1ControllPanel #dfDateEnd',
    HeadFieldSelector: '#form1ControllPanel #tfHead',
    PersonFieldSelector: '#form1ControllPanel #tfPerson',

    refs: [
        {
            ref: 'MunicipalityTriggerField',
            selector: '#form1ControllPanel #tfMunicipality'
        },
        {
            ref: 'DateStartField',
            selector: '#form1ControllPanel #dfDateStart'
        },
        {
            ref: 'DateEndField',
            selector: '#form1ControllPanel #dfDateEnd'
        },
        {
            ref: 'HeadField',
            selector: '#form1ControllPanel #tfHead'
        },
        {
            ref: 'PersonField',
            selector: '#form1ControllPanel #tfPerson'
        }
    ],

    aspects: [
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'form1ControllPanelMultiselectwindowaspect',
            fieldSelector: '#form1ControllPanel #tfMunicipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#form1ControllPanelMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Группа', xtype: 'gridcolumn', dataIndex: 'Group', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Федеральный номер', xtype: 'gridcolumn', dataIndex: 'FederalNumber', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ОКАТО', xtype: 'gridcolumn', dataIndex: 'OKATO', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'headInspectorMultiSelectWindowAspect',
            fieldSelector: '#form1ControllPanel #tfHead',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#form1ControllPanelHeadSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор руководителя организации',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',

            onRowSelect: function (rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'personInspectorMultiSelectWindowAspect',
            fieldSelector: '#form1ControllPanel #tfPerson',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#form1ControllPanelPersonSelectWindow',
            storeSelect: 'dict.InspectorForSelect',
            storeSelected: 'dict.InspectorForSelected',
            textProperty: 'Fio',
            columnsGridSelect: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор должностного лица',
            titleGridSelect: 'Инспекторы для отбора',
            titleGridSelected: 'Выбранные инспекторы',

            onRowSelect: function (rowModel, record) {
                //Поскольку наша форма множественного выборка должна возвращать только 1 значение
                //То Перекрываем метод select и перед добавлением выделенной записи сначала очищаем стор
                //куда хотим добавить запись
                var grid = this.getSelectedGrid();
                if (grid) {
                    var storeSelected = grid.getStore();
                    storeSelected.removeAll();
                    storeSelected.add(record);
                }
            }
        }
    ],

    validateParams: function () {
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        return (dateStartField && dateStartField.isValid() && dateEndField && dateEndField.isValid());
    },

    getParams: function () {

        var mcpField = Ext.ComponentQuery.query(this.MunicipalityTriggerFieldSelector)[0];
        var dateStartField = Ext.ComponentQuery.query(this.DateStartFieldSelector)[0];
        var dateEndField = Ext.ComponentQuery.query(this.DateEndFieldSelector)[0];
        var headField = Ext.ComponentQuery.query(this.HeadFieldSelector)[0];
        var personField = Ext.ComponentQuery.query(this.PersonFieldSelector)[0];

        return {
            municipalityIds: (mcpField ? mcpField.getValue() : null),
            headId: (headField ? headField.getValue() : null),
            personId: (personField ? personField.getValue() : null),
            dateStart: (dateStartField ? dateStartField.getValue() : null),
            dateEnd: (dateEndField ? dateEndField.getValue() : null)
        };
    }
});