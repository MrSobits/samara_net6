Ext.define('B4.controller.SMEVValidPassport', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
         'B4.aspects.GkhButtonPrintAspect'
    ],

    smevMVD: null,
  
    models: [
        'smev2.SMEVValidPassport',
        'smev2.SMEVValidPassportFile'
    ],
    stores: [
        'smev2.SMEVValidPassportFile',
        'smev2.SMEVValidPassport'
    ],
    views: [

        'smevvalidpassport.Grid',
        'smevvalidpassport.EditWindow',
        'smevvalidpassport.FileInfoGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'smevvalidpassportGridAspect',
            gridSelector: 'smevvalidpassportgrid',
            editFormSelector: '#smevvalidpassportEditWindow',
            storeName: 'smev2.SMEVValidPassport',
            modelName: 'smev2.SMEVValidPassport',
            editWindowView: 'smevvalidpassport.EditWindow',
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
                    var grid = form.down('smevvalidpassportfileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVValidPassport', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'smevvalidpassport.Grid',
    mainViewSelector: 'smevvalidpassportgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevvalidpassportgrid'
        },
        {
            ref: 'smevvalidpassportFileInfoGrid',
            selector: 'smevvalidpassportfileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevvalidpassportgrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }

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
            url: B4.Url.action('ValidPassportExecute', 'SMEVEGRIPExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev2.SMEVValidPassport').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev2.SMEVValidPassport').load();
            return false;
        });
    },

    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('smevvalidpassportGridAspect').editRecord(this.params);
            this.params = null;
        }
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevvalidpassportgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev2.SMEVValidPassport').load();
    }

});