Ext.define('B4.controller.CopyCr', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    stores: [
        'copycr.RealityObj',
        'copycr.RealityObjSelect',
        'copycr.RealityObjSelected'
    ],
    views: [
        'copycr.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],
    mainView: 'copycr.EditPanel',
    mainViewSelector: '#copyCrEditPanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'copyCrGridMultiSelectAspect',
            gridSelector: '#realityObjCopyCrGrid',
            storeName: 'copycr.RealityObj',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#copyCrMultiSelectWindow',
            storeSelect: 'copycr.RealityObjSelect',
            storeSelected: 'copycr.RealityObjSelected',
            titleSelectWindow: 'Выбор жилых домов',
            titleGridSelect: 'Жилые дома для отбора',
            titleGridSelected: 'Выбранные дома',
            columnsGridSelect: [
                { header: 'Адрес дома', xtype: 'gridcolumn', dataIndex: 'AddressName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Адрес дома', xtype: 'gridcolumn', dataIndex: 'AddressName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            onBeforeLoad: function(store, operation) {
                operation.params.disclosureInfoId = this.controller.params.disclosureInfoId;
                operation.params.programCrId = this.controller.getMainComponent().down('#sflProgramCr').getValue();
            },
            listeners: {
                getdata: function(asp, records) {
                    var store = this.controller.getStore('copycr.RealityObj');
                    store.removeAll();
                    store.add(records.items);
                }
            },
            rowAction: function(grid, action, record) {
                if (action.toLowerCase() == 'delete') {
                    this.deleteRecord(record);
                }
            }
        }
    ],

    init: function() {
        var actions = {};
        actions['#copyCrEditPanel #loadWorkCrButton'] = { 'click': { fn: this.btnLoadWorkCrClick, scope: this } };
        actions['#copyCrEditPanel #btnCopyCrClear'] = { 'click': { fn: this.btnCopyCrClearClick, scope: this } };
        actions['#copyCrEditPanel #sflProgramCr'] = {
            'change': { fn: this.onChangeProgramCr, scope: this },
            'beforeload': { fn: this.onBeforeLoadProgramCr, scope: this }
        };
        actions[this.mainViewSelector] = { 'afterrender': { fn: this.onMainViewAfterRender, scope: this } };
        this.control(actions);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('copycr.RealityObj').removeAll();
    },

    onMainViewAfterRender: function () {
        if (this.params) {
            var me = this;
            me.mask('Загрузка', this.getMainComponent());
            B4.Ajax.request(B4.Url.action('GetPeriodsDi', 'RealityObjCopyCr', {
                disclosureInfoId: this.params.disclosureInfoId
            })).next(function (response) {
                me.unmask();
                var obj = Ext.decode(response.responseText);
                me.params.dateStartPeriodDi = obj.dateStartPeriodDi;
                me.params.dateEndPeriodDi = obj.dateEndPeriodDi;
                return true;
            });
        }
    },

    onBeforeLoadProgramCr: function(field, options) {
        options.params = {};
        options.params.dateStartPeriodDi = this.params.dateStartPeriodDi;
        options.params.dateEndPeriodDi = this.params.dateEndPeriodDi;        
        options.params.state = true;
    },

    btnLoadWorkCrClick: function() {
        var realityObjIds = [];
        this.getStore('copycr.RealityObj').each(function(obj) {
            realityObjIds.push(obj.get('RealityObjectId'));
        });

        if (realityObjIds.length > 0) {
            var me = this;
            me.mask('Загрузка', this.getMainComponent());
            B4.Ajax.request(B4.Url.action('LoadWorkCr', 'RealityObjCopyCr', {
                realityObjIds: realityObjIds
            })).next(function() {
                me.unmask();
                Ext.Msg.alert('Загрузка', 'Загрузка работ проведена успешно');
                return true;
            }).error(function() {
                me.unmask();
                Ext.Msg.alert('Ошибка', 'Не удалось провести загрузку работ');
            });
        } else {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать один или несколько домов и программу капремонта');
        }
    },

    btnCopyCrClearClick: function() {
        this.getStore('copycr.RealityObj').removeAll();
    },

    onChangeProgramCr: function(tfield, newValue) {
        //this.params.programCrId = newValue.Id;
        this.getStore('copycr.RealityObj').removeAll();
    }
});