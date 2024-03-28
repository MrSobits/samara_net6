Ext.define('B4.store.AppealCits', {
    extend: 'B4.base.Store',
    requires: ['B4.model.AppealCits'],
    autoLoad: false,
    model: 'B4.model.AppealCits',

    relatesToId: null, //Загружать только обращения, связанные с указанным Id
    anotherRelatesToId: null, //Загружать только обращения, связанные с указанным Id

    load: function (options) {
        if (this.relatesToId) {
            options = options || {};
            options.params = options.params || { };
            options.params.relatesToId = this.relatesToId;
        }
        if (this.anotherRelatesToId) {
            options = options || {};
            options.params = options.params || {};
            options.params.anotherRelatesToId = this.anotherRelatesToId;
        }

        return this.callParent([options]);
    }
});