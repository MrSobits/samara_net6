Ext.define('B4.controller.SMEVMVD', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhButtonPrintAspect'
    ],

    smevMVD: null,
  
    models: [
        'smev.SMEVMVD',
        'smev.SMEVMVDFile'
    ],
    stores: [
        'smev.SMEVMVDFile',
        'smev.SMEVMVD'
    ],
    views: [

        'smevmvd.Grid',
        'smevmvd.EditWindow',
        'smevmvd.FileInfoGrid'

    ],

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'SMEVMVDPrintAspect',
            buttonSelector: '#smevmvdEditWindow #btnPrint',
            codeForm: 'SMEVMVD',
            getUserParams: function () {
                var param = { Id: this.controller.smevMVD };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'smevmvdGridAspect',
            gridSelector: 'smevmvdgrid',
            editFormSelector: '#smevmvdEditWindow',
            storeName: 'smev.SMEVMVD',
            modelName: 'smev.SMEVMVD',
            editWindowView: 'smevmvd.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var me = this;
                    me.controller.getAspect('SMEVMVDPrintAspect').loadReportStore();
                    asp.controller.smevMVD = record.getId();
                    var grid = form.down('smevmvdfileinfogrid'),
                        store = grid.getStore();
                    store.filter('smevMVD', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'smevmvd.Grid',
    mainViewSelector: 'smevmvdgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevmvdgrid'
        },
        {
            ref: 'smevmvdFileInfoGrid',
            selector: 'smevmvdfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevmvdgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }
        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0)
        {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('Execute', 'SMEVMVDExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.SMEVMVD').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.SMEVMVD').load();
            return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevmvdgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.SMEVMVD').load();
    }
});