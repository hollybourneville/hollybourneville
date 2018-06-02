$(document).ready(function () {
    // Get the modal
   
    $('#ancLogin').click(function () {
        $('#myForm').show();
        if (!sessionStorage.access_token || sessionStorage.access_token == null) {

            $("#myModal").modal();

            $(".modal-dialog table[id^='tb']").hide();
            $('.modal-footer a').hide();
            $('#tbLoginProfile').show();
            $('#ancRegister').show();
            $('#ancForgotPassword').show();
            $('#myModal .modal-title').html('NSAKL Login');
            if ($('.Nsakl-Registration').length > 0) {
                $('#myModal').toggleClass("Nsakl-Registration");
                $('#myModal .modal-body').toggleClass("modal-body-Nsakl-Registration");
            }
        }
        else {
            sessionStorage.clear();
            $('#ancLogin').html('Login');
            $('#ancLoginback').closest('li').remove();
        }
    });

    if (sessionStorage.access_token) {
        $('#ancLogin').html('Logout');
        $('#ancLogin').closest('ul').append('<li class="active"><a id="ancLoginback" href="#">Back to NSAKL</a></li>');
        $('#ancLoginback').on('click', function (event) {
            window.location.href = '/NSAKLView.html';
        });
    }
    else {

    }
    $('#btnsubmitLogin').on('click', function (event) {        
        event.preventDefault(); // To prevent following the link (optional)        
    });
    $('.modal-dialog a').click(function () {
        $(".modal-dialog table[id^='tb']").hide();
        $('.modal-footer a').hide();
    });
    $('#ancRegister').click(function () {        
        if (($('#myModal .modal-title').html().indexOf('Forgot Password') < 0)) {
            $('#myModal').toggleClass("Nsakl-Registration");
            $('#myModal .modal-body').toggleClass("modal-body-Nsakl-Registration");
        }

        $('#tbRegisterProfile').show();
        $('#ancRegister1').show();
        $('#myModal .modal-title').html('NSAKL Registration');
    });
    $('#ancForgotPassword').click(function () {
        $('#tbForgotLoginProfile').show();
        $('#ancRegister1').show();
        $('#myModal .modal-title').html('NSAKL Forgot Password');
    });
    $('#ancRegister1').click(function () {        
        if (($('#myModal .modal-title').html().indexOf('Forgot Password') < 0)) {
            $('#myModal').toggleClass("Nsakl-Registration");
            $('#myModal .modal-body').toggleClass("modal-body-Nsakl-Registration");
        }
        $('#tbLoginProfile').show();
        $('#ancRegister').show();
        $('#ancForgotPassword').show();
        $('#myModal .modal-title').html('NSAKL Login');
    });
    //$('#ancLogin').click(function () {
    //    if (event.target == modal) {
    //        modal.style.display = "none";
    //    }
    //});

    //$('.nav-link').click(function () {
    //    var goTo = $(this).attr("go-to");
    //    $("html, body").animate({ scrollTop: $('#' + goTo).offset().top }, 1000);
    //});

    //if (sessionStorage.getItem('access_token')) {
    //    $('#ancLogin').html('Logout');
    //    $('img[title="Alarm Switch"]').closest('div').show();
    //}

    //else {
    //    $('#ancLogin').html('Login');
    //    $('img[title="Alarm Switch"]').closest('div').hide();
    //}

});


var app = angular.module('myApp', []);
app.controller('validateCtrl', function ($scope, $http) {
    $('#lblUser').hide();
    $scope.onSubmitButtonClick = function () {
        if ($scope.user && $scope.password) {
            sessionStorage.setItem('LoginType', $('#ddnLoginType :selected').val());
            var data = "";

            var config = {
                headers: {
                    'Content-Type': ';charset=utf-8;'
                }
            }
            $('#myModalspin').show();
            $http.defaults.headers.post["Content-Type"] = "application/json;charset=UTF-8";
            $http.post('/token', "grant_type=password&username=" + $scope.user + "&password=" + $scope.password).success(function (data, status, headers, config) {

                $('#lblUser').hide();
                $scope.loading = false;
                sessionStorage.setItem('access_token', data.access_token);
                sessionStorage.setItem('userName', data.userName);
                sessionStorage.setItem('ContactID', data.uId);
                
                location.href = '/NSAKLView.html';
               
                $scope.working = false;
            }).error(function (data, status, headers, config) {
                $('#myModalspin').hide();
                $('#lblUser').html(data.error_description);
                $('#lblUser').show();
                window.location.href = '/';
                $scope.working = false;
            });
            
        }
        else
            $('#lblUser').show();
    }
    $scope.onForgotSubmitButtonClick = function () {
        if ($scope.fuser) {
            var data = "";
            var config = {
                headers: {
                    'Content-Type': ';charset=utf-8;'
                }
            }
            $('#myModalspin').show();
            $http.defaults.headers.post["Content-Type"] = "application/xml";
            $http.post('/api/ForgotUserPassword', '<SystemDataMessage><Contacts><Contact><Username>' + $scope.fuser + '</Username></Contact></Contacts></SystemDataMessage>').success(function (data, status, headers, config) {
                $scope.getuserprofileData = data.itemsField;
                $scope.getuserprofileData = $scope.getuserprofileData[0];
                $scope.getuserprofileData = $scope.getuserprofileData.contactField;

                for (var i = 0, l = $scope.getuserprofileData.length; i < l; i++) {
                    var d = $scope.getuserprofileData[i];
                    if (d["recidField"] || d["recidField"])
                        var a = 1;
                }

                $('#myModalspin').hide();
            }).error(function (data, status, headers, config) {

            }).finally(function () {
            });
        }
        else
            $('#lblUser').show();
    }
    $scope.onRegisterSubmitButtonClick = function () {
        if ($('#txt_UserName').val() && ($('#txt_Pwd').val() == $('#txt_CPwd').val())) {
            $('#myModalspin').show();
            $http.defaults.headers.post["Content-Type"] = "application/xml";
            var xmlrequest = "<SystemDataMessage><Contacts><Contact><Firstname>" + $('#txt_firstname').val() + "</Firstname><Middlename>" + $('#txt_middlename').val() + "</Middlename><Lastname>" + $('#txt_lastname').val() + "</Lastname><Gender>" + $('input[name=RadioButtonList1]:checked').val() + "</Gender><Physical_address_unit_number>" + $('#txt_unit_number').val() + "</Physical_address_unit_number><Physical_address_number>" + $('#txt_address_number').val() + "</Physical_address_number><Physical_address_name>" + $('#txt_address_name').val() + "</Physical_address_name><Physical_address_suburb>" + $('#txt_address_name').val() + "</Physical_address_suburb><Physical_address_postcode</Physical_address_postcode><Postal_address_number>" + $('#txt_box_number').val() + "</Postal_address_number><Postal_address_box_lobby_location>" + $('#ddl_box_lobby').val() + "</Postal_address_box_lobby_location><Postal_address_suburb>" + $('#txt_postal_suburb').val() + "</Postal_address_suburb><Postal_address_postcode>" + $('#txt_postal_postcode').val() + "</Postal_address_postcode><Phone_home>" + $('#txt_home_phone').val() + "</Phone_home><Phone_work>" + $('#txt_work_phone').val() + "</Phone_work><Phone_cell>" + $('#txt_cell_phone').val() + "</Phone_cell><Email_address_default>" + $('#txt_email').val() + "</Email_address_default><Email_address_alternate></Email_address_alternate><Username>" + $('#txt_UserName').val() + "</Username><Password>" + $('#txt_Pwd').val() + "</Password><Remarks></Remarks><Occupation></Occupation><Other_remarks></Other_remarks><Last_updated_by></Last_updated_by><Signup_date></Signup_date><Emergency_volunteering_status></Emergency_volunteering_status><Mailing_list_status></Mailing_list_status><Status></Status><Birth_date></Birth_date><Last_updated_date></Last_updated_date></Contact></Contacts></SystemDataMessage>";
            $http.defaults.headers.post["Content-Length"] = "3495";
            $http.post('/api/RegisterUserProfile', xmlrequest).success(function (data, status, headers, config) {
                $('#lblUserRegister').show();
                $('#lblUserRegister').html('Regitered Successfully! Please check your mail!');

                $scope.getprofileprofileData = data.itemsField;
                $scope.getprofileprofileData = $scope.getprofileprofileData[0];
                $scope.getprofileprofileData = $scope.getprofileprofileData.contactField;


                if ($scope.getprofileprofileData.length > 0)
                {
                    alert('Regitered Successfully! Please check your mail!');
                }
                for (var i = 0, l = $scope.getprofileprofileData.length; i < l; i++) {
                    var d = $scope.getprofileprofileData[i];
                    if (d["recidField"] || d["recidField"])
                        var a = 1;
                }

                $('#myModalspin').hide();
            }).error(function (data, status, headers, config) {

            }).finally(function () {
            });
        }
        else
            $('#lblUserRegister').show();
    }
});
