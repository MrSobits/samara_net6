Ext.define('B4.controller.SMEVSNILS', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    SMEVSNILS: null,
  
    models: [
        'smev.SMEVSNILS',
        'smev.SMEVSNILSFile'
    ],
    stores: [
        'smev.SMEVSNILS',
        'smev.SMEVSNILSFile'
    ],
    views: [
        'smevsnils.Grid',
        'smevsnils.EditWindow',
        'smevsnils.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevsnilsGridAspect',
            gridSelector: 'smevsnilsgrid',
            editFormSelector: '#smevsnilsEditWindow',
            storeName: 'smev.SMEVSNILS',
            modelName: 'smev.SMEVSNILS',
            editWindowView: 'smevsnils.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevdiskvlicEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
            },            
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    SMEVSNILS = record.getId();
                    var grid = form.down('smevsnilsfileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVSNILS', record.getId());
                }
            },
            get: function (record) {
                var me = this;
                var taskId = SMEVSNILS;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVSNILSExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevdiskvlicfileinfogrid'),
                        store = grid.getStore();
                        store.filter('SMEVSNILS', SMEVSNILS);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                        me.getStore('smev.SMEVSNILS').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevsnils.Grid',
    mainViewSelector: 'smevsnilsgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevsnilsgrid'
        },
        {
            ref: 'smevdiskvlicFileInfoGrid',
            selector: 'smevdiskvlicfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({

            'smevsnilsgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0) {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVSNILSExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVSNILS').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVSNILS').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevsnilsGridAspect').editRecord(this.params);
            this.params = null;
        }
    },


    index: function () {
        var view = this.getMainView() || Ext.widget('smevsnilsgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVSNILS').load();
    }
});