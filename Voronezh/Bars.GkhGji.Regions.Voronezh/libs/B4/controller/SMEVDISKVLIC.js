Ext.define('B4.controller.SMEVDISKVLIC', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    smevDISKVLIC: null,
  
    models: [
        'smev.SMEVDISKVLIC',
        'smev.SMEVDISKVLICFile'
    ],
    stores: [
        'smev.SMEVDISKVLIC',
        'smev.SMEVDISKVLICFile'
    ],
    views: [
        'smevdiskvlic.Grid',
        'smevdiskvlic.EditWindow',
        'smevdiskvlic.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevdiskvlicGridAspect',
            gridSelector: 'smevdiskvlicgrid',
            editFormSelector: '#smevdiskvlicEditWindow',
            storeName: 'smev.SMEVDISKVLIC',
            modelName: 'smev.SMEVDISKVLIC',
            editWindowView: 'smevdiskvlic.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevdiskvlicEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
            },            
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevDISKVLIC = record.getId();
                    var grid = form.down('smevdiskvlicfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevDISKVLIC', record.getId());
                }
            },
            get: function (record) {
                var me = this;
                var taskId = smevDISKVLIC;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVDISKVLICExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevdiskvlicfileinfogrid'),
                        store = grid.getStore();
                        store.filter('smevDISKVLIC', smevDISKVLIC);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smev.SMEVDISKVLIC').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevdiskvlic.Grid',
    mainViewSelector: 'smevdiskvlicgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevdiskvlicgrid'
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

            'smevdiskvlicgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
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
            url: B4.Url.action('Execute', 'SMEVDISKVLICExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVDISKVLIC').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVDISKVLIC').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevdiskvlicGridAspect').editRecord(this.params);
            this.params = null;
        }
    },


    index: function () {
        var view = this.getMainView() || Ext.widget('smevdiskvlicgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVDISKVLIC').load();
    }
});