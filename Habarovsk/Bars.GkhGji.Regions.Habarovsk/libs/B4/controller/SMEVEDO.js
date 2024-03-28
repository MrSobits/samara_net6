Ext.define('B4.controller.SMEVEDO', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
         'B4.aspects.GkhButtonPrintAspect'
    ],

    smevMVD: null,
  
    models: [
        'smev.SMEVEDO',
        'smev.SMEVEDOFile'
    ],
    stores: [
        'smev.SMEVEDOFile',
        'smev.SMEVEDO'
    ],
    views: [

        'smevdo.Grid',
        'smevdo.EditWindow',
        'smevdo.FileInfoGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'smevdoGridAspect',
            gridSelector: 'smevdogrid',
            editFormSelector: '#smevdoEditWindow',
            storeName: 'smev.SMEVEDO',
            modelName: 'smev.SMEVEDO',
            editWindowView: 'smevdo.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevMVD = record.getId();
                    var me = this;                
                    var grid = form.down('smevdofileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevDO', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'smevegrip.Grid',
    mainViewSelector: 'smevdogrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevdogrid'
        },
        {
            ref: 'smevdoFileInfoGrid',
            selector: 'smevdofileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevdogrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }

        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVDOExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVEDO').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVEDO').load();
            return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevdogrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVEDO').load();
    },

    onLaunch: function () {
        if (this.params && this.params.reqId > 0) {
            var model = this.getModel('smev.SMEVEDO');
            this.params.reqId = 0;
        }
    },
});