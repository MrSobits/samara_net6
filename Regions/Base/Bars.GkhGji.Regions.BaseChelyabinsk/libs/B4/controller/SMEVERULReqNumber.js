Ext.define('B4.controller.SMEVERULReqNumber', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
    ],

    smevEGRUL: null,

    models: [
        'smev.SMEVERULReqNumber',
        'smev.SMEVERULReqNumberFile'
    ],
    stores: [
        'smev.SMEVERULReqNumberFile',
        'smev.SMEVERULReqNumber'
    ],
    views: [

        'smeverul.Grid',
        'smeverul.EditWindow',
        'smeverul.FileInfoGrid'

    ],

    aspects: [
       
        {
            xtype: 'grideditwindowaspect',
            name: 'smeverulGridAspect',
            gridSelector: 'smeverulgrid',
            editFormSelector: '#smeverulEditWindow',
            storeName: 'smev.SMEVERULReqNumber',
            modelName: 'smev.SMEVERULReqNumber',
            editWindowView: 'smeverul.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },           
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevEGUL = record.getId();
                    var me = this;
                    var grid = form.down('smeverulfileinfogrid'),
                        store = grid.getStore();
                    store.filter('smevERUL', record.getId());                  
                }
            }           
        },

    ],

    mainView: 'smeverul.Grid',
    mainViewSelector: 'smeverulgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smeverulgrid'
        },
        {
            ref: 'smeverulFileInfoGrid',
            selector: 'smeverulfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({

            'smeverulgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
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

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVERULExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.decode(response.responseText);
            Ext.Msg.alert('Сообщение', data.data);

            me.unmask();
            me.getStore('smev.SMEVERULReqNumber').load();
            return true;
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
            me.unmask();

            me.getStore('smev.SMEVERULReqNumber').load();
            return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smeverulgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVERULReqNumber').load();
    }
});