Ext.define('B4.controller.LoadWorkController', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Url',
        'B4.Ajax'
    ],

    stores: [
        'service.LoadWorkPprRepair',
        'service.LoadWorkToRepair'
    ],
    views: ['service.LoadWorkEditPanel'],
    mainView: 'service.LoadWorkEditPanel',
    mainViewSelector: '#loadWorkEditPanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var actions = {};
        actions['#loadWorkEditPanel #saveWorkPprButton'] = { 'click': { fn: this.btnSaveWorkPprClick, scope: this } };
        actions['#loadWorkEditPanel #reloadWorkPprButton'] = { 'click': { fn: this.btnReloadWorkPprClick, scope: this } };
        actions['#loadWorkEditPanel #saveWorkToButton'] = { 'click': { fn: this.btnSaveWorkToClick, scope: this } };
        actions['#loadWorkEditPanel #reloadWorkToButton'] = { 'click': { fn: this.btnReloadWorkToClick, scope: this } };
        actions['#loadWorkEditPanel #sflRealityObj'] = {
            'change': { fn: this.onChangeRealityObj, scope: this },
            'beforeload': { fn: this.onBeforeLoadRealityObj, scope: this }
        };
        this.control(actions);

        this.getStore('service.LoadWorkPprRepair').on('beforeload', this.onBeforeLoad, this);

        this.getStore('service.LoadWorkToRepair').on('beforeload', this.onBeforeLoad, this);
    },

    onLaunch: function () {
        this.getMainComponent().down('#loadWorkPprGrid').on('beforeedit', this.onBeforeCellEdit, this);
        this.getMainComponent().down('#loadWorkPprGrid').on('edit', this.onAfterCellEdit, this);
        this.getMainComponent().down('#loadWorkToGrid').on('beforeedit', this.onBeforeCellEdit, this);

        this.getStore('service.LoadWorkPprRepair').removeAll();
        this.getStore('service.LoadWorkToRepair').removeAll();
    },

    onBeforeLoadRealityObj: function (field, options) {
        options.params = {};
        if (this.params)
            options.params.disclosureInfoId = this.params.disclosureInfoId;
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.realityObjId = this.params.realityObjId;
            operation.params.disclosureInfoId = this.params.disclosureInfoId;
        }
    },

    btnSaveWorkPprClick: function () {
        var store = this.getMainComponent().down('#loadWorkPprGrid').getStore();
        var modifyRecords = store.getModifiedRecords();

        var records = [];
        Ext.Array.each(modifyRecords, function(value) {
            records.push(
                {
                    Id: Ext.isEmpty(value.get('Id')) ? 0 : value.get('Id'),
                    ServiceId: value.get('ServiceId'),
                    GroupWorkPprId: value.get('GroupWorkPprId'),
                    PlannedCost: value.get('PlannedCost'),
                    PlannedVolume: value.get('PlannedVolume'),
                    FactCost: value.get('FactCost'),
                    FactVolume: value.get('FactVolume'),
                    DateStart: value.get('DateStart'),
                    DateEnd: value.get('DateEnd')
                });
        });

        if (records.length > 0) {
            var me = this;
            me.mask('Сохранение', me.getMainComponent());
            B4.Ajax.request(B4.Url.action('SaveWorkPpr', 'WorkRepairList', {
                records: Ext.encode(records),
                realityObjId: me.params.realityObjId,
                disclosureInfoId: me.params.disclosureInfoId
            })).next(function () {
                me.unmask();
                store.load();
                Ext.Msg.alert('Сохранение', 'Успешно сохранено');
                return true;
            }).error(function () {
                me.unmask();
                Ext.Msg.alert('Ошибка', 'Не удалось сохранить');
            });
        } else {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать объект недвижимости');
        }
    },

    btnSaveWorkToClick: function () {
        var store = this.getMainComponent().down('#loadWorkToGrid').getStore();
        var modifyRecords = store.getModifiedRecords();

        var records = [];
        Ext.Array.each(modifyRecords, function (value) {
        records.push(
            {
                RepairServiceId: value.get('RepairServiceId'),
                SumWorkTo: value.get('SumWorkTo')
            });
        });

        if (records.length > 0) {
            var me = this;
            me.mask('Сохранение', this.getMainComponent());
            B4.Ajax.request(B4.Url.action('SaveRepairService', 'WorkRepairTechServ', {
                records: Ext.encode(records),
                realityObjId: me.params.realityObjId,
                disclosureInfoId: me.params.disclosureInfoId
            })).next(function () {
                me.unmask();
                store.load();
                Ext.Msg.alert('Сохранение', 'Успешно сохранено');
                return true;
            }).error(function () {
                me.unmask();
                Ext.Msg.alert('Ошибка', 'Не удалось сохранить');
            });
        } else {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать объект недвижимости');
        }
    },

    btnReloadWorkPprClick: function () {
        if (this.checkAllowBlankFields()) {
            this.getStore('service.LoadWorkPprRepair').load();
        } else {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать объект недвижимости');
        }
    },

    btnReloadWorkToClick: function () {
        if (this.checkAllowBlankFields()) {
            this.getStore('service.LoadWorkToRepair').load();
        } else {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать объект недвижимости');
        }
    },
    
    checkAllowBlankFields: function() {
        return !Ext.isEmpty(this.getMainComponent().down('#sflRealityObj').getValue());
    },

    onChangeRealityObj: function (tfield, newValue) {
        this.params.realityObjId = newValue.RealityObjectId;
        this.getStore('service.LoadWorkPprRepair').load();
        this.getStore('service.LoadWorkToRepair').load();
    },

    onBeforeCellEdit: function (editor, e) {
        if (e.record.get('TypeColor') == 2) {
            Ext.Msg.alert('Внимание!', 'Не добавлена соответствующая услуга');
            return false;
        }
        return true;
    },

    onAfterCellEdit: function (editor, e) {
        e.record.modified = true;
    }
});