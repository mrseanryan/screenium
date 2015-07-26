/// <reference path="LogPlotSeriesHeaderLayout.js" />

/**
 * @module
 * @description MUST match C# LogPlotHeaderPosition
 */
var JsApiLogPlotHeaderPosition = {
    Top: "Top",
    Bottom: "Bottom",
    TopAndBottom: "TopAndBottom"
};

/** @constructor 
*/
var LogPlotHeaderLayoutDecorator = function (config) {
    this._config = config;
};
/**
 * @function
 */
LogPlotHeaderLayoutDecorator.prototype.decorate = function () {
    if (this._config.isDecorated) {
        throw "already decorated!";
    }

    this._addIsValid();
    this._addGetters();

    this._config.isDecorated = true;
};
/**
 * @function
 */
LogPlotHeaderLayoutDecorator.prototype._addIsValid = function () {
    this._config.isValid = function () {
        return typeof (this.showHeader) !== "undefined" &&
            typeof (this.position) !== "undefined" &&
            typeof (this.showName) !== "undefined" &&
            typeof (this.showUom) !== "undefined" &&
            typeof (this.showMetadataIndicators) !== "undefined" &&
            typeof (this.showSnapshotValue) !== "undefined" &&
            typeof (this.showScaleIntervals) !== "undefined" &&
            typeof (this.minHeight) !== "undefined";
    };
};
/**
 * @function
 */
LogPlotHeaderLayoutDecorator.prototype._addGetters = function () {
    this._config.isShowHeader = function () {
        return this.showHeader;
    };
    this._config.isInTop = function () {
        return this.position === JsApiLogPlotHeaderPosition.Top || this.position === JsApiLogPlotHeaderPosition.TopAndBottom;
    };
    this._config.isInBottom = function () {
        return this.position === JsApiLogPlotHeaderPosition.Bottom || this.position === JsApiLogPlotHeaderPosition.TopAndBottom;
    };
    this._config.getMinHeight = function () {
        return this.minHeight;
    };
    this._config.getShowName = function () {
        return this.showName;
    };
    this._config.getShowUom = function () {
        return this.showUom;
    };
    this._config.getShowMetadataIndicators = function () {
        return this.showMetadataIndicators;
    };
    this._config.getShowSnapshotValue = function () {
        return this.showSnapshotValue;
    };
    this._config.getShowScaleIntervals = function () {
        return this.showScaleIntervals;
    };
};

/* create layout for contents of header row
*/
var LogPlotHeaderLayout = function (config) {
    if (!config) {
        throw "bad args!";
    }

    this._config = config;

    var configDecorator = new LogPlotHeaderLayoutDecorator(config);
    configDecorator.decorate();
    if (!config.isValid()) {
        throw "bad log plot header config!";
    }
};
/**
 * @function
 */
LogPlotHeaderLayout.prototype.constructColumnHeader = function (headerContainerDiv, seriesInColumn, maxSeriesInColumn, seriesHeaderConfig) {
    headerContainerDiv.html(this._getHtmlForColumnHeader(seriesInColumn, maxSeriesInColumn, seriesHeaderConfig));
    this._cacheColumnHeaderParts(headerContainerDiv, seriesInColumn);
};
/**
 * @function
 */
LogPlotHeaderLayout.prototype.constructColumnFooter = function (footerContainerDiv, seriesInColumn, maxSeriesInColumn, seriesHeaderConfig) {
    footerContainerDiv.html(this._getHtmlForColumnFooter(seriesInColumn, maxSeriesInColumn, seriesHeaderConfig));
    this._cacheColumnFooterParts(footerContainerDiv, seriesInColumn);
};

/**
 * @function
 */
LogPlotHeaderLayout.prototype._cacheColumnHeaderParts = function (headerContainerDiv, seriesInColumn) {
    //TODO find the part divs and cache them (required for tooltips optimization)
};
/**
 * @function
 */
LogPlotHeaderLayout.prototype._cacheColumnFooterParts = function (headerContainerDiv, seriesInColumn) {
    //TODO find the part divs and cache them (required for tooltips optimization)
};
/**
 * @function
 */
LogPlotHeaderLayout.prototype._getHeaderLayoutPropertyName = function (isAtTop) {
    return isAtTop ? "headerLayoutAtTop" : "headerLayoutAtBottom";
};
/**
 * @function
 * @description Build the HTML for header for one column (at Top or at Bottom).
 * 
 * perSeriesHeaderConfig - optional param used for testing the various combinations of settings.
 * if null, then we use the common header config (this._config).
 */
LogPlotHeaderLayout.prototype._getHtmlForColumn = function (seriesInColumn, isAtTop, maxSeriesInColumn, perSeriesHeaderConfig) {
    var activeConfig = perSeriesHeaderConfig ? perSeriesHeaderConfig : this._config;
    if (!activeConfig.isDecorated) {
        var configDecorator = new LogPlotHeaderLayoutDecorator(activeConfig);
        configDecorator.decorate();
    }

    var html = "";
    var htmlTableStart = "<table style='width:100%; height:100%; padding-bottom: 1px;'>";
    var htmlRowStart = "<tr><td style='height:" + (100/maxSeriesInColumn) + "%;'>"; //calc height%
    var htmlRowEnd = "</td></tr>";
    var htmlTableEnd = "</table>";

    html += htmlTableStart;
    var countOfSeriesThisColumn = 0;
    for (var serie in seriesInColumn) {
        if (seriesInColumn.hasOwnProperty(serie)) {
            serie = seriesInColumn[serie];

            var headerLayoutProp = this._getHeaderLayoutPropertyName(isAtTop);
            //TODO review is OO but is this needed?
            serie[headerLayoutProp] = new LogPlotSeriesHeaderLayout(isAtTop, serie.Id, serie.Name, activeConfig);

            html += htmlRowStart;
            html += serie.headerLayoutAtTop.getInitialHtml();
            html += htmlRowEnd;

            countOfSeriesThisColumn++;
        }
    }

    //if this column has less series than others, then add some empty boxes:
    //TODO refactor to a LogPlotHeaderSerie class
    var dummySerie = {
        Id: -1,
        Name: ""
    };
    var dummyHeaderLayout = new LogPlotSeriesHeaderLayout(isAtTop, dummySerie.Id, dummySerie.Name, activeConfig);
    while (countOfSeriesThisColumn < maxSeriesInColumn) {
        html += htmlRowStart;
        html += dummyHeaderLayout.getInitialEmptyHtml();
        html += htmlRowEnd;

        countOfSeriesThisColumn++;
    }

    html += htmlTableEnd;

    return html;
};
/**
 * @function 
 */
LogPlotHeaderLayout.prototype._getHtmlForColumnHeader = function (seriesInColumn, maxSeriesInColumn, perSeriesHeaderConfig) {
    return this._getHtmlForColumn(seriesInColumn, true, maxSeriesInColumn, perSeriesHeaderConfig);
};

/**
 * @function 
 */
LogPlotHeaderLayout.prototype._getHtmlForColumnFooter = function (seriesInColumn, maxSeriesInColumn, perSeriesHeaderConfig) {
    return this._getHtmlForColumn(seriesInColumn, false, maxSeriesInColumn, perSeriesHeaderConfig);
};

/**
 * @function 
 */
LogPlotHeaderLayout.prototype.onResizeLayout = function(seriesInColumn) {
    //tell each of the series header layouts, to resize:

    var headerAtTop = this._getHeaderLayoutPropertyName(true);
    var headerAtBottom = this._getHeaderLayoutPropertyName(false);

    for (var serie in seriesInColumn) {
        if (seriesInColumn.hasOwnProperty(serie)) {
            serie = seriesInColumn[serie];

            serie[headerAtTop].onResizeLayout();
            serie[headerAtBottom].onResizeLayout();
        }
    }
};
