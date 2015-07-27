/// <reference path="LogPlotHeaderLayoutPartsCreator.js" />

/**
 * @constructor
 */
var LogPlotSeriesHeaderLayoutStrategySupport = {

};
/**
 * @function 
 * @description null object pattern - add empty divs for the parts that are not showing,
 * to avoid having null checks all over the log plot view :)
 */
LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml = function (innerHtml) {
    var hiddenClass = 'logPlotHeaderHidden';
    var emptyValueDiv = "<div class='" + hiddenClass + "' >" + innerHtml + "</div>";
    return emptyValueDiv;
};

/** @function
* @description Factory to create the appropriate layout strategy for the given config.
*/
var LogPlotSeriesHeaderLayoutStrategyFactory = function (seriesHeaderConfig, isAtTop, partsCreator) {
    if (!seriesHeaderConfig || typeof isAtTop === 'undefined' || !partsCreator) {
        throw 'bad args!';
    }

    this._config = seriesHeaderConfig;
    this._isAtTop = isAtTop;
    this._partsCreator = partsCreator;
};

LogPlotSeriesHeaderLayoutStrategyFactory.prototype.create = function () {
    //name/unit/metadata NO value
    if ((this._config.getShowName() || this._config.getShowUom()) && !this._config.getShowSnapshotValue()) {
        return this._isAtTop ? new LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtTop(this._partsCreator, this._config) : new LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom(this._partsCreator, this._config);
    }

    //name/unit With Value
    if (this._config.getShowSnapshotValue()) {
        return this._isAtTop ? new LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtTop(this._partsCreator, this._config) : new LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtBottom(this._partsCreator, this._config);
    }
    //metadata only
    return new LogPlotSeriesHeaderLayoutStrategyMetadataOnly(this._partsCreator, this._config);
};

/** @function
* @description name/metadata/unit NO value
* [name][Md12/34][unit]
* [value axis]
*/
var LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtTop = function (partsCreator, config) {
    this._partsCreator = partsCreator;
    this._config = config;
};
/** @function
*/
LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtTop.prototype.createLayoutHtml = function () {
    var html;

    var result = this._createLayoutHtmlRows();
    html = result.begin;
    html += result.nameUnitMetadataRow;
    html += result.valueAxisRow;
    html += result.end;

    return html;
};
/** @function
*/
LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtTop.prototype._createLayoutHtmlRows = function () {
    if (!this._partsCreator || !this._config) {
        throw 'bad args!';
    }
    //note: without-value strategy is the most complex - 7 different possible configs!
    //TODO review can we refactor to break the code down ...

    var heightOfValueAxis = this._partsCreator.getHeightOfValueAxis();

    var result = {};
    result.begin = "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >";
    var metaDataWidth = this._partsCreator.getMetaDataWidth();

    var nameClass, nameWidth;
    var unitClass;
    var hiddenClass = 'logPlotHeaderHidden';
    var metadataClass = 'pure-u-1-24';
    var unitWidth = "";

    if (this._config.getShowName() && this._config.getShowMetadataIndicators() && this._config.getShowUom()) {
        nameClass = "pure-u-20-24";
        nameWidth = "calc(83.333% - " + metaDataWidth + "px)";
        unitClass = "pure-u-4-24";
    } else if (this._config.getShowName() && this._config.getShowMetadataIndicators() && !this._config.getShowUom()) {
        nameClass = "pure-u-24-24";
        nameWidth = "calc(100% - " + metaDataWidth + "px)";
        unitClass = hiddenClass;
    } else if (this._config.getShowName() && !this._config.getShowMetadataIndicators() && this._config.getShowUom()) {
        nameClass = "pure-u-20-24";
        nameWidth = "calc(83.333% - " + metaDataWidth + "px)";
        metadataClass = hiddenClass;
        unitClass = "pure-u-4-24";
    } else if (this._config.getShowName() && !this._config.getShowMetadataIndicators() && !this._config.getShowUom()) {
        nameClass = "pure-u-24-24";
        nameWidth = "100%";
        metadataClass = hiddenClass;
        unitClass = hiddenClass;
    } else if (!this._config.getShowName() && this._config.getShowMetadataIndicators() && this._config.getShowUom()) {
        nameClass = hiddenClass;
        nameWidth = "0px";
        unitClass = "pure-u-24-24";
        unitWidth = "width: calc(100% - " + metaDataWidth + "px)";
    } else if (!this._config.getShowName() && this._config.getShowMetadataIndicators() && !this._config.getShowUom()) {
        nameClass = hiddenClass;
        nameWidth = "0px";
        unitClass = hiddenClass;
    } else if (!this._config.getShowName() && !this._config.getShowMetadataIndicators() && this._config.getShowUom()) {
        nameClass = hiddenClass;
        nameWidth = "0px";
        metadataClass = hiddenClass;
        unitClass = "pure-u-24-24";
        unitWidth = "width: 100%";
    } else {
        throw 'not impl - invalid operation';
    }

    var metaDataDiv = "<div class='" + metadataClass + "' style='width:" + metaDataWidth + "px; height: 100%;'>" + this._partsCreator.createDivHtmlForMetadata() + "</div>";

    var metadataRow = "<div class='" + nameClass + "' style='width: " + nameWidth + "; '>" + this._partsCreator.createDivHtmlForName() + "</div>" +
        metaDataDiv +
        "<div class='" + unitClass + "' style='" + unitWidth + ";'>" + this._partsCreator.createDivHtmlForUnits() + "</div>";

    //null object pattern - also add an empty value div:
    var emptyValueDiv = LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml(this._partsCreator.createDivHtmlForValue());
    result.nameUnitMetadataRow = metadataRow + emptyValueDiv;

    //[value axis]
    var valueAxisDiv = "<div class='pure-g pure-g-valign-fix' >" +
        "<div class='pure-u-24-24' style='height: " + heightOfValueAxis + "px; min-height: " + heightOfValueAxis + "px;'>" + this._partsCreator.createDivHtmlForValueAxis() + "</div>"
        + "</div>";

    result.valueAxisRow = valueAxisDiv;

    result.end = "</div>";

    return result;
};

/** @function
* @description name/unit/metadata NO value
* [value axis]
* [name][Md12/34][unit]
*/
var LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom = function (partsCreator, config) {
    if (!partsCreator || !config) {
        throw 'bad args!';
    }
    this._partsCreator = partsCreator;
    this._config = config;
};
LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype = new LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtTop();
LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype.constructor = LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom;
/** @function
*/
LogPlotSeriesHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype.createLayoutHtml = function () {
    var html;

    var result = this._createLayoutHtmlRows();

    html = result.begin;
    html += result.valueAxisRow;
    html += result.nameUnitMetadataRow;
    html += result.end;

    return html;
};

/** @function
* @description name/unit With Value
[name][unit]
[value][Md12/34]
[value axis]
*/
var LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtTop = function (partsCreator, config) {
    this._partsCreator = partsCreator;
    this._config = config;
};

/** @function
*/
LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtTop.prototype.createLayoutHtml = function () {
    var html;

    var result = this._createLayoutHtmlRows();

    /*
    [name][unit]
    [value][Md12/34]
    [value axis]
    */
    html = result.begin;

    html += result.nameUnitRow;
    html += result.valueMetadataRow;
    html += result.valueAxisRow;

    html += result.end;

    return html;
};
/** @function
*/
LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtTop.prototype._createLayoutHtmlRows = function () {
    if (!this._partsCreator || !this._config) {
        throw 'bad args!';
    }

    var heightOfValueAxis = this._partsCreator.getHeightOfValueAxis();

    var result = {};
    result.begin = "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >";
    var metaDataWidth = this._partsCreator.getMetaDataWidth();

    //TODO swap metadata and unit (so unit is beside the value)

    //TODO refactor extract fun

    var nameClass;
    var unitClass;
    var hiddenClass = 'logPlotHeaderHidden';
    var unitWidth = "";

    var nameUnitRow;

    if (this._config.getShowName() && this._config.getShowUom()) {
        nameClass = "pure-u-20-24";
        unitClass = "pure-u-4-24";
    } else if (this._config.getShowName() && !this._config.getShowUom()) {
        nameClass = "pure-u-24-24";
        unitClass = hiddenClass;
    } else if (!this._config.getShowName() && this._config.getShowUom()) {
        nameClass = hiddenClass;
        unitClass = "pure-u-24-24";
    } else if (!this._config.getShowName() && !this._config.getShowUom()) {
        var emptyNameDiv = LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml(this._partsCreator.createDivHtmlForName());
        var emptyUnitsDiv = LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml(this._partsCreator.createDivHtmlForUnits());
        nameUnitRow = emptyNameDiv + emptyUnitsDiv;
    } else {
        throw 'invalid operation';
    }

    nameUnitRow = nameUnitRow ? nameUnitRow : "<div class='" + nameClass + "' style=''>" + this._partsCreator.createDivHtmlForName() + "</div>" +
        "<div class='" + unitClass + "' style='" + unitWidth + ";'>" + this._partsCreator.createDivHtmlForUnits() + "</div>";

    result.nameUnitRow = nameUnitRow;

    //TODO refactor extract fun

    //value + metadata:
    var valueClass;
    var valueWidth;

    var metadataClass = 'pure-u';

    if (this._config.getShowSnapshotValue() && this._config.getShowMetadataIndicators()) {
        valueClass = "pure-u-24-24";
        valueWidth = "calc(100% - " + metaDataWidth + "px)";
    } else if (this._config.getShowSnapshotValue() && !this._config.getShowMetadataIndicators()) {
        valueClass = "pure-u-24-24";
        valueWidth = "100%";
        metadataClass = hiddenClass;
    } else if (!this._config.getShowSnapshotValue() && this._config.getShowMetadataIndicators()) {
        valueClass = hiddenClass;
    } else if (!this._config.getShowSnapshotValue() && !this._config.getShowMetadataIndicators()) {
        valueClass = hiddenClass;
        metadataClass = hiddenClass;
    } else {
        throw 'invalid operation';
    }

    var metaDataDiv = "<div class='" + metadataClass + "' style='width:" + metaDataWidth + "px; height: 100%;'>" + this._partsCreator.createDivHtmlForMetadata() + "</div>";

    var valueMetadataRow = "<div class='" + valueClass + "' style='width: " + valueWidth + "; '>" + this._partsCreator.createDivHtmlForValue() + "</div>" +
        metaDataDiv;

    result.valueMetadataRow = valueMetadataRow;

    //[value axis]
    var valueAxisDiv = "<div class='pure-g pure-g-valign-fix' >" +
        "<div class='pure-u-24-24' style='height: " + heightOfValueAxis + "px; min-height: " + heightOfValueAxis + "px;'>" + this._partsCreator.createDivHtmlForValueAxis() + "</div>"
        + "</div>";

    result.valueAxisRow = valueAxisDiv;

    result.end = "</div>";

    return result;
};
/** @function
* @description name/unit With Value
[value axis]
[value][Md12/34]
[name][unit]
*/
var LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtBottom = function (partsCreator, config) {
    this._partsCreator = partsCreator;
    this._config = config;
};
LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtBottom.prototype = new LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtTop();
LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtBottom.prototype.constructor = LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtBottom;
/** @function
*/
LogPlotSeriesHeaderLayoutStrategyNameUnitWithValueAtBottom.prototype.createLayoutHtml = function () {
    var html;

    var result = this._createLayoutHtmlRows();

    /*
    [value axis]
    [value][Md12/34]
    [name][unit]
    */
    html = result.begin;

    html += result.valueAxisRow;
    html += result.valueMetadataRow;
    html += result.nameUnitRow;

    html += result.end;

    return html;
};

/** function
* @description metadata only:
* [value axis][Md12/34]
*/
var LogPlotSeriesHeaderLayoutStrategyMetadataOnly = function (partsCreator, config) {
    if (!partsCreator || !config) {
        throw 'bad args!';
    }
    this._partsCreator = partsCreator;
    this._config = config;
};
/** @function
*/
LogPlotSeriesHeaderLayoutStrategyMetadataOnly.prototype.createLayoutHtml = function () {
    var html = '';
    var heightOfValueAxis = this._partsCreator.getHeightOfValueAxis();
    if (this._config.getShowMetadataIndicators()) {
        //[value axis][Md12/34]
        var metaDataWidth = this._partsCreator.getMetaDataWidth();
        html += "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >" +
        "<div class='pure-u-24-24' style='width: calc(100% - " + metaDataWidth + "px); height: " + heightOfValueAxis + "px; min-height: " + heightOfValueAxis + "px;'>" + this._partsCreator.createDivHtmlForValueAxis() + "</div>" +
        "<div style='width:" + metaDataWidth + "px; height: 100%; float: right;'>" + this._partsCreator.createDivHtmlForMetadata() + "</div>";
        html += "</div>";
    } else {
        //[value axis]
        html += "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >" +
            "<div class='pure-u-24-24' style='height: " + heightOfValueAxis + "px; min-height: " + heightOfValueAxis + "px;'>" + this._partsCreator.createDivHtmlForValueAxis() + "</div>";
        html += "</div>";
        //null object pattern - add empty divs for the parts that are not showing:
        var emptyMetadataDiv = LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml(this._partsCreator.createDivHtmlForMetadata());
        html += emptyMetadataDiv;
    }

    var emptyValueDiv = LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml(this._partsCreator.createDivHtmlForValue());
    var emptyNameDiv = LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml(this._partsCreator.createDivHtmlForName());
    var emptyUnitsDiv = LogPlotSeriesHeaderLayoutStrategySupport.createEmptyDivHtml(this._partsCreator.createDivHtmlForUnits());
    html += emptyValueDiv + emptyNameDiv + emptyUnitsDiv;

    return html;
};
