Ext.define('B4.controller.localgov.Reception', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.InlineGrid'
    ],

    models: ['localgov.WorkMode'],

    stores: [
        'localgov.ReceptionJurPerson',
        'localgov.ReceptionCitizens'
    ],
    views: ['localgov.ReceptionPanel'],

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'localGovReceptionJurPersonGridAspect',
            storeName: 'localgov.ReceptionJurPerson',
            modelName: 'localgov.WorkMode',
            gridSelector: '#receptionJurGrid',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) { records.push(rec.data); });
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'LocalGovernmentWorkMode', {
                    records: Ext.JSON.encode(records),
                    manorgId: asp.controller.params.get('Id')
                })).next(function () {
                    asp.updateGrid();
                    asp.controller.unmask();
                    B4.QuickMsg.msg('Сохранение!', 'Режимы сохранены успешно', 'success');
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'localGovReceptionCitizensGridAspect',
            storeName: 'localgov.ReceptionCitizens',
            modelName: 'localgov.WorkMode',
            gridSelector: '#receptionCitizensGrid',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) { records.push(rec.data); });
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'LocalGovernmentWorkMode', {
                    records: Ext.JSON.encode(records),
                    manorgId: asp.controller.params.get('Id')
                })).next(function () {
                    asp.controller.unmask();
                    asp.updateGrid();
                    B4.QuickMsg.msg('Сохранение!', 'Режимы сохранены успешно', 'success');
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        }
    ],

    params: null,
    mainView: 'localgov.ReceptionPanel',
    mainViewSelector: '#localGovReceptionPanel',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.getStore('localgov.ReceptionJurPerson').on('beforeload', this.onBeforeLoad, this, 40);
        this.getStore('localgov.ReceptionCitizens').on('beforeload', this.onBeforeLoad, this, 20);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('localgov.ReceptionCitizens').load();
        this.getStore('localgov.ReceptionJurPerson').load();
    },

    onBeforeLoad: function (store, operation, type) {
        if (this.params) {
            operation.params.localGovId = this.params.get('Id');
            operation.params.typeMode = type;
        }
    }
});