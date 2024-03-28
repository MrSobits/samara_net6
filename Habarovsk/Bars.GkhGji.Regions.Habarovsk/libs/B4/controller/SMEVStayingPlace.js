Ext.define('B4.controller.SMEVStayingPlace', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
         'B4.aspects.GkhButtonPrintAspect'
    ],

    smevMVD: null,
  
    models: [
        'smev2.SMEVStayingPlace',
        'smev2.SMEVStayingPlaceFile'
    ],
    stores: [
        'smev2.SMEVStayingPlaceFile',
        'smev2.SMEVStayingPlace'
    ],
    views: [

        'smevstayingplace.Grid',
        'smevstayingplace.EditWindow',
        'smevstayingplace.FileInfoGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'smevstayingplaceGridAspect',
            gridSelector: 'smevstayingplacegrid',
            editFormSelector: '#smevstayingplaceEditWindow',
            storeName: 'smev2.SMEVStayingPlace',
            modelName: 'smev2.SMEVStayingPlace',
            editWindowView: 'smevstayingplace.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevMVD = record.getId();
                    var me = this;
                    var grid = form.down('smevstayingplacefileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVStayingPlace', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'smevstayingplace.Grid',
    mainViewSelector: 'smevstayingplacegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevstayingplacegrid'
        },
        {
            ref: 'smevstayingplaceFileInfoGrid',
            selector: 'smevstayingplacefileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevstayingplacegrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }

        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        debugger;
        if (rec.get('RequestState') != 0)
        {
            Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
            return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('StayingPlaceExecute', 'SMEVEGRIPExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev2.SMEVStayingPlace').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev2.SMEVStayingPlace').load();
            return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevstayingplacegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev2.SMEVStayingPlace').load();
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.reqId > 0) {
            var model = this.getModel('smev2.SMEVStayingPlace');
            this.getAspect('smevstayingplaceGridAspect').editRecord(new model({ Id: this.params.reqId }));
            this.params.reqId = 0;
        }
        else if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevstayingplaceGridAspect').editRecord(this.params);
            this.params = null;
        }
    },
});