//  Аспект взаимодействия кнопки 'Добавить' в гриде 'Нарушения' Акта проверки с массовой формой выбора нарушений
Ext.define('B4.aspects.ActCheckActionViolation', {
    extend: 'B4.aspects.GkhButtonMultiSelectWindow',
    alias: 'widget.actcheckactionviolationaspect',
    buttonSelector: '', // Задать селектор
    multiSelectWindowSelector: '',  // Задать селектор
    multiSelectWindow: 'SelectWindow.MultiSelectWindow',
    storeSelect: 'dict.ViolationGjiForSelect',
    storeSelected: 'dict.ViolationGjiForSelected',
    inlineGridAspectName: '', // Задать наименование аспекта, где используются нарушения
    columnsGridSelect: [
        {
            header: 'Код ПиН',
            flex: 1,
            xtype: 'gridcolumn',
            dataIndex: 'CodePin',
            filter: { xtype: 'textfield' },
            sortable: false
        },
        {
            header: 'Наименование',
            xtype: 'gridcolumn',
            dataIndex: 'Name',
            flex: 3,
            filter: {
                xtype: 'textfield'
            },
            sortable: false,
            renderer: function(v, m) {
                m.tdAttr = 'data-qtip="' + v + '"';
                return v;
            }
        }
    ],
    columnsGridSelected: [
        { header: 'Код ПиН', xtype: 'gridcolumn', flex: 1, dataIndex: 'CodePin', sortable: false },
        {
            header: 'Наименование',
            xtype: 'gridcolumn',
            dataIndex: 'Name',
            flex: 2,
            filter: {
                xtype: 'textfield'
            },
            sortable: false,
            renderer: function(v, m) {
                m.tdAttr = 'data-qtip="' + v + '"';
                return v;
            }
        }
    ],
    titleSelectWindow: 'Выбор нарушений',
    titleGridSelect: 'Нарушения для отбора',
    titleGridSelected: 'Выбранные нарушения',
    onBeforeLoad: function(store, operation) {
        if (this.controller.params && this.controller.params.documentId > 0) {
            operation.params.documentId = this.controller.params.documentId;
            operation.params.forSelect = true;
        }
    },
    listeners: {
        getdata: function(asp, records) {
            var me = this,
                recordIds = [],
                controller = asp.controller;

            records.each(function(rec) {
                recordIds.push(rec.get('Id'));
            });

            if (recordIds[0] > 0) {
                controller.mask('Сохранение', controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('AddViolations', 'ActCheckActionViolation', {
                        violationIds: Ext.encode(recordIds),
                        Id: controller.getCurrentActCheckAction()
                    }))
                    .next(function(response) {
                        controller.unmask();
                        controller.getAspect(me.inlineGridAspectName).getStore().load();
                    })
                    .error(function(err) {
                        Ext.Msg.alert('Ошибка!', 'Ошибка сохранения');
                        controller.unmask();
                        return false;
                    });
            } else {
                Ext.Msg.alert('Ошибка!', 'Необходимо выбрать нарушения');
                return false;
            }
            return true;
        }
    }
})
