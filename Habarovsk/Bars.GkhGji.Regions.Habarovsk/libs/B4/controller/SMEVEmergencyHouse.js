Ext.define('B4.controller.SMEVEmergencyHouse', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    SMEVEmergencyHouse: null,
  
    models: [
        'smevemergencyhouse.SMEVEmergencyHouse',
        'smevemergencyhouse.SMEVEmergencyHouseFile'
    ],
    stores: [
        'smevemergencyhouse.SMEVEmergencyHouse',
        'smevemergencyhouse.SMEVEmergencyHouseFile'
    ],
    views: [
        'smevemergencyhouse.Grid',
        'smevemergencyhouse.EditWindow',
        'smevemergencyhouse.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevemergencyhouseGridAspect',
            gridSelector: 'smevemergencyhousegrid',
            editFormSelector: '#smevemergencyhouseEditWindow',
            storeName: 'smevemergencyhouse.SMEVEmergencyHouse',
            modelName: 'smevemergencyhouse.SMEVEmergencyHouse',
            editWindowView: 'smevemergencyhouse.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevemergencyhouseEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
                actions['#smevemergencyhouseEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRO, scope: this } };
                actions['#smevemergencyhouseEditWindow #cbEmergencyTypeSGIO'] = { 'change': { fn: this.onChangeEmergencyTypeSGIO, scope: this } };
                actions['#smevemergencyhouseEditWindow #showSysFiles'] = { 'change': { fn: this.showSysFiles, scope: this } };
            }, 

            showSysFiles: function (cb, checked) {
                var form = this.getForm();
                var grid = form.down('smevemergencyhousefileinfogrid');
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },

            onChangeEmergencyTypeSGIO: function (field, newValue) {
                var form = this.getForm(),
                    sfRoom = form.down('#sfRoom');                

                if (newValue == B4.enums.EmergencyTypeSGIO.Premice) {                   
                    sfRoom.setDisabled(false);
                    sfRoom.allowBlank = false;


                }
                else {
                    sfRoom.setDisabled(true);
                    sfRoom.allowBlank = true;
                }
            },
            onChangeRO: function (field, newValue) {
                //this.roId = newValue;
                if (newValue != null) {
                    var sfRoom = this.getForm().down('#sfRoom');
                    //sfRoom.setDisabled(false);
                    sfRoom.getStore().filter('RO', newValue.Id);
                }
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    SMEVEmergencyHouse = record.getId();
                    var grid = form.down('smevemergencyhousefileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVEmergencyHouse', record.getId());
                }
            },
            get: function (record) {
                var me = this;
                var taskId = SMEVEmergencyHouse;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVEmergencyHouseExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevemergencyhousefileinfogrid'),
                        store = grid.getStore();
                        store.filter('SMEVEmergencyHouse', SMEVSocialHire);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smevemergencyhouse.SMEVEmergencyHouse').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevemergencyhouse.Grid',
    mainViewSelector: 'smevemergencyhousegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevemergencyhousegrid'
        },
        {
            ref: 'smevemergencyhouseFileInfoGrid',
            selector: 'smevemergencyhousefileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            'smevemergencyhousegrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
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
            url: B4.Url.action('Execute', 'SMEVEmergencyHouseExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smevemergencyhouse.SMEVEmergencyHouse').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smevemergencyhouse.SMEVEmergencyHouse').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevemergencyhouseGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevemergencyhousegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smevemergencyhouse.SMEVEmergencyHouse').load();
    }
});