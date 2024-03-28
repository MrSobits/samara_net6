Ext.define('B4.controller.SSTUExportTask', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    sSTUExportTask: null,

    models: [
        'sstuexporttask.SSTUExportTask',
        'sstuexporttask.SSTUExportTaskAppeal'
    ],
    stores: [
        'sstuexporttask.SSTUExportTask',
        'sstuexporttask.SSTUExportTaskAppeal',
        'sstuexporttask.SSTUExportTaskAppealForSelect',
        'sstuexporttask.SSTUExportTaskAppealForSelected'
    ],
    views: [

        'sstuexporttask.SSTUExportTaskGrid',
        'sstuexporttask.SSTUExportTaskEditWindow',
        'sstuexporttask.SSTUExportTaskAppealGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'sstuExportTaskGridAspect',
            gridSelector: 'sstuexporttaskgrid',
            editFormSelector: '#sSTUExportTaskEditWindow',
            storeName: 'sstuexporttask.SSTUExportTask',
            modelName: 'sstuexporttask.SSTUExportTask',
            editWindowView: 'sstuexporttask.SSTUExportTaskEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    debugger;
                    sSTUExportTask = record.getId();
                    var grid = form.down('sstuexporttaskappealgrid'),
                        store = grid.getStore();
                    store.filter('SSTUExportTask', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
        {
            /* 
            Аспект взаимодействия таблицы предоставляемых документов с массовой формой выбора предоставляемых документов
            По нажатию на Добавить открывается форма выбора предоставляемых документов.
            По нажатию Применить на форме массовго выбора идет обработка выбранных строк в getdata
            И сохранение предоставляемых документов
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'sSTUExportTaskAppealAspect',
            gridSelector: 'sstuexporttaskappealgrid',
            storeName: 'sstuexporttask.SSTUExportTaskAppeal',
            modelName: 'sstuexporttask.SSTUExportTaskAppeal',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#sSTUExportTaskAppealForSelectedMultiSelectWindow',
            storeSelect: 'sstuexporttask.SSTUExportTaskAppealForSelect',
            storeSelected: 'sstuexporttask.SSTUExportTaskAppealForSelected',
            titleSelectWindow: 'Выбор обращений',
            titleGridSelect: 'Обращения для экспорта',
            titleGridSelected: 'Выбранные обращения',
            columnsGridSelect: [
                { header: 'Номер обращения', xtype: 'gridcolumn', dataIndex: 'AppealNum', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Дата обращения', xtype: 'gridcolumn', dataIndex: 'AppealDate', flex: 0.5, filter: { xtype: 'datefield' } },
                { header: 'Статус', xtype: 'gridcolumn', dataIndex: 'AppState', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Номер обращения', xtype: 'gridcolumn', dataIndex: 'AppealNum', flex: 1, sortable: false },
                { header: 'Дата обращения', xtype: 'gridcolumn', dataIndex: 'AppealDate', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                operation.params.sstuExportTaskId = sSTUExportTask;
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec, index) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        debugger;
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        debugger;
                        B4.Ajax.request(B4.Url.action('AddAppeal', 'SSTUExportTaskAppeal', {
                            appealIds: recordIds,
                            taskId: sSTUExportTask
                        })).next(function (response) {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],

    mainView: 'sstuexporttask.SSTUExportTaskGrid',
    mainViewSelector: 'sstuexporttaskgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'sstuexporttaskgrid'
        },
        {
            ref: 'sSTUExportTaskAppealGrid',
            selector: 'sstuexporttaskappealgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {

        this.control({

            'sstuexporttaskgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;

        me.mask('Экспорт', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');
        B4.Ajax.request(B4.Url.action('Execute', 'SSTUExportTaskExecute', {
            taskId: rec.getId()
        }
        )).next(function (response) {
            me.unmask();
            me.getStore('sstuexporttask.SSTUExportTask').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('sstuexporttask.SSTUExportTask').load();
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('sstuexporttaskgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('sstuexporttask.SSTUExportTask').load();
    }
});