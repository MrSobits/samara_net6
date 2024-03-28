Ext.define('B4.controller.SMEVPremises', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    smevPremises: null,
  
    models: [
        'smevpremises.SMEVPremises',
        'smevpremises.SMEVPremisesFile'
    ],
    stores: [
        'smevpremises.SMEVPremises',
        'smevpremises.SMEVPremisesFile'
    ],
    views: [
        'smevpremises.Grid',
        'smevpremises.EditWindow',
        'smevpremises.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevpremisesGridAspect',
            gridSelector: 'smevpremisesgrid',
            editFormSelector: '#smevpremisesEditWindow',
            storeName: 'smevpremises.SMEVPremises',
            modelName: 'smevpremises.SMEVPremises',
            editWindowView: 'smevpremises.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevpremisesEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
                actions['#smevpremisesEditWindow #showSysFiles'] = { 'change': { fn: this.showSysFiles, scope: this } };
            },

            showSysFiles: function (cb, checked) {
                var form = this.getForm();
                var grid = form.down('smevpremisesfileinfogrid');
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },
            
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    SMEVPremises = record.getId();
                    var grid = form.down('smevpremisesfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevPremises', record.getId());
                }
            },
            get: function (record) {
                var me = this;
                var taskId = smevPremises;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVPremisesExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevpremisesfileinfogrid'),
                        store = grid.getStore();
                        store.filter('smevPremises', smevPremises);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smevpremises.SMEVPremises').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevpremises.Grid',
    mainViewSelector: 'smevpremisesgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevpremisesgrid'
        },
        {
            ref: 'smevpremisesFileInfoGrid',
            selector: 'smevpremisesfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({

            'smevpremisesgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
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
            url: B4.Url.action('Execute', 'SMEVPremisesExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smevpremises.SMEVPremises').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smevpremises.SMEVPremises').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevpremisesGridAspect').editRecord(this.params);
            this.params = null;
        }
    },


    index: function () {
        var view = this.getMainView() || Ext.widget('smevpremisesgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smevpremises.SMEVPremises').load();
    }
});