/**
 * Модель для выборки записей в гриде, отличается от экстогого тем, что
 * умеет кешировать записи при пейджинации.
 *
 * @example
 *  {
 *      xtype: 'b4grid,
 *      selType: 'b4checkboxmodel',
 *      multiSelect: true
 *  }
 */
Ext.define('B4.ux.grid.selection.CheckboxModel', {
    alias: 'selection.b4checkboxmodel',
    extend: 'Ext.selection.CheckboxModel',

    idProperty: 'Id',

    injectCheckbox: 0,

    mode: 'MULTI',

    pruneRemoved: false,

    checkOnly: false,

    isSelectedAll: false,

    allowDeselect: true,
    deselectAllOnDeselect: true,

    constructor: function (config) {
        config = Ext.apply(this, config);

        if (this.mode.toUpperCase() !== 'MULTI'.toUpperCase()) {
            this.showHeaderCheckbox = false;
        }

        this.callParent(config);
    },

    selectAll: function (suppressEvent) {
        var me = this,
            selections = me.store.getRange(),
            start = me.getSelection().length;

        me.bulkChange = true;

        me.doSelect(selections, true, suppressEvent);

        delete me.bulkChange;

        me.maybeFireSelectionChange(me.getSelection().length !== start);

        this.isSelectedAll = true;
    },

    /**
    * deselectAll отключен, потому что в эксте происходит deselect перед
    * загрузкой новых записей.
    * Поэтому вместо deselectAll используется b4deselectAll
    */
    deselectAll: Ext.emptyFn,

    b4deselectAll: function () {
        this.superclass.deselectAll.call(this);

        this.isSelectedAll = false;
        this.fireEvent('deselectall', this);
    },

    doDeselect: function (records, suppressEvent) {
        var me = this,
            isSelectedAll = me.isSelectedAll,
            selected = me.selected,
            len;

        len = records.length;

        if (len > 1) {
            me.isSelectedAll = false;

            if (me.deselectAllOnDeselect && isSelectedAll) {
                me.b4deselectAll();
            }
        }
        return me.callParent(arguments);
    },

    doMultiSelect: function (record, keepExisting, suppressEvent) {
        var me = this,
            selected = me.selected,
            change = false,
            i = 0,
            len, records;

        if (me.locked) {
            return;
        }

        if (me.isSelected(record) && selected.getCount() > 0) {
            me.doDeselect([record], suppressEvent);
            return;
        }

        records = Ext.isArray(record) ? record : [record];
        len = records.length;


        function commit() {
            selected.add(record);
            change = true;
        }

        for (; i < len; i++) {
            record = records[i];
            if (keepExisting && me.isSelected(record)) {
                continue;
            }
            me.lastSelected = record;

            me.onSelectChange(record, true, suppressEvent, commit);
        }
        if (!me.preventFocus) {
            me.setLastFocused(record, suppressEvent);
        }

        me.maybeFireSelectionChange(change && !suppressEvent);
    },

    getSelection: function () {
        return this.selected.getRange();
    },

    onHeaderClick: function (headerCt, header, e) {
        if (header.isCheckerHd) {
            e.stopEvent();
            var me = this,
                isChecked = header.el.hasCls(this.checkerOnCls);

            me.preventFocus = true;
            if (isChecked) {
                me.b4deselectAll();
            } else {
                me.selectAll();
            }
            delete me.preventFocus;
        }
    },

    refresh: function () {
        var me = this,
            store = me.store,
            rec,
            toBeSelected = [],
            toBeReAdded = [],
            oldSelections = me.getSelection(),
            len = oldSelections.length,
            selection,
            change,
            i = 0,
            lastFocused = me.getLastFocused();

        // Not been bound yet.
        if (!store) {
            return;
        }

        // Add currently records to the toBeSelected list if present in the Store
        // If they are not present, and pruneRemoved is false, we must still retain the record
        for (; i < len; i++) {
            selection = oldSelections[i];
            if (store.indexOf(selection) !== -1) {
                toBeSelected.push(selection);
            }

            // Selected records no longer represented in Store must be retained
            else if (!me.pruneRemoved) {
                // See if a record by the same ID exists. If so, select it
                rec = store.getById(selection.getId());
                if (rec) {
                    toBeSelected.push(rec);
                }
                // If it does not exist, we have to re-add it to the selection
                else {
                    toBeReAdded.push(selection);
                }
            }

            // In single select mode, only one record may be selected
            if (me.mode === 'SINGLE' && toBeReAdded.length) {
                break;
            }
        }

        // there was a change from the old selected and
        // the new selection
        if (me.selected.getCount() != (toBeSelected.length + toBeReAdded.length)) {
            change = true;
        }

        me.clearSelections();

        if (lastFocused && store.indexOf(lastFocused) !== -1) {
            // restore the last focus but supress restoring focus
            me.setLastFocused(lastFocused, true);
        }

        if (toBeSelected.length) {
            // perform the selection again
            me.doSelect(toBeSelected, false, true);
        }

        // If some of the selections were not present in the Store, but pruneRemoved is false, we must add them back
        if (toBeReAdded.length) {
            me.selected.addAll(toBeReAdded);

            // No records reselected.
            if (!me.lastSelected) {
                me.lastSelected = toBeReAdded[toBeReAdded.length - 1];
            }
        }

        me.maybeFireSelectionChange(change);
    },

    getStoreRecord: function (record) {
        var store = this.store,
            records, rec, len, id, i;

        if (record) {
            if (record.getId()) {
                return store.getById(record.getId());
            } else {
                records = store.data.items;
                len = records.length;
                id = record.internalId;

                for (i = 0; i < len; ++i) {
                    rec = records[i];
                    if (id === rec.internalId) {
                        return rec;
                    }
                }
            }
        }
        return null;
    },

    updateHeaderState: function () {
        // check to see if all records are selected
        var me = this,
            store = me.store,
            storeCount = store.getCount(),
            views = me.views,
            hdSelectStatus = false,
            selectedCount = 0,
            selected, len, i;

        if (!store.buffered && storeCount > 0) {
            selected = me.selected;
            hdSelectStatus = true;
            for (i = 0, len = selected.getCount() ; i < len; ++i) {
                if (!me.getStoreRecord(selected.getAt(i))) {
                    break;
                }
                ++selectedCount;
            }
            hdSelectStatus = storeCount === selectedCount;
        }

        if (views && views.length) {
            me.toggleUiHeader(hdSelectStatus);
        }
    },

    getSelectionStart: function () {
        return this.selectionStart;
    },

    selectWithEvent: function (record, e) {
        var me = this,
            isSelected = me.isSelected(record),
            shift = e.shiftKey,
            ctrl = e.ctrlKey,
            start = shift && me.getSelectionStart(),
            selected = me.getSelection(),
            len = selected.length,
            allowDeselect = me.allowDeselect;

        switch (me.mode) {
            case 'MULTI':
                if (shift && start) {
                    me.selectRange(start, record, ctrl);
                } else if (ctrl && isSelected) {
                    me.doDeselect(record, false);
                } else if (ctrl) {
                    me.doSelect(record, true, false);
                } else if (isSelected && !shift && !ctrl && len > 0) {
                    me.doDeselect(record, false);
                } else if (!isSelected) {
                    me.doSelect(record, false);
                }
                break;
            case 'SIMPLE':
                if (isSelected) {
                    me.doDeselect(record);
                } else {
                    me.b4deselectAll();
                    me.doSelect(record, true);
                }
                break;
            case 'SINGLE':
                if (allowDeselect && !ctrl) {
                    allowDeselect = me.toggleOnClick;
                }
                if (allowDeselect && isSelected) {
                    me.doDeselect(record);
                } else {
                    me.b4deselectAll();
                    me.doSelect(record, false);
                }
                break;
        }

        if (!shift) {
            if (me.isSelected(record)) {
                me.selectionStart = record;
            } else {
                me.selectionStart = null;
            }
        }
    },
});