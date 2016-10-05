// Write your Javascript code.
$(document).ready(function () {
    var currentRoute = window.location.pathname;
    var currentPage = currentRoute.substring(currentRoute.indexOf("/") + 1, currentRoute.indexOf("/", 1));
    $(".btn").button();
    // Active tab by route
    switch (currentPage) {
        case "Study":
            $("#tabs").tabs();
            var index = $('#tabs ul li').index($("#tbStudy"));
            $("#tabs").tabs({
                active: index
            });
            break;
        case "Collection":
            $("#tabs").tabs();
            var index = $('#tabs ul li').index($("#tbCollection"));
            $("#tabs").tabs({
                active: index
            });
            break;
        case "File":
            $("#tabs").tabs();
            var index = $('#tabs ul li').index($("#tbFile"));
            $("#tabs").tabs({
                active: index
            });
            $("#tblFileElement").tablesorter(
                {
                    sortList: [[0, 0], [1, 0], [2, 0], [3, 0], [4, 0]]
                }
            );
            break;
        case "Package":
            $("#tabs").tabs();
            var index = $('#tabs ul li').index($("#tbPackage"));
            $("#tabs").tabs({
                active: index
            });
            break;
        case "Respondent":
            $("#tabs").tabs();
            var index = $('#tabs ul li').index($("#tbRespondent"));
            $("#tabs").tabs({
                active: index
            });
            break;
        case "Series":
            $("#tabs").tabs();
            var index = $('#tabs ul li').index($("#tbSeries"));
            $("#tabs").tabs({
                active: index
            });
            break;
        case "Search":
            $("#searchTabs").tabs();
            break;
        default:
            if (currentRoute == "/Search") {
                $("#searchTabs").tabs();
            }
            break;
    }
    //menu search
    $("body").on("click", "#menuSearchBtn", function () {
        var searchTerm = $("#txtMenuSearchTerm").val();
        if (searchTerm == '' || searchTerm == null) {
            console.log("not valid");
        } else if (/\S/.test(searchTerm))
        {
            var searchUrl = "/Study/Search/" + searchTerm.replace(/[|&;$%@"#?*~+=\-<>!()+\]\[\`\^,]/g, '');
            document.location.href = searchUrl;
        }
        searchTerm = null;
    });
    //Search Study
    $("body").on("click", "#studySearchBtn", function () {
        var searchTerm = $("#txtStudySearchTerm").val();
        if (searchTerm == '' || searchTerm == null) {
            console.log("not valid");
        } else if (/\S/.test(searchTerm)) {
            var searchUrl = "/Study/SearchFull/" + searchTerm.replace(/[|&;$%@"#?*~+=\-<>!()+\]\[\`\^,]/g, '');
            document.location.href = searchUrl;
        }
        searchTerm = null;
    });

    //Search Series
    $("body").on("click", "#seriesSearchBtn", function () {
        var searchTerm = $("#txtSeriesSearchTerm").val();
        if (searchTerm == '' || searchTerm == null) {
            console.log("not valid");
        } else if (/\S/.test(searchTerm)) {
            var searchUrl = "/Series/Search/" + searchTerm.replace(/[|&;$%@"#?*~+=\-<>!()+\]\[\`\^,]/g, '');
            document.location.href = searchUrl;
        }
        searchTerm = null;
    });

    //Search Collection
    $("body").on("click", "#collectionSearchBtn", function () {
        var searchTerm = $("#txtCollectionSearchTerm").val();
        if (searchTerm == '' || searchTerm == null) {
            console.log("not valid");
        } else if (/\S/.test(searchTerm)) {
            var searchUrl = "/Collection/Search/" + searchTerm.replace(/[|&;$%@"#?*~+=\-<>!()+\]\[\`\^,]/g, '');
            document.location.href = searchUrl;
        }
        searchTerm = null;
    });
    //Respondent Search
    $("body").on("click", "#respondentSearchBtn", function () {
        var searchTerm = $("#txtRespondentSearchTerm").val();
        if (searchTerm == '' || searchTerm == null) {
            console.log("not valid");
        } else if (/\S/.test(searchTerm)) {
            var searchUrl = "/Respondent/Search/" + searchTerm.replace(/[|&;$%@"#?*~+=\-<>!()+\]\[\`\^,]/g, '');
            document.location.href = searchUrl;
        }
        searchTerm = null;
    });
    //Package Search
    $("body").on("click", "#packageSearchBtn", function () {
        var searchTerm = $("#txtPackageSearchTerm").val();
        if (searchTerm == '' || searchTerm == null) {
            console.log("not valid");
        } else if (/\S/.test(searchTerm)) {
            var searchUrl = "/Package/Search/" + searchTerm.replace(/[|&;$%@"#?*~+=\-<>!()+\]\[\`\^,]/g, '');
            document.location.href = searchUrl;
        }
        searchTerm = null;
    });
    //File Search
    $("body").on("click", "#fileSearchBtn", function () {
        var searchTerm = $("#txtFileSearchTerm").val();
        if (searchTerm == '' || searchTerm == null) {
            console.log("not valid");
        } else if (/\S/.test(searchTerm)) {
            var searchUrl = "/File/Search/" + searchTerm.replace(/[|&;$%@"#?*~+=\-<>!()+\]\[\`\^,]/g, '');
            document.location.href = searchUrl;
        }
        searchTerm = null;
    });
    
});