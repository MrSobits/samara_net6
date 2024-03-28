Ext.define('B4.controller.protocol197.Protocol197', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['protocol197.Protocol197'],
    stores: ['protocol197.Protocol197'],

    views: [
            'protocol197.MainPanel',
            'protocol197.FilterPanel',
            'protocol197.Grid',
            'protocol197.AddWindow'
    ],

    mainView: 'protocol197.MainPanel',
    mainViewSelector: 'protocol197Panel',

    refs: [
        {
            ref: 'mainView',
            selector: 'protocol197Panel'
        }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'protocol197ButtonExportAspect',
            gridSelector: '#protocol197grid',
            buttonSelector: '#protocol197grid #btnExport',
            controllerName: 'Protocol197',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'protocol197StateTransferAspect',
            gridSelector: '#protocol197grid',
            stateType: 'gji_document_prot197',
            menuSelector: 'protocol197GridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('protocol197.Protocol197');
                    model.load(record.getId(), {
                        success: function (rec) {
                            record.set('DocumentNumber', rec.get('DocumentNumber'));
                        },
                        scope: this
                    });
                }
            }
        },
        {

            xtype: 'gkhgrideditformaspect',
            name: 'protocol197GridWindowAspect',
            gridSelector: '#protocol197grid',
            editFormSelector: '#protocol197AddWindow',
            storeName: 'protocol197.Protocol197',
            modelName: 'protocol197.Protocol197',
            editWindowView: 'protocol197.AddWindow',
            controllerEditName: 'B4.controller.protocol197.EditForm',
            otherActions: function (actions) {
                actions['#protocol197FilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#protocol197FilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#protocol197FilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('protocol197.Protocol197');
                str.currentPage = 1;
                str.load();
            },
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            },
            onSaveSuccess: function (aspect, rec) {
                //Закрываем окно после добавления новой записи
                aspect.getForm().close();

                //загружаем добавленный объект
                var model = this.controller.getModel(this.modelName);

                model.load(rec.getId(), {
                    success: function (record) {
                        //После загрузки объекта подменяем параметр и открываем вкладку
                        this.editRecord(record);
                    },
                    scope: this
                });
            }
        }
    ],

    init: function () {
        var me = this;

        me.getStore('protocol197.Protocol197').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },
    
    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('protocol197Panel');

        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date();

        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('protocol197.Protocol197').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;

        if (me.params) {
            operation.params.dateStart = me.params.dateStart;
            operation.params.dateEnd = me.params.dateEnd;
        }
    }
});