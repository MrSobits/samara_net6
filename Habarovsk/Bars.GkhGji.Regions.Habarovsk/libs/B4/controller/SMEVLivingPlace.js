Ext.define('B4.controller.SMEVLivingPlace', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
         'B4.aspects.GkhButtonPrintAspect'
    ],

    smevMVD: null,
  
    models: [
        'smev2.SMEVLivingPlace',
        'smev2.SMEVLivingPlaceFile'
    ],
    stores: [
        'smev2.SMEVLivingPlaceFile',
        'smev2.SMEVLivingPlace'
    ],
    views: [

        'smevlivingplace.Grid',
        'smevlivingplace.EditWindow',
        'smevlivingplace.FileInfoGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'smevlivingplaceGridAspect',
            gridSelector: 'smevlivingplacegrid',
            editFormSelector: '#smevlivingplaceEditWindow',
            storeName: 'smev2.SMEVLivingPlace',
            modelName: 'smev2.SMEVLivingPlace',
            editWindowView: 'smevlivingplace.EditWindow',
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
                    var grid = form.down('smevlivingplacefileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVLivingPlace', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'smevlivingplace.Grid',
    mainViewSelector: 'smevlivingplacegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevlivingplacegrid'
        },
        {
            ref: 'smevlivingplaceFileInfoGrid',
            selector: 'smevlivingplacefileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevlivingplacegrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }

        });

        this.callParent(arguments);
    },

    runexport: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        if (rec.get('RequestState') != 0)
        {
           // Ext.Msg.alert('Внимание', 'Данный запрос уже выполнен или выполняется, повторный запуск невозможен');
          //  return false;
        }
        me.mask('Обмен информацией со СМЭВ', this.getMainComponent());
        //Ext.Msg.alert('Внимание!', 'Запуск экспорта.');

        B4.Ajax.request({
            url: B4.Url.action('LivingPlaceExecute', 'SMEVEGRIPExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev2.SMEVLivingPlace').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev2.SMEVLivingPlace').load();
            return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevlivingplacegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev2.SMEVLivingPlace').load();
    },

    onLaunch: function () {
        if (this.params && this.params.reqId > 0) {
            var model = this.getModel('smev2.SMEVLivingPlace');
            this.getAspect('smevlivingplaceGridAspect').editRecord(new model({ Id: this.params.reqId }));
            this.params.reqId = 0;
        }
    }
});