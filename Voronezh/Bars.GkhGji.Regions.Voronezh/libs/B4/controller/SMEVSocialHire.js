Ext.define('B4.controller.SMEVSocialHire', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    SMEVSocialHire: null,
  
    models: [
        'smev.SMEVSocialHire',
        'smev.SMEVSocialHireFile'
    ],
    stores: [
        'smev.SMEVSocialHire',
        'smev.SMEVSocialHireFile'
    ],
    views: [
        'smevsocialhire.Grid',
        'smevsocialhire.EditWindow',
        'smevsocialhire.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'smevsocialhireGridAspect',
            gridSelector: 'smevsocialhiregrid',
            editFormSelector: '#smevsocialhireEditWindow',
            storeName: 'smev.SMEVSocialHire',
            modelName: 'smev.SMEVSocialHire',
            editWindowView: 'smevsocialhire.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevsocialhireEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
                actions['#smevsocialhireEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRO, scope: this } };
                actions['#smevsocialhireEditWindow #showSysFiles'] = { 'change': { fn: this.showSysFiles, scope: this } };
            },

            showSysFiles: function (cb, checked) {
                var form = this.getForm();
                var grid = form.down('smevsocialhirefileinfogrid');
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
                    SMEVSocialHire = record.getId();
                    var grid = form.down('smevsocialhirefileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVSocialHire', record.getId());
                }
            },
            get: function (record) {
                var me = this;
                var taskId = SMEVSocialHire;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'SMEVSocialHireExecute', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('smevsocialhirefileinfogrid'),
                        store = grid.getStore();
                        store.filter('SMEVSocialHire', SMEVSocialHire);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('smev.SMEVSocialHire').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'smevsocialhire.Grid',
    mainViewSelector: 'smevsocialhiregrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevsocialhiregrid'
        },
        {
            ref: 'smevsocialhireFileInfoGrid',
            selector: 'smevsocialhirefileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
            'smevsocialhiregrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
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
            url: B4.Url.action('Execute', 'SMEVSocialHireExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVSocialHire').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVSocialHire').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevsocialhireGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevsocialhiregrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVSocialHire').load();
    }
});