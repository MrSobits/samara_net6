/**
 * Такой лисапед, потому что в инлайн гридах поле b4selectfield не работало ввиду того,
 * что каждое поле редактора оборачивается в CellEditor, который, в свою очередь, определяет окончание
 * редактирования по событию потери фокуса полем редактирования. Тут и кроется корень проблемы —
 * SelectField бодро теряет фокус в тот момент, когда пользователь щелкает где-нибудь в
 * открывшемся окне выбора значения.
 * Пример использования: полностю аналогичен использованию чистого b4selectfield, все параметры
 * конфигурации прокидываются непосредственно в его конструктор.
 */
Ext.define('B4.grid.SelectFieldEditor', {
    extend: 'Ext.grid.CellEditor',
    alias: 'widget.b4selectfieldeditor',

    requires: ['B4.form.SelectField'],

    constructor: function (config) {
        config = Ext.apply({}, { field: config });

        if (config.field) {
            config.field.xtype = 'b4selectfield';
        }

        this.callParent([config]);
    },

    afterRender: function() {
        var me = this,
            field = me.field;

        me.callParent(arguments);
        field.mon(field, {
            windowcreated: me.onWindowCreated,
            change: me.onValueChanged,
            scope: me
        });
    },

    onWindowCreated: function(field, window) {
        this.completeEdit = Ext.emptyFn;
        window.on('close', this.onWindowClosed, this);
    },

    onValueChanged: function () {
        delete this.completeEdit;
        this.field.focus(false, 10);
    },

    onWindowClosed: function(window) {
        window.un('close', this.onWindowClosed, this);
        this.onValueChanged();
    }
});