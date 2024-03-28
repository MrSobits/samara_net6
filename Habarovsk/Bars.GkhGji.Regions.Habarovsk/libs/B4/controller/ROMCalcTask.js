Ext.define('B4.controller.ROMCalcTask', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    rOMCalcTask: null,
  

    models: [
        'romcalctask.ROMCalcTask',
        'romcalctask.ROMCalcTaskManOrg'
    ],
    stores: [
        'romcalctask.ROMCalcTask',
        'romcalctask.ROMCalcTaskManOrg',
         'romcalctask.ROMCalcTaskManOrgForSelect',
        'romcalctask.ROMCalcTaskManOrgForSelected'
    ],
    views: [

        'romcalctask.ROMCalcTaskGrid',
        'romcalctask.ROMCalcTaskEditWindow',
        'romcalctask.ROMCalcTaskManOrgGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'romCalcTaskGridAspect',
            gridSelector: 'romcalctaskgrid',
            editFormSelector: '#rOMCalcTaskEditWindow',
            storeName: 'romcalctask.ROMCalcTask',
            modelName: 'romcalctask.ROMCalcTask',
            editWindowView: 'romcalctask.ROMCalcTaskEditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    rOMCalcTask = record.getId();
                    var grid = form.down('romcalctaskmanorggrid'),
                    store = grid.getStore();
                    store.filter('ROMCalcTask', record.getId());
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
             name: 'rOMCalcTaskManOrgAspect',
             gridSelector: 'romcalctaskmanorggrid',
             storeName: 'romcalctask.ROMCalcTaskManOrg',
             modelName: 'romcalctask.ROMCalcTaskManOrg',
             multiSelectWindow: 'SelectWindow.MultiSelectWindow',
             multiSelectWindowSelector: '#rOMCalcTaskManOrgForSelectedMultiSelectWindow',
             storeSelect: 'romcalctask.ROMCalcTaskManOrgForSelect',
             storeSelected: 'romcalctask.ROMCalcTaskManOrgForSelected',
             titleSelectWindow: 'Выбор организаций',
             titleGridSelect: 'Организации для расчета',
             titleGridSelected: 'Выбранные организации',
             columnsGridSelect: [
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                 { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'datefield' } }
             ],
             columnsGridSelected: [
                 { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, sortable: false },
                 { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, sortable: false }
             ],
             onBeforeLoad: function (store, operation) {
                 operation.params.rOMCalcTaskId = rOMCalcTask;
             },
             listeners: {
                 getdata: function (asp, records) {
                     var recordIds = [];
                     records.each(function (rec, index) {
                         recordIds.push(rec.get('Id'));
                     });

                     if (recordIds[0] > 0) {
                         asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                         B4.Ajax.request(B4.Url.action('AddManOrg', 'ROMCalcTaskManOrg', {
                             manorgIds: recordIds,
                             taskId: rOMCalcTask
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
         },
    ],

    mainView: 'romcalctask.ROMCalcTaskGrid',
    mainViewSelector: 'romcalctaskgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'romcalctaskgrid'
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
           
            'romcalctaskgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('CalcState') == "Рассчитано")
        {
            Ext.Msg.alert('Внимание','Данная задача уже выполнена, повторный расчет запрещен');
        }
        me.mask('Расчет', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');
        B4.Ajax.request(B4.Url.action('Execute', 'ROMCalcTaskExecute', {
            taskId: rec.getId()
        }
        )).next(function (response) {
            me.unmask();
            me.getStore('romcalctask.ROMCalcTask').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('romcalctask.ROMCalcTask').load();
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('romcalctaskgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('romcalctask.ROMCalcTask').load();
    }
});