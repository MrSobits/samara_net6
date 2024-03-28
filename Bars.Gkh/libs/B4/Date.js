Ext.define('B4.Date', {
    singleton: true,

    /**
     * Проверка состовляют ли переданные даты период
     */
    isPeriod: function (startDate, endDate, allowEmptyEnd, allowEmptyStart) {
        function isFullPeriod(start, end) {
            return Ext.isDate(start) && Ext.isDate(end) &&
                start <= end;
        }

        if (allowEmptyStart === true) {
            if (Ext.isEmpty(startDate)) {
                return Ext.isDate(endDate);
            }
        }

        if (allowEmptyEnd === true) {
            if (Ext.isEmpty(endDate)) {
                return Ext.isDate(startDate);
            }
        }

        return isFullPeriod(startDate, endDate);
    },

    /**
     * Проверка что переданный массив является валидным набором периодов.
     * Набор периодов считается валидным, если конец предыдущего периода является
     * днем, предшествующим началу следующего за ним периода.
     * Пустой массив периодов считается валидным.
     * @param {Array} periods массив периодов (объект вида 
     *          {
     *              DateStart: Mon Dec 15 2014 10:31:27 GMT+0300 (Russia Standard Time),
     *              DateEnd: Mon Dec 15 2014 10:31:27 GMT+0300 (Russia Standard Time)
     *          })
     */
    isPeriodRange: function (periods, allowEmptyEnds, allowEmptyStarts) {
        var isValid = true, i, last, current, prev;
        if (!periods || periods.length < 1) {
            return true;
        }

        periods = Ext.Array.sort(periods, function (a, b) {
            if (a.DateStart < b.DateStart) {
                return -1;
            }
            if (a.DateStart > b.DateStart) {
                return 1;
            }
            return 0;
        });

        for (i = 1; i < periods.length; i++) {
            current = periods[i];
            prev = periods[i - 1];
            isValid = isValid && Ext.isDate(prev.DateEnd)
                && Ext.isDate(current.DateStart)
                && B4.Date.isSameDay(prev.DateEnd, B4.Date.previousDayFor(current.DateStart));
        }

        last = periods[periods.length - 1];
        return isValid && B4.Date.isPeriod(last.DateStart, last.DateEnd, allowEmptyEnds, allowEmptyStarts);
    },

    /**
     * Получение предыдущего дня для даты.
     */
    previousDayFor: function (date) {
        var prevDay = new Date(date);
        prevDay.setDate(date.getDate() - 1);
        return prevDay;
    },

    nextDayFor: function (date) {
        var nextDay = new Date();
        nextDay.setDate(date.getDate() + 1);
        return nextDay;
    },

    isSameDay: function(date1, date2) {
        var day1 = new Date(date1), day2 = new Date(date2);
        day1.setHours(0, 0, 0, 0);
        day2.setHours(0, 0, 0, 0);
        return Ext.Date.isEqual(day1, day2);
    },

    rangeYears: function (startDate, endDate) {
        if (endDate === undefined) {
            endDate = new Date()
        };
        var startYear = startDate.getFullYear(), endYear = endDate.getFullYear(), range  = [];
        for (var i = startYear; i <= endYear; i++) {
            range.push(i)
        };
        return range;
    }
});