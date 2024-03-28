Ext.define('B4.controller.LicenseAction', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextMenu'
    ],

    licenseActionId: null,
  
    models: [
        'licenseaction.LicenseAction',
        'licenseaction.LicenseActionFile'
    ],
    stores: [
        'licenseaction.LicenseAction',
        'licenseaction.LicenseActionFile'
    ],
    views: [
        'licenseaction.Grid',
        'licenseaction.EditWindow',
        'licenseaction.FileInfoGrid'
    ],

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'licenseactionStateTransferAspect',
            gridSelector: 'licenseactiongrid',
            menuSelector: 'licenseactiongridStateMenu',
            stateType: 'gji_license_action'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'licenseactionGridAspect',
            gridSelector: 'licenseactiongrid',
            editFormSelector: '#licenseactionEditWindow',
            storeName: 'licenseaction.LicenseAction',
            modelName: 'licenseaction.LicenseAction',
            editWindowView: 'licenseaction.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#licenseactionEditWindow #sendGetrequestButton'] = { 'click': { fn: this.get, scope: this } };
            },            
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    licenseActionId = record.getId();
                    debugger;
                    var grid = form.down('licenseactionfileinfogrid'),
                        store = grid.getStore();
                    store.on('beforeload', function (store, operation) {
                        operation.params.licenseActionId = record.getId();
                    },
                        me);
                    store.load();
                    //store.filter('licenseActionId', record.getId());
                }
            },
            get: function (record) {
                var me = this;
                var taskId = licenseActionId;
                var form = this.getForm();
                var dfAnswerGet = form.down('#dfAnswerGet');

                if (1 == 2) {
                    Ext.Msg.alert('Внимание!', 'Перед выгрузкой необходимо сохранить запись');
                }
                else {
                    me.mask('Получение информации', me.getForm());
                    var result = B4.Ajax.request(B4.Url.action('GetResponse', 'EXECUTE(!)', {
                        taskId: taskId
                    }
                    )).next(function (response) {                        
                        var data = Ext.decode(response.responseText);
                        Ext.Msg.alert('Сообщение', data.data);

                        var data = Ext.decode(response.responseText);
                        var grid = form.down('licenseactionfileinfogrid'),
                        store = grid.getStore();
                        store.filter('licenseActionId', licenseActionId);

                        dfAnswerGet.setValue(data.data.answer);
                        me.unmask();
                        return true;
                    }).error(function (response) {
                            Ext.Msg.alert('Ошибка', response.message);
                            me.unmask();
                            me.getStore('licenseaction.LicenseAction').load();
                    });

                }
            }
        },
   
    ],

    mainView: 'licenseaction.Grid',
    mainViewSelector: 'licenseactiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'licenseactiongrid'
        },
        {
            ref: 'licenseactionFileInfoGrid',
            selector: 'licenseactionfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({

            'licenseactiongrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0) {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'EXECUTE(!)'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('licenseaction.LicenseAction').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('licenseaction.LicenseAction').load();
            return false;
        });
    },


    index: function () {
        var view = this.getMainView() || Ext.widget('licenseactiongrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('licenseaction.LicenseAction').load();
    }
});