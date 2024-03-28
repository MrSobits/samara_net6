Ext.define('B4.controller.SMEVChangePremisesState', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    SMEVChangePremisesState: null,
  
    models: [
        'smev.SMEVChangePremisesState',
        'smev.SMEVChangePremisesStateFile'
    ],
    stores: [
        'smev.SMEVChangePremisesState',
        'smev.SMEVChangePremisesStateFile'
    ],
    views: [
        'smevchangepremisesstate.Grid',
        'smevchangepremisesstate.EditWindow',
        'smevchangepremisesstate.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevchangepremisesstateGridAspect',
            gridSelector: 'smevchangepremisesstategrid',
            editFormSelector: '#smevchangepremisesstateEditWindow',
            storeName: 'smev.SMEVChangePremisesState',
            modelName: 'smev.SMEVChangePremisesState',
            editWindowView: 'smevchangepremisesstate.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevchangepremisesstateEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
                actions['#smevchangepremisesstateEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRO, scope: this } };
                actions['#smevchangepremisesstateEditWindow #dfChangePremisesType'] = { 'change': { fn: this.onChangeType, scope: this } };
                actions['#smevchangepremisesstateEditWindow #showSysFiles'] = { 'change': { fn: this.showSysFiles, scope: this } };

            }, 
            showSysFiles: function (cb, checked) {
                var form = this.getForm();
                var grid = form.down('smevchangepremisesstatefileinfogrid');
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },
            onChangeRO: function (field, newValue) {
                //this.roId = newValue;
                if (newValue != null) {
                    var sfRoom = this.getForm().down('#sfRoom');
                    sfRoom.setDisabled(false);
                    sfRoom.getStore().filter('RO', newValue.Id);
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    SMEVChangePremisesState = record.getId();
                    var grid = form.down('smevchangepremisesstatefileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVChangePremisesState', record.getId());
                }
            },
            onChangeType: function (field, newValue) {
                var form = this.getForm(),
                    dfRealRoom = form.down('#dfRealRoom'),
                    dfCadastralNumber = form.down('#dfCadastralNumber')
                    ;

                if (newValue == B4.enums.ChangePremisesType.Address) {
                    dfRealRoom.show();
                    dfRealRoom.setDisabled(false);
                    dfCadastralNumber.hide();
                    dfCadastralNumber.setDisabled(true);
                }
                else if (newValue == B4.enums.ChangePremisesType.CadastralNumber) {
                    dfRealRoom.hide();
                    dfRealRoom.setDisabled(true);
                    dfCadastralNumber.show();
                    dfCadastralNumber.setDisabled(false);
                }
            },
            get: function (record) {
                var me = this;
                var taskId = SMEVChangePremisesState;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVChangePremisesStateExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevchangepremisesstatefileinfogrid'),
                        store = grid.getStore();
                        store.filter('SMEVChangePremisesState', SMEVChangePremisesState);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smev.SMEVChangePremisesState').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevchangepremisesstate.Grid',
    mainViewSelector: 'smevchangepremisesstategrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevchangepremisesstategrid'
        },
        {
            ref: 'smevchangepremisesstateFileInfoGrid',
            selector: 'smevchangepremisesstatefileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            'smevchangepremisesstategrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
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
            url: B4.Url.action('Execute', 'SMEVChangePremisesStateExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVChangePremisesState').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVChangePremisesState').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevchangepremisesstateGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevchangepremisesstategrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVChangePremisesState').load();
    }
});