Ext.define('B4.controller.SMEVPropertyType', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
         'B4.aspects.GkhButtonPrintAspect'
    ],

    smevMVD: null,
  
    models: [
        'smev2.SMEVPropertyType',
        'smev2.SMEVPropertyTypeFile'
    ],
    stores: [
        'smev2.SMEVPropertyTypeFile',
        'smev2.SMEVPropertyType'
    ],
    views: [

        'smevpropertytype.Grid',
        'smevpropertytype.EditWindow',
        'smevpropertytype.FileInfoGrid'

    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'smevpropertytypeGridAspect',
            gridSelector: 'smevpropertytypegrid',
            editFormSelector: '#smevpropertytypeEditWindow',
            storeName: 'smev2.SMEVPropertyType',
            modelName: 'smev2.SMEVPropertyType',
            editWindowView: 'smevpropertytype.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#smevpropertytypeEditWindow #sfRealityObject'] = { 'change': { fn: this.onChangeRO, scope: this } };
            },
            onChangeRO: function (field, newValue) {
                //this.roId = newValue;
                var sfRoom = this.getForm().down('#sfRoom');
                sfRoom.setDisabled(false);
                sfRoom.getStore().filter('RO', newValue.Id);
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    smevMVD = record.getId();
                    var me = this;
                    var grid = form.down('smevpropertytypefileinfogrid'),
                    store = grid.getStore();
                    store.filter('SMEVPropertyType', record.getId());
                }
                //aftersetformdata: function (asp, record, form) {
                //    appealCitsAdmonition = record.getId();
                //}
            }
        },
    ],

    mainView: 'smevpropertytype.Grid',
    mainViewSelector: 'smevpropertytypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'smevpropertytypegrid'
        },
        {
            ref: 'smevpropertytypeFileInfoGrid',
            selector: 'smevpropertytypefileinfogrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.control({
           
            'smevpropertytypegrid actioncolumn[action="openpassport"]': { click: { fn: this.runexport, scope: this } }

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
            url: B4.Url.action('PropTypeExecute', 'SMEVEGRIPExecute'),
            params: {
                taskId: rec.getId()
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev2.SMEVPropertyType').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev2.SMEVPropertyType').load();
            return false;
        });
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('smevpropertytypegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev2.SMEVPropertyType').load();
    },

    onLaunch: function () {
        if (this.params && this.params.reqId > 0) {
            var model = this.getModel('smev2.SMEVPropertyType');
            this.getAspect('smevpropertytypeGridAspect').editRecord(new model({ Id: this.params.reqId }));
            this.params.reqId = 0;
        }
    },
});