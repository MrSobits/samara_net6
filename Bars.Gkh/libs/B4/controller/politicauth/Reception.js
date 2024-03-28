Ext.define('B4.controller.politicauth.Reception', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.InlineGrid'
    ],

    models: ['politicauth.WorkMode'],

    stores: [
        'politicauth.ReceptionJurPerson',
        'politicauth.ReceptionCitizens'
    ],
    views: ['politicauth.ReceptionPanel'],

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'politicAuthReceptionJurPersonGridAspect',
            storeName: 'politicauth.ReceptionJurPerson',
            modelName: 'politicauth.WorkMode',
            gridSelector: '#politicAuthRecepJurGrid',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) { records.push(rec.data); });
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'PoliticAuthorityWorkMode', {
                    records: Ext.JSON.encode(records),
                    manorgId: asp.controller.params.get('Id')
                })).next(function () {
                    asp.updateGrid();
                    asp.controller.unmask();
                    Ext.Msg.alert('Сохранение!', 'Режимы сохранены успешно');
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'inlinegridaspect',
            name: 'politicAuthReceptionCitizensGridAspect',
            storeName: 'politicauth.ReceptionCitizens',
            modelName: 'politicauth.WorkMode',
            gridSelector: '#politicAuthRecepCitsGrid',

            save: function () {
                var asp = this;

                var store = this.controller.getStore(this.storeName);
                var modifRecords = store.getModifiedRecords();

                var records = [];
                Ext.Array.each(modifRecords, function (rec) { records.push(rec.data); });
                asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                
                B4.Ajax.request(B4.Url.action('AddWorkMode', 'PoliticAuthorityWorkMode', {
                    records: Ext.JSON.encode(records),
                    manorgId: asp.controller.params.get('Id')
                })).next(function () {
                    asp.controller.unmask();
                    asp.updateGrid();
                    Ext.Msg.alert('Сохранение!', 'Режимы сохранены успешно');
                    return true;
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        }
    ],

    params: null,
    mainView: 'politicauth.ReceptionPanel',
    mainViewSelector: '#politicAuthReceptionPanel',
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        this.getStore('politicauth.ReceptionJurPerson').on('beforeload', this.onBeforeLoad, this, 40);
        this.getStore('politicauth.ReceptionCitizens').on('beforeload', this.onBeforeLoad, this, 20);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('politicauth.ReceptionCitizens').load();
        this.getStore('politicauth.ReceptionJurPerson').load();
    },

    onBeforeLoad: function (store, operation, type) {
        if (this.params) {
            operation.params.politicAuthId = this.params.get('Id');
            operation.params.typeMode = type;
        }
    }
});