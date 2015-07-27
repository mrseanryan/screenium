//TODO rename LogPlotHeaderLayoutStrategy* to be LogPlotSeriesHeaderLayoutStrategy*

/// <reference path="LogPlotHeaderLayoutPartsCreator.js" />
/** @function
* @description Factory to create the appropriate layout strategy for the given config.
*/
var LogPlotHeaderLayoutStrategyFactory = function (seriesHeaderConfig, isAtTop, partsCreator) {
    if (!seriesHeaderConfig || typeof isAtTop === 'undefined' || !partsCreator) {
        throw 'bad args!';
    }

    this._config = seriesHeaderConfig;
    this._isAtTop = isAtTop;
    this._partsCreator = partsCreator;
};

LogPlotHeaderLayoutStrategyFactory.prototype.create = function () {
    //name/unit/metadata NO value
    if ((this._config.getShowName() || this._config.getShowUom()) && !this._config.getShowSnapshotValue()) {
        return this._isAtTop ? new LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop(this._partsCreator, this._config) : new LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom(this._partsCreator, this._config);
    }

    //name/unit With Value
    if (this._config.getShowSnapshotValue()) {
        return this._isAtTop ? new LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop(this._partsCreator, this._config) : new LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom(this._partsCreator, this._config);
    }
    //metadata only
    return new LogPlotHeaderLayoutStrategyMetadataOnly(this._partsCreator, this._config);
};

/** @function
* @description name/metadata/unit NO value
* [name][Md12/34][unit]
* [value axis]
*/
var LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop = function (_partsCreator, _config) {
    this._partsCreator = _partsCreator;
    this._config = _config
};
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop.prototype.createLayoutHtml = function () {
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
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop.prototype._createLayoutHtmlRows = function () {
    if (!this._partsCreator || !this._config) {
        throw 'bad args!';
    }

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

    result.nameUnitMetadataRow = metadataRow;

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
var LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom = function (_partsCreator, _config) {
    if (!_partsCreator || !_config) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
    this._config = _config
};
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype = new LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop();
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype.constructor = LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom;
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype.createLayoutHtml = function () {
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
var LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop = function (_partsCreator, _config) {
    this._partsCreator = _partsCreator;
    this._config = _config
};

/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop.prototype.createLayoutHtml = function () {
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
LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop.prototype._createLayoutHtmlRows = function () {
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
        nameWidth = "0px";
        unitClass = "pure-u-24-24";
    } else if (!this._config.getShowName() && !this._config.getShowUom()) {
        nameUnitRow = ' '; //empty HTML
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
var LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom = function (_partsCreator, _config) {
    this._partsCreator = _partsCreator;
    this._config = _config
};
LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom.prototype = new LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop();
LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom.prototype.constructor = LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom;
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom.prototype.createLayoutHtml = function () {
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
var LogPlotHeaderLayoutStrategyMetadataOnly = function (_partsCreator, _config) {
    if (!_partsCreator || !_config) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
    this._config = _config
};
/** @function
*/
LogPlotHeaderLayoutStrategyMetadataOnly.prototype.createLayoutHtml = function () {
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
    }

    return html;
};