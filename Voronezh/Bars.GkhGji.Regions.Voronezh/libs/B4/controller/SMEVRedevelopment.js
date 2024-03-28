Ext.define('B4.controller.SMEVRedevelopment', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    SMEVRedevelopment: null,
  
    models: [
        'smevredevelopment.SMEVRedevelopment',
        'smevredevelopment.SMEVRedevelopmentFile'
    ],
    stores: [
        'smevredevelopment.SMEVRedevelopment',
        'smevredevelopment.SMEVRedevelopmentFile'
    ],
    views: [
        'smevredevelopment.Grid',
        'smevredevelopment.EditWindow',
        'smevredevelopment.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevredevelopmentGridAspect',
            gridSelector: 'smevredevelopmentgrid',
            editFormSelector: '#smevredevelopmentEditWindow',
            storeName: 'smevredevelopment.SMEVRedevelopment',
            modelName: 'smevredevelopment.SMEVRedevelopment',
            editWindowView: 'smevredevelopment.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevredevelopmentEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
                actions['#smevredevelopmentEditWindow #showSysFiles'] = { 'change': { fn: this.showSysFiles, scope: this } };
            },    
            showSysFiles: function (cb, checked) {
                var form = this.getForm();
                var grid = form.down('smevredevelopmentfileinfogrid');
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    SMEVRedevelopment = record.getId();
                    var grid = form.down('smevredevelopmentfileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVRedevelopment', record.getId());
                }
            },
            get: function (record) {
                var me = this;
                var taskId = SMEVRedevelopment;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVRedevelopmentExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevredevelopmentfileinfogrid'),
                        store = grid.getStore();
                        store.filter('SMEVRedevelopment', SMEVRedevelopment);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                        me.getStore('smevredevelopment.SMEVRedevelopment').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevredevelopment.Grid',
    mainViewSelector: 'smevredevelopmentgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevredevelopmentgrid'
        },
        {
            ref: 'smevredevelopmentFileInfoGrid',
            selector: 'smevredevelopmentfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            'smevredevelopmentgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;

        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVRedevelopmentExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smevredevelopment.SMEVRedevelopment').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smevredevelopment.SMEVRedevelopment').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevredevelopmentGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevredevelopmentgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smevredevelopment.SMEVRedevelopment').load();
    }
});