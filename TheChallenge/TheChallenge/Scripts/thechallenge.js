$(document).ready(function () {
    $("#the-challenge-tabstrip").kendoTabStrip({
        animation: {
            open: {
                effects: "fadeIn"
            }
        }

    });


    var tcAuthorizationToken = null;
    var dateResults = [];


    function InitiateUserData() {
        var currentLiftsSource = new kendo.data.DataSource({
            transport: {
                read: {
                    beforeSend: function (XMLHttpRequest) {
                        XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                    },
                    url: '/api/profile/current'
                },
                type: 'json',
                pagesize: 5

            },
            group: {
                field: "EventType",
                value: "Event Type"
            }
        });

        var goalsSource = new kendo.data.DataSource({
            transport: {
                read: {
                    beforeSend: function (XMLHttpRequest) {
                        XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                    },
                    url: '/api/goal'
                },
                type: 'json',
                pagesize: 5

            }
        });

        var contestSource = new kendo.data.DataSource({
            transport: {
                read: {
                    beforeSend: function (XMLHttpRequest) {
                        XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                    },
                    url: '/api/contest'
                },
                type: 'json',
                pagesize: 5
            }
        });

        var foodSource = new kendo.data.DataSource({
            transport: {
                read: {
                    beforeSend: function (XMLHttpRequest) {
                        XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                    },
                    url: '/api/food/'
                },
                type: 'json'
            }
        });

        $("#contestTable").kendoGrid({
            dataSource: contestSource,
            sortable: true,
            scrollable: true,
            pageable: true,
            detailTemplate: kendo.template($("#contest-events-template").html()),
            detailInit: detailInit,
            dataBound: function () {
                this.expandRow(this.tbody.find("tr.k-master-row").first());
            },
            columns: [
			{
				field: "Name",
				title: "Contest Name",
				width: 100
			},
			{
				field: "ContestDate",
				title: "Contest Date",
				template: '#= kendo.toString(new Date(parseInt(ContestDate.substr(6))),"MM/dd/yyyy") #',
				width: 75
			},
			{
				field: "Place",
				title: "Contest Location",
				width: 100
			},
			{
				field: "Details",
				title: "Contest Details",
				width: 100
			}
		]

        });

        $("#currentLiftsTable").kendoGrid({
            dataSource: currentLiftsSource,
            pageable: true,
            columns: [
			{
				field: "Event",
				title: "Exercise Name",
				width: 100
			},
			{
				field: "DateLifted",
				title: "Date Lifted",
				template: '#= kendo.toString(new Date(parseInt(DateLifted.substr(6))),"MM/dd/yyyy") #',
				width: 75
			},
			{
				field: "Result",
				width: 100
			}
		]

        });

        $("#goalsTable").kendoGrid({
            dataSource: goalsSource,
            pageable: true,
            columns: [
				    {
				        field: "Event.Name",
				        title: "Exercise Name",
				        width: 100
				    },
				    {
				        field: "Contest.Name",
				        title: "Contest Name",
				        width: 75
				    },
				    {
				        field: "Contest.ContestDate",
				        template: '#= kendo.toString(new Date(parseInt(Contest.ContestDate.substr(6))),"MM/dd/yyyy") # (#= Math.ceil(((new Date(parseInt(Contest.ContestDate.substr(6)))).getTime() - (new Date()).getTime())/(1000*60*60*24)) # days left)',
				        width: 100
				    },
				    {
				        field: "Result",
				        width: 100
				    }
			    ]

        });
        /**
        THIS IS FOR CALENDAR
        */

        getWorkoutDates();


    }

    function getWorkoutDates() {
        $.ajax('/api/workout/', {
            type: 'get', contentType: 'application/json',
            beforeSend: function (XMLHttpRequest) {
                XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
            },
            success: function (result) {
                for (var i = 0, max = result.length; i < max; i++)
                    dateResults.push(parseInt(result[i].substr(6, 13)));
                //+new Date(parseInt(result[i].substr(6)))
                $('#calendar').kendoCalendar({
                    month: {
                        // template for dates in month view
                        content: '# if ($.inArray(+data.date, [' + dateResults + ']) != -1) { #' +
                '<div class="workoutDate">#=data.value#</div>' +
                '# } #' +
                '#= data.value #'
                    },
                    change: onChange
                });
                $("#contestTable").data("kendoGrid").dataSource.read();
            },
            error: function (result, t2, t3) {
                if (result.status === 403) {
                    var window = $("#sign-in-window").data('kendoWindow');
                    window.center();
                    window.open();
                }
            }
        });
    }


    function onChange() {
        if ($.inArray(+this.value(), dateResults) != -1) {
            $('#add-workout-id').css('display', 'none');
            $('#add-diet-id').css('display', 'none');
            $.ajax('/api/meal/' + kendo.toString(this.value(), 'MM-dd-yyyy'), {
                type: 'get',
                contentType: 'application/json',
                beforeSend: function (XMLHttpRequest) {
                    XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                },
                success: function (result) {
                    $('#meal-view').empty();
                    if(result.length > 0){
                        $('#meal-view').css('display','block');
                        $('#meal-view').kendoGrid({
                            dataSource: {
                                data: result,
                                group: {
                                    field: 'EntryDate' ,
                                    value: "Time"
                                },
                                aggregate: [
                                    { field: "TotalCalories", aggregate: "sum" },
                                    { field: "TotalCarbs", aggregate: "sum" },
                                    { field: "TotalFats", aggregate: "sum" },
                                    { field: "TotalProtein", aggregate: "sum" }
                                ]
                            },
                            pageSize: 5,
                            detailTemplate: kendo.template($("#meal-nutrient-template").html()),
                            detailInit:  function(e){
                                console.log(e.data);
                                var detailRow = e.detailRow;
                                detailRow.find(".meal-nutrients").kendoGrid({
                                    dataSource: {
                                        data: e.data.CalculatedNutrients,
                                        pageSize: 5
                                    },
                                    scrollable: true,
                                    sortable: true,
                                    pageable: true,
                                    columns: [
                                        { field: "Description", title: "Nutrient", width: 100 },
                                        { field: "AmountIn100Grams", title: "Amount" },
                                        { field: "Units", title: "Units" },
                                        { field: "IsNutrientAdded", title: "Is Added" }
                                    ]
                                });
                            },
                            scrollable: true,
                            sortable: true,
                            pageable: true,
                            columns: [
                                { field: "Name", title: "Name", width: 100 },
                                { field: "ServingSize", title: "Amount" },
                                { field: "SelectedServing.Description", title: "Units" },
                                { field: "TotalCarbs", title: "Carbs",footerTemplate: "#=sum#" },
                                { field: "TotalFats", title: "Fats",footerTemplate: "#=sum#" },
                                { field: "TotalProtein", title: "Protein",footerTemplate: "#=sum#" },
                                { field: "TotalCalories", title: "Calories",footerTemplate: "#=sum#" }
                            ]
                        });
                    }
                },
                error: function (returnedObject, t2, t3) {
                    console.log(returnedObject);
                    console.log(t2);
                    console.log(t3);
                }
            });
            $.ajax('/api/workout/' + kendo.toString(this.value(), 'MM-dd-yyyy'), {
                type: 'get',
                contentType: 'application/json',
                beforeSend: function (XMLHttpRequest) {
                    XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                },
                success: function (result) {
                    $('#workout-view').empty();
                    if(result.length > 0){
                        $('#workout-view').css('display', 'block');
                        $('#workout-view').kendoGrid({
                            dataSource: {
                                data: result,
                                schema: {
                                    model: {
                                        fields: {
                                            ContestId: { type: "number" },
                                            Name: { type: "string" },
                                            Reps: { type: "number" },
                                            Weight: { type: "float" },
                                            Time: { type: "string" },
                                            Distance: { type: "float" }
                                        }
                                    }
                                },
                                pageSize: 5
                            },
                            scrollable: true,
                            sortable: true,
                            pageable: true,
                            columns: [
				                {
				                    field: "Name",
				                    title: "Exercise/Routine",
				                    width: 100
				                },
				                {
				                    field: "Reps",
				                    title: "Repetitions",
				                    width: 50
				                },
				                {
				                    field: "Weight",
				                    width: 50
				                },
				                {
				                    field: "Time",
				                    title: "Time",
				                    width: 100
				                },
				                {
				                    field: "Distance",
				                    width: 50
				                }
			                ]
                        });
                    }
                },
                error: function (returnedObject, t2, t3) {
                    console.log(returnedObject);
                    console.log(t2);
                    console.log(t3);
                }
            });
        }
        else {
            $('#workout-view').css('display', 'none');
        }
    }

    function detailInit(e) {
        var detailRow = e.detailRow;
        detailRow.find(".contest-events").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        beforeSend: function (XMLHttpRequest) {
                            XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                        },
                        url: 'api/contest/' + e.data.ContestId
                    },
                    type: 'json'
                },
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 6,
                filter: { field: "ContestId", operator: "eq", value: e.data.ContestId }
            },
            scrollable: false,
            sortable: true,
            pageable: true,
            columns: [
                { field: "EventName", title: "Event/Routine", width: 100 },
                { field: "EventGoal", title: "Extra Information", width: 250 }
            ]
        });
    }

    //logic for calendar:
    //add exercise
    //  - add to array of exercises that always populates the preview div
    //add workout
    $('#add-workout-id').css('display', 'none');
    $('#add-diet-id').css('display', 'none');
    $('#add-workout').click(function () {
        $('#add-workout-id').css('display', 'block');
        $('#add-diet-id').css('display', 'none');
        $('#workout-view').css('display', 'none');
        $('#meal-view').css('display', 'none');
    });
    $('#add-diet').click(function () {
        $('#add-diet-id').css('display', 'block');
        $('#add-workout-id').css('display', 'none');
        $('#workout-view').css('display', 'none');
        $('#meal-view').css('display', 'none');
    });

    var validatable = $("#add-workout-id").kendoValidator().data("kendoValidator");
    var dietValidatable = $("#add-diet-id").kendoValidator().data("kendoValidator");
    var registerValidatable = $("#register-information").kendoValidator({
        rules: {
            custom: function (input) {
                if (!input.is("[id=confirm-register-password]")) return true;
                return input.is("[id=confirm-register-password]") && input.val() === $('#register-password').val();
            }
        }
    }).data('kendoValidator');
    var signinValidatable = $("#sign-in-information").kendoValidator().data('kendoValidator');

    var viewModel = kendo.observable({
        events: new kendo.data.DataSource({
            transport: {
                read: {
                    url: '/api/exercise',
                    beforeSend: function (XMLHttpRequest) {
                        XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                    },
                    dataType: "json"
                }
            }
        }),
        entryDate: new Date(),
        exerciseName: null,
        reps: 0,
        weight: 0,
        time: '00:00:00.000',
        distance: 0,
        addExercise: function () {
            if (validatable.validate()) {
                this.get("exercises").push({
                    id: this.get("exerciseName").Id,
                    name: this.get("exerciseName").Name,
                    reps: parseInt(this.get("reps")),
                    weight: parseFloat(this.get("weight")),
                    time: this.get("time"),
                    distance: parseFloat(this.get("distance"))
                });
                this.set("isEnabled", true);
            }
        },
        deleteExercise: function (e) {
            // the current data item (exercise) is passed as the "data" field of the event argument
            var exercise = e.data;
            var exercises = this.get("exercises");
            var index = exercises.indexOf(exercise);
            // remove the product by using the splice method
            exercises.splice(index, 1);
            if (exercises.length == 0)
                this.set("isEnabled", false);
        },
        total: function () {
            return this.get("exercises").length;
        },
        save: function () {
            //send this.exercises to get saved for the day and forget about it
            //json call here for post
            var exercisesPost = [];
            for (var i = 0, max = this.exercises.length; i < max; i++) {
                exercisesPost.push({
                    Id: this.exercises[i].id,
                    Name: this.exercises[i].name,
                    Reps: this.exercises[i].reps,
                    Weight: this.exercises[i].weight,
                    Time: this.exercises[i].time,
                    Distance: this.exercises[i].distance
                });
            }

            var post = {
                Exercises: exercisesPost,
                EntryDate: this.get("entryDate")
            }

            var self = this;
            $.ajax('/api/exercise', {
                data: JSON.stringify(post),
                type: 'post', contentType: 'application/json',
                beforeSend: function (XMLHttpRequest) {
                    XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                },
                success: function (result) {
                    self.set("exercises", []);
                    alert('Saved!');
                    dateResults.push(+self.get("entryDate"));
                    $('#calendar').empty();
                    getWorkoutDates();
                    $('#add-workout-id').css('display', 'none');
                    self.set("isEnabled", false);
                },
                error: function (returnedObject, t2, t3) {
                    console.log(returnedObject);
                    console.log(t2);
                    console.log(t3);
                }
            });
        },
        exercises: [],
        isEnabled: false
    });

    kendo.bind($("#add-workout-id"), viewModel);


    var dietViewModel = kendo.observable({
        events: new kendo.data.DataSource({
            transport: {
                read: {
                    beforeSend: function (XMLHttpRequest) {
                        XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                    },
                    url: '/api/food'
                },
                type: 'json'
            },
            filter: "contains"
        }),
        servingTypes: 0,
        entryDate: new Date(),
        entryTime: null,
        foodName: null,
        dietServing: 0,
        dietServingType: null,
        selectFoodForAdding: function(){
            var self = this;
            $.ajax('/api/food/' + this.get("foodName").Id, {
                type: 'get', contentType: 'application/json',
                beforeSend: function (XMLHttpRequest) {
                    XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                },
                success: function (result) {
                    //get servings
                    self.set("servingTypes",new kendo.data.DataSource({data: result.Servings}));
                    self.set("dietServingType",result.Servings[0]);
                    // self.get("servingTypes").read();
                },
                error: function (returnedObject, t2, t3) {
                    console.log(returnedObject);
                    console.log(t2);
                    console.log(t3);
                }
            });
        },
        addFood: function () {
            var entryDate = kendo.toString(this.get("entryDate"),"MM/dd/yyyy");
            var entryTime = kendo.toString(this.get("entryTime"),"hh:mm tt");
            if (dietValidatable.validate()) {
                this.get("foods").push({
                    FoodId: this.get("foodName").Id,
                    Name: this.get("foodName").Name,
                    ServingSize: parseInt(this.get("dietServing")),
                    servingTypeDesc: this.get("dietServingType").Description,
                    ServingTypeId: this.get("dietServingType").Id,
                    entryDate: entryDate,
                    entryTime: entryTime,
                    Date: entryDate + ' ' + entryTime,
                    serving: parseInt(this.get("dietServing")) + ' ' + this.get("dietServingType").Description
                });
                this.set("isEnabled", true);
            }
        },
        deleteFood: function (e) {
            // the current data item (exercise) is passed as the "data" field of the event argument
            var food = e.data;
            var foods = this.get("foods");
            var index = foods.indexOf(food);
            // remove the product by using the splice method
            foods.splice(index, 1);
            if (foods.length == 0)
                this.set("isEnabled", false);
        },
        total: function () {
            return this.get("foods").length;
        },
        save: function () {
            //send this.exercises to get saved for the day and forget about it
            //json call here for post
            var foodsPost = [];
            for (var i = 0, max = this.foods.length; i < max; i++) {
                foodsPost.push({
                    FoodId: this.foods[i].FoodId,
                    Name: this.foods[i].Name,
                    ServingSize: this.foods[i].ServingSize,
                    Date: this.foods[i].Date,
                    ServingTypeId: this.foods[i].ServingTypeId
                });
            }

            var self = this;
            $.ajax('/api/food', {
                data: JSON.stringify(foodsPost),
                type: 'post', contentType: 'application/json',
                beforeSend: function (XMLHttpRequest) {
                    XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                },
                success: function (result) {
                    self.set("foods", []);
                    alert('Saved!');
                    dateResults.push(+self.get("entryDate"));
                    $('#calendar').empty();
                    getWorkoutDates();
                    $('#add-diet-id').css('display', 'none');
                    self.set("isEnabled", false);
                },
                error: function (returnedObject, t2, t3) {
                    console.log(returnedObject);
                    console.log(t2);
                    console.log(t3);
                }
            });

        },
        foods: [],
        isEnabled: false
    });

    kendo.bind($("#add-diet-id"), dietViewModel);

    var registerModel = kendo.observable({
        registerName: '',
        registerPassword: '',
        confirmPassword: '',
        register: function () {
            if (registerValidatable.validate()) {
                var self = this;
                $.ajax('/api/account', {
                    data: JSON.stringify({ UserName: this.get('registerName'), Password: this.get('registerPassword') }),
                    type: 'post', contentType: 'application/json',
                    success: function (result, status, xmlResponse) {
                        if (typeof (localStorage) == 'undefined') {
                            tcAuthorizationToken = xmlResponse.getResponseHeader('TC-Authorization');
                        } else {
                            localStorage.setItem('TC-Authorization', xmlResponse.getResponseHeader('TC-Authorization'));
                        }
                        var registerWindow = $("#register-window").data('kendoWindow');
                        registerWindow.close();
                        InitiateUserData();
                    },
                    error: function (returnedObject, t2, t3) {
                        console.log(returnedObject);
                        console.log(t2);
                        console.log(t3);
                    }
                });
            }

        }
    });

    kendo.bind($("#register-information"), registerModel);

    var signInModel = kendo.observable({
        signInName: '',
        signInPassword: '',
        errorMessage: '',
        signIn: function () {
            var self = this;
            self.set("errorMessage",'');
            if(signinValidatable.validate()){
                $.ajax('/api/signin', {
                    data: JSON.stringify({ UserName: this.get('signInName'), Password: this.get('signInPassword') }),
                    type: 'post', contentType: 'application/json',
                    beforeSend: function(xhr) {
                        xhr.setRequestHeader("X-MicrosoftAjax","Delta=true");
                    },
                    success: function (result, status, xmlResponse) {
                        if(xmlResponse.statusText === "Unable to validate user id and password combination"){
                            self.set("errorMessage","Unable to log this user in. Please verify your credentials.");
                        }else{
                            console.log('logged in');
                            tcAuthorizationToken = xmlResponse.getResponseHeader('TC-Authorization');
                            localStorage.setItem('TC-Authorization', xmlResponse.getResponseHeader('TC-Authorization'));
                            var window = $("#sign-in-window").data('kendoWindow');
                            window.close();
                            InitiateUserData();
                        }
                    },
                    error: function (returnedObject, t2, statusCode) {
                        console.log(returnedObject);
                        console.log(t2);
                        console.log(statusCode);
                    }
                });
            }
        }
    });

    kendo.bind($("#sign-in-information"), signInModel);


    $('#view-register').click(function () {
        var signInWindow = $("#sign-in-window").data('kendoWindow');
        signInWindow.close();
        var registerWindow = $("#register-window").data('kendoWindow');
        registerWindow.center();
        registerWindow.open();
    });


    $("#workout-preview-table").kendoGrid();
    $("#diet-preview-table").kendoGrid();


    //sign in
    $("#sign-in-window").kendoWindow({
        actions: ["Maximize"],
        draggable: false,
        height: "300px",
        modal: true,
        resizable: false,
        title: "Sign In",
        visible: false,
        width: "500px"
    });

    $("#register-window").kendoWindow({
        actions: ["Maximize"],
        draggable: false,
        height: "300px",
        modal: true,
        resizable: false,
        title: "Sign In",
        visible: false,
        width: "500px"
    });


    $('#sign-out').click(function () {
        $.ajax('/api/account', {
            type: 'get', contentType: 'application/json',
            beforeSend: function (XMLHttpRequest) {
                XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
            },
            success: function (result) {
                //clear all values
                $('#contestTable').empty();
                $('#contestTable').kendoGrid();
                $('#currentLiftsTable').empty();
                $('#currentLiftsTable').kendoGrid();
                $('#goalsTable').empty();
                $('#goalsTable').kendoGrid();
                localStorage.removeItem('TC-Authorization');
                var window = $("#sign-in-window").data('kendoWindow');
                window.center();
                window.open();
            },
            error: function (returnedObject, t2, t3) {
                console.log(returnedObject);
                console.log(t2);
                console.log(t3);
            }
        });
    });

    if (localStorage.getItem('TC-Authorization') == 'undefined' || localStorage.getItem('TC-Authorization') == null) {
        var window = $("#sign-in-window").data('kendoWindow');
        window.center();
        window.open();
    } else {
        InitiateUserData();
    }

    var foodSource = new kendo.data.DataSource({
        transport: {
            read: {
                beforeSend: function (XMLHttpRequest) {
                    XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
                },
                url: '/api/food'
            },
            type: 'json'
        }
    });

    $('#food-repository').kendoAutoComplete({
        select: selectFood,
        dataTextField: "Name",
        filter: "contains",
        dataSource: foodSource
    });

    function selectFood(e) {
        $.ajax('/api/food/' + this.dataItem(e.item.index()).Id, {
            type: 'get', contentType: 'application/json',
            beforeSend: function (XMLHttpRequest) {
                XMLHttpRequest.setRequestHeader("TC-Authorization", tcAuthorizationToken != null ? tcAuthorizationToken : localStorage.getItem('TC-Authorization'));
            },
            success: function (result) {
                //get servings
                $("#serving-repository-view").css('display', 'block');
                $("#serving-repository-view").kendoDropDownList({
                    dataTextField: "Description",
                    dataValueField: "Id",
                    select: function(e){
                        LoadNutritionalTable(this.dataItem(e.item.index()).WeightInGrams, result);
                    },
                    template: "${ data.Amount } - ${ data.Description }",
                    dataSource: result.Servings
                });
                LoadNutritionalTable($("#serving-repository-view").data('kendoDropDownList').dataItem(0).WeightInGrams, result);
            },
            error: function (returnedObject, t2, t3) {
                console.log(returnedObject);
                console.log(t2);
                console.log(t3);
            }
        });
    }

    function LoadNutritionalTable(servingMultiplier, result){
        $('#nutrient-repository-view').empty();
        $('#nutrient-repository-view').kendoGrid({
            columns: ["Description",
                {
                    field: "AmountIn100Grams",
                    title: "Amount",
                    template: '#= AmountIn100Grams * ' + servingMultiplier + ' / 100 #',
                }, "Units",
                {
                    field: "IsNutrientAdded",
                    title: "Additive?"
                }
            ],
            pageable: true,
            sortable: true,
            dataSource: {
                data: result.Nutrients,
                pageSize: 10
            }
        });
    }
});
