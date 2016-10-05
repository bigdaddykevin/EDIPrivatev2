$(document).ready(function () {
    var currentRoute = window.location.pathname;
    var currentPage = currentRoute.substring(currentRoute.indexOf("/") + 1, currentRoute.indexOf("/", 1));

    if (currentPage === "Search" || currentPage === "search" || currentRoute === "/Search" || currentRoute === "/search") {
        $(".datepicker").datepicker({
            dateFormat: "yy-mm-dd"
        });

        $("body").on("click", "#searchTabs ul li", function () {
            sessionStorage.setItem("currentSearchTab", $(this).attr("id"));
        });

        // Search Drop down listeners
        $("body").on("change", ".search-dropdown", function () {
            var thisId = $(this).attr("id");
            var thisVal = $(this).val();
            var eleType = dropDownWatch(thisVal);
            if (eleType != null && eleType != undefined) {
                var targetId = "";
                $("#" + thisId.replace("_key", "_term")).length > 0 ? targetId = thisId.replace("_key", "_term") : null;
                $("#" + thisId.replace("_key", "_num")).length > 0 ? targetId = thisId.replace("_key", "_num") : null;
                $("#" + thisId.replace("_key", "_bool")).length > 0 ? targetId = thisId.replace("_key", "_bool") : null;
                $("#" + thisId.replace("_key", "_date")).length > 0 ? targetId = thisId.replace("_key", "_date") : null;
                if (targetId != "") {
                    changeInputType(eleType, targetId);
                }
            }
            sessionStorage.setItem(thisId, thisVal);
        });

        // check if there is any session
        var sessionFlag = 1;
        if (window.sessionStorage && sessionFlag===1) {
            if (sessionStorage.length > 0) {
                var nameList = [
                        "series",
                        "study",
                        "collection",
                        "file",
                        "package",
                        "respondent"
                ];
                if (sessionStorage.getItem("last-search") != null) {
                    if (sessionStorage.getItem("currentSearchTab") != null) {
                        $("#searchTabs").tabs({
                            active: $("#searchTabs ul li").index($("#"+sessionStorage.getItem("currentSearchTab")))
                        });
                    } else {
                        $("#searchTabs").tabs({
                            active: $("#searchTabs ul li").index($("#searchtab" + sessionStorage.getItem("last-search")))
                        });
                    }
                } else if (sessionStorage.getItem("currentSearchTab") != null) {
                    $("#searchTabs").tabs({
                        active: $("#searchTabs ul li").index($("#" + sessionStorage.getItem("currentSearchTab")))
                    });
                }
                // set previous search fields
                for (var n = 0; n < nameList.length; n++) {
                    if (sessionStorage.getItem(nameList[n] + "_search_fields") > $("." + nameList[n] + "_search").length) {
                        for (var y = 1; y < (sessionStorage.getItem(nameList[n] + "_search_fields") - $("." + nameList[n] + "_search").length)+1 ; y++) {
                            addNewField(nameList[n] + "_search", $("." + nameList[n] + "_search").length + 1);
                        }
                    }
                }
                for (var i = 0; i < sessionStorage.length; i++) {
                    if (sessionStorage.key(i).indexOf("_key") > 0) {
                        $("#" + sessionStorage.key(i)).val(sessionStorage.getItem(sessionStorage.key(i)));
                        $("#" + sessionStorage.key(i)).trigger("change");
                    }
                }
                for (var i = 0; i < sessionStorage.length; i++) {
                    if (sessionStorage.key(i).indexOf("_key") < 0) {
                        $("#" + sessionStorage.key(i)).val(sessionStorage.getItem(sessionStorage.key(i)));
                    }
                }
            } else {
                for (var y = 0; y < $("select").length; y++) {
                    sessionStorage.setItem($("select").eq(y).attr("id"), $("#" + $("select").eq(y).attr("id")).val());
                }
                for (var z = 0; z < $("input").length; z++) {
                    sessionStorage.setItem($("input").eq(y).attr("id"), $("#" + $("input").eq(y).attr("id")).val());
                }
            }
            sessionFlag = 2;
        }

        // Add field button listeners
        $(".addFieldBtn").on("click", function (event) {
            event.preventDefault();
            var ele = $(this).attr("id").slice(6, -5).toLowerCase() + "_search";
            addNewField(ele, $("." + ele).length + 1);
        });

        // Remove field button listeners
        $(".removeFieldBtn").on("click", function (event) {
            event.preventDefault();
            var ele = $(this).attr("id").slice(6, -5).toLowerCase() + "_search";
            removeLastField(ele);
            sessionStorage.setItem(ele + "_fields", $("." + ele).length - 1);
        });

        function dropDownWatch(eValue) {
            var dateList = [
                    "publicationstatisticsdate",
                    "publicationdatadate",
                    "publicationrestrictedusedatadate",
                    "recruitmentstartdateestimated",
                    "collectionstartdateestimated",
                    "collectionstartdate",
                    "collectionenddateestimated",
                    "collectionenddate",
                    "issuedate",
                    "expirationdate"
            ];
            var boolList = [
                    "collectionuniverse",
                    "collectionsample",
                    "collectionlongitudinal",
                    "collectioncrosssectional",
                    "collectionprogrammonitoring",
                    "collectiongranteereporting",
                    "collectionvoluntary",
                    "collectionmandatory",
                    "collectionrequiredforbenefits",
                    "pii_di",
                    "pia",
                    "datacouldbepublic",
                    "responsevoluntary",
                    "responsemandatory",
                    "responserequiredforbenefits",
                    "consentexplicit",
                    "consentimplicit",
                    "consentnotapplicable"
            ];
            var numList = [
                    "populationsizeestimated",
                    "populationsize",
                    "responsesizeestimated",
                    "responsesize",
                    "responserateestimated",
                    "responserate",
                    "burden",
                    "burdenperrespondent",
                    "burdenperrespondentsurvey",
                    "burdenperrespondentassessment",
                    "numberrespondents",
                    "numberresponses",
                    "percentcollectedelectronically",
                    "burdentotal",
                    "burdenchange",
                    "burdenadjustment",
                    "totalcost",
                    "cost"
            ];
            if ($.inArray(eValue, dateList) >= 0) {
                return "date";
            }
            if ($.inArray(eValue, numList) >= 0) {
                return "number";
            }
            if ($.inArray(eValue, boolList) >= 0) {
                return "bool";
            }
            return "txt";
        }

        function changeInputType(type, eleId) {
            switch (type) {
                case "date":
                    setDatePicker(eleId);
                    break;
                case "number":
                    setNumInput(eleId);
                    break;
                case "bool":
                    setBoolInput(eleId);
                    break;
                case "txt":
                    setTxtInput(eleId);
                    break;
                default:
                    console.log("no match");
                    break;
            }
        }

        // Search button listeners
        $(".advanceSearchBtn").on("click", function (event) {
            event.preventDefault();
            $("#emptySearchMessage").hide();
            $("#ajaxMenuSearchWait").show();
            var target = $(this).attr("id").slice(6, -3).toLowerCase();
            sessionStorage.setItem("last-search", target);
            sessionStorage.setItem(target + "_search_fields", $("." + target
                + "_search").length);
            var targetUrl = buildUrl(target);
            if (targetUrl != "" && targetUrl != null) {
                $("#emptySearchMessage").hide();
                $("#ajaxMenuSearchWait").hide();
                document.location.href = targetUrl;
            } else {
                $("#ajaxMenuSearchWait").hide();
                $("#emptySearchMessage").show();
            }
        });

        function getParams(target) {
            var targetList = [
                "series",
                "study",
                "collection",
                "file",
                "package",
                "respondent"
            ];

            var group = target + "_search";
            if ($.inArray(target, targetList) >= 0) {
                var params = getInputValues(group, target);
                return params != null ? params : null;
            }
            return null;
        }

        function getInputValues(group, target) {
            var defaultSearchKey = {
                "series": "name",
                "study": "name",
                "collection": "name",
                "package": "title",
                "respondent": "description",
                "file": "name"
            };
            var params = {};
            // get default term
            if ($("#" + group + "_default_term").length == 0) {
                //date
                if ($("#" + group + "_default_date").length > 0) {
                    if ($("#" + group + "_default_date_d1").val() != "") {
                        //save to session
                        sessionStorage.setItem($("#" + group + "_default_date_d1").attr("id"), $("#" + group + "_default_date_d1").val());
                        if ($("#" + group + "_default_date_d2").val() != "") {
                            //save to session
                            sessionStorage.setItem($("#" + group + "_default_date_d2").attr("id"), $("#" + group + "_default_date_d2").val());
                            params["default_term"] = $("#" + group + "_default_date_d1").val() + "~" + $("#" + group + "_default_date_d2").val();
                        } else {
                            params["default_term"] = $("#" + group + "_default_date_d1").val();
                        }
                        params["default_key"] = $("#" + group + "_default_key").val() == null ? defaultSearchKey[target] : $("#" + group + "_default_key").val();
                    } else {
                        if ($("#" + group + "_default_date_d2").val() != "") {
                            //save to session
                            sessionStorage.setItem($("#" + group + "_default_date_d2").attr("id"), $("#" + group + "_default_date_d2").val());
                            params["default_term"] = $("#" + group + "_default_date_d2").val();
                            params["default_key"] = $("#" + group + "_default_key").val();
                        }
                    }
                }
                //num
                if ($("#" + group + "_default_num").length > 0) {
                    if ($("#" + group + "_default_num_n1").val() != "") {
                        //save to session
                        sessionStorage.setItem($("#" + group + "_default_num_n1").attr("id"), $("#" + group + "_default_num_n1").val());
                        if ($("#" + group + "_default_num_n2").val() != "") {
                            //save to session
                            sessionStorage.setItem($("#" + group + "_default_num_n2").attr("id"), $("#" + group + "_default_num_n2").val());
                            params["default_term"] = $("#" + group + "_default_num_n1").val() + "~" + $("#" + group + "_default_num_n2").val();
                        } else {
                            params["default_term"] = $("#" + group + "_default_num_n1").val();
                        }
                        params["default_key"] = $("#" + group + "_default_key").val();
                    } else {
                        if ($("#" + group + "_default_num_n2").val() != "") {
                            //save to session
                            sessionStorage.setItem($("#" + group + "_default_num_n2").attr("id"), $("#" + group + "_default_num_n2").val());
                            params["default_term"] = $("#" + group + "_default_num_n2").val();
                            params["default_key"] = $("#" + group + "_default_key").val() == null ? defaultSearchKey[target] : $("#" + group + "_default_key").val();
                        }
                    }
                }
                //bool
                if ($("#" + group + "_default_bool").length > 0) {
                    if ($("#" + group + "_default_bool input[name=" + group + "_default_bool]:checked").val() != "") {
                        //save to session
                        sessionStorage.setItem($("#" + group + "_default_bool").attr("id"), $("#" + group + "_default_bool input[name=" + group + "_default_bool]:checked").val());
                        params["default_term"] = $("#" + group + "_default_bool input[name=" + group + "_default_bool]:checked").val();
                        params["default_key"] = $("#" + group + "_default_key").val();
                    }
                }
                if (params["default_term"] == "" || params["default_term"] == null) {
                    return null;
                }
            } else {
                params["default_term"] = $("#" + group + "_default_term").val();
                if (params["default_term"] != null && params["default_term"] != "") {
                    params["default_key"] = $("#" + group + "_default_key").val();
                    //save to session
                    sessionStorage.setItem($("#" + group + "_default_term").attr("id"), $("#" + group + "_default_term").val());
                } else {
                    return null;
                }
            }

            // get other terms
            var formFields = $("." + group);
            if (formFields != null && formFields != undefined) {
                params["operatorList"] = [];
                params["keyList"] = [];
                params["termList"] = [];
                for (var i = 0; i < formFields.length; i++) {
                    //txt
                    if ($("#" + group + "_term_" + (i + 1)).val() != null && $("#" + group + "_term_" + (i + 1)).val() != "") {
                        params["operatorList"].push($("#" + group + "_operator_" + (i + 1)).val());
                        params["keyList"].push($("#" + group + "_key_" + (i + 1)).val());
                        params["termList"].push($("#" + group + "_term_" + (i + 1)).val());
                        //save to session
                        sessionStorage.setItem($("#" + group + "_term_" + (i + 1)).attr("id"), $("#" + group + "_term_" + (i + 1)).val());
                        sessionStorage.setItem($("#" + group + "_operator_" + (i + 1)).attr("id"), $("#" + group + "_operator_" + (i + 1)).val());
                        continue;
                    }
                    //date
                    if ($("#" + group + "_date_" + (i + 1)).length > 0) {
                        if ($("#" + group + "_date_" + (i + 1) + "_d1").val() != "") {
                            //save to session
                            sessionStorage.setItem($("#" + group + "_date_" + (i + 1) + "_d1").attr("id"), $("#" + group + "_date_" + (i + 1) + "_d1").val());
                            if ($("#" + group + "_date_" + (i + 1) + "_d2").val() != "") {
                                //save to session
                                sessionStorage.setItem($("#" + group + "_date_" + (i + 1) + "_d2").attr("id"), $("#" + group + "_date_" + (i + 1) + "_d2").val());
                                params["termList"].push($("#" + group + "_date_" + (i + 1) + "_d1").val() + "~" + $("#" + group + "_date_" + (i + 1) + "_d2").val());
                            } else {
                                params["termList"].push($("#" + group + "_date_" + (i + 1) + "_d1").val());
                            }
                            params["operatorList"].push($("#" + group + "_operator_" + (i + 1)).val());
                            sessionStorage.setItem($("#" + group + "_operator_" + (i + 1)).attr("id"), $("#" + group + "_operator_" + (i + 1)).val());
                            params["keyList"].push($("#" + group + "_key_" + (i + 1)).val());
                            continue;
                        } else {
                            if ($("#" + group + "_date_" + (i + 1) + "_d2").val() != "") {
                                //save to session
                                sessionStorage.setItem($("#" + group + "_date_" + (i + 1) + "_d2").attr("id"), $("#" + group + "_date_" + (i + 1) + "_d2").val());
                                params["termList"].push($("#" + group + "_date_" + (i + 1) + "_d2").val());
                                params["operatorList"].push($("#" + group + "_operator_" + (i + 1)).val());
                                sessionStorage.setItem($("#" + group + "_operator_" + (i + 1)).attr("id"), $("#" + group + "_operator_" + (i + 1)).val());
                                params["keyList"].push($("#" + group + "_key_" + (i + 1)).val());
                                continue;
                            }
                        }
                    }
                    //num
                    if ($("#" + group + "_num_" + (i + 1)).length > 0) {
                        if ($("#" + group + "_num_" + (i + 1) + "_n1").val() != "") {
                            //save to session
                            sessionStorage.setItem($("#" + group + "_num_" + (i + 1) + "_n1").attr("id"), $("#" + group + "_date_" + (i + 1) + "_n1").val());
                            if ($("#" + group + "_num_" + (i + 1) + "_n2").val() != "") {
                                //save to session
                                sessionStorage.setItem($("#" + group + "_num_" + (i + 1) + "_n2").attr("id"), $("#" + group + "_num_" + (i + 1) + "_n2").val());
                                params["termList"].push($("#" + group + "_num_" + (i + 1) + "_n1").val() + "~" + $("#" + group + "_num_" + (i + 1) + "_n2").val());
                            } else {
                                params["termList"].push($("#" + group + "_num_" + (i + 1) + "_n1").val());
                            }
                            params["operatorList"].push($("#" + group + "_operator_" + (i + 1)).val());
                            sessionStorage.setItem($("#" + group + "_operator_" + (i + 1)).attr("id"), $("#" + group + "_operator_" + (i + 1)).val());
                            params["keyList"].push($("#" + group + "_key_" + (i + 1)).val());

                            continue;
                        } else {
                            if ($("#" + group + "_num_" + (i + 1) + "_n2").val() != "") {
                                //save to session
                                sessionStorage.setItem($("#" + group + "_num_" + (i + 1) + "_n2").attr("id"), $("#" + group + "_num_" + (i + 1) + "_n2").val());
                                params["termList"].push($("#" + group + "_num_" + (i + 1) + "_n2").val());
                                params["operatorList"].push($("#" + group + "_operator_" + (i + 1)).val());
                                sessionStorage.setItem($("#" + group + "_operator_" + (i + 1)).attr("id"), $("#" + group + "_operator_" + (i + 1)).val());
                                params["keyList"].push($("#" + group + "_key_" + (i + 1)).val());
                                continue;
                            }
                        }
                    }
                    //bool
                    if ($("#" + group + "_bool_" + (i + 1)).length > 0) {
                        $("#" + group + "_default_bool input[name=" + group + "_default_bool]:checked").val()
                        if ($("#" + group + "_bool_" + (i + 1)+" input[name="+group+"_bool_"+(i+1)+"]:checked").val() != "") {
                            //save to session
                            sessionStorage.setItem($("#" + group + "_bool_" + (i + 1)).attr("id"), $("#" + group + "_bool_" + (i + 1) + " input[name=" + group + "_bool_" + (i + 1) + "]:checked").val());
                            params["termList"].push($("#" + group + "_bool_" + (i + 1) + " input[name=" + group + "_bool_" + (i + 1) + "]:checked").val());
                            params["operatorList"].push($("#" + group + "_operator_" + (i + 1)).val());
                            sessionStorage.setItem($("#" + group + "_operator_" + (i + 1)).attr("id"), $("#" + group + "_operator_" + (i + 1)).val());
                            params["keyList"].push($("#" + group + "_key_" + (i + 1)).val());
                            continue;
                        }
                    }
                }
            }
            return params;
        }

        function buildUrl(target) {
            var targetUrl = "";
            var params = getParams(target);
            if (params != null) {
                targetUrl = "/" + target + "/" + params["default_key"] + "=" + encodeURIComponent(params["default_term"]);
                if (params["termList"].length > 0) {
                    for (i = 0; i < params["termList"].length; i++) {
                        targetUrl = targetUrl + "&cmp=" + params["operatorList"][i] + "&" + params["keyList"][i] + "=" + encodeURIComponent(params["termList"][i]);
                    }
                }
            }
            return targetUrl;
        }

        function addNewField(group, n) {
            var formGroup = $("." + group).eq(0).clone();
            var selList = formGroup.find("select");
            selList[0].id = group + "_operator_" + n;
            selList[0].name = group + "_operator_" + n;
            selList[1].id = group + "_key_" + n;
            selList[1].name = group + "_key_" + n;
            var inputTemp = "<input type='text' size='50' height='30' name='" + group + "_term_" + n + "' id='" + group + "_term_" + n + "' />";
            //add count check here
            if (formGroup.find("input").length > 1) {
                formGroup.find("div.date_search").eq(0).replaceWith(inputTemp);
                formGroup.find("div.num_search").eq(0).replaceWith(inputTemp);
                formGroup.find("div.bool_search").eq(0).replaceWith(inputTemp);
            } else if (formGroup.find("input").length = 1) {
                formGroup.find("input").eq(0).replaceWith(inputTemp);
            }
            $("." + group).last().after(formGroup);
        }

        function removeLastField(group) {
            if ($("." + group).length > 1) {
                $("." + group).last().remove();
                if (sessionStorage.removeItem($("." + group).last().attr("id")) != null) {
                    sessionStorage.removeItem($("." + group).last().attr("id"));
                }
            }
        }
        function setDatePicker(eleId) {
            var dateDivId = eleId.replace("_term", "_date").replace("_bool", "_date").replace("_num", "_date");
            var dateId1 = dateDivId + "_d1";
            var dateId2 = dateDivId + "_d2";
            var eleClass = eleId.substr(0, eleId.indexOf("_", (eleId.indexOf("_") + 1)));
            var dateFieldTemp = "<div class='input-group " + eleClass + "_date date_search' id='" + dateDivId + "'><label for='" + dateId1 + "'>Start date: </label><input class='datepicker' type='date' height='30' name='" + dateId1 + "' id='" + dateId1 + "' /><label for='" + dateId2 + "' style='margin-left:15px;'>End date: </label><input class='datepicker' type='date' height='30' name='" + dateId2 + "' id='" + dateId2 + "' /></div>";
            // Replace the current input field
            $("#" + eleId).replaceWith(dateFieldTemp);
            $("#" + dateId1 + ", " + "#" + dateId2).datepicker({ dateFormat: "yy-mm-dd" });
            //Assign stored dates
            if (sessionStorage.getItem(dateId1) != null) {
                $("#"+dateId1).val(sessionStorage.getItem(dateId1));
            }
            if (sessionStorage.getItem(dateId2) != null) {
                $("#" + dateId2).val(sessionStorage.getItem(dateId2));
            }
        }
        function setTxtInput(eleId) {
            var txtId = eleId.replace("_date", "_term").replace("_bool", "_term").replace("_num", "_term");
            var txtTemp = "<input type='text' title='"+txtId+"' size='50' height='30' name='" + txtId + "' id='" + txtId + "' />";
            $("#" + eleId).replaceWith(txtTemp);
            if (sessionStorage.getItem(txtId) != null) {
                $("#" + txtId).val(sessionStorage.getItem(txtId));
            }
        }
        function setNumInput(eleId) {
            var numDivId = eleId.replace("_term", "_num").replace("_date", "_num").replace("_bool", "_num");
            var numId1 = numDivId + "_n1";
            var numId2 = numDivId + "_n2";
            var eleClass = eleId.substr(0, eleId.indexOf("_", (eleId.indexOf("_") + 1)));
            var numFieldTemp = "<div class='input-group " + eleClass + "_num num_search' id='" + numDivId + "'><label for='" + numId1 + "'>From: </label><input class='spinner' type='number' height='30' name='" + numId1 + "' id='" + numId1 + "' /><label for='" + numId2 + "' style='margin-left:15px;'>To: </label><input class='spinner' type='number' height='30' name='" + numId2 + "' id='" + numId2 + "' /></div>";
            // Replace the current input field
            $("#" + eleId).replaceWith(numFieldTemp);

            if (sessionStorage.getItem(numId1) != null) {
                $("#" + numId1).val(sessionStorage.getItem(numId1));
            }
            if (sessionStorage.getItem(numId2) != null) {
                $("#" + numId2).val(sessionStorage.getItem(numId2));
            }
        }
        function setBoolInput(eleId) {
            var boolDivId = eleId.replace("_term", "_bool").replace("_date", "_bool").replace("_num", "_bool");
            var eleClass = eleId.substr(0, eleId.indexOf("_", (eleId.indexOf("_") + 1)));
            var boolFieldTemp = "<div class='input-group " + eleClass + "_bool bool_search' id='" + boolDivId + "'><label for='" + boolDivId + "'>True</label><input type='radio' name='" + boolDivId + "' value='true' /> <label for='" + boolDivId + "'>False </label><input type='radio' name='" + boolDivId + "' value='false' /> </div>";
            // Replace the current input field
            $("#" + eleId).replaceWith(boolFieldTemp);
            if(sessionStorage.getItem(boolDivId) != null){
                $("#" + boolDivId + "input:radio[name='" + boolDivId + "']").val([sessionStorage.getItem(boolDivId)]);
            }
        }
    }
});